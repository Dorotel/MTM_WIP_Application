<#
- Searches all .cs files for likely hardcoded SQL command strings (case sensitive).
- Outputs a CSV file: File,LineNumber,CommandString
- Only prints minimal progress (one line per file after scanning).
- Only the file name (not full path) is shown in the CSV.
#>

$csFiles = Get-ChildItem -Path . -Recurse -Filter *.cs -File
$totalFiles = $csFiles.Count

# For CSV output
$csvPath = "HardcodedSqlReport.csv"
$csvRows = @()

# Regex to match SQL command patterns in C# strings (case sensitive)
$patterns = @(
    '(["''])[ \t]*SELECT[ \t]',
    '(["''])[ \t]*UPDATE[ \t]',
    '(["''])[ \t]*INSERT[ \t]+INTO[ \t]',
    '(["''])[ \t]*DELETE[ \t]+FROM[ \t]',
    '(["''])[ \t]*CREATE[ \t]+(TABLE|PROCEDURE|VIEW)[ \t]',
    '(["''])[ \t]*ALTER[ \t]+(TABLE|PROCEDURE|VIEW)[ \t]',
    '(["''])[ \t]*DROP[ \t]+(TABLE|PROCEDURE|VIEW)[ \t]',
    '(["''])[ \t]*EXEC[ \t]+\w+',
    '(["''])[ \t]*MERGE[ \t]+INTO[ \t]'
)

for ($idx = 0; $idx -lt $totalFiles; $idx++) {
    $file = $csFiles[$idx]
    $percent = [math]::Round((($idx + 1) / $totalFiles) * 100)
    $barLen = 40
    $filled = [math]::Round((($idx + 1) / $totalFiles) * $barLen)
    $bar = "#" * $filled + "." * ($barLen - $filled)
    Write-Host ("[{0}] {1,3}%  {2}" -f $bar, $percent, $file.Name)

    $lines = Get-Content $file.FullName
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        foreach ($pat in $patterns) {
            if ($line -cmatch $pat) {  # Use -cmatch for case-sensitive match
                $csvRows += [PSCustomObject]@{
                    File         = $file.Name
                    LineNumber   = $i + 1
                    CommandString= $line.Trim()
                }
                break
            }
        }
    }
}

Write-Host
Write-Host "========== HARDCODED SQL REPORT =========="
$csvRows | Select-Object File,LineNumber,CommandString | Export-Csv -Path $csvPath -NoTypeInformation -Encoding UTF8
Write-Host "CSV output written to: $csvPath"

if (Get-Command Set-Clipboard -ErrorAction SilentlyContinue) {
    Get-Content $csvPath | Set-Clipboard
    Write-Host "(CSV has been copied to your clipboard.)"
} else {
    Write-Host "(Set-Clipboard not available; run in Windows 10+ for clipboard support.)"
}

Pause