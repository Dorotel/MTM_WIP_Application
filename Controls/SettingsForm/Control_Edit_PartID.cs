using System.Data;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Edit_PartID : UserControl
    {
        #region Fields

        private DataRow? _currentPart;

        #endregion

        #region Properties

        // Public properties would go here if needed

        #endregion

        #region Progress Control Methods

        // Progress control methods would go here if needed for this control

        #endregion

        #region Constructors

        public Control_Edit_PartID()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["ControlType"] = nameof(Control_Edit_PartID),
                ["InitializationTime"] = DateTime.Now
            }, nameof(Control_Edit_PartID), nameof(Control_Edit_PartID));

            try
            {
                Service_DebugTracer.TraceUIAction("CONTROL_INITIALIZATION", nameof(Control_Edit_PartID),
                    new Dictionary<string, object>
                    {
                        ["Phase"] = "START",
                        ["ComponentType"] = "UserControl"
                    });

                InitializeComponent();
                
                Service_DebugTracer.TraceUIAction("EVENT_HANDLERS_BINDING", nameof(Control_Edit_PartID),
                    new Dictionary<string, object>
                    {
                        ["Handlers"] = new[] { "SelectedIndexChanged", "Click_Save", "Click_Cancel" }
                    });

                Control_Edit_PartID_ComboBox_Part.SelectedIndexChanged +=
                    Control_Edit_PartID_ComboBox_Part_SelectedIndexChanged;
                saveButton.Click += SaveButton_Click;
                cancelButton.Click += CancelButton_Click;
                
                Service_DebugTracer.TraceUIAction("LOADING_PART_TYPES", nameof(Control_Edit_PartID));
                LoadPartTypes();

                Service_DebugTracer.TraceUIAction("CONTROL_INITIALIZATION", nameof(Control_Edit_PartID),
                    new Dictionary<string, object>
                    {
                        ["Phase"] = "COMPLETE",
                        ["Status"] = "SUCCESS"
                    });
            }
            catch (Exception ex)
            {
                Service_DebugTracer.TraceBusinessLogic("CONTROL_INITIALIZATION_ERROR", 
                    inputData: new Dictionary<string, object> { ["ControlType"] = nameof(Control_Edit_PartID) },
                    validationResults: new Dictionary<string, object>
                    {
                        ["Exception"] = ex.GetType().Name,
                        ["Message"] = ex.Message,
                        ["InnerException"] = ex.InnerException?.Message ?? "None"
                    });

                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, 
                    controlName: nameof(Control_Edit_PartID));
            }
            
            Service_DebugTracer.TraceMethodExit(new Dictionary<string, object>
            {
                ["InitializationSuccess"] = true
            }, nameof(Control_Edit_PartID), nameof(Control_Edit_PartID));
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Event for notifying when a part has been updated
        /// </summary>
        public event EventHandler? PartUpdated;

        private async void LoadPartTypes()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillItemTypeComboBoxesAsync(Control_Edit_PartID_ComboBox_ItemType);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(Control_Edit_PartID));
            }
        }

        private async void LoadParts()
        {
            try
            {
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_Edit_PartID_ComboBox_Part);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
                    controlName: nameof(Control_Edit_PartID), callerName: nameof(LoadParts));
            }
        }

        protected override async void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                if (issuedByValueLabel != null)
                {
                    issuedByValueLabel.Text = Model_AppVariables.User ?? "Current User";
                }

                LoadParts();
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.High, 
                    controlName: nameof(Control_Edit_PartID), callerName: nameof(OnLoad));
            }
        }

        #endregion

        #region Key Processing

        // Keyboard shortcut processing would go here if needed
        // Currently not implemented for this settings control

        #endregion

        #region Button Clicks

        // Button click event handlers will be moved here

        #endregion

        #region ComboBox & UI Events

        private async void Control_Edit_PartID_ComboBox_Part_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Control_Edit_PartID_ComboBox_Part.SelectedIndex <= 0)
            {
                ClearForm();
                SetFormEnabled(false);
                return;
            }

            try
            {
                string? selectedText = Control_Edit_PartID_ComboBox_Part.Text;
                if (string.IsNullOrEmpty(selectedText))
                {
                    Service_ErrorHandler.HandleValidationError(
                        "Please select a valid part from the dropdown list.",
                        "Part Selection", controlName: nameof(Control_Edit_PartID));
                    return;
                }

                _currentPart = await Dao_Part.GetPartByNumber(selectedText);
                if (_currentPart != null)
                {
                    LoadPartData();
                    SetFormEnabled(true);
                }
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleDatabaseError(ex, controlName: nameof(Control_Edit_PartID));
            }
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            var performanceKey = Service_DebugTracer.StartPerformanceTrace("SAVE_PART_OPERATION");
            
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["HasCurrentPart"] = _currentPart != null,
                ["OriginalPartID"] = _currentPart?["PartID"]?.ToString() ?? "NULL"
            }, nameof(SaveButton_Click), nameof(Control_Edit_PartID));

            Service_DebugTracer.TraceUIAction("SAVE_BUTTON_CLICKED", nameof(Control_Edit_PartID),
                new Dictionary<string, object>
                {
                    ["UserAction"] = "SAVE_PART_DATA",
                    ["FormState"] = _currentPart != null ? "EDITING" : "NO_SELECTION"
                });

            if (_currentPart == null)
            {
                Service_DebugTracer.TraceBusinessLogic("SAVE_VALIDATION_FAILED",
                    validationResults: new Dictionary<string, object>
                    {
                        ["Issue"] = "NO_CURRENT_PART",
                        ["Action"] = "ABORT_SAVE"
                    });

                Service_DebugTracer.TraceMethodExit("ABORTED - No current part", nameof(SaveButton_Click), nameof(Control_Edit_PartID));
                Service_DebugTracer.StopPerformanceTrace(performanceKey, new Dictionary<string, object> { ["Result"] = "ABORTED" });
                return;
            }

            try
            {
                // Collect all form data for debugging
                var formData = new Dictionary<string, object>
                {
                    ["ItemNumber"] = itemNumberTextBox.Text?.Trim() ?? "",
                    ["Customer"] = customerTextBox.Text?.Trim() ?? "",
                    ["Description"] = descriptionTextBox.Text?.Trim() ?? "",
                    ["ItemTypeIndex"] = Control_Edit_PartID_ComboBox_ItemType.SelectedIndex,
                    ["ItemTypeText"] = Control_Edit_PartID_ComboBox_ItemType.Text ?? ""
                };

                Service_DebugTracer.TraceBusinessLogic("FORM_DATA_COLLECTION", 
                    inputData: formData,
                    businessRules: new Dictionary<string, object>
                    {
                        ["ItemNumberRequired"] = true,
                        ["DescriptionRequired"] = true,
                        ["ItemTypeRequired"] = true,
                        ["CustomerOptional"] = true
                    });

                // Validation: Item Number
                if (string.IsNullOrWhiteSpace(itemNumberTextBox.Text))
                {
                    Service_DebugTracer.TraceDataValidation("ITEM_NUMBER_VALIDATION",
                        dataToValidate: itemNumberTextBox.Text,
                        validationRules: new Dictionary<string, object> { ["Required"] = true },
                        isValid: false,
                        errorMessages: new List<string> { "Item Number is required" });

                    Service_ErrorHandler.HandleValidationError(
                        "Item Number is required to save the part.",
                        "Item Number", controlName: nameof(Control_Edit_PartID));
                    itemNumberTextBox.Focus();
                    
                    Service_DebugTracer.StopPerformanceTrace(performanceKey, new Dictionary<string, object> { ["Result"] = "VALIDATION_FAILED", ["Field"] = "ItemNumber" });
                    return;
                }

                // Auto-correct customer field
                if (string.IsNullOrWhiteSpace(customerTextBox.Text))
                {
                    Service_DebugTracer.TraceBusinessLogic("AUTO_CORRECT_CUSTOMER",
                        inputData: new Dictionary<string, object> { ["OriginalValue"] = customerTextBox.Text ?? "" },
                        outputData: new Dictionary<string, object> { ["CorrectedValue"] = "[ No Customer ]" },
                        businessRules: new Dictionary<string, object> { ["Rule"] = "Empty customer field gets default value" });

                    customerTextBox.Text = "[ No Customer ]";
                    return;
                }

                // Validation: Description
                if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
                {
                    Service_DebugTracer.TraceDataValidation("DESCRIPTION_VALIDATION",
                        dataToValidate: descriptionTextBox.Text,
                        validationRules: new Dictionary<string, object> { ["Required"] = true },
                        isValid: false,
                        errorMessages: new List<string> { "Description is required" });

                    Service_ErrorHandler.HandleValidationError(
                        "Description is required to save the part.",
                        "Description", controlName: nameof(Control_Edit_PartID));
                    descriptionTextBox.Focus();
                    
                    Service_DebugTracer.StopPerformanceTrace(performanceKey, new Dictionary<string, object> { ["Result"] = "VALIDATION_FAILED", ["Field"] = "Description" });
                    return;
                }

                // Validation: Item Type
                if (Control_Edit_PartID_ComboBox_ItemType.SelectedIndex <= 0)
                {
                    Service_DebugTracer.TraceDataValidation("ITEM_TYPE_VALIDATION",
                        dataToValidate: new Dictionary<string, object>
                        {
                            ["SelectedIndex"] = Control_Edit_PartID_ComboBox_ItemType.SelectedIndex,
                            ["SelectedText"] = Control_Edit_PartID_ComboBox_ItemType.Text ?? ""
                        },
                        validationRules: new Dictionary<string, object> { ["MinIndex"] = 1, ["Required"] = true },
                        isValid: false,
                        errorMessages: new List<string> { "Item type selection required" });

                    Service_ErrorHandler.HandleValidationError(
                        "Please select a part type from the dropdown list.",
                        "Part Type", controlName: nameof(Control_Edit_PartID));
                    Control_Edit_PartID_ComboBox_ItemType.Focus();
                    
                    Service_DebugTracer.StopPerformanceTrace(performanceKey, new Dictionary<string, object> { ["Result"] = "VALIDATION_FAILED", ["Field"] = "ItemType" });
                    return;
                }

                // Check for duplicate part number
                string? originalItemNumber = _currentPart["PartID"].ToString();
                string newItemNumber = itemNumberTextBox.Text.Trim();
                
                Service_DebugTracer.TraceBusinessLogic("DUPLICATE_CHECK_LOGIC",
                    inputData: new Dictionary<string, object>
                    {
                        ["OriginalItemNumber"] = originalItemNumber ?? "NULL",
                        ["NewItemNumber"] = newItemNumber,
                        ["NumberChanged"] = originalItemNumber != newItemNumber
                    });

                if (originalItemNumber != newItemNumber && await Dao_Part.PartExists(newItemNumber))
                {
                    Service_DebugTracer.TraceDataValidation("DUPLICATE_PART_CHECK",
                        dataToValidate: new Dictionary<string, object>
                        {
                            ["NewPartNumber"] = newItemNumber,
                            ["OriginalPartNumber"] = originalItemNumber ?? "NULL"
                        },
                        validationRules: new Dictionary<string, object> { ["MustBeUnique"] = true },
                        isValid: false,
                        errorMessages: new List<string> { $"Part number '{newItemNumber}' already exists" });

                    Service_ErrorHandler.HandleValidationError(
                        $"Part number '{newItemNumber}' already exists. Please use a different part number.",
                        "Duplicate Part Number", controlName: nameof(Control_Edit_PartID));
                    itemNumberTextBox.Focus();
                    
                    Service_DebugTracer.StopPerformanceTrace(performanceKey, new Dictionary<string, object> { ["Result"] = "VALIDATION_FAILED", ["Field"] = "DuplicateCheck" });
                    return;
                }

                Service_DebugTracer.TraceBusinessLogic("ALL_VALIDATIONS_PASSED",
                    inputData: formData,
                    validationResults: new Dictionary<string, object>
                    {
                        ["ItemNumberValid"] = true,
                        ["DescriptionValid"] = true,
                        ["ItemTypeValid"] = true,
                        ["NoDuplicates"] = true,
                        ["ReadyForUpdate"] = true
                    });

                Service_DebugTracer.TraceUIAction("INITIATING_DATABASE_UPDATE", nameof(Control_Edit_PartID),
                    new Dictionary<string, object>
                    {
                        ["UpdateType"] = "PART_DATA_UPDATE",
                        ["PartChanged"] = originalItemNumber != newItemNumber
                    });

                await UpdatePartAsync();
                
                Service_DebugTracer.TraceBusinessLogic("PART_UPDATE_COMPLETE",
                    outputData: new Dictionary<string, object>
                    {
                        ["UpdateSuccess"] = true,
                        ["UpdatedPartID"] = newItemNumber,
                        ["DatabaseOperationComplete"] = true
                    });

                Service_DebugTracer.TraceUIAction("SHOWING_SUCCESS_MESSAGE", nameof(Control_Edit_PartID),
                    new Dictionary<string, object>
                    {
                        ["MessageType"] = "SUCCESS_INFORMATION",
                        ["UserNotification"] = "Part updated successfully"
                    });

                Service_ErrorHandler.ShowInformation(
                    "Part has been updated successfully!",
                    "Update Complete", controlName: nameof(Control_Edit_PartID));
                
                Service_DebugTracer.TraceUIAction("REFRESHING_DATA", nameof(Control_Edit_PartID),
                    new Dictionary<string, object>
                    {
                        ["Action"] = "RELOAD_PARTS_LIST",
                        ["TriggerEvent"] = "PartUpdated"
                    });

                LoadParts();
                PartUpdated?.Invoke(this, EventArgs.Empty);

                var finalElapsed = Service_DebugTracer.StopPerformanceTrace(performanceKey, new Dictionary<string, object> 
                { 
                    ["Result"] = "SUCCESS",
                    ["PartUpdated"] = newItemNumber
                });

                Service_DebugTracer.TraceMethodExit(new Dictionary<string, object>
                {
                    ["Success"] = true,
                    ["ElapsedMs"] = finalElapsed,
                    ["UpdatedPartID"] = newItemNumber
                }, nameof(SaveButton_Click), nameof(Control_Edit_PartID));
            }
            catch (Exception ex)
            {
                Service_DebugTracer.TraceBusinessLogic("SAVE_OPERATION_ERROR",
                    validationResults: new Dictionary<string, object>
                    {
                        ["ExceptionType"] = ex.GetType().Name,
                        ["ExceptionMessage"] = ex.Message,
                        ["StackTracePreview"] = ex.StackTrace?.Substring(0, Math.Min(ex.StackTrace.Length, 300)) ?? "No stack trace",
                        ["DatabaseError"] = ex.Message.ToLower().Contains("mysql") || ex.Message.ToLower().Contains("database")
                    });

                Service_ErrorHandler.HandleDatabaseError(ex, controlName: nameof(Control_Edit_PartID));
                
                Service_DebugTracer.StopPerformanceTrace(performanceKey, new Dictionary<string, object> 
                { 
                    ["Result"] = "EXCEPTION",
                    ["ExceptionType"] = ex.GetType().Name
                });

                Service_DebugTracer.TraceMethodExit(new Dictionary<string, object>
                {
                    ["Success"] = false,
                    ["Exception"] = ex.GetType().Name
                }, nameof(SaveButton_Click), nameof(Control_Edit_PartID));
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (_currentPart != null)
            {
                LoadPartData();
            }
            else
            {
                ClearForm();
            }
        }

        #endregion

        #region Methods

        private void LoadPartData()
        {
            if (_currentPart == null)
            {
                return;
            }

            itemNumberTextBox.Text = _currentPart["PartID"].ToString();
            customerTextBox.Text = _currentPart["Customer"].ToString();
            descriptionTextBox.Text = _currentPart["Description"].ToString();
            string? partType = _currentPart["ItemType"].ToString();

            int index = -1;
            for (int i = 0; i < Control_Edit_PartID_ComboBox_ItemType.Items.Count; i++)
            {
                if (Control_Edit_PartID_ComboBox_ItemType.GetItemText(Control_Edit_PartID_ComboBox_ItemType.Items[i]) ==
                    partType)
                {
                    index = i;
                    break;
                }
            }

            Control_Edit_PartID_ComboBox_ItemType.SelectedIndex = index > 0 ? index : 0;

            issuedByValueLabel.Text = _currentPart["IssuedBy"].ToString();
        }

        #endregion

        #region Helpers

        private void SetFormEnabled(bool enabled)
        {
            itemNumberTextBox.Enabled = enabled;
            customerTextBox.Enabled = enabled;
            descriptionTextBox.Enabled = enabled;
            Control_Edit_PartID_ComboBox_ItemType.Enabled = enabled;
            saveButton.Enabled = enabled;
        }

        private async Task UpdatePartAsync()
        {
            if (_currentPart == null)
            {
                return;
            }

            int id = Convert.ToInt32(_currentPart["ID"]);
            string itemNumber = itemNumberTextBox.Text.Trim();
            string customer = customerTextBox.Text.Trim();
            string description = descriptionTextBox.Text.Trim();
            string issuedBy = Model_AppVariables.User;
            string type = Control_Edit_PartID_ComboBox_ItemType.Text;
            await Dao_Part.UpdatePartWithStoredProcedure(id, itemNumber, customer, description, issuedBy, type);
        }

        private void ClearForm()
        {
            itemNumberTextBox.Clear();
            customerTextBox.Clear();
            descriptionTextBox.Clear();
            Control_Edit_PartID_ComboBox_ItemType.SelectedIndex = 0;
            issuedByValueLabel.Text = "";
            _currentPart = null;
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    // Dispose managed resources if any
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Service_ErrorHandler.HandleException(ex, ErrorSeverity.Low, 
                    controlName: nameof(Control_Edit_PartID));
            }
        }

        #endregion
    }
}
