#!/bin/bash

# Simple script to apply renaming transformations

echo "MTM WIP Application - Renamer Tool (Simple Version)"
echo "Running in PREVIEW mode"
echo "======================================="

cd /home/runner/work/MTM_WIP_Application/MTM_WIP_Application

# Create log directory
mkdir -p Tools/RenamerTool/Logs

# Process Transactions.cs and Transactions.Designer.cs
echo "Processing Transactions form files..."

# Create the log file
LOG_FILE="Tools/RenamerTool/Logs/RenamerTool_Run_$(date +%Y-%m-%d_%H-%M-%S).log"

echo "[$(date '+%Y-%m-%d %H:%M:%S')] Starting renaming process" > "$LOG_FILE"

# Define the mappings
declare -A MAPPINGS=(
    ["comboSortBy"]="Transactions_ComboBox_SortBy"
    ["lblSortBy"]="Transactions_Label_SortBy"
    ["txtSearchPartID"]="Transactions_TextBox_SearchPartID"
    ["lblSearchPartID"]="Transactions_Label_SearchPartID"
    ["btnReset"]="Transactions_Button_Reset"
    ["tabControlMain"]="Transactions_TabControl_Main"
    ["tabPartEntry"]="Transactions_TabPage_PartEntry"
    ["tabPartRemoval"]="Transactions_TabPage_PartRemoval"
    ["tabPartTransfer"]="Transactions_TabPage_PartTransfer"
    ["dataGridTransactions"]="Transactions_DataGridView_Transactions"
    ["panelBottom"]="Transactions_Panel_Bottom"
    ["lblSortByUser"]="Transactions_Label_SortByUser"
    ["comboUser"]="Transactions_ComboBox_User"
    ["lblUser"]="Transactions_Label_User"
    ["comboUserName"]="Transactions_ComboBox_UserName"
    ["lblUserName"]="Transactions_Label_UserName"
    ["comboShift"]="Transactions_ComboBox_Shift"
    ["lblShift"]="Transactions_Label_Shift"
)

# Process each file
for file in "Forms/Transactions/Transactions.cs" "Forms/Transactions/Transactions.Designer.cs"; do
    if [[ -f "$file" ]]; then
        echo "Processing $file..."
        echo "[$(date '+%Y-%m-%d %H:%M:%S')] Processing $file" >> "$LOG_FILE"
        
        # Count occurrences for each mapping
        for old_name in "${!MAPPINGS[@]}"; do
            new_name="${MAPPINGS[$old_name]}"
            count=$(grep -o "\b$old_name\b" "$file" | wc -l)
            if [[ $count -gt 0 ]]; then
                echo "  Found $count occurrences of '$old_name' -> '$new_name'"
                echo "[$(date '+%Y-%m-%d %H:%M:%S')] Would rename '$old_name' to '$new_name' in $file ($count occurrences)" >> "$LOG_FILE"
            fi
        done
    else
        echo "File not found: $file"
        echo "[$(date '+%Y-%m-%d %H:%M:%S')] File not found: $file" >> "$LOG_FILE"
    fi
done

echo ""
echo "=== RENAMER TOOL SUMMARY ==="
echo "Mode: PREVIEW"
echo "Check log file: $LOG_FILE"
echo "============================"