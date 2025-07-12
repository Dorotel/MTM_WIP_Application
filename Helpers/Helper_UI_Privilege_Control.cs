using MTM_Inventory_Application.Models;
using System.Drawing;
using System.Windows.Forms;

namespace MTM_Inventory_Application.Helpers;

#region Helper_UI_Privilege_Control

/// <summary>
/// Helper class for managing UI control states based on user privileges.
/// Implements the UI layer privilege enforcement per the privilege matrix.
/// </summary>
internal static class Helper_UI_Privilege_Control
{
    #region Control State Management

    /// <summary>
    /// Sets the enabled state of controls based on current user privileges.
    /// </summary>
    /// <param name="control">The control to configure</param>
    /// <param name="requiredPrivilege">The privilege level required</param>
    public static void SetControlByPrivilege(Control control, PrivilegeLevel requiredPrivilege)
    {
        switch (requiredPrivilege)
        {
            case PrivilegeLevel.ReadOnly:
                // All users have read access
                control.Enabled = Helper_Privilege_Validation.HasReadAccess();
                break;
                
            case PrivilegeLevel.InventoryWrite:
                // Admin and Normal users can write to inventory
                control.Enabled = Model_AppVariables.UserTypeAdmin || Model_AppVariables.UserTypeNormal;
                break;
                
            case PrivilegeLevel.AdminOnly:
                // Only Admin users have full access
                control.Enabled = Model_AppVariables.UserTypeAdmin;
                break;
                
            case PrivilegeLevel.SearchOnly:
                // All users can search
                control.Enabled = true;
                break;
                
            default:
                control.Enabled = false;
                break;
        }
    }

    /// <summary>
    /// Configures multiple controls based on user privilege level.
    /// </summary>
    /// <param name="controls">Dictionary of controls and their required privilege levels</param>
    public static void ConfigureControlsByPrivilege(Dictionary<Control, PrivilegeLevel> controls)
    {
        foreach (var kvp in controls)
        {
            SetControlByPrivilege(kvp.Key, kvp.Value);
        }
    }

    /// <summary>
    /// Disables all controls in a container and shows access denied message.
    /// </summary>
    /// <param name="container">The container control (Panel, Form, etc.)</param>
    /// <param name="message">The access denied message to display</param>
    public static void ShowAccessDeniedMessage(Control container, string message = "Access denied. Insufficient privileges.")
    {
        // Disable all child controls
        foreach (Control control in container.Controls)
        {
            control.Enabled = false;
        }

        // Create and add access denied label
        var accessLabel = new Label
        {
            Text = message,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.Red,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.LightYellow,
            BorderStyle = BorderStyle.FixedSingle
        };

        container.Controls.Add(accessLabel);
        accessLabel.BringToFront();
    }

    #endregion

    #region Role-Based UI Configuration

    /// <summary>
    /// Configures UI controls based on the current user's role.
    /// Implements the privilege matrix from the comprehensive checklist.
    /// </summary>
    /// <param name="form">The form to configure</param>
    public static void ConfigureUIForCurrentRole(Form form)
    {
        if (Model_AppVariables.UserTypeAdmin)
        {
            ConfigureUIForAdmin(form);
        }
        else if (Model_AppVariables.UserTypeNormal)
        {
            ConfigureUIForNormal(form);
        }
        else if (Model_AppVariables.UserTypeReadOnly)
        {
            ConfigureUIForReadOnly(form);
        }
        else
        {
            // No role configured - deny access
            ShowAccessDeniedMessage(form, "User role not configured. Contact system administrator.");
        }
    }

    /// <summary>
    /// Configures UI for Admin users - full access to all features.
    /// </summary>
    private static void ConfigureUIForAdmin(Form form)
    {
        // Admin users have access to all controls
        EnableAllControls(form);
        
        // Add visual indicator for admin access
        SetFormTitle(form, $"{form.Text} - Admin Access");
    }

    /// <summary>
    /// Configures UI for Normal users - limited write access.
    /// </summary>
    private static void ConfigureUIForNormal(Form form)
    {
        // Enable inventory and transaction controls
        EnableInventoryControls(form);
        
        // Disable administrative controls
        DisableAdministrativeControls(form);
        
        // Add visual indicator for normal access
        SetFormTitle(form, $"{form.Text} - Normal Access");
    }

    /// <summary>
    /// Configures UI for ReadOnly users - search and view only.
    /// </summary>
    private static void ConfigureUIForReadOnly(Form form)
    {
        // Enable only search and view controls
        EnableSearchControls(form);
        
        // Disable all write and administrative controls
        DisableWriteControls(form);
        DisableAdministrativeControls(form);
        
        // Add visual indicator for read-only access
        SetFormTitle(form, $"{form.Text} - Read Only Access");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Enables all controls in the form.
    /// </summary>
    private static void EnableAllControls(Control container)
    {
        foreach (Control control in container.Controls)
        {
            control.Enabled = true;
            
            // Recursively enable child controls
            if (control.HasChildren)
            {
                EnableAllControls(control);
            }
        }
    }

    /// <summary>
    /// Enables controls related to inventory operations.
    /// </summary>
    private static void EnableInventoryControls(Control container)
    {
        var inventoryControlNames = new[]
        {
            "inventory", "transaction", "add", "edit", "update", "remove", "transfer"
        };

        foreach (Control control in container.Controls)
        {
            var controlName = control.Name.ToLowerInvariant();
            control.Enabled = inventoryControlNames.Any(name => controlName.Contains(name));
            
            // Recursively check child controls
            if (control.HasChildren)
            {
                EnableInventoryControls(control);
            }
        }
    }

    /// <summary>
    /// Enables only search and view controls.
    /// </summary>
    private static void EnableSearchControls(Control container)
    {
        var searchControlNames = new[]
        {
            "search", "view", "display", "show", "filter", "query", "report"
        };

        foreach (Control control in container.Controls)
        {
            var controlName = control.Name.ToLowerInvariant();
            control.Enabled = searchControlNames.Any(name => controlName.Contains(name)) ||
                             control is DataGridView || control is ListView ||
                             control is TextBox && control.Name.ToLowerInvariant().Contains("search");
            
            // Recursively check child controls
            if (control.HasChildren)
            {
                EnableSearchControls(control);
            }
        }
    }

    /// <summary>
    /// Disables administrative controls.
    /// </summary>
    private static void DisableAdministrativeControls(Control container)
    {
        var adminControlNames = new[]
        {
            "admin", "user", "role", "setting", "config", "manage", "system"
        };

        foreach (Control control in container.Controls)
        {
            var controlName = control.Name.ToLowerInvariant();
            if (adminControlNames.Any(name => controlName.Contains(name)))
            {
                control.Enabled = false;
            }
            
            // Recursively check child controls
            if (control.HasChildren)
            {
                DisableAdministrativeControls(control);
            }
        }
    }

    /// <summary>
    /// Disables all write-related controls.
    /// </summary>
    private static void DisableWriteControls(Control container)
    {
        var writeControlNames = new[]
        {
            "add", "edit", "update", "delete", "remove", "save", "create", "modify"
        };

        foreach (Control control in container.Controls)
        {
            var controlName = control.Name.ToLowerInvariant();
            if (writeControlNames.Any(name => controlName.Contains(name)) ||
                control is Button && writeControlNames.Any(name => control.Text.ToLowerInvariant().Contains(name)))
            {
                control.Enabled = false;
            }
            
            // Recursively check child controls
            if (control.HasChildren)
            {
                DisableWriteControls(control);
            }
        }
    }

    /// <summary>
    /// Sets the form title with role information.
    /// </summary>
    private static void SetFormTitle(Form form, string title)
    {
        if (form != null)
        {
            form.Text = title;
        }
    }

    #endregion
}

/// <summary>
/// Enumeration of privilege levels used for UI control configuration.
/// </summary>
public enum PrivilegeLevel
{
    ReadOnly,
    InventoryWrite,
    AdminOnly,
    SearchOnly
}

#endregion