using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Imaging;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Properties;
using MTM_Inventory_Application.Core; // For Core_Themes

namespace MTM_Inventory_Application.Core;

#region Core_DgvPrinter

public class Core_DgvPrinter
{
    #region Fields

    private DataGridView? _dgv;
    private readonly PrintDocument _printDocument;
    private int _currentRow;
    private int _pageNumber = 1;
#pragma warning disable IDE0028
    private readonly Dictionary<string, float> _columnWidths = new();
    private readonly Dictionary<string, StringAlignment> _columnAlignments = new();
#pragma warning restore IDE0028
    private HashSet<string>? _printVisibleColumns;

    #endregion

    #region Initialization

    public Core_DgvPrinter()
    {
        _printDocument = new PrintDocument();
        _printDocument.PrintPage += PrintPage;
    }

    #endregion

    #region Properties

    public PrintDocument? PrintDocument => _printDocument;

    #endregion

    #region Column Layout

    public void SetColumnLayout(string columnName, float? width = null, StringAlignment? alignment = null)
    {
        if (width.HasValue)
            _columnWidths[columnName] = width.Value;
        if (alignment.HasValue)
            _columnAlignments[columnName] = alignment.Value;
    }

    public void SetPrintVisibleColumns(IEnumerable<string> columnNames)
    {
        _printVisibleColumns = columnNames != null
            ? new HashSet<string>(columnNames)
            : null;
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

            // Show print dialog only (no preview)
            using (PrintDialog printDialog = new PrintDialog())
            {
                printDialog.Document = _printDocument;
                printDialog.AllowSomePages = true;
                printDialog.UseEXDialog = true;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    _printDocument.Print();
                    LoggingUtility.Log("Print operation completed in Core_DgvPrinter.");
                }
                else
                {
                    LoggingUtility.Log("Print operation cancelled by user.");
                }
            }
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

            var g = e.Graphics;
            float marginLeft = e.MarginBounds.Left;
            float marginTop = e.MarginBounds.Top;
            float marginRight = e.MarginBounds.Right;
            float marginBottom = e.MarginBounds.Bottom;
            float pageWidth = e.MarginBounds.Width;
            float pageHeight = e.MarginBounds.Height;
            float y = marginTop;
            float x = marginLeft;
            float rowHeight = _dgv.RowTemplate.Height;
            var font = _dgv.Font;
            var brush = Brushes.Black;
            float headerHeight = rowHeight * 2.5f;

            // Get theme colors for DataGridView
            var theme = Core_Themes.Core_AppThemes.GetCurrentTheme();
            var colors = theme.Colors;
            Color headerBackColor = colors.DataGridHeaderBackColor ?? Color.Black;
            Color headerForeColor = colors.DataGridHeaderForeColor ?? Color.White;
            Color rowBackColor = colors.DataGridRowBackColor ?? Color.White;
            Color altRowBackColor = colors.DataGridAltRowBackColor ?? Color.Gainsboro;
            Color rowForeColor = colors.DataGridForeColor ?? Color.Black;
            Color gridColor = colors.DataGridGridColor ?? Color.Gray;

            // 1. Draw watermark (MTM logo) at 50% opacity, centered on page
            var watermark = Resources.MTM;
            if (watermark != null)
            {
                int wmWidth = (int)(pageWidth * 0.5f);
                int wmHeight = (int)(pageHeight * 0.3f);
                int wmX = (int)(marginLeft + (pageWidth - wmWidth) / 2);
                int wmY = (int)(marginTop + (pageHeight - wmHeight) / 2);
                var wmRect = new Rectangle(wmX, wmY, wmWidth, wmHeight);
                using (ImageAttributes ia = new ImageAttributes())
                {
                    ColorMatrix cm = new ColorMatrix { Matrix33 = 0.15f };
                    ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(watermark, wmRect, 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, ia);
                }
            }

            // 2. Draw report header (centered, at top)
            var headerFont = new Font(font.FontFamily, font.Size + 4, FontStyle.Bold);
            var subHeaderFont = new Font(font.FontFamily, font.Size + 1, FontStyle.Regular);
            string reportTitle = "INVENTORY TRANSACTION REPORT";
            string partId = "Part ID: " + (_dgv.Tag?.ToString() ?? "");
            var titleSize = g.MeasureString(reportTitle, headerFont);
            var partIdSize = g.MeasureString(partId, subHeaderFont);
            float titleX = marginLeft + (pageWidth - titleSize.Width) / 2;
            float partIdX = marginLeft + (pageWidth - partIdSize.Width) / 2;
            float headerY = y;
            g.DrawString(reportTitle, headerFont, brush, titleX, headerY);
            headerY += titleSize.Height;
            g.DrawString(partId, subHeaderFont, brush, partIdX, headerY);
            headerY += partIdSize.Height + 10;
            y = headerY;

            // 3. Reserve space for page number at bottom right
            var pageNumFont = new Font(font.FontFamily, font.Size, FontStyle.Regular);
            string pageNumText = $"Page {_pageNumber}";
            var pageNumSize = g.MeasureString(pageNumText, pageNumFont);
            float pageNumY = marginBottom - pageNumSize.Height;
            float pageNumX = marginRight - pageNumSize.Width;

            // 4. Draw table header row (theme background, theme text)
            float tableX = marginLeft;
            float tableY = y;
            float colY = tableY;
            float colX = tableX;
            var headerRowHeight = rowHeight;
            using var headerBackBrush = new SolidBrush(headerBackColor);
            using var headerTextBrush = new SolidBrush(headerForeColor);
            List<float> colWidths = new();
            List<DataGridViewColumn> visibleCols = new();
            foreach (DataGridViewColumn col in _dgv.Columns)
            {
                bool isVisible = col.Visible;
                if (_printVisibleColumns != null)
                    isVisible = isVisible && _printVisibleColumns.Contains(col.Name);

                if (!isVisible) continue;
                float colWidth = _columnWidths.TryGetValue(col.Name, out var w) ? w : col.Width;
                colWidths.Add(colWidth);
                visibleCols.Add(col);
            }
            float totalTableWidth = pageWidth;
            float totalOriginalWidth = 0;
            List<float> originalColWidths = new();
            foreach (DataGridViewColumn col in visibleCols)
            {
                float colWidth = _columnWidths.TryGetValue(col.Name, out var w) ? w : col.Width;
                originalColWidths.Add(colWidth);
                totalOriginalWidth += colWidth;
            }
            for (int i = 0; i < originalColWidths.Count; i++)
            {
                colWidths[i] = originalColWidths[i] / totalOriginalWidth * totalTableWidth;
            }
            g.FillRectangle(headerBackBrush, tableX, colY, totalTableWidth, headerRowHeight);
            colX = tableX;
            for (int i = 0; i < visibleCols.Count; i++)
            {
                var col = visibleCols[i];
                float colWidth = colWidths[i];
                g.DrawString(col.HeaderText ?? string.Empty, font, headerTextBrush, colX + 2, colY + 2);
                colX += colWidth;
            }
            y += headerRowHeight;

            // 5. Draw table rows (theme colors), fit between header and page number
            float tableBottomLimit = pageNumY - 10; // 10px padding above page number
            int startRow = _currentRow;
            for (; _currentRow < _dgv.Rows.Count; )
            {
                var row = _dgv.Rows[_currentRow];
                if (row.IsNewRow)
                {
                    _currentRow++;
                    continue;
                }
                if (y + rowHeight > tableBottomLimit)
                {
                    e.HasMorePages = true;
                    _pageNumber++;
                    LoggingUtility.Log("PrintPage: more pages required.");
                    // Draw page number before returning
                    g.DrawString(pageNumText, pageNumFont, brush, pageNumX, pageNumY);
                    return;
                }
                colX = tableX;
                var rowBackBrush = new SolidBrush((_currentRow % 2 == 0) ? rowBackColor : altRowBackColor);
                var rowTextBrush = new SolidBrush(rowForeColor);
                g.FillRectangle(rowBackBrush, colX, y, totalTableWidth, rowHeight);
                for (int i = 0; i < visibleCols.Count; i++)
                {
                    var col = visibleCols[i];
                    var cell = row.Cells[col.Index];
                    float colWidth = colWidths[i];
                    var align = _columnAlignments.GetValueOrDefault(col.Name, StringAlignment.Near);
                    using StringFormat format = new() { Alignment = align, LineAlignment = StringAlignment.Center };
                    var text = cell.FormattedValue?.ToString() ?? string.Empty;
                    g.DrawString(text, font, rowTextBrush, new RectangleF(colX + 2, y + 2, colWidth - 4, rowHeight - 4), format);
                    colX += colWidth;
                }
                y += rowHeight;
                rowBackBrush.Dispose();
                rowTextBrush.Dispose();
                _currentRow++;
            }
            // Draw page number at the end of the last page
            g.DrawString(pageNumText, pageNumFont, brush, pageNumX, pageNumY);
            e.HasMorePages = false;
            _pageNumber = 1; // Reset for next print job
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
