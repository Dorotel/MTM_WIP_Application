using FluentAssertions;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using Xunit;

namespace MTM_WIP_Application.UnitTests.Helpers;

/// <summary>
/// Unit tests for Helper_Privilege_Validation class.
/// Tests the role-based access control system per the privilege matrix.
/// </summary>
public class Helper_Privilege_ValidationTests
{
    #region Test Setup

    public Helper_Privilege_ValidationTests()
    {
        // Reset user roles before each test
        ResetUserRoles();
    }

    private static void ResetUserRoles()
    {
        Model_AppVariables.UserTypeAdmin = false;
        Model_AppVariables.UserTypeReadOnly = false;
        Model_AppVariables.UserTypeNormal = false;
    }

    private static void SetAdminRole()
    {
        Model_AppVariables.UserTypeAdmin = true;
        Model_AppVariables.UserTypeReadOnly = false;
        Model_AppVariables.UserTypeNormal = false;
    }

    private static void SetNormalRole()
    {
        Model_AppVariables.UserTypeAdmin = false;
        Model_AppVariables.UserTypeReadOnly = false;
        Model_AppVariables.UserTypeNormal = true;
    }

    private static void SetReadOnlyRole()
    {
        Model_AppVariables.UserTypeAdmin = false;
        Model_AppVariables.UserTypeReadOnly = true;
        Model_AppVariables.UserTypeNormal = false;
    }

    #endregion

    #region Read Access Tests

    [Fact]
    public void HasReadAccess_AdminRole_ShouldReturnTrue()
    {
        // Arrange
        SetAdminRole();

        // Act
        var result = Helper_Privilege_Validation.HasReadAccess();

        // Assert
        result.Should().BeTrue("Admin users should have read access");
    }

    [Fact]
    public void HasReadAccess_NormalRole_ShouldReturnTrue()
    {
        // Arrange
        SetNormalRole();

        // Act
        var result = Helper_Privilege_Validation.HasReadAccess();

        // Assert
        result.Should().BeTrue("Normal users should have read access");
    }

    [Fact]
    public void HasReadAccess_ReadOnlyRole_ShouldReturnTrue()
    {
        // Arrange
        SetReadOnlyRole();

        // Act
        var result = Helper_Privilege_Validation.HasReadAccess();

        // Assert
        result.Should().BeTrue("ReadOnly users should have read access");
    }

    [Fact]
    public void HasReadAccess_NoRole_ShouldReturnFalse()
    {
        // Arrange
        ResetUserRoles();

        // Act
        var result = Helper_Privilege_Validation.HasReadAccess();

        // Assert
        result.Should().BeFalse("Users with no role should not have read access");
    }

    #endregion

    #region Write Access Tests

    [Fact]
    public void HasWriteAccess_AdminRole_AllTables_ShouldReturnTrue()
    {
        // Arrange
        SetAdminRole();

        // Act & Assert
        Helper_Privilege_Validation.HasWriteAccess("any_table").Should().BeTrue("Admin should have write access to all tables");
        Helper_Privilege_Validation.HasWriteAccess("inv_inventory").Should().BeTrue("Admin should have write access to inventory tables");
        Helper_Privilege_Validation.HasWriteAccess("usr_users").Should().BeTrue("Admin should have write access to user tables");
        Helper_Privilege_Validation.HasWriteAccess("sys_roles").Should().BeTrue("Admin should have write access to system tables");
    }

    [Fact]
    public void HasWriteAccess_NormalRole_InventoryTables_ShouldReturnTrue()
    {
        // Arrange
        SetNormalRole();

        // Act & Assert
        Helper_Privilege_Validation.HasWriteAccess("inv_inventory").Should().BeTrue("Normal users should have write access to inventory tables");
        Helper_Privilege_Validation.HasWriteAccess("inv_transaction").Should().BeTrue("Normal users should have write access to transaction tables");
        Helper_Privilege_Validation.HasWriteAccess("inventory").Should().BeTrue("Normal users should have write access to inventory tables");
        Helper_Privilege_Validation.HasWriteAccess("transaction").Should().BeTrue("Normal users should have write access to transaction tables");
    }

    [Fact]
    public void HasWriteAccess_NormalRole_NonInventoryTables_ShouldReturnFalse()
    {
        // Arrange
        SetNormalRole();

        // Act & Assert
        Helper_Privilege_Validation.HasWriteAccess("usr_users").Should().BeFalse("Normal users should not have write access to user tables");
        Helper_Privilege_Validation.HasWriteAccess("sys_roles").Should().BeFalse("Normal users should not have write access to system tables");
        Helper_Privilege_Validation.HasWriteAccess("random_table").Should().BeFalse("Normal users should not have write access to other tables");
    }

    [Fact]
    public void HasWriteAccess_ReadOnlyRole_AllTables_ShouldReturnFalse()
    {
        // Arrange
        SetReadOnlyRole();

        // Act & Assert
        Helper_Privilege_Validation.HasWriteAccess("inv_inventory").Should().BeFalse("ReadOnly users should not have write access to any tables");
        Helper_Privilege_Validation.HasWriteAccess("usr_users").Should().BeFalse("ReadOnly users should not have write access to any tables");
        Helper_Privilege_Validation.HasWriteAccess("any_table").Should().BeFalse("ReadOnly users should not have write access to any tables");
    }

    #endregion

    #region Admin Access Tests

    [Fact]
    public void HasAdminAccess_AdminRole_ShouldReturnTrue()
    {
        // Arrange
        SetAdminRole();

        // Act
        var result = Helper_Privilege_Validation.HasAdminAccess();

        // Assert
        result.Should().BeTrue("Admin users should have admin access");
    }

    [Fact]
    public void HasAdminAccess_NormalRole_ShouldReturnFalse()
    {
        // Arrange
        SetNormalRole();

        // Act
        var result = Helper_Privilege_Validation.HasAdminAccess();

        // Assert
        result.Should().BeFalse("Normal users should not have admin access");
    }

    [Fact]
    public void HasAdminAccess_ReadOnlyRole_ShouldReturnFalse()
    {
        // Arrange
        SetReadOnlyRole();

        // Act
        var result = Helper_Privilege_Validation.HasAdminAccess();

        // Assert
        result.Should().BeFalse("ReadOnly users should not have admin access");
    }

    #endregion

    #region Validation Exception Tests

    [Fact]
    public void ValidateWriteAccess_AdminRole_ShouldNotThrow()
    {
        // Arrange
        SetAdminRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateWriteAccess("any_table");
        act.Should().NotThrow("Admin users should be able to write to any table");
    }

    [Fact]
    public void ValidateWriteAccess_NormalRole_InventoryTable_ShouldNotThrow()
    {
        // Arrange
        SetNormalRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateWriteAccess("inv_inventory");
        act.Should().NotThrow("Normal users should be able to write to inventory tables");
    }

    [Fact]
    public void ValidateWriteAccess_NormalRole_UserTable_ShouldThrow()
    {
        // Arrange
        SetNormalRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateWriteAccess("usr_users");
        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("*Normal users cannot modify table: usr_users*");
    }

    [Fact]
    public void ValidateWriteAccess_ReadOnlyRole_AnyTable_ShouldThrow()
    {
        // Arrange
        SetReadOnlyRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateWriteAccess("inv_inventory");
        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("*Read-Only users cannot modify data*");
    }

    [Fact]
    public void ValidateAdminAccess_AdminRole_ShouldNotThrow()
    {
        // Arrange
        SetAdminRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateAdminAccess();
        act.Should().NotThrow("Admin users should have admin access");
    }

    [Fact]
    public void ValidateAdminAccess_NormalRole_ShouldThrow()
    {
        // Arrange
        SetNormalRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateAdminAccess();
        act.Should().Throw<UnauthorizedAccessException>()
            .WithMessage("*Administrative access required*");
    }

    #endregion

    #region Role Information Tests

    [Fact]
    public void GetCurrentUserRole_AdminRole_ShouldReturnAdmin()
    {
        // Arrange
        SetAdminRole();

        // Act
        var result = Helper_Privilege_Validation.GetCurrentUserRole();

        // Assert
        result.Should().Be("Admin");
    }

    [Fact]
    public void GetCurrentUserRole_NormalRole_ShouldReturnNormal()
    {
        // Arrange
        SetNormalRole();

        // Act
        var result = Helper_Privilege_Validation.GetCurrentUserRole();

        // Assert
        result.Should().Be("Normal");
    }

    [Fact]
    public void GetCurrentUserRole_ReadOnlyRole_ShouldReturnReadOnly()
    {
        // Arrange
        SetReadOnlyRole();

        // Act
        var result = Helper_Privilege_Validation.GetCurrentUserRole();

        // Assert
        result.Should().Be("ReadOnly");
    }

    [Fact]
    public void GetCurrentUserRole_NoRole_ShouldReturnUnknown()
    {
        // Arrange
        ResetUserRoles();

        // Act
        var result = Helper_Privilege_Validation.GetCurrentUserRole();

        // Assert
        result.Should().Be("Unknown");
    }

    [Fact]
    public void GetUserPermissionDescription_AdminRole_ShouldReturnFullAccessDescription()
    {
        // Arrange
        SetAdminRole();

        // Act
        var result = Helper_Privilege_Validation.GetUserPermissionDescription();

        // Assert
        result.Should().Contain("Full access");
        result.Should().Contain("Administrative functions");
    }

    [Fact]
    public void GetUserPermissionDescription_NormalRole_ShouldReturnLimitedAccessDescription()
    {
        // Arrange
        SetNormalRole();

        // Act
        var result = Helper_Privilege_Validation.GetUserPermissionDescription();

        // Assert
        result.Should().Contain("Limited access");
        result.Should().Contain("inventory/transaction");
    }

    [Fact]
    public void GetUserPermissionDescription_ReadOnlyRole_ShouldReturnReadOnlyDescription()
    {
        // Arrange
        SetReadOnlyRole();

        // Act
        var result = Helper_Privilege_Validation.GetUserPermissionDescription();

        // Assert
        result.Should().Contain("Read-only");
        result.Should().Contain("Search and view only");
    }

    #endregion

    #region Specific Operation Tests

    [Fact]
    public void ValidateUserManagementAccess_AdminRole_ShouldNotThrow()
    {
        // Arrange
        SetAdminRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateUserManagementAccess();
        act.Should().NotThrow("Admin users should have user management access");
    }

    [Fact]
    public void ValidateUserManagementAccess_NormalRole_ShouldThrow()
    {
        // Arrange
        SetNormalRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateUserManagementAccess();
        act.Should().Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void ValidateInventoryWriteAccess_NormalRole_ShouldNotThrow()
    {
        // Arrange
        SetNormalRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateInventoryWriteAccess();
        act.Should().NotThrow("Normal users should have inventory write access");
    }

    [Fact]
    public void ValidateInventoryWriteAccess_ReadOnlyRole_ShouldThrow()
    {
        // Arrange
        SetReadOnlyRole();

        // Act & Assert
        var act = () => Helper_Privilege_Validation.ValidateInventoryWriteAccess();
        act.Should().Throw<UnauthorizedAccessException>();
    }

    #endregion
}