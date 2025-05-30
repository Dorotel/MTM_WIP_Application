using System.Drawing.Printing;

namespace MTM_WIP_Application.Core;

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
        _dgv = dgv ?? throw new ArgumentNullException(nameof(dgv));
        _currentRow = 0;
        _printDocument.Print();
    }

    private void PrintPage(object? sender, PrintPageEventArgs e)
    {
        if (_dgv == null || e.Graphics == null)
        {
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
                return;
            }
        }

        e.HasMorePages = false;
    }
}