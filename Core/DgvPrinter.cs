using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Data;
using System.Drawing.Printing;

namespace MTM_WIP_Application.Core;

/// <summary>
/// 
/// Testing Passed: 05/31/2025
/// 
/// DgvPrinter is a utility class for printing the contents of a DataGridView in a Windows Forms application.
/// 
/// Features:
/// - Allows customization of column widths and text alignment via SetColumnLayout.
/// - Handles printing of column headers and rows, including pagination for multi-page output.
/// - Skips invisible columns and the "new row" placeholder.
/// - Uses the DataGridView's font and row height for consistent appearance.
/// - Includes robust error handling and logging using AppLogger and ErrorLogDao.
/// 
/// Usage:
/// 1. Optionally call SetColumnLayout for custom column settings.
/// 2. Call Print with the target DataGridView.
/// </summary>
public class DgvPrinter
{
    private DataGridView? _dgv;
    private readonly PrintDocument _printDocument;
    private int _currentRow;
#pragma warning disable IDE0028
    private readonly Dictionary<string, float> _columnWidths = new();
    private readonly Dictionary<string, StringAlignment> _columnAlignments = new();
#pragma warning restore IDE0028

    public DgvPrinter()
    {
        _printDocument = new PrintDocument();
        _printDocument.PrintPage += PrintPage;
    }

    // Call this before Print() to customize columns
    public void SetColumnLayout(string columnName, float? width = null, StringAlignment? alignment = null)
    {
        if (width.HasValue)
            _columnWidths[columnName] = width.Value;
        if (alignment.HasValue)
            _columnAlignments[columnName] = alignment.Value;
    }

    public void Print(DataGridView dgv)
    {
        try
        {
            _dgv = dgv ?? throw new ArgumentNullException(nameof(dgv));
            _currentRow = 0;
            AppLogger.Log("Starting print operation in DgvPrinter.");
            _printDocument.Print();
            AppLogger.Log("Print operation completed in DgvPrinter.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(
                ex, false,
                new System.Text.StringBuilder().Append("DgvPrinter.Print").ToString());
            throw;
        }
    }

    private void PrintPage(object? sender, PrintPageEventArgs e)
    {
        try
        {
            if (_dgv == null || e.Graphics == null)
            {
                AppLogger.Log("PrintPage aborted: _dgv or Graphics is null.");
                e.HasMorePages = false;
                return;
            }

            float y = e.MarginBounds.Top;
            float x = e.MarginBounds.Left;
            float rowHeight = _dgv.RowTemplate.Height;
            var font = _dgv.Font;
            var brush = Brushes.Black;

            // Print column headers
            foreach (DataGridViewColumn col in _dgv.Columns)
            {
                if (!col.Visible) continue;
                var colWidth = _columnWidths.TryGetValue(col.Name, out var w) ? w : col.Width;
                e.Graphics.DrawString(col.HeaderText ?? string.Empty, font, brush, x, y);
                x += colWidth;
            }

            y += rowHeight;

            // Print rows
            while (_currentRow < _dgv.Rows.Count)
            {
                var row = _dgv.Rows[_currentRow];
                if (row.IsNewRow)
                {
                    _currentRow++;
                    continue;
                }

                x = e.MarginBounds.Left;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    var col = _dgv.Columns[cell.ColumnIndex];
                    if (!col.Visible) continue;
                    var colWidth = _columnWidths.TryGetValue(col.Name, out var w) ? w : col.Width;
                    var align = _columnAlignments.GetValueOrDefault(col.Name, StringAlignment.Near);
                    using StringFormat format = new() { Alignment = align };
                    var text = cell.FormattedValue?.ToString() ?? string.Empty;
                    e.Graphics.DrawString(text, font, brush, new RectangleF(x, y, colWidth, rowHeight), format);
                    x += colWidth;
                }

                y += rowHeight;
                _currentRow++;

                if (y + rowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    AppLogger.Log("PrintPage: more pages required.");
                    return;
                }
            }

            e.HasMorePages = false;
            AppLogger.Log("PrintPage finished without more pages required.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(
                ex, false,
                new System.Text.StringBuilder().Append("DgvPrinter.PrintPage").ToString());
            e.HasMorePages = false;
        }
    }
}