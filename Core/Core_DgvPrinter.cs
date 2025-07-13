// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing.Printing;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;

namespace MTM_Inventory_Application.Core;

#region Core_DgvPrinter

public class Core_DgvPrinter
{
    #region Fields

    private DataGridView? _dgv;
    private readonly PrintDocument _printDocument;
    private int _currentRow;
#pragma warning disable IDE0028
    private readonly Dictionary<string, float> _columnWidths = new();
    private readonly Dictionary<string, StringAlignment> _columnAlignments = new();
#pragma warning restore IDE0028

    #endregion

    #region Initialization

    public Core_DgvPrinter()
    {
        _printDocument = new PrintDocument();
        _printDocument.PrintPage += PrintPage;
    }

    #endregion

    #region Properties

    // Add this property to expose the PrintDocument for testing
    public PrintDocument? PrintDocument => _printDocument;

    #endregion

    #region Column Layout

    // Call this before Print() to customize columns
    public void SetColumnLayout(string columnName, float? width = null, StringAlignment? alignment = null)
    {
        if (width.HasValue)
            _columnWidths[columnName] = width.Value;
        if (alignment.HasValue)
            _columnAlignments[columnName] = alignment.Value;
    }

    #endregion

    #region Printing

    public void Print(DataGridView dgv)
    {
        try
        {
            _dgv = dgv ?? throw new ArgumentNullException(nameof(dgv));
            _currentRow = 0;
            LoggingUtility.Log("Starting print operation in Core_DgvPrinter.");
            _printDocument.Print();
            LoggingUtility.Log("Print operation completed in Core_DgvPrinter.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(
                ex, false,
                new System.Text.StringBuilder().Append("Core_DgvPrinter.Print").ToString());
            throw;
        }
    }

    private void PrintPage(object? sender, PrintPageEventArgs e)
    {
        try
        {
            if (_dgv == null || e.Graphics == null)
            {
                LoggingUtility.Log("PrintPage aborted: _dgv or Graphics is null.");
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
                    LoggingUtility.Log("PrintPage: more pages required.");
                    return;
                }
            }

            e.HasMorePages = false;
            LoggingUtility.Log("PrintPage finished without more pages required.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(
                ex, false,
                new System.Text.StringBuilder().Append("Core_DgvPrinter.PrintPage").ToString());
            e.HasMorePages = false;
        }
    }

    #endregion
}

#endregion