using MTM_MAUI_Application.Models;
using System.Data;

namespace MTM_MAUI_Application.Services;

/// <summary>
/// Part service implementation
/// </summary>
public class PartService : IPartService
{
    private readonly IDatabaseService _database;
    private readonly ILoggingService _logging;

    public PartService(IDatabaseService database, ILoggingService logging)
    {
        _database = database;
        _logging = logging;
    }

    public async Task<List<Part>> GetAllPartsAsync()
    {
        try
        {
            var dataTable = await _database.ExecuteStoredProcedureAsync("mfg_parts_Get_All");
            return ConvertToParts(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Error getting all parts", ex);
            return new List<Part>();
        }
    }

    public async Task<Part?> GetPartByNumberAsync(string partNumber)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "PartNumber", partNumber }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("mfg_parts_Get_ByPartNumber", parameters);
            var parts = ConvertToParts(dataTable);
            return parts.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error getting part {partNumber}", ex);
            return null;
        }
    }

    public async Task<bool> AddPartAsync(Part part)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "PartNumber", part.PartNumber },
                { "Description", part.Description },
                { "PartType", part.PartType },
                { "Customer", part.Customer },
                { "Notes", part.Notes ?? string.Empty }
            };

            await _database.ExecuteNonQueryAsync("mfg_parts_Add_Part", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error adding part {part.PartNumber}", ex);
            return false;
        }
    }

    public async Task<bool> UpdatePartAsync(Part part)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "PartNumber", part.PartNumber },
                { "Description", part.Description },
                { "PartType", part.PartType },
                { "Customer", part.Customer },
                { "Notes", part.Notes ?? string.Empty }
            };

            await _database.ExecuteNonQueryAsync("mfg_parts_Update_Part", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error updating part {part.PartNumber}", ex);
            return false;
        }
    }

    public async Task<bool> DeletePartAsync(int partId)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "PartId", partId }
            };

            await _database.ExecuteNonQueryAsync("mfg_parts_Delete_Part", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error deleting part {partId}", ex);
            return false;
        }
    }

    private List<Part> ConvertToParts(DataTable dataTable)
    {
        var parts = new List<Part>();

        foreach (DataRow row in dataTable.Rows)
        {
            parts.Add(new Part
            {
                Id = row.Field<int>("ID") ?? 0,
                PartNumber = row.Field<string>("PartNumber") ?? string.Empty,
                Description = row.Field<string>("Description") ?? string.Empty,
                PartType = row.Field<string>("PartType") ?? string.Empty,
                Customer = row.Field<string>("Customer") ?? string.Empty,
                Notes = row.Field<string>("Notes"),
                IsActive = row.Field<bool>("IsActive") ?? true,
                CreatedDate = row.Field<DateTime>("CreatedDate") ?? DateTime.Now,
                ModifiedDate = row.Field<DateTime?>("ModifiedDate")
            });
        }

        return parts;
    }
}

/// <summary>
/// Location service implementation
/// </summary>
public class LocationService : ILocationService
{
    private readonly IDatabaseService _database;
    private readonly ILoggingService _logging;

    public LocationService(IDatabaseService database, ILoggingService logging)
    {
        _database = database;
        _logging = logging;
    }

    public async Task<List<Location>> GetAllLocationsAsync()
    {
        try
        {
            var dataTable = await _database.ExecuteStoredProcedureAsync("mfg_locations_Get_All");
            return ConvertToLocations(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Error getting all locations", ex);
            return new List<Location>();
        }
    }

    public async Task<Location?> GetLocationByNameAsync(string name)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "LocationName", name }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("mfg_locations_Get_ByName", parameters);
            var locations = ConvertToLocations(dataTable);
            return locations.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error getting location {name}", ex);
            return null;
        }
    }

    public async Task<bool> AddLocationAsync(Location location)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "LocationName", location.Name },
                { "Building", location.Building },
                { "Description", location.Description ?? string.Empty }
            };

            await _database.ExecuteNonQueryAsync("mfg_locations_Add_Location", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error adding location {location.Name}", ex);
            return false;
        }
    }

    public async Task<bool> UpdateLocationAsync(Location location)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "LocationName", location.Name },
                { "Building", location.Building },
                { "Description", location.Description ?? string.Empty }
            };

            await _database.ExecuteNonQueryAsync("mfg_locations_Update_Location", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error updating location {location.Name}", ex);
            return false;
        }
    }

    public async Task<bool> DeleteLocationAsync(int locationId)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "LocationId", locationId }
            };

            await _database.ExecuteNonQueryAsync("mfg_locations_Delete_Location", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error deleting location {locationId}", ex);
            return false;
        }
    }

    private List<Location> ConvertToLocations(DataTable dataTable)
    {
        var locations = new List<Location>();

        foreach (DataRow row in dataTable.Rows)
        {
            locations.Add(new Location
            {
                Id = row.Field<int>("ID") ?? 0,
                Name = row.Field<string>("LocationName") ?? string.Empty,
                Building = row.Field<string>("Building") ?? string.Empty,
                Description = row.Field<string>("Description"),
                IsActive = row.Field<bool>("IsActive") ?? true,
                CreatedDate = row.Field<DateTime>("CreatedDate") ?? DateTime.Now,
                ModifiedDate = row.Field<DateTime?>("ModifiedDate")
            });
        }

        return locations;
    }
}

/// <summary>
/// Operation service implementation
/// </summary>
public class OperationService : IOperationService
{
    private readonly IDatabaseService _database;
    private readonly ILoggingService _logging;

    public OperationService(IDatabaseService database, ILoggingService logging)
    {
        _database = database;
        _logging = logging;
    }

    public async Task<List<Operation>> GetAllOperationsAsync()
    {
        try
        {
            var dataTable = await _database.ExecuteStoredProcedureAsync("mfg_operations_Get_All");
            return ConvertToOperations(dataTable);
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync("Error getting all operations", ex);
            return new List<Operation>();
        }
    }

    public async Task<Operation?> GetOperationByNumberAsync(string number)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "OperationNumber", number }
            };

            var dataTable = await _database.ExecuteStoredProcedureAsync("mfg_operations_Get_ByNumber", parameters);
            var operations = ConvertToOperations(dataTable);
            return operations.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error getting operation {number}", ex);
            return null;
        }
    }

    public async Task<bool> AddOperationAsync(Operation operation)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "OperationNumber", operation.Number },
                { "Description", operation.Description },
                { "Notes", operation.Notes ?? string.Empty }
            };

            await _database.ExecuteNonQueryAsync("mfg_operations_Add_Operation", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error adding operation {operation.Number}", ex);
            return false;
        }
    }

    public async Task<bool> UpdateOperationAsync(Operation operation)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "OperationNumber", operation.Number },
                { "Description", operation.Description },
                { "Notes", operation.Notes ?? string.Empty }
            };

            await _database.ExecuteNonQueryAsync("mfg_operations_Update_Operation", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error updating operation {operation.Number}", ex);
            return false;
        }
    }

    public async Task<bool> DeleteOperationAsync(int operationId)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                { "OperationId", operationId }
            };

            await _database.ExecuteNonQueryAsync("mfg_operations_Delete_Operation", parameters);
            return true;
        }
        catch (Exception ex)
        {
            await _logging.LogErrorAsync($"Error deleting operation {operationId}", ex);
            return false;
        }
    }

    private List<Operation> ConvertToOperations(DataTable dataTable)
    {
        var operations = new List<Operation>();

        foreach (DataRow row in dataTable.Rows)
        {
            operations.Add(new Operation
            {
                Id = row.Field<int>("ID") ?? 0,
                Number = row.Field<string>("OperationNumber") ?? string.Empty,
                Description = row.Field<string>("Description") ?? string.Empty,
                Notes = row.Field<string>("Notes"),
                IsActive = row.Field<bool>("IsActive") ?? true,
                CreatedDate = row.Field<DateTime>("CreatedDate") ?? DateTime.Now,
                ModifiedDate = row.Field<DateTime?>("ModifiedDate")
            });
        }

        return operations;
    }
}