using FluentAssertions;
using MTM_Inventory_Application.Models;
using Xunit;

namespace MTM_WIP_Application.UnitTests.Models;

/// <summary>
/// Unit tests for Model_AppVariables class.
/// Tests the user role privilege system and application variables.
/// </summary>
public class Model_AppVariablesTests
{
    #region User Role Tests

    [Fact]
    public void UserRole_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var userTypeAdmin = Model_AppVariables.UserTypeAdmin;
        var userTypeReadOnly = Model_AppVariables.UserTypeReadOnly;
        var userTypeNormal = Model_AppVariables.UserTypeNormal;

        // Assert
        userTypeAdmin.Should().BeFalse("Admin should be false by default");
        userTypeReadOnly.Should().BeFalse("ReadOnly should be false by default");
        userTypeNormal.Should().BeTrue("Normal should be true by default");
    }

    [Fact]
    public void UserRole_AdminRole_ShouldHaveFullAccess()
    {
        // Arrange
        Model_AppVariables.UserTypeAdmin = true;
        Model_AppVariables.UserTypeReadOnly = false;
        Model_AppVariables.UserTypeNormal = false;

        // Act & Assert
        Model_AppVariables.UserTypeAdmin.Should().BeTrue("Admin should have full access");
        Model_AppVariables.UserTypeReadOnly.Should().BeFalse("Admin should not be ReadOnly");
        Model_AppVariables.UserTypeNormal.Should().BeFalse("Admin should not be Normal");
    }

    [Fact]
    public void UserRole_ReadOnlyRole_ShouldHaveReadOnlyAccess()
    {
        // Arrange
        Model_AppVariables.UserTypeAdmin = false;
        Model_AppVariables.UserTypeReadOnly = true;
        Model_AppVariables.UserTypeNormal = false;

        // Act & Assert
        Model_AppVariables.UserTypeAdmin.Should().BeFalse("ReadOnly should not be Admin");
        Model_AppVariables.UserTypeReadOnly.Should().BeTrue("ReadOnly should have read-only access");
        Model_AppVariables.UserTypeNormal.Should().BeFalse("ReadOnly should not be Normal");
    }

    [Fact]
    public void UserRole_NormalRole_ShouldHaveLimitedAccess()
    {
        // Arrange
        Model_AppVariables.UserTypeAdmin = false;
        Model_AppVariables.UserTypeReadOnly = false;
        Model_AppVariables.UserTypeNormal = true;

        // Act & Assert
        Model_AppVariables.UserTypeAdmin.Should().BeFalse("Normal should not be Admin");
        Model_AppVariables.UserTypeReadOnly.Should().BeFalse("Normal should not be ReadOnly");
        Model_AppVariables.UserTypeNormal.Should().BeTrue("Normal should have limited access");
    }

    #endregion

    #region User Information Tests

    [Fact]
    public void EnteredUser_DefaultValue_ShouldBeDefaultUser()
    {
        // Act & Assert
        Model_AppVariables.EnteredUser.Should().Be("Default User");
    }

    [Fact]
    public void User_ShouldNotBeNullOrEmpty()
    {
        // Act & Assert
        Model_AppVariables.User.Should().NotBeNullOrEmpty("User should always have a value");
    }

    [Fact]
    public void UserVersion_ShouldNotBeNullOrEmpty()
    {
        // Act & Assert
        Model_AppVariables.UserVersion.Should().NotBeNullOrEmpty("UserVersion should always have a value");
    }

    #endregion

    #region Theme Tests

    [Fact]
    public void Theme_DefaultValues_ShouldBeCorrect()
    {
        // Act & Assert
        Model_AppVariables.ThemeName.Should().Be("Default");
        Model_AppVariables.ThemeFontSize.Should().Be(9f);
    }

    [Fact]
    public void UserUiColors_ShouldNotBeNull()
    {
        // Act & Assert
        Model_AppVariables.UserUiColors.Should().NotBeNull("UserUiColors should always be initialized");
    }

    #endregion

    #region Connection String Tests

    [Fact]
    public void ConnectionString_ShouldNotBeNullOrEmpty()
    {
        // Act & Assert
        Model_AppVariables.ConnectionString.Should().NotBeNullOrEmpty("ConnectionString should always have a value");
    }

    #endregion

    #region Privilege Matrix Validation Tests

    [Theory]
    [InlineData(true, false, false, "Admin")]
    [InlineData(false, true, false, "ReadOnly")]
    [InlineData(false, false, true, "Normal")]
    public void UserRole_OnlyOneRoleShouldBeActiveAtATime(bool admin, bool readOnly, bool normal, string expectedRole)
    {
        // Arrange
        Model_AppVariables.UserTypeAdmin = admin;
        Model_AppVariables.UserTypeReadOnly = readOnly;
        Model_AppVariables.UserTypeNormal = normal;

        // Act & Assert
        var activeRoles = new List<string>();
        if (Model_AppVariables.UserTypeAdmin) activeRoles.Add("Admin");
        if (Model_AppVariables.UserTypeReadOnly) activeRoles.Add("ReadOnly");
        if (Model_AppVariables.UserTypeNormal) activeRoles.Add("Normal");

        activeRoles.Should().HaveCount(1, "Only one role should be active at a time");
        activeRoles.First().Should().Be(expectedRole);
    }

    #endregion
}