using FluentAssertions;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using System.Windows.Forms;
using Xunit;

namespace MTM_WIP_Application.UnitTests.Helpers;

/// <summary>
/// Unit tests for Helper_UI_Privilege_Control class.
/// Tests UI control privilege enforcement.
/// </summary>
public class Helper_UI_Privilege_ControlTests
{
    #region Test Setup

    public Helper_UI_Privilege_ControlTests()
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

    #region SetControlByPrivilege Tests

    [Fact]
    public void SetControlByPrivilege_ReadOnlyAccess_AdminUser_ShouldEnable()
    {
        // Arrange
        SetAdminRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.ReadOnly);

        // Assert
        button.Enabled.Should().BeTrue("Admin users should have read access");
    }

    [Fact]
    public void SetControlByPrivilege_ReadOnlyAccess_NormalUser_ShouldEnable()
    {
        // Arrange
        SetNormalRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.ReadOnly);

        // Assert
        button.Enabled.Should().BeTrue("Normal users should have read access");
    }

    [Fact]
    public void SetControlByPrivilege_ReadOnlyAccess_ReadOnlyUser_ShouldEnable()
    {
        // Arrange
        SetReadOnlyRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.ReadOnly);

        // Assert
        button.Enabled.Should().BeTrue("ReadOnly users should have read access");
    }

    [Fact]
    public void SetControlByPrivilege_InventoryWrite_AdminUser_ShouldEnable()
    {
        // Arrange
        SetAdminRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.InventoryWrite);

        // Assert
        button.Enabled.Should().BeTrue("Admin users should have inventory write access");
    }

    [Fact]
    public void SetControlByPrivilege_InventoryWrite_NormalUser_ShouldEnable()
    {
        // Arrange
        SetNormalRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.InventoryWrite);

        // Assert
        button.Enabled.Should().BeTrue("Normal users should have inventory write access");
    }

    [Fact]
    public void SetControlByPrivilege_InventoryWrite_ReadOnlyUser_ShouldDisable()
    {
        // Arrange
        SetReadOnlyRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.InventoryWrite);

        // Assert
        button.Enabled.Should().BeFalse("ReadOnly users should not have inventory write access");
    }

    [Fact]
    public void SetControlByPrivilege_AdminOnly_AdminUser_ShouldEnable()
    {
        // Arrange
        SetAdminRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.AdminOnly);

        // Assert
        button.Enabled.Should().BeTrue("Admin users should have admin access");
    }

    [Fact]
    public void SetControlByPrivilege_AdminOnly_NormalUser_ShouldDisable()
    {
        // Arrange
        SetNormalRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.AdminOnly);

        // Assert
        button.Enabled.Should().BeFalse("Normal users should not have admin access");
    }

    [Fact]
    public void SetControlByPrivilege_AdminOnly_ReadOnlyUser_ShouldDisable()
    {
        // Arrange
        SetReadOnlyRole();
        var button = new Button();

        // Act
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.AdminOnly);

        // Assert
        button.Enabled.Should().BeFalse("ReadOnly users should not have admin access");
    }

    [Fact]
    public void SetControlByPrivilege_SearchOnly_AllUsers_ShouldEnable()
    {
        // Arrange & Act & Assert
        var button = new Button();
        
        SetAdminRole();
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.SearchOnly);
        button.Enabled.Should().BeTrue("Admin users should have search access");
        
        SetNormalRole();
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.SearchOnly);
        button.Enabled.Should().BeTrue("Normal users should have search access");
        
        SetReadOnlyRole();
        Helper_UI_Privilege_Control.SetControlByPrivilege(button, PrivilegeLevel.SearchOnly);
        button.Enabled.Should().BeTrue("ReadOnly users should have search access");
    }

    #endregion

    #region ConfigureControlsByPrivilege Tests

    [Fact]
    public void ConfigureControlsByPrivilege_MultipleControls_ShouldSetCorrectly()
    {
        // Arrange
        SetNormalRole();
        var adminButton = new Button();
        var inventoryButton = new Button();
        var searchButton = new Button();
        
        var controls = new Dictionary<Control, PrivilegeLevel>
        {
            { adminButton, PrivilegeLevel.AdminOnly },
            { inventoryButton, PrivilegeLevel.InventoryWrite },
            { searchButton, PrivilegeLevel.SearchOnly }
        };

        // Act
        Helper_UI_Privilege_Control.ConfigureControlsByPrivilege(controls);

        // Assert
        adminButton.Enabled.Should().BeFalse("Normal users should not have admin access");
        inventoryButton.Enabled.Should().BeTrue("Normal users should have inventory write access");
        searchButton.Enabled.Should().BeTrue("Normal users should have search access");
    }

    #endregion

    #region ShowAccessDeniedMessage Tests

    [Fact]
    public void ShowAccessDeniedMessage_ShouldDisableChildControls()
    {
        // Arrange
        var panel = new Panel();
        var button1 = new Button();
        var button2 = new Button();
        panel.Controls.Add(button1);
        panel.Controls.Add(button2);

        // Act
        Helper_UI_Privilege_Control.ShowAccessDeniedMessage(panel);

        // Assert
        button1.Enabled.Should().BeFalse("Child controls should be disabled");
        button2.Enabled.Should().BeFalse("Child controls should be disabled");
    }

    [Fact]
    public void ShowAccessDeniedMessage_ShouldAddAccessDeniedLabel()
    {
        // Arrange
        var panel = new Panel();

        // Act
        Helper_UI_Privilege_Control.ShowAccessDeniedMessage(panel, "Test access denied message");

        // Assert
        var accessLabel = panel.Controls.OfType<Label>().FirstOrDefault();
        accessLabel.Should().NotBeNull("Access denied label should be added");
        accessLabel!.Text.Should().Be("Test access denied message");
    }

    #endregion

    #region ConfigureUIForCurrentRole Tests

    [Fact]
    public void ConfigureUIForCurrentRole_NoRoleSet_ShouldShowAccessDenied()
    {
        // Arrange
        ResetUserRoles();
        var form = new Form();
        var button = new Button();
        form.Controls.Add(button);

        // Act
        Helper_UI_Privilege_Control.ConfigureUIForCurrentRole(form);

        // Assert
        button.Enabled.Should().BeFalse("Controls should be disabled when no role is set");
        
        var accessLabel = form.Controls.OfType<Label>().FirstOrDefault();
        accessLabel.Should().NotBeNull("Access denied label should be added");
        accessLabel!.Text.Should().Contain("role not configured");
    }

    [Fact]
    public void ConfigureUIForCurrentRole_AdminRole_ShouldModifyFormTitle()
    {
        // Arrange
        SetAdminRole();
        var form = new Form { Text = "Test Form" };

        // Act
        Helper_UI_Privilege_Control.ConfigureUIForCurrentRole(form);

        // Assert
        form.Text.Should().Contain("Admin Access");
    }

    [Fact]
    public void ConfigureUIForCurrentRole_NormalRole_ShouldModifyFormTitle()
    {
        // Arrange
        SetNormalRole();
        var form = new Form { Text = "Test Form" };

        // Act
        Helper_UI_Privilege_Control.ConfigureUIForCurrentRole(form);

        // Assert
        form.Text.Should().Contain("Normal Access");
    }

    [Fact]
    public void ConfigureUIForCurrentRole_ReadOnlyRole_ShouldModifyFormTitle()
    {
        // Arrange
        SetReadOnlyRole();
        var form = new Form { Text = "Test Form" };

        // Act
        Helper_UI_Privilege_Control.ConfigureUIForCurrentRole(form);

        // Assert
        form.Text.Should().Contain("Read Only Access");
    }

    #endregion
}