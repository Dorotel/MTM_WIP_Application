using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Helpers;

#region Helper_Privilege_Validation

/// <summary>
/// Centralized privilege validation helper for role-based access control.
/// Implements the privilege matrix defined in REPO_COMPREHENSIVE_CHECKLIST.md.
/// </summary>
internal static class Helper_Privilege_Validation
{
    #region Role Check Methods

    /// <summary>
    /// Validates if the current user has write access to the specified table.
    /// </summary>
    /// <param name="tableName">The name of the table to check write access for</param>
    /// <throws>UnauthorizedAccessException if user lacks write permission</throws>
    internal static void ValidateWriteAccess(string tableName)
    {
        if (Model_AppVariables.UserTypeAdmin)
        {
            // Admin has full write access to all tables
            return;
        }

        if (Model_AppVariables.UserTypeReadOnly)
        {
            // ReadOnly users cannot write to any table
            throw new UnauthorizedAccessException($"Read-Only users cannot modify data in table: {tableName}");
        }

        if (Model_AppVariables.UserTypeNormal)
        {
            // Normal users can only write to inventory and transaction tables
            if (IsInventoryOrTransactionTable(tableName))
            {
                return;
            }
            
            throw new UnauthorizedAccessException($"Normal users cannot modify table: {tableName}. Only inventory and transaction tables are allowed.");
        }

        // If no role is set, default to denying access
        throw new UnauthorizedAccessException("User role is not properly configured. Write access denied.");
    }

    /// <summary>
    /// Validates if the current user has administrative access.
    /// </summary>
    /// <throws>UnauthorizedAccessException if user lacks admin permission</throws>
    internal static void ValidateAdminAccess()
    {
        if (!Model_AppVariables.UserTypeAdmin)
        {
            throw new UnauthorizedAccessException("Administrative access required. Only Admin users can perform this operation.");
        }
    }

    /// <summary>
    /// Validates if the current user has read access (all roles have read access).
    /// </summary>
    /// <returns>True if user has read access</returns>
    internal static bool HasReadAccess()
    {
        // All roles (Admin, Normal, ReadOnly) have read access
        return Model_AppVariables.UserTypeAdmin || 
               Model_AppVariables.UserTypeNormal || 
               Model_AppVariables.UserTypeReadOnly;
    }

    /// <summary>
    /// Checks if the current user has write access to the specified table without throwing exception.
    /// </summary>
    /// <param name="tableName">The name of the table to check</param>
    /// <returns>True if user has write access, false otherwise</returns>
    internal static bool HasWriteAccess(string tableName)
    {
        if (Model_AppVariables.UserTypeAdmin)
        {
            return true;
        }

        if (Model_AppVariables.UserTypeReadOnly)
        {
            return false;
        }

        if (Model_AppVariables.UserTypeNormal)
        {
            return IsInventoryOrTransactionTable(tableName);
        }

        return false;
    }

    /// <summary>
    /// Checks if the current user has administrative access without throwing exception.
    /// </summary>
    /// <returns>True if user has admin access, false otherwise</returns>
    internal static bool HasAdminAccess()
    {
        return Model_AppVariables.UserTypeAdmin;
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Determines if a table is an inventory or transaction table that Normal users can modify.
    /// </summary>
    /// <param name="tableName">The name of the table to check</param>
    /// <returns>True if table is inventory or transaction table, false otherwise</returns>
    private static bool IsInventoryOrTransactionTable(string tableName)
    {
        var normalizedTableName = tableName.ToLowerInvariant();
        
        // Tables that Normal users can write to
        return normalizedTableName.Contains("inv_inventory") || 
               normalizedTableName.Contains("inv_transaction") ||
               normalizedTableName.Contains("inventory") ||
               normalizedTableName.Contains("transaction");
    }

    #endregion

    #region Role Information Methods

    /// <summary>
    /// Gets the current user's role as a string.
    /// </summary>
    /// <returns>Role name: "Admin", "Normal", "ReadOnly", or "Unknown"</returns>
    internal static string GetCurrentUserRole()
    {
        if (Model_AppVariables.UserTypeAdmin) return "Admin";
        if (Model_AppVariables.UserTypeNormal) return "Normal";
        if (Model_AppVariables.UserTypeReadOnly) return "ReadOnly";
        return "Unknown";
    }

    /// <summary>
    /// Gets a description of the current user's permissions.
    /// </summary>
    /// <returns>String describing user permissions</returns>
    internal static string GetUserPermissionDescription()
    {
        return GetCurrentUserRole() switch
        {
            "Admin" => "Full access - Read/Write to all tables, Administrative functions",
            "Normal" => "Limited access - Read all tables, Write to inventory/transaction tables only",
            "ReadOnly" => "Read-only access - Search and view only, No write operations",
            _ => "Unknown role - Access denied"
        };
    }

    #endregion

    #region Validation Methods for Specific Operations

    /// <summary>
    /// Validates access for user management operations.
    /// </summary>
    internal static void ValidateUserManagementAccess()
    {
        ValidateAdminAccess();
    }

    /// <summary>
    /// Validates access for system settings operations.
    /// </summary>
    internal static void ValidateSystemSettingsAccess()
    {
        ValidateAdminAccess();
    }

    /// <summary>
    /// Validates access for role assignment operations.
    /// </summary>
    internal static void ValidateRoleAssignmentAccess()
    {
        ValidateAdminAccess();
    }

    /// <summary>
    /// Validates access for inventory operations.
    /// </summary>
    internal static void ValidateInventoryWriteAccess()
    {
        ValidateWriteAccess("inv_inventory");
    }

    /// <summary>
    /// Validates access for transaction operations.
    /// </summary>
    internal static void ValidateTransactionWriteAccess()
    {
        ValidateWriteAccess("inv_transaction");
    }

    #endregion
}

#endregion