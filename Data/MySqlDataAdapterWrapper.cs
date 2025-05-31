using System.Data;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Data;

public interface IDataAdapter
{
    int Fill(DataTable dataTable);
}

public class MySqlDataAdapterWrapper : IDataAdapter
{
    protected readonly MySqlDataAdapter adapter;

    public MySqlDataAdapterWrapper(MySqlDataAdapter adapter)
    {
        this.adapter = adapter;
    }

    public virtual MySqlCommand? SelectCommand
    {
        get => adapter.SelectCommand;
        set => adapter.SelectCommand = value;
    }

    // Implement the Fill method as required by your IDataAdapter interface
    public virtual int Fill(DataTable dataTable)
    {
        return adapter.Fill(dataTable);
    }
}