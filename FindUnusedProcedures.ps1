<#
- Uses a static list of procedure names (as in your original v4).
- Scans all .cs files for each procedure usage.
- Outputs CSV: Procedure,TotalUsage,File,HitsInFile
- Minimal screen output (just a progress bar, no spam).
- Only the file name (not full path) is shown in the CSV.
#>

# ===== BEGIN PROCEDURE NAMES =====
$procList = @(
    "GetUserRoleNamesList", "GetUserRolesList", "InsertMissingUsrUiSettings", "Reload_md_operation_numbers_from_md_part_ids", "Reload_md_part_ids_from_sequences",
    "inv_inventory_Add_Item", "inv_inventory_Fix_BatchNumbers", "inv_inventory_Get_ByPartID", "inv_inventory_Get_ByPartIDandOperation", "inv_inventory_Get_ByUser",
    "inv_inventory_Remove_Item", "inv_inventory_Transfer_Part", "inv_inventory_Transfer_Quantity", "inv_transaction_Add", "log_changelog_Get_Current",
    "md_item_types_Add_ItemType", "md_item_types_Delete_ByID", "md_item_types_Delete_ByType", "md_item_types_Get_All", "md_item_types_Update_ItemType",
    "md_locations_Add_Location", "md_locations_Delete_ByLocation", "md_locations_Get_All", "md_locations_Update_Location",
    "md_operation_numbers_Add_Operation", "md_operation_numbers_Delete_ByOperation", "md_operation_numbers_Get_All", "md_operation_numbers_Update_Operation",
    "md_part_ids_Add_Part", "md_part_ids_Delete_ByItemNumber", "md_part_ids_Get_All", "md_part_ids_Get_ByItemNumber", "md_part_ids_Update_Part",
    "migrate_user_roles_debug", "sp_ReassignBatchNumbers", "sys_last_10_transactions_Get_ByUser", "sys_roles_Get_ById", "sys_user_roles_Add", "sys_user_roles_Delete", "sys_user_roles_Update",
    "usr_ui_settings_Get", "usr_ui_settings_GetShortcutsJson", "usr_ui_settings_SetShortcutsJson", "usr_ui_settings_SetThemeJson",
    "usr_users_Add_User", "usr_users_Delete_User", "usr_users_Exists", "usr_users_Get_All", "usr_users_Get_ByUser", "usr_users_Update_User"
)
# ===== END PROCEDURE NAMES =====

$csFiles = Get-ChildItem -Path . -Recurse -Filter *.cs -File | Where-Object { $_.Name -ne $MyInvocation.MyCommand.Name }
$totalProcs = $procList.Count

# For CSV output
$csvPath = "ProcedureUsageReport.csv"
$csvRows = @()

for ($procIdx = 0; $procIdx -lt $procList.Count; $procIdx++) {
    $proc = $procList[$procIdx]
    $usageTotal = 0
    $fileHits = @()
    $percent = [math]::Round((($procIdx + 1) / $totalProcs) * 100)
    $barLen = 40
    $filled = [math]::Round((($procIdx + 1) / $totalProcs) * $barLen)
    $bar = "#" * $filled + "." * ($barLen - $filled)

    foreach ($file in $csFiles) {
        $matches = Select-String -Path $file.FullName -Pattern $proc -SimpleMatch
        $count = $matches.Count
        if ($count -gt 0) {
            $usageTotal += $count
            $fileHits += [PSCustomObject]@{
                Procedure  = $proc
                TotalUsage = $null # Will be filled in after loop
                File       = $file.Name
                HitsInFile = $count
            }
        }
    }

    if ($fileHits.Count -gt 0) {
        foreach ($row in $fileHits) {
            $row.TotalUsage = $usageTotal
            $csvRows += $row
        }
    } else {
        $csvRows += [PSCustomObject]@{
            Procedure  = $proc
            TotalUsage = 0
            File       = ""
            HitsInFile = ""
        }
    }

    # Print one progress bar per procedure after it's processed
    Write-Host ("[{0}] {1,3}%  {2}" -f $bar, $percent, $proc)
}

Write-Host
Write-Host "========== PROCEDURE USAGE REPORT =========="
$csvRows | Select-Object Procedure,TotalUsage,File,HitsInFile | Export-Csv -Path $csvPath -NoTypeInformation -Encoding UTF8
Write-Host "CSV output written to: $csvPath"

if (Get-Command Set-Clipboard -ErrorAction SilentlyContinue) {
    Get-Content $csvPath | Set-Clipboard
    Write-Host "(CSV has been copied to your clipboard.)"
} else {
    Write-Host "(Set-Clipboard not available; run in Windows 10+ for clipboard support.)"
}

Pause