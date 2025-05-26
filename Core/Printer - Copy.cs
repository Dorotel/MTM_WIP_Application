using System.Collections;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms.VisualStyles;


//[module:CLSCompliant(true)]
namespace MTM_WIP_Application.Core;

#region Supporting Classes

/// <summary>
///     Setup and implements logs for internal logging
/// </summary>
internal class LogManager
{
    /// <summary>
    ///     Constructor, allow user to override path and name of logging file
    /// </summary>
    /// <param name="userbasepath"></param>
    /// <param name="userlogname"></param>
    public LogManager(string userbasepath, string userlogname)
    {
        BasePath = string.IsNullOrEmpty(userbasepath) ? "." : userbasepath;
        LogNameHeader = string.IsNullOrEmpty(userlogname) ? "MsgLog" : userlogname;

        Log(Categories.Info, "********************* New Trace *********************");
    }

    /// <summary>
    ///     Define logging message categories
    /// </summary>
    public enum Categories
    {
        Info = 1,
        Error,
        Exception
    }

    private int _useFrame = 1;

    /// <summary>
    ///     Path to log file
    /// </summary>
    public string BasePath { get; set; }

    /// <summary>
    ///     Header for log file name
    /// </summary>
    public string LogNameHeader { get; set; }

    /// <summary>
    ///     Log a message, using the provided category
    /// </summary>
    /// <param name="category"></param>
    /// <param name="msg"></param>
    public void Log(Categories category, string msg)
    {
        // get call stack
        StackTrace stackTrace = new();

        // get calling method name
        var caller = stackTrace.GetFrame(_useFrame).GetMethod().Name;

        // log it
        LogWriter.Write(caller, category, msg, BasePath, LogNameHeader);

        // reset frame pointer
        _useFrame = 1;
    }

    /// <summary>
    ///     Log an informational message
    /// </summary>
    /// <param name="msg"></param>
    public void LogInfoMsg(string msg)
    {
        _useFrame++; // bump up the stack frame pointer to skip this entry
        Log(Categories.Info, msg);
    }
}

/// <summary>
///     Do the actual log writing using setup info in Log Manager class
/// </summary>
internal class LogWriter
{
    /// <summary>
    ///     Create standard log file name with "our" name format
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string LogFileName(string name)
    {
        return $"{name}_{DateTime.Now:yyyyMMdd}.Log";
    }

    /// <summary>
    ///     Write the log entry to the file. Note that the log file is always flushed and closed. This
    ///     will impact performance, but ensures that messages aren't lost
    /// </summary>
    /// <param name="from"></param>
    /// <param name="category"></param>
    /// <param name="msg"></param>
    /// <param name="path"></param>
    /// <param name="name"></param>
    public static void Write(string from, LogManager.Categories category, string msg, string path, string name)
    {
        StringBuilder line = new();
        line.Append(DateTime.Now.ToShortDateString());
        line.Append('-');
        line.Append(DateTime.Now.ToLongTimeString());
        line.Append(", ");
        line.Append(category.ToString().PadRight(6, ' '));
        line.Append(',');
        line.Append(from.PadRight(13, ' '));
        line.Append(',');
        line.Append(msg);
        StreamWriter w = new(path + "\\" + LogFileName(name), true);
        w.WriteLine(line.ToString());
        w.Flush();
        w.Close();
    }
}

/// <summary>
///     Class for the ownerdraw event. Provide the caller with the cell data, the current
///     graphics context and the location in which to draw the cell.
/// </summary>
public class DgvCellDrawingEventArgs(
    Graphics g,
    RectangleF bounds,
    DataGridViewCellStyle style,
    int row,
    int column)
    : EventArgs
{
    public DataGridViewCellStyle CellStyle = style;
    public int Column = column;
    public RectangleF DrawingBounds = bounds;
    public Graphics G = g;
    public bool Handled = false;
    public int Row = row;
}

/// <summary>
///     Delegate for ownerdraw cells - allow the caller to provide drawing for the cell
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public delegate void CellOwnerDrawEventHandler(object sender, DgvCellDrawingEventArgs e);

/// <summary>
///     Hold Extension methods
/// </summary>
public static class Extensions
{
    /// <summary>
    ///     Extension method to print all the "ImbeddedImages" in a provided list
    /// </summary>
    /// <typeparam name="?"></typeparam>
    /// <param name="list"></param>
    /// <param name="g"></param>
    /// <param name="pagewidth"></param>
    /// <param name="pageheight"></param>
    /// <param name="margins"></param>
    public static void DrawImbeddedImage<T>(this IEnumerable<T> list,
        Graphics g, int pagewidth, int pageheight, Margins margins)
    {
        foreach (var t in list)
            if (t is DgvPrinter.ImbeddedImage)
            {
                var ii = (DgvPrinter.ImbeddedImage)Convert.ChangeType(t, typeof(DgvPrinter.ImbeddedImage));
                // Fix - DrawImageUnscaled was actually scaling the images!!?! Oh well...
                //g.DrawImageUnscaled(ii.theImage, ii.upperleft(pagewidth, pageheight, margins));
                g.DrawImage(ii.TheImage,
                    new Rectangle(ii.Upperleft(pagewidth, pageheight, margins),
                        new Size(ii.TheImage.Width, ii.TheImage.Height)));
            }
    }
}

#endregion

/// <summary>
///     Data Grid View Printer. Print functions for a datagridview, since MS
///     didn't see fit to do it.
/// </summary>
public class DgvPrinter
{
    //---------------------------------------------------------------------
    // Constructor
    //---------------------------------------------------------------------
    /// <summary>
    ///     Constructor for DGVPrinter
    /// </summary>
    public DgvPrinter()
    {
        // create print document
        PrintDocument = new PrintDocument();
        //printDoc.PrintPage += new PrintPageEventHandler(PrintPageEventHandler);
        //printDoc.BeginPrint += new PrintEventHandler(BeginPrintEventHandler);
        PrintMargins = new Margins(60, 60, 40, 40);

        // set default fonts
        PageNumberFont = new Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);
        PageNumberColor = Color.Black;
        TitleFont = new Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point);
        TitleColor = Color.Black;
        SubTitleFont = new Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Point);
        SubTitleColor = Color.Black;
        FooterFont = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Point);
        FooterColor = Color.Black;

        // default spacing
        TitleSpacing = 0;
        SubTitleSpacing = 0;
        FooterSpacing = 0;

        // Create string formatting objects
        Buildstringformat(ref _titleformat, null, StringAlignment.Center, StringAlignment.Center,
            StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
        Buildstringformat(ref _subtitleformat, null, StringAlignment.Center, StringAlignment.Center,
            StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
        Buildstringformat(ref _footerformat, null, StringAlignment.Center, StringAlignment.Center,
            StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
        Buildstringformat(ref _pagenumberformat, null, StringAlignment.Far, StringAlignment.Center,
            StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);

        // Set these formatting objects to null to flag whether or not they were set by the caller
        _columnheadercellformat = null;
        _rowheadercellformat = null;
        _cellformat = null;

        // Print Preview properties
        Owner = null;
        PrintPreviewZoom = 1.0;

        // Deprecated properties - retain for backwards compatibility
        HeaderCellAlignment = StringAlignment.Near;
        HeaderCellFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
        CellAlignment = StringAlignment.Near;
        CellFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
    }

    public enum Alignment
    {
        NotSet,
        Left,
        Right,
        Center
    }

    public enum Location
    {
        Header,
        Footer,
        Absolute
    }

    public enum PrintLocation
    {
        All,
        FirstOnly,
        LastOnly,
        None
    }

    public enum SizeType
    {
        CellSize,
        StringSize,
        Porportional
    }

    /// <summary>
    ///     Adjust column widths for fixed and porportional columns, set the
    ///     margins to enforce the selected tablealignment.
    /// </summary>
    /// <param name="g">The graphics context for all measurements</param>
    /// <param name="pageset">The pageset to adjust</param>
    private void AdjustPageSets(Graphics g, PageDef pageset)
    {
        int i;
        var fixedcolwidth = _rowheaderwidth;
        float remainingcolwidth = 0;
        float ratio;

        //-----------------------------------------------------------------
        // Adjust the column widths in the page set to their final values,
        // accounting for overridden widths and porportional column stretching
        //-----------------------------------------------------------------

        // calculate the amount of space reserved for fixed width columns
        for (i = 0; i < pageset.Colwidthsoverride.Count; i++)
            if (pageset.Colwidthsoverride[i] >= 0)
                fixedcolwidth += pageset.Colwidthsoverride[i];

        // calculate the amount space requested for non-overridden columns
        for (i = 0; i < pageset.Colwidths.Count; i++)
            if (pageset.Colwidthsoverride[i] < 0)
                remainingcolwidth += pageset.Colwidths[i];

        // calculate the ratio for porportional columns, use 1 for 
        // non-overridden columns or not porportional
        if ((_porportionalcolumns || ColumnWidthSetting.Porportional == ColumnWidth) &&
            0 < remainingcolwidth)
            ratio = (_printWidth - fixedcolwidth) / remainingcolwidth;
        else
            ratio = (float)1.0;

        // reset all column widths for override and/or porportionality. coltotalwidth
        // for each pageset should be <= pageWidth
        pageset.Coltotalwidth = _rowheaderwidth;
        for (i = 0; i < pageset.Colwidths.Count; i++)
        {
            if (pageset.Colwidthsoverride[i] >= 0)
                // use set width
                pageset.Colwidths[i] = pageset.Colwidthsoverride[i];
            else if (ColumnWidthSetting.Porportional == ColumnWidth)
                // change the width by the ratio
                pageset.Colwidths[i] = pageset.Colwidths[i] * ratio;
            else if (pageset.Colwidths[i] > _printWidth - pageset.Coltotalwidth)
                pageset.Colwidths[i] = _printWidth - pageset.Coltotalwidth;

            //recalculate any rows that need to flow down the page
            RecalcRowHeights(g, pageset.Columnindex[i], pageset.Colwidths[i]);

            pageset.Coltotalwidth += pageset.Colwidths[i];
        }

        //-----------------------------------------------------------------
        // Table Alignment - now that we have the column widths established
        // we can reset the table margins to get left, right and centered
        // for the table on the page
        //-----------------------------------------------------------------

        // Reset Print Margins based on table alignment
        if (Alignment.Left == TableAlignment)
        {
            // Bias table to the left by setting "right" value
            pageset.Margins.Right = _pageWidth - pageset.Margins.Left - (int)pageset.Coltotalwidth;
            if (0 > pageset.Margins.Right) pageset.Margins.Right = 0;
        }
        else if (Alignment.Right == TableAlignment)
        {
            // Bias table to the right by setting "left" value
            pageset.Margins.Left = _pageWidth - pageset.Margins.Right - (int)pageset.Coltotalwidth;
            if (0 > pageset.Margins.Left) pageset.Margins.Left = 0;
        }
        else if (Alignment.Center == TableAlignment)
        {
            // Bias the table to the center by setting left and right equal
            pageset.Margins.Left = (_pageWidth - (int)pageset.Coltotalwidth) / 2;
            if (0 > pageset.Margins.Left) pageset.Margins.Left = 0;

            pageset.Margins.Right = pageset.Margins.Left;
        }
    }

    /// <summary>
    ///     BeginPrint Event Handler
    ///     Set values at start of print run
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void BeginPrintEventHandler(object sender, PrintEventArgs e)
    {
        if (EnableLogging) _logger.LogInfoMsg("BeginPrintEventHandler called. Printing started.");

        // reset counters since we'll go through this twice if we print from preview
        _currentpageset = 0;
        _lastrowprinted = -1;
        _currentPage = 0;
    }

    /// <summary>
    ///     Centralize the string format settings. Build a string format object
    ///     using passed in settings, (allowing a user override of a single setting)
    ///     and get the alignment from the cell control style.
    /// </summary>
    /// <param name="format">String format, ref parameter with return settings</param>
    /// <param name="controlstyle">DataGridView style to apply (if available)</param>
    /// <param name="alignment">Override text Alignment</param>
    /// <param name="linealignment">Override line alignment</param>
    /// <param name="flags">String format flags</param>
    /// <param name="trim">Override string trimming flags</param>
    /// <returns></returns>
    private void Buildstringformat(ref StringFormat format, DataGridViewCellStyle controlstyle,
        StringAlignment alignment, StringAlignment linealignment, StringFormatFlags flags,
        StringTrimming trim)
    {
        // allocate format if it doesn't already exist
        if (null == format) format = new StringFormat();

        // Set defaults
        format.Alignment = alignment;
        format.LineAlignment = linealignment;
        format.FormatFlags = flags;
        format.Trimming = trim;

        // Check on right-to-left flag. This is set at the grid level, but doesn't show up 
        // as a cell format. Urgh.
        if (null != _dgv && RightToLeft.Yes == _dgv.RightToLeft)
            format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

        // use cell alignment to override defaulted alignments
        if (null != controlstyle)
        {
            // Adjust the format based on the control settings, bias towards centered
            var cellalign = controlstyle.Alignment;
            if (cellalign.ToString().Contains("Center"))
                format.Alignment = StringAlignment.Center;
            else if (cellalign.ToString().Contains("Left"))
                format.Alignment = StringAlignment.Near;
            else if (cellalign.ToString().Contains("Right")) format.Alignment = StringAlignment.Far;

            if (cellalign.ToString().Contains("Top"))
                format.LineAlignment = StringAlignment.Near;
            else if (cellalign.ToString().Contains("Middle"))
                format.LineAlignment = StringAlignment.Center;
            else if (cellalign.ToString().Contains("Bottom")) format.LineAlignment = StringAlignment.Far;
        }
    }

    /// <summary>
    ///     Calculate cell size based on data versus size settings
    /// </summary>
    /// <param name="g">Current graphics context</param>
    /// <param name="cell">Cell being measured</param>
    /// <param name="index">Column index of cell being measured</param>
    /// <param name="cellstyle">Computed Style of cell being measured</param>
    /// <param name="basewidth">Initial width for size calculation</param>
    /// <param name="format">Computed string format for cell data</param>
    /// <returns>Size of printed cell</returns>
    private SizeF Calccellsize(Graphics g, DataGridViewCell cell, DataGridViewCellStyle cellstyle,
        float basewidth, float overridewidth, StringFormat format)
    {
        // Start with the grid view cell size
        SizeF size = new(cell.Size);

        // If we need to do any calculated cell sizes, we need to measure the cell contents
        if (RowHeightSetting.DataHeight == RowHeight ||
            ColumnWidthSetting.DataWidth == ColumnWidth ||
            ColumnWidthSetting.Porportional == ColumnWidth)
        {
            SizeF datasize;

            //-------------------------------------------------------------
            // Measure cell contents
            //-------------------------------------------------------------
            if ("DataGridViewImageCell" == _dgv.Columns[cell.ColumnIndex].CellType.Name
                && ("Image" == cell.ValueType.Name || "Byte[]" == cell.ValueType.Name))
            {
                // image to measure
                Image img;

                // if we don't actually have a value, then just exit with a minimum size.
                if (null == cell.Value || typeof(DBNull) == cell.Value.GetType()) return new SizeF(1, 1);

                // Check on type of image cell value - may not be an actual "image" type
                if ("Image" == cell.ValueType.Name || "Object" == cell.ValueType.Name)
                {
                    // if it's an "image" type, then load it directly
                    img = (Image)cell.Value;
                }
                else if ("Byte[]" == cell.ValueType.Name)
                {
                    // if it's not an "image" type (i.e. loaded from a database to a bound column)
                    // convert the underlying byte array to an image
                    ImageConverter ic = new();
                    img = (Image)ic.ConvertFrom((byte[])cell.Value);
                }
                else
                {
                    throw new Exception(string.Format("Unknown image cell underlying type: {0} in column {1}",
                        cell.ValueType.Name, cell.ColumnIndex));
                }

                // size to print is size of image
                datasize = img.Size;
            }
            else
            {
                var width = -1 != overridewidth ? overridewidth : basewidth;

                // measure the data for each column, keep widths and biggest height
                datasize = g.MeasureString(cell.EditedFormattedValue.ToString(), cellstyle.Font,
                    new SizeF(width, MaxPages), format);

                // if we have excessively large cell, limit it to one page width
                if (_printWidth < datasize.Width)
                    datasize = g.MeasureString(cell.FormattedValue.ToString(), cellstyle.Font,
                        new SizeF(_pageWidth - cellstyle.Padding.Left - cellstyle.Padding.Right, MaxPages),
                        format);
            }

            //-------------------------------------------------------------
            // Add in padding for data based cell sizes and porportional columns
            //-------------------------------------------------------------

            // set cell height to string height if indicated
            if (RowHeightSetting.DataHeight == RowHeight)
                size.Height = datasize.Height + cellstyle.Padding.Top + cellstyle.Padding.Bottom;

            // set cell width to calculated width if indicated
            if (ColumnWidthSetting.DataWidth == ColumnWidth ||
                ColumnWidthSetting.Porportional == ColumnWidth)
                size.Width = datasize.Width + cellstyle.Padding.Left + cellstyle.Padding.Right;
        }

        return size;
    }

    /// <summary>
    ///     Check for more pages. This is called at the end of printing a page set.
    ///     If there's another page set to print, we return true.
    /// </summary>
    private bool DetermineHasMorePages()
    {
        _currentpageset++;
        if (_currentpageset < _pagesets.Count)
            //currentpageset--;   // decrement back to a valid pageset number
            return true; // tell the caller we're through.
        return false;
    }

    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    // Alternative Interface. In order to set the print information correctly
    // either the DisplayPrintDialog() routine must be called, OR the 
    // PrintDocument (and PrinterSettings) must be Handled through calling
    // PrintDialog separately.
    //
    // Once the PrintDocument has been setup, the PrintNoDisplay() and/or
    // PrintPreviewNoDisplay() routines can be called to print multiple
    // DataGridViews using the same print setup.
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------

    /// <summary>
    ///     Display a printdialog and return the result. Either this method or
    ///     the equivalent must be done prior to calling either of the PrintNoDisplay
    ///     or PrintPreviewNoDisplay methods.
    /// </summary>
    /// <returns></returns>
    public DialogResult DisplayPrintDialog()
    {
        if (EnableLogging) _logger.LogInfoMsg("DisplayPrintDialog process started");

        // create new print dialog and set options
        PrintDialog pd = new()
        {
            UseEXDialog = PrintDialogSettings.UseExDialog,
            AllowSelection = PrintDialogSettings.AllowSelection,
            AllowSomePages = PrintDialogSettings.AllowSomePages,
            AllowCurrentPage = PrintDialogSettings.AllowCurrentPage,
            AllowPrintToFile = PrintDialogSettings.AllowPrintToFile,
            ShowHelp = PrintDialogSettings.ShowHelp,
            ShowNetwork = PrintDialogSettings.ShowNetwork,

            //// setup print dialog with internal setttings
            Document = PrintDocument
        };
        if (!string.IsNullOrEmpty(PrinterName)) PrintDocument.PrinterSettings.PrinterName = PrinterName;

        // show the dialog and display the result
        return pd.ShowDialog();
    }

    /// <summary>
    ///     Draw a cell. Used for column and row headers and body cells.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="cellprintarea"></param>
    /// <param name="style"></param>
    /// <param name="cell"></param>
    /// <param name="startlocation"></param>
    /// <param name="cellformat"></param>
    /// <param name="lines"></param>
    private void DrawCell(Graphics g, RectangleF cellprintarea, DataGridViewCellStyle style,
        DataGridViewCell cell, float startlocation, StringFormat cellformat, Pen lines)
    {
        // Draw the cell if it's not overridden by ownerdrawing
        if (!DrawOwnerDrawCell(g, cell.RowIndex, cell.ColumnIndex, cellprintarea, style))
        {
            // save original clipping bounds
            var clip = g.ClipBounds;

            // fill in the full cell background - using the selected style
            //g.FillRectangle(new SolidBrush(colstyle.BackColor), cellprintarea);
            g.FillRectangle(new SolidBrush(style.BackColor), cellprintarea);

            // reset print area for this individual cell, adjusting 'inward' for cell padding
            RectangleF paddedcellprintarea = new(cellprintarea.X + style.Padding.Left,
                cellprintarea.Y + style.Padding.Top,
                cellprintarea.Width - style.Padding.Right - style.Padding.Left,
                cellprintarea.Height - style.Padding.Bottom - style.Padding.Top);

            // set clipping to current print area - i.e. our cell
            g.SetClip(cellprintarea);

            // define the *actual* print area based on the given startlocation. Offset the start by 
            // minus the start location, increase the print area height by the startlocation
            RectangleF actualprint = new(paddedcellprintarea.X, paddedcellprintarea.Y - startlocation,
                paddedcellprintarea.Width, paddedcellprintarea.Height + startlocation);

            // draw content based on cell style, but only for "body" cells
            if (0 <= cell.RowIndex && 0 <= cell.ColumnIndex)
            {
                if ("DataGridViewImageCell" == _dgv.Columns[cell.ColumnIndex].CellType.Name)
                    // draw the image for image cells
                    DrawImageCell(g, (DataGridViewImageCell)cell, actualprint);
                else if ("DataGridViewCheckBoxCell" == _dgv.Columns[cell.ColumnIndex].CellType.Name)
                    // draw a checkbox for checkbox cells
                    DrawCheckBoxCell(g, (DataGridViewCheckBoxCell)cell, actualprint);
                else
                    // this handles drawing for textbox, button, combobox, and link cell types.
                    // currently these are not drawn as "controls" for performance reasons.
                    // draw the text for the cell at the row / col intersection
                    g.DrawString(cell.FormattedValue.ToString(), style.Font,
                        new SolidBrush(style.ForeColor), actualprint, cellformat);
            }
            else
            {
                // draw the text for the cell at the row / col intersection
                g.DrawString(cell.FormattedValue.ToString(), style.Font,
                    new SolidBrush(style.ForeColor), actualprint, cellformat);
            }

            // reset clipping bounds to "normal"
            g.SetClip(clip);

            // draw the borders - default to the dgv's border setting, and use unpadded cell print area
            if (_dgv.CellBorderStyle != DataGridViewCellBorderStyle.None)
                g.DrawRectangle(lines, cellprintarea.X, cellprintarea.Y, cellprintarea.Width, cellprintarea.Height);
        }
    }

    /// <summary>
    ///     Draw a body cell that is a checkbox
    /// </summary>
    /// <param name="g"></param>
    /// <param name="checkboxcell"></param>
    /// <param name="rectf"></param>
    private static void DrawCheckBoxCell(Graphics g, DataGridViewCheckBoxCell checkboxcell, RectangleF rectf)
    {
        // create a non-printing graphics context in which to draw the checkbox control
        Image i = new Bitmap((int)rectf.Width, (int)rectf.Height);
        var tg = Graphics.FromImage(i);

        // determine checked or not checked (or undetermined for tristate checkboxes)
        var state = CheckBoxState.UncheckedNormal;
        if (checkboxcell.ThreeState)
        {
            if (checkboxcell.EditedFormattedValue is CheckState checkState)
                state = checkState switch
                {
                    CheckState.Checked => CheckBoxState.CheckedNormal,
                    CheckState.Indeterminate => CheckBoxState.MixedNormal,
                    _ => CheckBoxState.UncheckedNormal
                };
        }
        else if (checkboxcell.EditedFormattedValue is bool isChecked && isChecked)
        {
            state = CheckBoxState.CheckedNormal;
        }

        // get the size and location to print the checkbox - currently centered, may change later
        var size = CheckBoxRenderer.GetGlyphSize(tg, state);
        var x = ((int)rectf.Width - size.Width) / 2;
        var y = ((int)rectf.Height - size.Height) / 2;

        // draw the checkbox in our temporary graphics context
        CheckBoxRenderer.DrawCheckBox(tg, new Point(x, y), state);

        // calculate image drawing origin based on cell alignment
        switch (checkboxcell.InheritedStyle.Alignment)
        {
            case DataGridViewContentAlignment.BottomCenter:
                rectf.Y += y;
                break;
            case DataGridViewContentAlignment.BottomLeft:
                rectf.X -= x;
                rectf.Y += y;
                break;
            case DataGridViewContentAlignment.BottomRight:
                rectf.X += x;
                rectf.Y += y;
                break;
            case DataGridViewContentAlignment.MiddleCenter:
                break;
            case DataGridViewContentAlignment.MiddleLeft:
                rectf.X -= x;
                break;
            case DataGridViewContentAlignment.MiddleRight:
                rectf.X += x;
                break;
            case DataGridViewContentAlignment.TopCenter:
                rectf.Y -= y;
                break;
            case DataGridViewContentAlignment.TopLeft:
                rectf.X -= x;
                rectf.Y -= y;
                break;
            case DataGridViewContentAlignment.TopRight:
                rectf.X += x;
                rectf.Y -= y;
                break;
            case DataGridViewContentAlignment.NotSet:
                break;
        }

        // now draw the image of the checkbox to our print output
        g.DrawImage(i, rectf);

        // clean up after ourselves
        tg.Dispose();
        i.Dispose();
    }

    /// <summary>
    ///     Draw a body cell that has an imbedded image
    /// </summary>
    /// <param name="g"></param>
    /// <param name="imagecell"></param>
    /// <param name="rectf"></param>
    private static void DrawImageCell(Graphics g, DataGridViewImageCell imagecell, RectangleF rectf)
    {
        // image to draw
        Image img;

        // if we don't actually have a value, then just exit.
        if (null == imagecell.Value || typeof(DBNull) == imagecell.Value.GetType()) return;

        // Check on type of image cell value - may not be an actual "image" type
        if ("Image" == imagecell.ValueType.Name)
        {
            // if it's an "image" type, then load it directly
            img = (Image)imagecell.Value;
        }
        else if ("Byte[]" == imagecell.ValueType.Name)
        {
            // if it's not an "image" type (i.e. loaded from a database to a bound column)
            // convert the underlying byte array to an image
            ImageConverter ic = new();
            img = (Image)ic.ConvertFrom((byte[])imagecell.Value);
        }
        else
        {
            throw new Exception(string.Format("Unknown image cell underlying type: {0} in column {1}",
                imagecell.ValueType.Name, imagecell.ColumnIndex));
        }

        // clipping bounds. This is the portion of the image to fit into the drawing rectangle
        Rectangle src = new();


        // calculate deltas
        int dx;
        int dy;
        // drawn normal size, clipped to cell 
        if (DataGridViewImageCellLayout.Normal == imagecell.ImageLayout ||
            DataGridViewImageCellLayout.NotSet == imagecell.ImageLayout)
        {
            // calculate origin deltas, used to move image
            dx = img.Width - (int)rectf.Width;
            dy = img.Height - (int)rectf.Height;

            // set destination width and height to clip to cell
            if (0 > dx)
                rectf.Width = src.Width = img.Width;
            else
                src.Width = (int)rectf.Width;

            if (0 > dy)
                rectf.Height = src.Height = img.Height;
            else
                src.Height = (int)rectf.Height;
        }
        else if (DataGridViewImageCellLayout.Stretch == imagecell.ImageLayout)
        {
            // stretch image to fit cell size
            src.Width = img.Width;
            src.Height = img.Height;

            // change the origin delta's to 0 so we don't move the image
            dx = 0;
            dy = 0;
        }
        else // DataGridViewImageCellLayout.Zoom
        {
            // scale image to fit in cell
            src.Width = img.Width;
            src.Height = img.Height;

            var vertscale = rectf.Height / src.Height;
            var horzscale = rectf.Width / src.Width;
            float scale;

            // use the smaller scaling factor to ensure the image will fit in the cell
            if (vertscale > horzscale)
            {
                // use horizontal scale, don't move image horizontally
                scale = horzscale;
                dx = 0;
                dy = (int)(src.Height * scale - rectf.Height);
            }
            else
            {
                // use vertical scale, don't move image vertically
                scale = vertscale;
                dy = 0;
                dx = (int)(src.Width * scale - rectf.Width);
            }

            // set target size to match scaled image
            rectf.Width = src.Width * scale;
            rectf.Height = src.Height * scale;
        }

        //calculate image drawing origin based on origin deltas
        switch (imagecell.InheritedStyle.Alignment)
        {
            case DataGridViewContentAlignment.BottomCenter:
                if (0 > dy)
                    rectf.Y -= dy;
                else
                    src.Y = dy;

                if (0 > dx)
                    rectf.X -= dx / 2;
                else
                    src.X = dx / 2;

                break;
            case DataGridViewContentAlignment.BottomLeft:
                if (0 > dy)
                    rectf.Y -= dy;
                else
                    src.Y = dy;

                src.X = 0;
                break;
            case DataGridViewContentAlignment.BottomRight:
                if (0 > dy)
                    rectf.Y -= dy;
                else
                    src.Y = dy;

                if (0 > dx)
                    rectf.X -= dx;
                else
                    src.X = dx;

                break;
            case DataGridViewContentAlignment.MiddleCenter:
                if (0 > dy)
                    rectf.Y -= dy / 2;
                else
                    src.Y = dy / 2;

                if (0 > dx)
                    rectf.X -= dx / 2;
                else
                    src.X = dx / 2;

                break;
            case DataGridViewContentAlignment.MiddleLeft:
                if (0 > dy)
                    rectf.Y -= dy / 2;
                else
                    src.Y = dy / 2;

                src.X = 0;
                break;
            case DataGridViewContentAlignment.MiddleRight:
                if (0 > dy)
                    rectf.Y -= dy / 2;
                else
                    src.Y = dy / 2;

                if (0 > dx)
                    rectf.X -= dx;
                else
                    src.X = dx;

                break;
            case DataGridViewContentAlignment.TopCenter:
                src.Y = 0;
                if (0 > dx)
                    rectf.X -= dx / 2;
                else
                    src.X = dx / 2;

                break;
            case DataGridViewContentAlignment.TopLeft:
                src.Y = 0;
                src.X = 0;
                break;
            case DataGridViewContentAlignment.TopRight:
                src.Y = 0;
                if (0 > dx)
                    rectf.X -= dx;
                else
                    src.X = dx;

                break;
            case DataGridViewContentAlignment.NotSet:
                if (0 > dy)
                    rectf.Y -= dy / 2;
                else
                    src.Y = dy / 2;

                if (0 > dx)
                    rectf.X -= dx / 2;
                else
                    src.X = dx / 2;

                break;
        }

        // Now we can draw our image
        g.DrawImage(img, rectf, src, GraphicsUnit.Pixel);
    }

    /// <summary>
    ///     Allow override of cell drawing. This is to support grids that have onPaint
    ///     overridden to do things like images in header rows and vertical printing
    /// </summary>
    /// <param name="g"></param>
    /// <param name="rowindex"></param>
    /// <param name="columnindex"></param>
    /// <param name="rectf"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    private bool DrawOwnerDrawCell(Graphics g, int rowindex, int columnindex, RectangleF rectf,
        DataGridViewCellStyle style)
    {
        DgvCellDrawingEventArgs args = new(g, rectf, style,
            rowindex, columnindex);
        OnCellOwnerDraw(args);
        return args.Handled;
    }


    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    // Print Process Interface Methods
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------

    // NOTE: This is retained only for backward compatibility, and should 
    // not be used for printing grid views that might be larger than the 
    // input print area.
    public bool EmbeddedPrint(DataGridView dgv, Graphics g, Rectangle area)
    {
        if (EnableLogging) _logger.LogInfoMsg("EmbeddedPrint process started");

        // set the embedded print flag
        _embeddedPrinting = true;

        // save the grid we're printing
        _dgv = dgv ?? throw new Exception("Null Parameter passed to DGVPrinter.");

        //-----------------------------------------------------------------
        // Force setting for embedded printing
        //-----------------------------------------------------------------

        // set margins so we print within the provided area
        _ = PrintMargins;
        PrintMargins.Top = area.Top;
        PrintMargins.Bottom = 0;
        PrintMargins.Left = area.Left;
        PrintMargins.Right = 0;

        // set "page" height and width to our destination area
        _pageHeight = area.Height + area.Top;
        _printWidth = area.Width;
        _pageWidth = area.Width + area.Left;

        // force 'off' header and footer
        PrintHeader = false;
        PrintFooter = false;
        PageNumbers = false;

        //-----------------------------------------------------------------
        // Determine what's going to be printed and set the columns to print
        //-----------------------------------------------------------------
        SetupPrint();

        //-----------------------------------------------------------------
        // Do a single "Print" and return false - we're just printing what
        // we can in the space provided.
        //-----------------------------------------------------------------
        PrintPage(g);
        return false;
    }

    public void EmbeddedPrintMultipageSetup(DataGridView dgv, Rectangle area)
    {
        if (EnableLogging) _logger.LogInfoMsg("EmbeddedPrintMultipageSetup process started");

        // set the embedded print flag
        _embeddedPrinting = true;

        // save the grid we're printing
        _dgv = dgv ?? throw new Exception("Null Parameter passed to DGVPrinter.");

        //-----------------------------------------------------------------
        // Force setting for embedded printing
        //-----------------------------------------------------------------

        // set margins so we print within the provided area
        _ = PrintMargins;
        PrintMargins.Top = area.Top;
        PrintMargins.Bottom = 0;
        PrintMargins.Left = area.Left;
        PrintMargins.Right = 0;

        // set "page" height and width to our destination area
        _pageHeight = area.Height + area.Top;
        _printWidth = area.Width;
        _pageWidth = area.Width + area.Left;

        // force 'off' header and footer
        PrintHeader = false;
        PrintFooter = false;
        PageNumbers = false;

        //-----------------------------------------------------------------
        // Determine what's going to be printed and set the columns to print
        //-----------------------------------------------------------------
        SetupPrint();
    }


    /// <summary>
    ///     Scan all the rows and columns to be printed and calculate the
    ///     overall individual column width (based on largest column value),
    ///     the header sizes, and determine all the row heights.
    /// </summary>
    /// <param name="g">The graphics context for all measurements</param>
    private void Measureprintarea(Graphics g)
    {
        int i, j;
        _colwidths = new List<float>(_colstoprint.Count);
        _footerHeight = 0;

        // temp variables
        DataGridViewColumn col;
        DataGridViewRow row;

        //-----------------------------------------------------------------
        // measure the page headers and footers, including the grid column header cells
        //-----------------------------------------------------------------

        // set initial column sizes based on column titles
        for (i = 0; i < _colstoprint.Count; i++)
        {
            col = (DataGridViewColumn)_colstoprint[i];

            //-------------------------------------------------------------
            // Build String format and Cell style
            //-------------------------------------------------------------

            // get gridview style, and override if we have a set style for this column
            StringFormat currentformat = null;
            var headercolstyle = col.HeaderCell.InheritedStyle.Clone();
            if (ColumnHeaderStyles.ContainsKey(col.Name))
            {
                headercolstyle = ColumnHeaderStyles[col.Name];

                // build the cell style and font 
                Buildstringformat(ref currentformat, headercolstyle, _cellformat.Alignment,
                    _cellformat.LineAlignment,
                    _cellformat.FormatFlags, _cellformat.Trimming);
            }
            else if (col.HasDefaultCellStyle)
            {
                // build the cell style and font 
                Buildstringformat(ref currentformat, headercolstyle, _cellformat.Alignment,
                    _cellformat.LineAlignment,
                    _cellformat.FormatFlags, _cellformat.Trimming);
            }
            else
            {
                currentformat = _columnheadercellformat;
            }

            //-------------------------------------------------------------
            // Calculate and accumulate column header width and height
            //-------------------------------------------------------------
            SizeF size = col.HeaderCell.Size;

            // deal with overridden col widths
            float usewidth;
            if (0 <= _colwidthsoverride[i])
                //usewidth = colwidthsoverride[i];
            {
                _colwidths.Add(_colwidthsoverride[i]); // override means set that size
            }
            else if (ColumnWidthSetting.CellWidth == ColumnWidth || ColumnWidthSetting.Porportional == ColumnWidth)
            {
                usewidth = col.HeaderCell.Size.Width;
                // calculate the size of column header cells
                size = Calccellsize(g, col.HeaderCell, headercolstyle, usewidth, _colwidthsoverride[i],
                    _columnheadercellformat);
                _colwidths.Add(col.Width); // otherwise use the data width
            }
            else
            {
                usewidth = _printWidth;
                // calculate the size of column header cells
                size = Calccellsize(g, col.HeaderCell, headercolstyle, usewidth, _colwidthsoverride[i],
                    _columnheadercellformat);
                _colwidths.Add(size.Width);
            }

            // accumulate heights, saving largest for data sized option
            if (RowHeightSetting.DataHeight == RowHeight)
                _colheaderheight = _colheaderheight < size.Height ? size.Height : _colheaderheight;
            else
                _colheaderheight = col.HeaderCell.Size.Height;
        }

        //-----------------------------------------------------------------
        // measure the page number
        //-----------------------------------------------------------------

        if (PageNumbers)
            _pagenumberHeight = g.MeasureString("Page", PageNumberFont, _printWidth, _pagenumberformat).Height;

        //-----------------------------------------------------------------
        // Calc height of header.
        // Header height is height of page number, title, subtitle and height of column headers
        //-----------------------------------------------------------------
        if (PrintHeader)
        {
            // calculate title and subtitle heights
            _titleheight = g.MeasureString(_title, TitleFont, _printWidth, _titleformat).Height;
            _subtitleheight = g.MeasureString(SubTitle, SubTitleFont, _printWidth, _subtitleformat).Height;
        }

        //-----------------------------------------------------------------
        // measure the footer, if one is provided. Include the page number if we're printing
        // it on the bottom
        //-----------------------------------------------------------------
        if (PrintFooter)
        {
            if (!string.IsNullOrEmpty(Footer))
                _footerHeight += g.MeasureString(Footer, FooterFont, _printWidth, _footerformat).Height;

            _footerHeight += FooterSpacing;
        }

        //-----------------------------------------------------------------
        // Calculate column widths, adjusting for porportional columns
        // and datawidth columns. Row heights are calculated later
        //-----------------------------------------------------------------
        for (i = 0; i < _rowstoprint.Count; i++)
        {
            row = (DataGridViewRow)_rowstoprint[i].Row;

            // add row headers if they're visible
#pragma warning disable CS8629 // Nullable value type may be null.
            if ((bool)PrintRowHeaders)
            {
                // provide a default 'blank' value to prevent a 0 length if we're supposed to show
                // row headers
                var rowheadertext = string.IsNullOrEmpty(row.HeaderCell.FormattedValue.ToString())
                    ? RowHeaderCellDefaultText
                    : row.HeaderCell.FormattedValue.ToString();

                var rhsize = g.MeasureString(rowheadertext,
                    row.HeaderCell.InheritedStyle.Font);
                _rowheaderwidth = _rowheaderwidth < rhsize.Width ? rhsize.Width : _rowheaderwidth;
            }
#pragma warning restore CS8629 // Nullable value type may be null.

            // calculate widths for each column. We're looking for the largest width needed for
            // all the rows of data.
            for (j = 0; j < _colstoprint.Count; j++)
            {
                col = (DataGridViewColumn)_colstoprint[j];

                //-------------------------------------------------------------
                // Build string format and cell style 
                //-------------------------------------------------------------

                // get gridview style, and override if we have a set style for this column
                StringFormat currentformat = null;
#pragma warning disable CS8604 // Possible null reference argument.
                var colstyle = GetStyle(row, col); // = row.Cells[col.Index].InheritedStyle.Clone();
#pragma warning restore CS8604 // Possible null reference argument.

                // build the cell style and font 
                Buildstringformat(ref currentformat, colstyle, _cellformat.Alignment, _cellformat.LineAlignment,
                    _cellformat.FormatFlags, _cellformat.Trimming);

                //-------------------------------------------------------------
                // Calculate and accumulate cell widths and heights
                //-------------------------------------------------------------
                float basewidth;

                // get the default width, depending on overrides. Only calculate data
                // sizes for DataWidth column setting.
                if (0 <= _colwidthsoverride[j])
                    // set overridden column width
                {
                    basewidth = _colwidthsoverride[j];
                }
                else if (ColumnWidthSetting.CellWidth == ColumnWidth ||
                         ColumnWidthSetting.Porportional == ColumnWidth)
                    // set default to same as title cell width
                {
                    basewidth = _colwidths[j];
                }
                else
                {
                    // limit to one page
                    basewidth = _printWidth;

                    // remove padding
                    basewidth -= colstyle.Padding.Left + colstyle.Padding.Right;

                    // calc cell size
                    var size = Calccellsize(g, row.Cells[col.Index], colstyle,
                        basewidth, _colwidthsoverride[j], currentformat);

                    basewidth = size.Width;
                }

                // if width is not overridden and we're using data width then accumulate column widths
                if (!(0 <= _colwidthsoverride[j]) && ColumnWidthSetting.DataWidth == ColumnWidth)
                    _colwidths[j] = _colwidths[j] < basewidth ? basewidth : _colwidths[j];
            }
        }

        //-----------------------------------------------------------------
        // Break the columns accross page sets. This is the key to printing
        // where the total width is wider than one page.
        //-----------------------------------------------------------------

        // assume everything will fit on one page
        _pagesets = new List<PageDef>
        {
            new(PrintMargins, _colstoprint.Count, _pageWidth)
        };
        var pset = 0;

        // Account for row headers 
        _pagesets[pset].Coltotalwidth = _rowheaderwidth;

        // account for 'fixed' columns - these appear on every pageset
        for (j = 0; j < _fixedcolumns.Count; j++)
        {
            var fixedcol = _fixedcolumns[j];
            _pagesets[pset].Columnindex.Add(fixedcol);
#pragma warning disable CS8604 // Possible null reference argument.
            _pagesets[pset].Colstoprint.Add(_colstoprint[fixedcol]);
#pragma warning restore CS8604 // Possible null reference argument.
            _pagesets[pset].Colwidths.Add(_colwidths[fixedcol]);
            _pagesets[pset].Colwidthsoverride.Add(_colwidthsoverride[fixedcol]);
            _pagesets[pset].Coltotalwidth += _colwidthsoverride[fixedcol] >= 0
                ? _colwidthsoverride[fixedcol]
                : _colwidths[fixedcol];
        }

        // check on fixed columns
        if (_printWidth < _pagesets[pset].Coltotalwidth)
            throw new Exception("Fixed column widths exceed the page width.");

        // split remaining columns into page sets
        float columnwidth;
        for (i = 0; i < _colstoprint.Count; i++)
        {
            // skip 'fixed' columns since we've already accounted for them
            if (_fixedcolumns.Contains(i)) continue;

            // get initial column width
            columnwidth = _colwidthsoverride[i] >= 0
                ? _colwidthsoverride[i]
                : _colwidths[i];

            // See if the column width takes us off the page - Except for the 
            // first column. This will prevent printing an empty page!! Otherwise,
            // columns longer than the page width are printed on their own page
            if (_printWidth < _pagesets[pset].Coltotalwidth + columnwidth && i != 0)
            {
                _pagesets.Add(new PageDef(PrintMargins, _colstoprint.Count, _pageWidth));
                pset++;

                // Account for row headers 
                _pagesets[pset].Coltotalwidth = _rowheaderwidth;

                // account for 'fixed' columns - these appear on every pageset
                for (j = 0; j < _fixedcolumns.Count; j++)
                {
                    var fixedcol = _fixedcolumns[j];
                    _pagesets[pset].Columnindex.Add(fixedcol);
#pragma warning disable CS8604 // Possible null reference argument.
                    _pagesets[pset].Colstoprint.Add(_colstoprint[fixedcol]);
#pragma warning restore CS8604 // Possible null reference argument.
                    _pagesets[pset].Colwidths.Add(_colwidths[fixedcol]);
                    _pagesets[pset].Colwidthsoverride.Add(_colwidthsoverride[fixedcol]);
                    _pagesets[pset].Coltotalwidth += _colwidthsoverride[fixedcol] >= 0
                        ? _colwidthsoverride[fixedcol]
                        : _colwidths[fixedcol];
                }

                // check on fixed columns
                if (_printWidth < _pagesets[pset].Coltotalwidth)
                    throw new Exception("Fixed column widths exceed the page width.");
            }

            // update page set definition 
            _pagesets[pset].Columnindex.Add(i);
#pragma warning disable CS8604 // Possible null reference argument.
            _pagesets[pset].Colstoprint.Add(_colstoprint[i]);
#pragma warning restore CS8604 // Possible null reference argument.
            _pagesets[pset].Colwidths.Add(_colwidths[i]);
            _pagesets[pset].Colwidthsoverride.Add(_colwidthsoverride[i]);
            _pagesets[pset].Coltotalwidth += columnwidth;
        }

        // for right to left language, reverse the column order for each page set
        if (RightToLeft.Yes == _dgv.RightToLeft)
            for (pset = 0; pset < _pagesets.Count; pset++)
            {
                _pagesets[pset].Columnindex.Reverse();
                _pagesets[pset].Colstoprint.Reverse();
                _pagesets[pset].Colwidths.Reverse();
                _pagesets[pset].Colwidthsoverride.Reverse();
            }

        for (i = 0; i < _pagesets.Count; i++)
        {
            var pageset = _pagesets[i];
            if (EnableLogging)
            {
                var columnlist = "";

                _logger.LogInfoMsg(
                    string.Format("PageSet {0} Information ----------------------------------------------", i));

                // list out all the columns printed on this page since we may have fixed columns to account for
                for (var k = 0; k < pageset.Colstoprint.Count; k++)
                    columnlist = string.Format("{0},{1}", columnlist,
                        ((DataGridViewColumn)pageset.Colstoprint[k]).Index);

                _logger.LogInfoMsg(string.Format("Measured columns {0}", columnlist[1..]));
                columnlist = "";

                // list original column widths for this page
                for (var k = 0; k < pageset.Colstoprint.Count; k++)
                    columnlist = string.Format("{0},{1}", columnlist, pageset.Colwidths[k]);

                _logger.LogInfoMsg(string.Format("Original Column Widths: {0}", columnlist[1..]));
                columnlist = "";

                // list column width override values
                for (var k = 0; k < pageset.Colstoprint.Count; k++)
                    columnlist = string.Format("{0},{1}", columnlist, pageset.Colwidthsoverride[k]);

                _logger.LogInfoMsg(string.Format("Overridden Column Widths: {0}", columnlist[1..]));
            }

            //-----------------------------------------------------------------
            // Adjust column widths and table margins for each page
            //-----------------------------------------------------------------
            AdjustPageSets(g, pageset);

            //-----------------------------------------------------------------
            // Log Pagesets 
            //-----------------------------------------------------------------
            if (EnableLogging)
            {
                var columnlist = "";

                // list final column widths for this page
                for (var k = 0; k < pageset.Colstoprint.Count; k++)
                    columnlist = string.Format("{0},{1}", columnlist, pageset.Colwidths[k]);

                _logger.LogInfoMsg(string.Format("Final Column Widths: {0}", columnlist[1..]));
                _logger.LogInfoMsg(string.Format(
                    "pageset print width is {0}, total column width to be printed is {1}",
                    pageset.PrintWidth, pageset.Coltotalwidth));
            }
        }
    }

    /// <summary>
    ///     Set page breaks for the rows to be printed, and count total pages
    /// </summary>
    private int Pagination()
    {
        var newpage = Paging.Keepgoing;

        //// if we're printing by pages, the total pages is the last page to 
        //// print
        //if (toPage < maxPages)
        //    return toPage;

        // Start counting pages at 1
        _currentPage = 1;

        // Calculate where to stop printing the grid - count up from the bottom of the page.
        _staticheight =
            _pageHeight - FooterHeight - _pagesets[_currentpageset].Margins.Bottom; //PrintMargins.Bottom;

        // add in the page number height - doesn't matter at this point if it's printing on top or bottom
        _staticheight -= PageNumberHeight;

        // Calculate where to start printing the grid for page 1
        var pos = PrintMargins.Top + HeaderHeight;

        // set starting value for 'break on value change' column
        if (!string.IsNullOrEmpty(BreakOnValueChange))
            _oldvalue = _rowstoprint[0].Row.Cells[BreakOnValueChange].EditedFormattedValue;

        // if we're printing by rows, sum up rowheights until we're done.
        for (var currentrow = 0; currentrow < _rowstoprint.Count; currentrow++)
        {
            // end of page: Count the page and reset to top of next page
            if (pos + _rowstoprint[currentrow].Height >= _staticheight) newpage = Paging.Outofroom;

            // if we're breaking on value change in a column then watch that column
            if (!string.IsNullOrEmpty(BreakOnValueChange) &&
                !_oldvalue.Equals(_rowstoprint[currentrow].Row.Cells[BreakOnValueChange].EditedFormattedValue))
            {
                newpage = Paging.Datachange;
                _oldvalue = _rowstoprint[currentrow].Row.Cells[BreakOnValueChange].EditedFormattedValue;
            }

            // if we need to start a new page, count it and reset counters
            if (newpage != Paging.Keepgoing)
            {
                // note page break
                _rowstoprint[currentrow].Pagebreak = true;

                // count the page
                _currentPage++;

                // if we're printing by pages, stop when we pass our limit
                if (_currentPage > _toPage)
                    // we're done
                    return _toPage;

                // reset the counter - depending on setting
                if (KeepRowsTogether
                    || newpage == Paging.Datachange
                    || (newpage == Paging.Outofroom && _staticheight - pos < KeepRowsTogetherTolerance))
                {
                    // if we are keeping rows together and too little would be showing, put whole row on next page
                    pos = _rowstoprint[currentrow].Height;
                }
                else
                {
                    // note page split
                    _rowstoprint[currentrow].Splitrow = true;

                    // if we're not keeping rows together, only put remainder on next page
                    pos = pos + _rowstoprint[currentrow].Height - _staticheight;
                }

                // Recalculate where to stop printing the grid because available space can change w/ dynamic header/footers.
                _staticheight =
                    _pageHeight - FooterHeight - _pagesets[_currentpageset].Margins.Bottom; //PrintMargins.Bottom;

                // add in the page number height - doesn't matter at this point if it's printing on top or bottom
                _staticheight += PageNumberHeight;


                // account for static space at the top of the page
                pos += PrintMargins.Top + HeaderHeight + PageNumberHeight;
            }
            else
            {
                // add row space
                pos += _rowstoprint[currentrow].Height;
            }

            // reset flag
            newpage = Paging.Keepgoing;
        }

        // return counted pages
        return _currentPage;
    }

    /// <summary>
    ///     Print the column headers. Most printing format info is retrieved from the
    ///     source DataGridView.
    /// </summary>
    /// <param name="g">Graphics Context to print within</param>
    /// <param name="pos">Track vertical space used; 'y' location</param>
    /// <param name="pageset">Current pageset - defines columns and margins</param>
    private void Printcolumnheaders(Graphics g, ref float pos, PageDef pageset)
    {
        // track printing location accross the page. start position is hard left,
        // adjusted for the row headers. Note rowheaderwidth is 0 if row headers are not printed
        var xcoord = pageset.Margins.Left + _rowheaderwidth;

        // set the pen for drawing the grid lines
        Pen lines = new(_dgv.GridColor, 1);

        //-----------------------------------------------------------------
        // Print the column headers
        //-----------------------------------------------------------------
        DataGridViewColumn col;
        for (var i = 0; i < pageset.Colstoprint.Count; i++)
        {
            col = (DataGridViewColumn)pageset.Colstoprint[i];

            // calc cell width, account for columns larger than the print area!
            var cellwidth = pageset.Colwidths[i] > pageset.PrintWidth - _rowheaderwidth
                ? pageset.PrintWidth - _rowheaderwidth
                : pageset.Colwidths[i];

            // get column style
            var style = col.HeaderCell.InheritedStyle.Clone();
            if (ColumnHeaderStyles.ContainsKey(col.Name)) style = ColumnHeaderStyles[col.Name];

            // set print area for this individual cell, account for cells larger
            // than the print area!
            RectangleF cellprintarea = new(xcoord, pos, cellwidth, _colheaderheight);

            DrawCell(g, cellprintarea, style, col.HeaderCell, 0, _columnheadercellformat, lines);

            xcoord += pageset.Colwidths[i];
        }

        // all done, consume "used" vertical space, including space for border lines
        pos += _colheaderheight +
               (_dgv.ColumnHeadersBorderStyle != DataGridViewHeaderBorderStyle.None ? lines.Width : 0);
    }


    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    // Primary Interface - Presents a dialog and then prints or previews the 
    // indicated data grid view
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------

    /// <summary>
    ///     Start the printing process, print to a printer.
    /// </summary>
    /// <param name="dgv">The DataGridView to print</param>
    /// NOTE: Any changes to this method also need to be done in PrintPreviewDataGridView
    public void PrintDataGridView(DataGridView dgv)
    {
        if (EnableLogging) _logger.LogInfoMsg("PrintDataGridView process started");

        if (null == dgv) throw new Exception("Null Parameter passed to DGVPrinter.");

        if (!typeof(DataGridView).IsInstanceOfType(dgv)) throw new Exception("Invalid Parameter passed to DGVPrinter.");

        // save the datagridview we're printing
        _dgv = dgv;

        // display dialog and print
        if (DialogResult.OK == DisplayPrintDialog()) PrintNoDisplay(dgv);
    }

    /// <summary>
    ///     Print the footer. This handles the footer spacing, and printing the page number
    ///     at the bottom of the page (if the page number is not in the header).
    /// </summary>
    /// <param name="g">Graphic context to print in</param>
    /// <param name="pos">Track vertical space used; 'y' location</param>
    /// <param name="margins">The table's print margins</param>
    private void Printfooter(Graphics g, ref float pos, PageDef pageset)
    {
        // print the footer
        Printsection(g, ref pos, Footer, FooterFont, FooterColor, _footerformat,
            _overridefooterformat, pageset, FooterBackground, FooterBorder);
    }

    /// <summary>
    ///     Print the provided grid view. Either DisplayPrintDialog() or it's equivalent
    ///     setup must be completed prior to calling this routine
    /// </summary>
    /// <param name="dgv"></param>
    public void PrintNoDisplay(DataGridView dgv)
    {
        if (EnableLogging) _logger.LogInfoMsg("PrintNoDisplay process started");

        if (null == dgv) throw new Exception("Null Parameter passed to DGVPrinter.");

        if (!(dgv is not null)) throw new Exception("Invalid Parameter passed to DGVPrinter.");

        // save the grid we're printing
        _dgv = dgv;

        PrintDocument.PrintPage += PrintPageEventHandler;
        PrintDocument.BeginPrint += BeginPrintEventHandler;

        try
        {
            // setup and do printing
            SetupPrint();
            PrintDocument.Print();
        }
        finally
        {
            // Unsubscribe from events to prevent freezing or memory leaks
            PrintDocument.PrintPage -= PrintPageEventHandler;
            PrintDocument.BeginPrint -= BeginPrintEventHandler;
        }
    }

    /// <summary>
    ///     This routine prints one page. It will skip non-printable pages if the user
    ///     selected the "some pages" option on the print dialog. This is called during
    ///     the Print event.
    /// </summary>
    /// <param name="g">Graphics object to print to</param>
    private bool PrintPage(Graphics g)
    {
        // for tracing and logging purposes
        var firstrow = 0;

        // flag for continuing or ending print process
        var hasMorePages = false;

        // flag for handling printing some pages rather than all
        var printthispage = false;

        // current printing position within one page
        float printpos = _pagesets[_currentpageset].Margins.Top;

        // increment page number & check page range
        _currentPage++;
        if (EnableLogging)
            _logger.LogInfoMsg(
                string.Format("Print Page processing page {0} -----------------------", _currentPage));

        if (_currentPage >= _fromPage && _currentPage <= _toPage) printthispage = true;

        // calculate the static vertical space available - this is where we stop printing rows
        // Note: leave room for the page number if it's on the bottom
        _staticheight = _pageHeight - FooterHeight - _pagesets[_currentpageset].Margins.Bottom;
        if (!PageNumberInHeader) _staticheight -= PageNumberHeight;

        // count space used as we work our way down the page
        float used = 0;

        // current row information block
        Rowdata thisrow = null;

        // next row (lookahead) information block
        Rowdata nextrow = null;

        //-----------------------------------------------------------------
        // scan down heights until we're off this (non-printing) page
        //-----------------------------------------------------------------

        while (!printthispage)
        {
            if (EnableLogging)
                _logger.LogInfoMsg(string.Format("Print Page skipping page {0} part {1}", _currentPage,
                    _currentpageset + 1));

            // calculate and increment over the page we're not printing
            printpos = _pagesets[_currentpageset].Margins.Top + HeaderHeight + PageNumberHeight;

            // are we done with this page?
            var pagecomplete = false;
            _currentrow = _lastrowprinted + 1;

            // for logging
            firstrow = _currentrow;

            do
            {
                thisrow = _rowstoprint[_currentrow];

                // this is how much space this row will use on this page
                used = thisrow.Height - _rowstartlocation > _staticheight - printpos
                    ? _staticheight - printpos
                    : thisrow.Height - _rowstartlocation;
                printpos += used;

                // Now, look at the next row and start checking on whether or not we're out of room & need to count a page
                _lastrowprinted++;
                _currentrow++;
                nextrow = _currentrow < _rowstoprint.Count ? _rowstoprint[_currentrow] : null;
                if (null != nextrow && nextrow.Pagebreak) // pagebreak before the next row
                {
                    pagecomplete = true;

                    if (nextrow.Splitrow)
                        // account for the partial row that would go on this page
                        _rowstartlocation += nextrow.Height - _rowstartlocation > _staticheight - printpos
                            ? _staticheight - printpos
                            : nextrow.Height - _rowstartlocation;
                }
                else
                {
                    // completed a row, so reset startlocation and count this row.
                    _rowstartlocation = 0;
                }

                // if we're out of data (no partial rows and no more rows)
                if (0 == _rowstartlocation && _lastrowprinted >= _rowstoprint.Count - 1) pagecomplete = true;
            } while (!pagecomplete);

            // log rows skipped
            if (EnableLogging)
                _logger.LogInfoMsg(string.Format("Print Page skipped rows {0} to {1}", firstrow, _currentrow));

            // skip to the next page & see if it's in the print range
            _currentPage++;

            if (_currentPage >= _fromPage && _currentPage <= _toPage) printthispage = true;

            // partial row means more to print
            if (0 != _rowstartlocation)
            {
                // we're not done with this row yet
                hasMorePages = true;
            }
            // done with this page set so see if there are any more pagesets to print
            else if (_lastrowprinted >= _rowstoprint.Count - 1 || _currentPage > _toPage)
            {
                // reset for next pageset or tell the caller we're complete
                hasMorePages = DetermineHasMorePages();

                // reset counters since we'll go through this twice if we print from preview
                _lastrowprinted = -1;
                _currentPage = 0;

                return hasMorePages;
            }
        }

        if (EnableLogging)
        {
            _logger.LogInfoMsg(string.Format("Print Page printing page {0} part {1}", _currentPage,
                _currentpageset + 1));
            var m = _pagesets[_currentpageset].Margins;
            _logger.LogInfoMsg(string.Format("Current Margins are {0}, {1}, {2}, {3}", m.Left, m.Right, m.Top,
                m.Bottom));
        }

        //-----------------------------------------------------------------
        // print statically located images
        //-----------------------------------------------------------------

        // print any "absolute" images so that anything else we print will be 'on top'
        ImbeddedImageList.Where(p => p.ImageLocation == Location.Absolute).DrawImbeddedImage(g,
            _pagesets[_currentpageset].PrintWidth,
            _pageHeight, _pagesets[_currentpageset].Margins);

        //-----------------------------------------------------------------
        // print headers
        //-----------------------------------------------------------------

        // reset printpos as it may have changed during the 'skip pages' routine just above.
        printpos = _pagesets[_currentpageset].Margins.Top;

        // Skip headers if the flag is false
        if (PrintHeader)
        {
            // print any "header" images so that anything else we print will be 'on top'
            ImbeddedImageList.Where(p => p.ImageLocation == Location.Header).DrawImbeddedImage(g,
                _pagesets[_currentpageset].PrintWidth,
                _pageHeight, _pagesets[_currentpageset].Margins);

            // print page number if user selected it
            if (PageNumberInHeader) printpos = PrintPageNo(g, printpos);

            // print title if provided, & we're not skipping it
            if (0 != TitleHeight && !string.IsNullOrEmpty(_title))
                Printsection(g, ref printpos, _title, TitleFont,
                    TitleColor, _titleformat, _overridetitleformat,
                    _pagesets[_currentpageset],
                    TitleBackground, TitleBorder);

            // account for title spacing
            printpos += TitleHeight;

            // print subtitle if provided
            if (0 != SubTitleHeight && !string.IsNullOrEmpty(SubTitle))
                Printsection(g, ref printpos, SubTitle, SubTitleFont,
                    SubTitleColor, _subtitleformat, _overridesubtitleformat,
                    _pagesets[_currentpageset],
                    SubTitleBackground, SubTitleBorder);

            // account for subtitle spacing
            printpos += SubTitleHeight;
        }

        // print the column headers or not based on our processing flag
#pragma warning disable CS8629 // Nullable value type may be null.
        if ((bool)PrintColumnHeaders)
            // print column headers
            Printcolumnheaders(g, ref printpos, _pagesets[_currentpageset]);
#pragma warning restore CS8629 // Nullable value type may be null.

        //-----------------------------------------------------------------
        // print rows until the page is complete
        //-----------------------------------------------------------------
        var continueprinting = true;
        _currentrow = _lastrowprinted + 1;

        // for logging
        firstrow = _currentrow;

        if (_currentrow >= _rowstoprint.Count)
            // indicate that we're done printing 
            continueprinting = false;

        while (continueprinting)
        {
            thisrow = _rowstoprint[_currentrow];

            // print the part of the row that we can, and accumulate the space used
            used = Printrow(g, printpos, (DataGridViewRow)thisrow.Row,
                _pagesets[_currentpageset], _rowstartlocation);
            printpos += used;

            // Now, start checking on whether or not to print the next row 
            // (or if we even have a next row)
            _lastrowprinted++;
            _currentrow++;
            nextrow = _currentrow < _rowstoprint.Count ? _rowstoprint[_currentrow] : null;
            if (null != nextrow && nextrow.Pagebreak)
            {
                continueprinting = false;

                // print a partial row before breaking
                if (nextrow.Splitrow)
                    // print what we can on this page, print the remainder on the next page
                    _rowstartlocation += Printrow(g, printpos, (DataGridViewRow)nextrow.Row,
                        _pagesets[_currentpageset], _rowstartlocation);
            }
            else
            {
                // completed a row, so reset startlocation.
                _rowstartlocation = 0;
            }

            // if we're out of data (no partial rows and no more rows)
            if (0 == _rowstartlocation && _lastrowprinted >= _rowstoprint.Count - 1) continueprinting = false;
        }

        // log rows skipped
        if (EnableLogging)
        {
            _logger.LogInfoMsg(string.Format("Print Page printed rows {0} to {1}", firstrow, _currentrow));
            var pageset = _pagesets[_currentpageset];
            var columnlist = "";

            // list out all the columns printed on this page since we may have fixed columns to account for
            for (var i = 0; i < pageset.Colstoprint.Count; i++)
                columnlist = string.Format("{0},{1}", columnlist,
                    ((DataGridViewColumn)pageset.Colstoprint[i]).Index);

            _logger.LogInfoMsg(string.Format("Print Page printed columns {0}", columnlist[1..]));
        }

        //-----------------------------------------------------------------
        // print footer
        //-----------------------------------------------------------------
        if (PrintFooter)
        {
            // print any "footer" images so that anything else we print will be 'on top'
            ImbeddedImageList.Where(p => p.ImageLocation == Location.Footer).DrawImbeddedImage(g,
                _pagesets[_currentpageset].PrintWidth,
                _pageHeight, _pagesets[_currentpageset].Margins);

            //Note: need to force printpos to the bottom of the page
            // as we may have run out of data anywhere on the page
            printpos = _pageHeight - _footerHeight - _pagesets[_currentpageset].Margins.Bottom; // - margins.Top

            // add spacing
            printpos += FooterSpacing;

            // print the page number if it's on the bottom.
            if (!PageNumberInHeader) printpos = PrintPageNo(g, printpos);

            if (0 != FooterHeight) Printfooter(g, ref printpos, _pagesets[_currentpageset]);
        }

        //-----------------------------------------------------------------
        // bottom check, see if this is the last page to print
        //-----------------------------------------------------------------

        // partial row means more to print
        if (0 != _rowstartlocation)
            // we're not done with this row yet
            hasMorePages = true;

        // done with this page set so see if there are any more pagesets to print
        if (_currentPage >= _toPage || _lastrowprinted >= _rowstoprint.Count - 1)
        {
            // reset for next pageset or tell the caller we're complete
            hasMorePages = DetermineHasMorePages();

            // reset counters since we'll go through this twice if we print from preview
            _rowstartlocation = 0;
            _lastrowprinted = -1;
            _currentPage = 0;
        }
        else
        {
            // we're not done yet
            hasMorePages = true;
        }

        return hasMorePages;
    }

    /// <summary>
    ///     PrintPage event handler. This routine prints one page. It will
    ///     skip non-printable pages if the user selected the "some pages" option
    ///     on the print dialog.
    /// </summary>
    /// <param name="sender">default object from windows</param>
    /// <param name="e">Event info from Windows about the printing</param>
    public void PrintPageEventHandler(object sender, PrintPageEventArgs e)
    {
        if (EnableLogging) _logger.LogInfoMsg("PrintPageEventHandler called. Printing a page.");
#pragma warning disable CS8604 // Possible null reference argument.
        e.HasMorePages = PrintPage(e.Graphics);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    /// <summary>
    ///     Print the page number
    /// </summary>
    /// <param name="g"></param>
    /// <param name="printpos"></param>
    /// <returns></returns>
    private float PrintPageNo(Graphics g, float printpos)
    {
        if (PageNumbers)
        {
            var pagenumber = PageText + _currentPage.ToString(CultureInfo.CurrentCulture);
            if (ShowTotalPageNumber) pagenumber += PageSeparator + _totalpages.ToString(CultureInfo.CurrentCulture);

            if (1 < _pagesets.Count)
                pagenumber += PartText + (_currentpageset + 1).ToString(CultureInfo.CurrentCulture);

            // ... then print it
            Printsection(g, ref printpos,
                pagenumber, PageNumberFont, PageNumberColor, _pagenumberformat,
                _overridepagenumberformat, _pagesets[_currentpageset],
                null, null);

            // if the page number is not on a separate line, don't "use up" it's vertical space
            if (PageNumberOnSeparateLine) printpos += _pagenumberHeight;
        }

        return printpos;
    }

    /// <summary>
    ///     Start the printing process, print to a print preview dialog
    /// </summary>
    /// <param name="dgv">The DataGridView to print</param>
    /// NOTE: Any changes to this method also need to be done in PrintDataGridView
    public void PrintPreviewDataGridView(DataGridView dgv)
    {
        if (EnableLogging) _logger.LogInfoMsg("PrintPreviewDataGridView process started");

        if (null == dgv) throw new Exception("Null Parameter passed to DGVPrinter.");

        if (!typeof(DataGridView).IsInstanceOfType(dgv)) throw new Exception("Invalid Parameter passed to DGVPrinter.");

        // save the datagridview we're printing
        _dgv = dgv;

        // display dialog and print
        if (DialogResult.OK == DisplayPrintDialog()) PrintPreviewNoDisplay(dgv);
    }


    /// <summary>
    ///     Preview the provided grid view. Either DisplayPrintDialog() or it's equivalent
    ///     setup must be completed prior to calling this routine
    /// </summary>
    /// <param name="dgv"></param>
    public void PrintPreviewNoDisplay(DataGridView dgv)
    {
        if (EnableLogging) _logger.LogInfoMsg("PrintPreviewNoDisplay process started");

        if (null == dgv) throw new Exception("Null Parameter passed to DGVPrinter.");

        if (!(dgv is not null)) throw new Exception("Invalid Parameter passed to DGVPrinter.");

        // save the grid we're printing
        _dgv = dgv;

        PrintDocument.PrintPage += PrintPageEventHandler;
        PrintDocument.BeginPrint += BeginPrintEventHandler;

        // display the preview dialog
        SetupPrint();

        // if the caller hasn't provided a print preview dialog, then create one
        if (null == PreviewDialog) PreviewDialog = new PrintPreviewDialog();

        // set up dialog for preview
        PreviewDialog.Document = PrintDocument;
        PreviewDialog.UseAntiAlias = true;
        PreviewDialog.Owner = Owner;
        PreviewDialog.PrintPreviewControl.Zoom = PrintPreviewZoom;
        PreviewDialog.Width = PreviewDisplayWidth();
        PreviewDialog.Height = PreviewDisplayHeight();

        if (null != PreviewDialogIcon) PreviewDialog.Icon = PreviewDialogIcon;

        // show the dialog
        PreviewDialog.ShowDialog();
    }

    /// <summary>
    ///     Print one row of the DataGridView. Most printing format info is retrieved
    ///     from the DataGridView.
    /// </summary>
    /// <param name="g">Graphics Context to print within</param>
    /// <param name="pos">Track vertical space used; 'y' location</param>
    /// <param name="row">The row that will be printed</param>
    /// <param name="pageset">Current Pageset - defines columns and margins</param>
    /// <param name="startline">Line no. in row to start printing text at</param>
    private float Printrow(Graphics g, float finalpos, DataGridViewRow row, PageDef pageset,
        float startlocation)
    {
        // track printing location accross the page
        float xcoord = pageset.Margins.Left;
        var pos = finalpos;

        // set the pen for drawing the grid lines
        Pen lines = new(_dgv.GridColor, 1);

        // calc row width, account for columns wider than the print area!
        var rowwidth = pageset.Coltotalwidth > pageset.PrintWidth ? pageset.PrintWidth : pageset.Coltotalwidth;

        // calc row heigth in pixels to print
        var rowheight = _rowstoprint[_currentrow].Height - startlocation > _staticheight - pos
            ? _staticheight - pos
            : _rowstoprint[_currentrow].Height - startlocation;

        //-----------------------------------------------------------------
        // Print Row background
        //-----------------------------------------------------------------

        // get current row style, and current header style
        var rowstyle = row.InheritedStyle.Clone();
        var headerstyle = row.HeaderCell.InheritedStyle.Clone();

        // define print rectangle
        RectangleF printarea = new(xcoord, pos, rowwidth,
            rowheight);

        // fill in the row background as the default color
        g.FillRectangle(new SolidBrush(rowstyle.BackColor), printarea);

        //-----------------------------------------------------------------
        // Print the Row Headers, if they are visible
        //-----------------------------------------------------------------
#pragma warning disable CS8629 // Nullable value type may be null.
        if ((bool)PrintRowHeaders)
        {
            // set print area for this individual cell
            RectangleF headercellprintarea = new(xcoord, pos,
                _rowheaderwidth, rowheight);

            DrawCell(g, headercellprintarea, headerstyle, row.HeaderCell, startlocation,
                _rowheadercellformat, lines);

            // track horizontal space used
            xcoord += _rowheaderwidth;
        }
#pragma warning restore CS8629 // Nullable value type may be null.

        //-----------------------------------------------------------------
        // Print the row: write and draw each cell
        //-----------------------------------------------------------------
        DataGridViewColumn col;
        for (var i = 0; i < pageset.Colstoprint.Count; i++)
        {
            // access the cell and column being printed
            col = (DataGridViewColumn)pageset.Colstoprint[i];
            var cell = row.Cells[col.Index];

            // calc cell width, account for columns larger than the print area!
            var cellwidth = pageset.Colwidths[i] > pageset.PrintWidth - _rowheaderwidth
                ? pageset.PrintWidth - _rowheaderwidth
                : pageset.Colwidths[i];

            // SLG 01112010 - only draw columns with an actual width
            if (cellwidth > 0)
            {
                // get DGV column style and see if we have an override for this column
                StringFormat finalformat = null;
                var colstyle = GetStyle(row, col); // = row.Cells[col.Index].InheritedStyle.Clone(); 

                // set string format
                Buildstringformat(ref finalformat, colstyle, _cellformat.Alignment, _cellformat.LineAlignment,
                    _cellformat.FormatFlags, _cellformat.Trimming);
                _ = colstyle.Font;

                // set overall print area for this individual cell 
                RectangleF cellprintarea = new(xcoord, pos, cellwidth,
                    rowheight);

                DrawCell(g, cellprintarea, colstyle, cell, startlocation, finalformat, lines);
            }

            // track horizontal space used
            xcoord += pageset.Colwidths[i];
        }

        //-----------------------------------------------------------------
        // All done with this row, consume "used" vertical space
        //-----------------------------------------------------------------
        return rowheight;
    }

    /// <summary>
    ///     Print a header or footer section. Used for page numbers and titles
    /// </summary>
    /// <param name="g">Graphic context to print in</param>
    /// <param name="pos">Track vertical space used; 'y' location</param>
    /// <param name="text">String to print</param>
    /// <param name="font">Font to use for printing</param>
    /// <param name="color">Color to print in</param>
    /// <param name="format">String format for text</param>
    /// <param name="useroverride">True if the user overrode the alignment or flags</param>
    /// <param name="margins">The table's print margins</param>
    /// <param name="background">Background fill for the section; may be null for no background</param>
    /// <param name="border">Border for the section; may be null for no border</param>
    private static void Printsection(Graphics g,
        ref float pos,
        string text,
        Font font,
        Color color,
        StringFormat format,
        bool overridetitleformat,
        PageDef pageset,
        Brush background,
        Pen border)
    {
        // measure string
        var printsize = g.MeasureString(text, font, pageset.PrintWidth, format);

        // build area to print within
        RectangleF printarea = new(pageset.Margins.Left, pos, pageset.PrintWidth,
            printsize.Height);

        // draw a background, if a Brush has been provided
        if (null != background) g.FillRectangle(background, printarea);

        // draw a border, if a Pen has been provided
        if (null != border) g.DrawRectangle(border, printarea.X, printarea.Y, printarea.Width, printarea.Height);

        // do the actual print
        g.DrawString(text, font, new SolidBrush(color), printarea, format);
    }

    /// <summary>
    ///     Recalculate row heights for cells whose width is greater than the set column width.
    ///     Called when column widths are changed in order to flow text down the page instead of
    ///     accross.
    /// </summary>
    /// <param name="g">Graphics Context for measuring image columns</param>
    /// <param name="colindex">column index in colstoprint</param>
    /// <param name="newcolwidth">new column width</param>
    private void RecalcRowHeights(Graphics g, int colindex, float newcolwidth)
    {
        // search calculated cell sizes for widths larger than our new width
        for (var i = 0; i < _rowstoprint.Count; i++)
        {
            var cell = ((DataGridViewRow)_rowstoprint[i].Row).Cells[
                ((DataGridViewColumn)_colstoprint[colindex]).Index];

            float finalsize;
            if (RowHeightSetting.DataHeight == RowHeight)
            {
                StringFormat currentformat = null;

                // get column style
#pragma warning disable CS8604 // Possible null reference argument.
                var colstyle = GetStyle((DataGridViewRow)_rowstoprint[i].Row,
                    (DataGridViewColumn)_colstoprint[colindex]);
#pragma warning restore CS8604 // Possible null reference argument.

                // build the cell style and font 
                Buildstringformat(ref currentformat, colstyle, _cellformat.Alignment, _cellformat.LineAlignment,
                    _cellformat.FormatFlags, _cellformat.Trimming);

                // recalculate cell size using new width. This will flow data down the page and 
                // change the row height
                var size = Calccellsize(g, cell, colstyle, newcolwidth, _colwidthsoverride[colindex],
                    currentformat);

                finalsize = size.Height;
            }
            else
            {
                finalsize = cell.Size.Height;
            }

            // change the saved row height based on the recalculated size
            _rowstoprint[i].Height = _rowstoprint[i].Height < finalsize ? finalsize : _rowstoprint[i].Height;
        }
    }

    /// <summary>
    ///     Set up width override and fixed columns lists
    /// </summary>
    private void SetupColumns()
    {
        // identify fixed columns by their column number in the print list
        foreach (var colname in FixedColumns)
            try
            {
                _fixedcolumns.Add(GetColumnIndex(colname));
            }
            catch (Exception)
            {
                // missing column, so add it to print list and retry
                _colstoprint.Add(_dgv.Columns[colname]);
                _fixedcolumns.Add(GetColumnIndex(colname));
            }

        // Adjust override list to have the same number of entries as colstoprint,
        foreach (DataGridViewColumn col in _colstoprint)
            if (ColumnWidths.ContainsKey(col.Name))
                _colwidthsoverride.Add(ColumnWidths[col.Name]);
            else
                _colwidthsoverride.Add(-1);
    }


    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    // Internal Methods
    //---------------------------------------------------------------------
    //---------------------------------------------------------------------

    /// <summary>
    ///     Set up the print job. Save information from print dialog
    ///     and print document for easy access. Also sets up the rows
    ///     and columns that will be printed. At this point, we're
    ///     collecting all columns in colstoprint. This will be broken
    ///     up into pagesets later on
    /// </summary>
    private void SetupPrint()
    {
        if (EnableLogging)
        {
            _logger.LogInfoMsg("SetupPrint process started");
            var m = PrintDocument.DefaultPageSettings.Margins;
            _logger.LogInfoMsg(string.Format("Initial Printer Margins are {0}, {1}, {2}, {3}", m.Left, m.Right,
                m.Top, m.Bottom));
        }

        if (null == PrintColumnHeaders) PrintColumnHeaders = _dgv.ColumnHeadersVisible;

        if (null == PrintRowHeaders) PrintRowHeaders = _dgv.RowHeadersVisible;

        // Set the default row header style where we don't have an override
        // and we do have rows
        if (null == RowHeaderCellStyle && 0 != _dgv.Rows.Count)
            RowHeaderCellStyle = _dgv.Rows[0].HeaderCell.InheritedStyle;

        /* Functionality to come - redo of styling
        foreach (DataGridViewColumn col in dgv.Columns)
        {
            // Set the default column styles where we've not been given an override
            if (!ColumnStyles.ContainsKey(col.Name))
                ColumnStyles[col.Name] = dgv.Columns[col.Name].InheritedStyle;

            // Set the default column header styles where we don't have an override
            if (!ColumnHeaderStyles.ContainsKey(col.Name))
                ColumnHeaderStyles[col.Name] = dgv.Columns[col.Name].HeaderCell.InheritedStyle;
        }
        */

        //-----------------------------------------------------------------
        // Set row and column headercell and normal cell print formats if they were not
        // explicitly set by the caller
        //-----------------------------------------------------------------
        if (null == _columnheadercellformat)
            Buildstringformat(ref _columnheadercellformat, _dgv.Columns[0].HeaderCell.InheritedStyle,
                HeaderCellAlignment, StringAlignment.Near, HeaderCellFormatFlags,
                StringTrimming.Word);

        if (null == _rowheadercellformat)
#pragma warning disable CS8604 // Possible null reference argument.
            Buildstringformat(ref _rowheadercellformat, RowHeaderCellStyle,
                HeaderCellAlignment, StringAlignment.Near, HeaderCellFormatFlags,
                StringTrimming.Word);
#pragma warning restore CS8604 // Possible null reference argument.
        if (null == _cellformat)
            Buildstringformat(ref _cellformat, _dgv.DefaultCellStyle,
                CellAlignment, StringAlignment.Near, CellFormatFlags,
                StringTrimming.Word);

        //-----------------------------------------------------------------
        // get info on the limits of the printer's actual print area available. Convert
        // to int's to work with margins.
        //
        // note: do this only if we're not doing embedded printing.
        //-----------------------------------------------------------------

        if (!_embeddedPrinting)
        {
            int printareawidth;
            var hardx = (int)Math.Round(PrintDocument.DefaultPageSettings.HardMarginX);
            var hardy = (int)Math.Round(PrintDocument.DefaultPageSettings.HardMarginY);
            if (PrintDocument.DefaultPageSettings.Landscape)
                printareawidth = (int)Math.Round(PrintDocument.DefaultPageSettings.PrintableArea.Height);
            else
                printareawidth = (int)Math.Round(PrintDocument.DefaultPageSettings.PrintableArea.Width);

            //-----------------------------------------------------------------
            // set the print area we're working within
            //-----------------------------------------------------------------

            _pageHeight = PrintDocument.DefaultPageSettings.Bounds.Height;
            _pageWidth = PrintDocument.DefaultPageSettings.Bounds.Width;

            //-----------------------------------------------------------------
            // Set the printable area: margins and pagewidth
            //-----------------------------------------------------------------

            // Set initial printer margins 
            PrintMargins = PrintDocument.DefaultPageSettings.Margins;

            // adjust for when the margins are less than the printer's hard x/y limits
            PrintMargins.Right = hardx > PrintMargins.Right ? hardx : PrintMargins.Right;
            PrintMargins.Left = hardx > PrintMargins.Left ? hardx : PrintMargins.Left;
            PrintMargins.Top = hardy > PrintMargins.Top ? hardy : PrintMargins.Top;
            PrintMargins.Bottom = hardy > PrintMargins.Bottom ? hardy : PrintMargins.Bottom;

            // Now, we can calc default print width, again, respecting the printer's limitations
            _printWidth = _pageWidth - PrintMargins.Left - PrintMargins.Right;
            _printWidth = _printWidth > printareawidth ? printareawidth : _printWidth;

            // log margin changes
            if (EnableLogging)
            {
                _logger.LogInfoMsg(string.Format("Printer 'Hard' X limit is {0} and 'Hard' Y limit is {1}", hardx,
                    hardy));
                _logger.LogInfoMsg(string.Format(
                    "Printer height limit is {0} and width limit is {1}, print width is {2}",
                    _pageHeight, _pageWidth, _printWidth));
                _logger.LogInfoMsg(string.Format("Final overall margins are {0}, {1}, {2}, {3}",
                    PrintMargins.Left, PrintMargins.Right, PrintMargins.Top, PrintMargins.Bottom));
                _logger.LogInfoMsg(string.Format("Table Alignment is {0}", TableAlignment.ToString()));
            }
        }

        //-----------------------------------------------------------------
        // Figure out which pages / rows to print
        //-----------------------------------------------------------------

        // save print range 
        _printRange = PrintDocument.PrinterSettings.PrintRange;
        if (EnableLogging) _logger.LogInfoMsg(string.Format("PrintRange is {0}", _printRange));

        // pages to print handles "some pages" option
        if (PrintRange.SomePages == _printRange)
        {
            // set limits to only print some pages
            _fromPage = PrintDocument.PrinterSettings.FromPage;
            _toPage = PrintDocument.PrinterSettings.ToPage;
        }
        else
        {
            // set extremes so that we'll print all pages
            _fromPage = 0;
            _toPage = MaxPages;
        }

        //-----------------------------------------------------------------
        // Determine what's going to be printed
        //-----------------------------------------------------------------
        SetupPrintRange();

        //-----------------------------------------------------------------
        // Set up width overrides and fixed columns
        //-----------------------------------------------------------------
        SetupColumns();

        //-----------------------------------------------------------------
        // Now that we know what we're printing, measure the print area and
        // count the pages.
        //-----------------------------------------------------------------

        // Measure the print area
        Measureprintarea(PrintDocument.PrinterSettings.CreateMeasurementGraphics());

        // Count the pages
        _totalpages = Pagination();
    }

    /// <summary>
    ///     Determine the print range based on dialog selections and user input. The rows
    ///     and columns are sorted to ensure that the rows appear in their correct index
    ///     order and the columns appear in DisplayIndex order to account for added columns
    ///     and re-ordered columns.
    /// </summary>
    private void SetupPrintRange()
    {
        SortedList tempcolstoprint;

        //-----------------------------------------------------------------
        // set up the rows and columns to print
        //
        // Note: The "Selectedxxxx" lists in the datagridview are 'stacks' that
        //  have the selected items pushed in the *in the order they were selected*
        //  i.e. not the order you want to print them in!
        //-----------------------------------------------------------------
        SortedList temprowstoprint;
        // rows to print (handles "selection" and "current page" options
        if (PrintRange.Selection == _printRange)
        {
            _ = new SortedList(_dgv.SelectedCells.Count);
            _ = new SortedList(_dgv.SelectedCells.Count);

            //if DGV has rows selected, it's easy, selected rows and all visible columns
            if (0 != _dgv.SelectedRows.Count)
            {
                _ = new SortedList(_dgv.SelectedRows.Count);
                tempcolstoprint = new SortedList(_dgv.Columns.Count);

                // sort the rows into index order
                temprowstoprint = new SortedList(_dgv.SelectedRows.Count);
                foreach (DataGridViewRow row in _dgv.SelectedRows)
                    if (row.Visible && !row.IsNewRow)
                        temprowstoprint.Add(row.Index, row);

                // sort the columns into display order
                foreach (DataGridViewColumn col in _dgv.Columns)
                    if (col.Visible)
                        tempcolstoprint.Add(col.DisplayIndex, col);
            }
            // if selected columns, then all rows, and selected columns
            else if (0 != _dgv.SelectedColumns.Count)
            {
                temprowstoprint = new SortedList(_dgv.Rows.Count);
                tempcolstoprint = new SortedList(_dgv.SelectedColumns.Count);

                foreach (DataGridViewRow row in _dgv.Rows)
                    if (row.Visible && !row.IsNewRow)
                        temprowstoprint.Add(row.Index, row);

                foreach (DataGridViewColumn col in _dgv.SelectedColumns)
                    if (col.Visible)
                        tempcolstoprint.Add(col.DisplayIndex, col);
            }
            // we just have a bunch of selected cells so we have to do some work
            else
            {
                // set up sorted lists. the selectedcells method does not guarantee
                // that the cells will always be in left-right top-bottom order. 
                temprowstoprint = new SortedList(_dgv.SelectedCells.Count);
                tempcolstoprint = new SortedList(_dgv.SelectedCells.Count);

                // for each selected cell, add unique rows and columns
                int displayindex, colindex, rowindex;
                foreach (DataGridViewCell cell in _dgv.SelectedCells)
                {
                    displayindex = cell.OwningColumn.DisplayIndex;
                    colindex = cell.ColumnIndex;
                    rowindex = cell.RowIndex;

                    // add unique rows
                    if (!temprowstoprint.Contains(rowindex))
                    {
                        var row = _dgv.Rows[rowindex];
                        if (row.Visible && !row.IsNewRow) temprowstoprint.Add(rowindex, _dgv.Rows[rowindex]);
                    }

                    // add unique columns
                    if (!tempcolstoprint.Contains(displayindex))
                        tempcolstoprint.Add(displayindex, _dgv.Columns[colindex]);
                }
            }
        }
        // if current page was selected, print visible columns for the
        // displayed rows                
        else if (PrintRange.CurrentPage == _printRange)
        {
            // create lists
            temprowstoprint = new SortedList(_dgv.DisplayedRowCount(true));
            tempcolstoprint = new SortedList(_dgv.Columns.Count);

            // select all visible rows on displayed page
            for (var i = _dgv.FirstDisplayedScrollingRowIndex;
                 i < _dgv.FirstDisplayedScrollingRowIndex + _dgv.DisplayedRowCount(true);
                 i++)
            {
                var row = _dgv.Rows[i];
                if (row.Visible) temprowstoprint.Add(row.Index, row);
            }

            // select all visible columns
            foreach (DataGridViewColumn col in _dgv.Columns)
                if (col.Visible)
                    tempcolstoprint.Add(col.DisplayIndex, col);
        }
        // this is the default for print all - everything marked visible will be printed
        // this is also used when printing specific pages or page ranges as we won't know
        // what to print until we size all the rows
        else
        {
            temprowstoprint = new SortedList(_dgv.Rows.Count);
            tempcolstoprint = new SortedList(_dgv.Columns.Count);

            // select all visible rows and all visible columns - but don't include the new 'data entry row' 
            foreach (DataGridViewRow row in _dgv.Rows)
                if (row.Visible && !row.IsNewRow)
                    temprowstoprint.Add(row.Index, row);

            // sort the columns into display order
            foreach (DataGridViewColumn col in _dgv.Columns)
                if (col.Visible)
                    tempcolstoprint.Add(col.DisplayIndex, col);
        }

        // move rows and columns into global containers
        _rowstoprint = new List<Rowdata>(temprowstoprint.Count);
        foreach (var item in temprowstoprint.Values) _rowstoprint.Add(new Rowdata { Row = (DataGridViewRow)item });

        _colstoprint = new List<DataGridViewColumn>(tempcolstoprint.Count);
        foreach (var item in tempcolstoprint.Values) _colstoprint.Add(item);

        // remove "hidden" columns from list of columns to print
        foreach (var columnname in HideColumns) _colstoprint.Remove(_dgv.Columns[columnname]);

        if (EnableLogging) _logger.LogInfoMsg(string.Format("Grid Printout Range is {0} columns", _colstoprint.Count));

        if (EnableLogging) _logger.LogInfoMsg(string.Format("Grid Printout Range is {0} rows", _rowstoprint.Count));
    }

    //---------------------------------------------------------------------
    // internal classes/structs
    //---------------------------------------------------------------------

    #region Internal Classes

    // Identify the reason for a new page when tracking rows
    private enum Paging
    {
        Keepgoing,
        Outofroom,
        Datachange
    }

    // Allow the user to provide images that will be printed as either logos in the
    // header and/or footer or watermarked as in printed behind the text.
    public class ImbeddedImage
    {
        public Alignment ImageAlignment { get; set; }
        public Location ImageLocation { get; set; }
        public int ImageX { get; set; }
        public int ImageY { get; set; }
        public Image TheImage { get; set; }

        internal Point Upperleft(int pagewidth, int pageheight, Margins margins)
        {
            // if we've been given an absolute location, just use it
            if (ImageLocation == Location.Absolute) return new Point(ImageX, ImageY);

            var y = ImageLocation switch
            {
                Location.Header => margins.Top,
                Location.Footer => pageheight - TheImage.Height - margins.Bottom,
                _ => throw new ArgumentException($@"Unknown value: {ImageLocation}")
            };
            var x = ImageAlignment switch
            {
                Alignment.Left => margins.Left,
                Alignment.Center => pagewidth / 2 - TheImage.Width / 2 + margins.Left,
                Alignment.Right => pagewidth - TheImage.Width + margins.Left,
                Alignment.NotSet => ImageX,
                _ => throw new ArgumentException($@"Unknown value: {ImageAlignment}")
            };
            return new Point(x, y);
        }
    }

    public IList<ImbeddedImage> ImbeddedImageList = new List<ImbeddedImage>();

    // handle wide-column printing - that is, lists of columns that extend
    // wider than one page width. Columns are broken up into "Page Sets" that
    // are printed one after another until all columns are printed.
    private class PageDef
    {
        public PageDef(Margins m, int count, int pagewidth)
        {
            Columnindex = new List<int>(count);
            Colstoprint = new List<object>(count);
            Colwidths = new List<float>(count);
            Colwidthsoverride = new List<float>(count);
            Coltotalwidth = 0;
            Margins = (Margins)m.Clone();
            _pageWidth = pagewidth;
        }

        private readonly int _pageWidth;
        public readonly List<object> Colstoprint;
        public float Coltotalwidth;
        public readonly List<int> Columnindex;
        public readonly List<float> Colwidths;
        public readonly List<float> Colwidthsoverride;
        public readonly Margins Margins;
        public int PrintWidth => _pageWidth - Margins.Left - Margins.Right;
    }

    private IList<PageDef> _pagesets;
    private int _currentpageset;

    // class to hold settings for the PrintDialog presented to the user during
    // the print process
    public class PrintDialogSettingsClass
    {
        public bool AllowCurrentPage = true;
        public bool AllowPrintToFile = false;
        public bool AllowSelection = true;
        public bool AllowSomePages = true;
        public bool ShowHelp = true;
        public bool ShowNetwork = true;
        public bool UseExDialog = true;
    }

    // class to identify row data for printing
    public class Rowdata
    {
        public float Height;
        public bool Pagebreak;
        public DataGridViewRow Row;
        public bool Splitrow;
    }

    #endregion

    //---------------------------------------------------------------------
    // global variables
    //---------------------------------------------------------------------

    #region global variables

    // the data grid view we're printing
    private DataGridView _dgv;

    // print document

    // logging 
    private LogManager _logger;

    // print status items
    private bool _embeddedPrinting;
    private List<Rowdata> _rowstoprint;
    private IList _colstoprint; // divided into pagesets for printing
    private int _lastrowprinted = -1;
    private int _currentrow = -1;
    private int _fromPage;
    private int _toPage = -1;
    private const int MaxPages = 2147483647;

    // page formatting options
    private int _pageHeight;
    private float _staticheight;
    private float _rowstartlocation;
    private int _pageWidth;
    private int _printWidth;
    private float _rowheaderwidth;
    private int _currentPage;
    private int _totalpages;
    private PrintRange _printRange;

    // calculated values
    //private float headerHeight = 0;
    private float _footerHeight;
    private float _pagenumberHeight;

    private float _colheaderheight;

    //private List<float> rowheights;
    private List<float> _colwidths;

    #endregion

    //---------------------------------------------------------------------
    // properties - settable by user
    //---------------------------------------------------------------------

    #region properties

    #region global properties

    /// <summary>
    ///     Enable logging of of the print process. Default is to log to a file named
    ///     'DGVPrinter_yyyymmdd.Log' in the current directory. Since logging may have
    ///     an impact on performance, it should be used for troubleshooting purposes only.
    /// </summary>
    protected bool Enablelogging;

    public bool EnableLogging
    {
        get => Enablelogging;
        set
        {
            Enablelogging = value;
            if (Enablelogging) _logger = new LogManager(".", "DGVPrinter");
        }
    }

    /// <summary>
    ///     Allow the user to change the logging directory. Setting this enables logging by default.
    /// </summary>
    public string LogDirectory
    {
        get
        {
            if (null != _logger)
                return _logger.BasePath;
            // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
        set
        {
            if (null == _logger) EnableLogging = true;

            _logger.BasePath = value;
        }
    }

    /// <summary>
    ///     OwnerDraw Event declaration. Callers can subscribe to this event to override the
    ///     cell drawing.
    /// </summary>
    public event CellOwnerDrawEventHandler OwnerDraw;

    /// <summary>
    ///     provide an override for the print preview dialog "owner" field
    ///     Note: Changed style for VS2005 compatibility
    /// </summary>
    //public Form Owner
    //{ get; set; }
    protected Form _owner; // Renamed the private field to avoid conflict

    public Form Owner
    {
        get => _owner;
        set => _owner = value;
    }

    /// <summary>
    ///     provide an override for the print preview zoom setting
    ///     Note: Changed style for VS2005 compatibility
    /// </summary>
    //public Double PrintPreviewZoom
    //{ get; set; }
    protected double _printPreviewZoom = 1.0;

    public double PrintPreviewZoom
    {
        get => _printPreviewZoom;
        set => _printPreviewZoom = value;
    }


    /// <summary>
    ///     expose printer settings to allow access to calling program
    /// </summary>
    public PrinterSettings PrintSettings => PrintDocument.PrinterSettings;

    /// <summary>
    ///     expose settings for the PrintDialog displayed to the user
    /// </summary>
    public PrintDialogSettingsClass PrintDialogSettings { get; } = new();

    /// <summary>
    ///     Set Printer Name
    /// </summary>
    public string PrinterName { get; set; }

    /// <summary>
    ///     Allow access to the underlying print document
    /// </summary>
    public PrintDocument PrintDocument { get; set; }

    /// <summary>
    ///     Allow caller to set the upper-left corner icon used
    ///     in the print preview dialog
    /// </summary>
    public Icon PreviewDialogIcon { get; set; } = null;

    /// <summary>
    ///     Allow caller to set print preview dialog
    /// </summary>
    public PrintPreviewDialog PreviewDialog { get; set; }

    /// <summary>
    ///     Flag to control whether or not we print the Page Header
    /// </summary>
    public bool PrintHeader { get; set; } = true;

    /// <summary>
    ///     Determine the height of the header
    /// </summary>
    private float HeaderHeight
    {
        get
        {
            float headerheight = 0;

            // Add in title and subtitle heights - this is sensitive to 
            // wether or not titles are printed on the current page
            // TitleHeight and SubTitleHeight have their respective spacing
            // already included
            headerheight += TitleHeight + SubTitleHeight;

            // Add in column header heights
#pragma warning disable CS8629 // Nullable value type may be null.
            if ((bool)PrintColumnHeaders) headerheight += _colheaderheight;
#pragma warning restore CS8629 // Nullable value type may be null.

            // return calculated height
            return headerheight;
        }
    }

    /// <summary>
    ///     Flag to control whether or not we print the Page Footer
    /// </summary>
    public bool PrintFooter { get; set; } = true;

    /// <summary>
    ///     Flag to control whether or not we print the Column Header line
    /// </summary>
    public bool? PrintColumnHeaders { get; set; }

    /// <summary>
    ///     Flag to control whether or not we print the Column Header line
    ///     Defaults to False to match previous functionality
    /// </summary>
    public bool? PrintRowHeaders { get; set; } = false;

    /// <summary>
    ///     Flag to control whether rows are printed whole or if partial
    ///     rows should be printed to fill the bottom of the page. Turn this
    ///     "Off" (i.e. false) to print cells/rows deeper than one page
    /// </summary>
    public bool KeepRowsTogether { get; set; } = true;

    /// <summary>
    ///     How much of a row must show on the current page before it is
    ///     split when KeepRowsTogether is set to true.
    /// </summary>
    public float KeepRowsTogetherTolerance { get; set; } = 15;

    #endregion

    // Title

    #region title properties

    // override flag
    private bool _overridetitleformat;

    // formatted height of title
    private float _titleheight;

    /// <summary>
    ///     Title for this report. Default is empty.
    /// </summary>
    private string _title;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            if (_docName == null) PrintDocument.DocumentName = value;
        }
    }

    /// <summary>
    ///     Name of the document. Default is report title (can be empty)
    /// </summary>
    private string _docName;

    public string DocName
    {
        get => _docName;
        set
        {
            PrintDocument.DocumentName = value;
            _docName = value;
        }
    }

    /// <summary>
    ///     Font for the title. Default is Tahoma, 18pt.
    /// </summary>
    public Font TitleFont { get; set; }

    /// <summary>
    ///     Foreground color for the title. Default is Black
    /// </summary>
    public Color TitleColor { get; set; }

    /// <summary>
    ///     Allow override of the header cell format object
    /// </summary>
    private StringFormat _titleformat;

    public StringFormat TitleFormat
    {
        get => _titleformat;
        set
        {
            _titleformat = value;
            _overridetitleformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to override the title string alignment. Default value is
    ///     Alignment - Near;
    /// </summary>
    public StringAlignment TitleAlignment
    {
        get => _titleformat.Alignment;
        set
        {
            _titleformat.Alignment = value;
            _overridetitleformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to override the title string format flags. Default values
    ///     are: FormatFlags - NoWrap, LineLimit, NoClip
    /// </summary>
    public StringFormatFlags TitleFormatFlags
    {
        get => _titleformat.FormatFlags;
        set
        {
            _titleformat.FormatFlags = value;
            _overridetitleformat = true;
        }
    }

    /// <summary>
    ///     Control where in the document the title prints
    /// </summary>
    public PrintLocation TitlePrint { get; set; } = PrintLocation.All;

    /// <summary>
    ///     Return the title height based whether to print it or not
    /// </summary>
    private float TitleHeight
    {
        get
        {
            if (PrintLocation.All == TitlePrint) return _titleheight + TitleSpacing;

            if (PrintLocation.FirstOnly == TitlePrint && 1 == _currentPage) return _titleheight + TitleSpacing;

            if (PrintLocation.LastOnly == TitlePrint && _totalpages == _currentPage)
                return _titleheight + TitleSpacing;

            return 0;
        }
    }

    /// <summary>
    ///     Mandatory spacing between the grid and the footer
    /// </summary>
    public float TitleSpacing { get; set; }

    /// <summary>
    ///     Title Block Background Color
    /// </summary>
    public Brush TitleBackground { get; set; }

    /// <summary>
    ///     Title Block Border
    /// </summary>
    public Pen TitleBorder { get; set; }

    #endregion

    // SubTitle

    #region subtitle properties

    // override flat
    private bool _overridesubtitleformat;

    // formatted height of subtitle
    private float _subtitleheight;

    /// <summary>
    ///     SubTitle for this report. Default is empty.
    /// </summary>
    public string SubTitle { get; set; }

    /// <summary>
    ///     Font for the subtitle. Default is Tahoma, 12pt.
    /// </summary>
    public Font SubTitleFont { get; set; }

    /// <summary>
    ///     Foreground color for the subtitle. Default is Black
    /// </summary>
    public Color SubTitleColor { get; set; }

    /// <summary>
    ///     Allow override of the header cell format object
    /// </summary>
    private StringFormat _subtitleformat;

    public StringFormat SubTitleFormat
    {
        get => _subtitleformat;
        set
        {
            _subtitleformat = value;
            _overridesubtitleformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to override the subtitle string alignment. Default value is
    ///     Alignment - Near;
    /// </summary>
    public StringAlignment SubTitleAlignment
    {
        get => _subtitleformat.Alignment;
        set
        {
            _subtitleformat.Alignment = value;
            _overridesubtitleformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to override the subtitle string format flags. Default values
    ///     are: FormatFlags - NoWrap, LineLimit, NoClip
    /// </summary>
    public StringFormatFlags SubTitleFormatFlags
    {
        get => _subtitleformat.FormatFlags;
        set
        {
            _subtitleformat.FormatFlags = value;
            _overridesubtitleformat = true;
        }
    }

    /// <summary>
    ///     Control where in the document the title prints
    /// </summary>
    public PrintLocation SubTitlePrint { get; set; } = PrintLocation.All;

    /// <summary>
    ///     Return the title height based whether to print it or not
    /// </summary>
    private float SubTitleHeight
    {
        get
        {
            if (PrintLocation.All == SubTitlePrint) return _subtitleheight + SubTitleSpacing;

            if (PrintLocation.FirstOnly == SubTitlePrint && 1 == _currentPage)
                return _subtitleheight + SubTitleSpacing;

            if (PrintLocation.LastOnly == SubTitlePrint && _totalpages == _currentPage)
                return _subtitleheight + SubTitleSpacing;

            return 0;
        }
    }

    /// <summary>
    ///     Mandatory spacing between the grid and the footer
    /// </summary>
    public float SubTitleSpacing { get; set; }

    /// <summary>
    ///     Title Block Background Color
    /// </summary>
    public Brush SubTitleBackground { get; set; }

    /// <summary>
    ///     Title Block Border
    /// </summary>
    public Pen SubTitleBorder { get; set; }

    #endregion

    // Footer

    #region footer properties

    // override flag
    private bool _overridefooterformat;

    /// <summary>
    ///     footer for this report. Default is empty.
    /// </summary>
    public string Footer { get; set; }

    /// <summary>
    ///     Font for the footer. Default is Tahoma, 10pt.
    /// </summary>
    public Font FooterFont { get; set; }

    /// <summary>
    ///     Foreground color for the footer. Default is Black
    /// </summary>
    public Color FooterColor { get; set; }

    /// <summary>
    ///     Allow override of the header cell format object
    /// </summary>
    private StringFormat _footerformat;

    public StringFormat FooterFormat
    {
        get => _footerformat;
        set
        {
            _footerformat = value;
            _overridefooterformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to override the footer string alignment. Default value is
    ///     Alignment - Center;
    /// </summary>
    public StringAlignment FooterAlignment
    {
        get => _footerformat.Alignment;
        set
        {
            _footerformat.Alignment = value;
            _overridefooterformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to override the footer string format flags. Default values
    ///     are: FormatFlags - NoWrap, LineLimit, NoClip
    /// </summary>
    public StringFormatFlags FooterFormatFlags
    {
        get => _footerformat.FormatFlags;
        set
        {
            _footerformat.FormatFlags = value;
            _overridefooterformat = true;
        }
    }

    /// <summary>
    ///     Mandatory spacing between the grid and the footer
    /// </summary>
    public float FooterSpacing { get; set; }

    /// <summary>
    ///     Control where in the document the title prints
    /// </summary>
    public PrintLocation FooterPrint { get; set; } = PrintLocation.All;

    /// <summary>
    ///     Determine the height of the footer
    /// </summary>
    private float FooterHeight
    {
        get
        {
            float footerheight = 0;

            // return calculated height if we're printing the footer
            if (PrintLocation.All == FooterPrint
                || (PrintLocation.FirstOnly == FooterPrint && 1 == _currentPage)
                || (PrintLocation.LastOnly == FooterPrint && _totalpages == _currentPage))
                // Add in footer text height 
                footerheight += _footerHeight + FooterSpacing;

            return footerheight;
        }
    }

    /// <summary>
    ///     Title Block Background Color
    /// </summary>
    public Brush FooterBackground { get; set; }

    /// <summary>
    ///     Title Block Border
    /// </summary>
    public Pen FooterBorder { get; set; }

    #endregion

    // Page Numbering

    #region page number properties

    // override flag
    private bool _overridepagenumberformat;

    /// <summary>
    ///     Include page number in the printout. Default is true.
    /// </summary>
    public bool PageNumbers { get; set; } = true;

    /// <summary>
    ///     Font for the page number, Default is Tahoma, 8pt.
    /// </summary>
    public Font PageNumberFont { get; set; }

    /// <summary>
    ///     Text color (foreground) for the page number. Default is Black
    /// </summary>
    public Color PageNumberColor { get; set; }

    /// <summary>
    ///     Allow override of the header cell format object
    /// </summary>
    private StringFormat _pagenumberformat;

    public StringFormat PageNumberFormat
    {
        get => _pagenumberformat;
        set
        {
            _pagenumberformat = value;
            _overridepagenumberformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to override the page number string alignment. Default value is
    ///     Alignment - Near;
    /// </summary>
    public StringAlignment PageNumberAlignment
    {
        get => _pagenumberformat.Alignment;
        set
        {
            _pagenumberformat.Alignment = value;
            _overridepagenumberformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to override the pagenumber string format flags. Default values
    ///     are: FormatFlags - NoWrap, LineLimit, NoClip
    /// </summary>
    public StringFormatFlags PageNumberFormatFlags
    {
        get => _pagenumberformat.FormatFlags;
        set
        {
            _pagenumberformat.FormatFlags = value;
            _overridepagenumberformat = true;
        }
    }

    /// <summary>
    ///     Allow the user to select whether to have the page number at the top or bottom
    ///     of the page. Default is false: page numbers on the bottom of the page
    /// </summary>
    public bool PageNumberInHeader { get; set; }

    /// <summary>
    ///     Should the page number be printed on a separate line, or printed on the
    ///     same line as the header / footer? Default is false;
    /// </summary>
    public bool PageNumberOnSeparateLine { get; set; }

    /// <summary>
    ///     Show the total page number as n of total
    /// </summary>
    public bool ShowTotalPageNumber { get; set; }

    /// <summary>
    ///     Text separating page number and total page number. Default is ' of '.
    /// </summary>
    public string PageSeparator { get; set; } = " of ";

    public string PageText { get; set; } = "Page ";

    public string PartText { get; set; } = " - Part ";

    /// <summary>
    ///     Control where in the document the title prints
    /// </summary>
    public PrintLocation PageNumberPrint { get; set; } = PrintLocation.All;

    /// <summary>
    ///     Determine the height of the footer
    /// </summary>
    private float PageNumberHeight
    {
        get
        {
            // return calculated height if we're printing the footer
            if (PrintLocation.All == PageNumberPrint
                || (PrintLocation.FirstOnly == PageNumberPrint && 1 == _currentPage)
                || (PrintLocation.LastOnly == PageNumberPrint && _totalpages == _currentPage))
            {
                // return page number height if we're printing it on a separate line
                // if we're not printing on a separate line, but we're suppressing the
                // header or footer then we still need to reserve space for the page number
                if (PageNumberOnSeparateLine)
                    return _pagenumberHeight;
                if (PageNumberInHeader && 0 == TitleHeight && 0 == SubTitleHeight)
                    return _pagenumberHeight;
                if (!PageNumberInHeader && 0 == FooterHeight) return FooterSpacing + _pagenumberHeight;
            }

            return 0;
        }
    }

    #endregion

    // Header Cell Printing 

    #region header cell properties

    public DataGridViewCellStyle RowHeaderCellStyle { get; set; }

    /// <summary>
    ///     Allow override of the row header cell format object
    /// </summary>
    private StringFormat _rowheadercellformat;

    public StringFormat GetRowHeaderCellFormat(DataGridView grid)
    {
        // get default values from provided data grid view, but only
        // if we don't already have a header cell format
        if (null != grid && null == _rowheadercellformat)
            Buildstringformat(ref _rowheadercellformat, grid.Rows[0].HeaderCell.InheritedStyle,
                HeaderCellAlignment, StringAlignment.Near, HeaderCellFormatFlags,
                StringTrimming.Word);

        // if we still don't have a header cell format, create an empty
        if (null == _rowheadercellformat) _rowheadercellformat = new StringFormat(HeaderCellFormatFlags);

        return _rowheadercellformat;
    }

    /// <summary>
    ///     Default value to show in the row header cell if no value is provided in the DataGridView.
    ///     Defaults to one tab space
    /// </summary>
    public string RowHeaderCellDefaultText { get; set; } = "\t";

    /// <summary>
    ///     Allow override of the header cell format object
    /// </summary>
    public Dictionary<string, DataGridViewCellStyle> ColumnHeaderStyles { get; } = new();

    /// <summary>
    ///     Allow override of the header cell format object
    /// </summary>
    private StringFormat _columnheadercellformat;

    public StringFormat GetColumnHeaderCellFormat(DataGridView grid)
    {
        // get default values from provided data grid view, but only
        // if we don't already have a header cell format
        if (null != grid && null == _columnheadercellformat)
            Buildstringformat(ref _columnheadercellformat, grid.Columns[0].HeaderCell.InheritedStyle,
                HeaderCellAlignment, StringAlignment.Near, HeaderCellFormatFlags,
                StringTrimming.Word);

        // if we still don't have a header cell format, create an empty
        if (null == _columnheadercellformat) _columnheadercellformat = new StringFormat(HeaderCellFormatFlags);

        return _columnheadercellformat;
    }

    /// <summary>
    ///     Deprecated - use HeaderCellFormat
    ///     Allow the user to override the header cell string alignment. Default value is
    ///     Alignment - Near;
    /// </summary>
    public StringAlignment HeaderCellAlignment { get; set; }

    /// <summary>
    ///     Deprecated - use HeaderCellFormat
    ///     Allow the user to override the header cell string format flags. Default values
    ///     are: FormatFlags - NoWrap, LineLimit, NoClip
    /// </summary>
    public StringFormatFlags HeaderCellFormatFlags { get; set; }

    #endregion

    // Individual Cell Printing

    #region cell properties

    /// <summary>
    ///     Allow override of the cell printing format
    /// </summary>
    private StringFormat _cellformat;

    public StringFormat GetCellFormat(DataGridView grid)
    {
        // get default values from provided data grid view, but only
        // if we don't already have a cell format
        if (null != grid && null == _cellformat)
            Buildstringformat(ref _cellformat, grid.Rows[0].Cells[0].InheritedStyle,
                CellAlignment, StringAlignment.Near, CellFormatFlags,
                StringTrimming.Word);

        // if we still don't have a cell format, create an empty
        if (null == _cellformat) _cellformat = new StringFormat(CellFormatFlags);

        return _cellformat;
    }

    /// <summary>
    ///     Deprecated - use GetCellFormat
    ///     Allow the user to override the cell string alignment. Default value is
    ///     Alignment - Near;
    /// </summary>
    public StringAlignment CellAlignment { get; set; }

    /// <summary>
    ///     Deprecated - use GetCellFormat
    ///     Allow the user to override the cell string format flags. Default values
    ///     are: FormatFlags - NoWrap, LineLimit, NoClip
    /// </summary>
    public StringFormatFlags CellFormatFlags { get; set; }

    /// <summary>
    ///     allow the user to override the column width calcs with their own defaults
    /// </summary>
    private readonly List<float> _colwidthsoverride = [];

    public Dictionary<string, float> ColumnWidths { get; } = new();

    /// <summary>
    ///     Allow per column style overrides
    /// </summary>
    public Dictionary<string, DataGridViewCellStyle> ColumnStyles { get; } = new();

    /// <summary>
    ///     Allow per column style overrides
    /// </summary>
    public Dictionary<string, DataGridViewCellStyle> AlternatingRowColumnStyles { get; } = new();

    /// <summary>
    ///     Allow the user to set columns that appear on every pageset. Only used when
    ///     the printout is wider than one page.
    /// </summary>
    private readonly List<int> _fixedcolumns = [];

    public List<string> FixedColumns { get; } = [];

    /// <summary>
    ///     List of columns to not display in the grid view printout.
    /// </summary>
    public List<string> HideColumns { get; } = [];

    /// <summary>
    ///     Insert a page break when the value in this column changes
    /// </summary>
    private object _oldvalue;

    public string BreakOnValueChange { get; set; }

    #endregion

    // Page Level Properties

    #region page level properties

    /// <summary>
    ///     Page margins override. Default is (60, 60, 40, 40)
    /// </summary>
    public Margins PrintMargins
    {
        get => PageSettings.Margins;
        set => PageSettings.Margins = value;
    }

    /// <summary>
    ///     Expose the printdocument default page settings to the caller
    /// </summary>
    public PageSettings PageSettings => PrintDocument.DefaultPageSettings;

    /// <summary>
    ///     Spread the columns porportionally accross the page. Default is false.
    ///     Deprecated. Please use the ColumnWidth property
    /// </summary>
    private bool _porportionalcolumns;

    public bool PorportionalColumns
    {
        get => _porportionalcolumns;
        set
        {
            _porportionalcolumns = value;
            if (_porportionalcolumns)
                ColumnWidth = ColumnWidthSetting.Porportional;
            else
                ColumnWidth = ColumnWidthSetting.CellWidth;
        }
    }

    /// <summary>
    ///     Center the table on the page.
    /// </summary>
    public Alignment TableAlignment { get; set; } = Alignment.NotSet;

    /// <summary>
    ///     Change the default row height to either the height of the string or the size of
    ///     the cell. Added for image cell handling; set to CellHeight for image cells
    /// </summary>
    public enum RowHeightSetting
    {
        DataHeight,
        CellHeight
    }

    public RowHeightSetting RowHeight { get; set; } = RowHeightSetting.DataHeight;

    /// <summary>
    ///     Change the default column width to be spread porportionally accross the page,
    ///     to the size of the grid cell or the size of the formatted data string.
    ///     Set to CellWidth for image cells.
    /// </summary>
    public enum ColumnWidthSetting
    {
        DataWidth,
        CellWidth,
        Porportional
    }

    private ColumnWidthSetting _rowwidth = ColumnWidthSetting.CellWidth;

    public ColumnWidthSetting ColumnWidth
    {
        get => _rowwidth;
        set
        {
            _rowwidth = value;
            if (value == ColumnWidthSetting.Porportional)
                _porportionalcolumns = true;
            else
                _porportionalcolumns = false;
        }
    }

    #endregion

    // Utility Functions

    #region

    /// <summary>
    ///     calculate the print preview window width to show the entire page
    /// </summary>
    /// <returns></returns>
    private int PreviewDisplayWidth()
    {
        double displayWidth = PrintDocument.DefaultPageSettings.Bounds.Width
                              + 3 * PrintDocument.DefaultPageSettings.HardMarginY;
        return (int)(displayWidth * PrintPreviewZoom);
    }

    /// <summary>
    ///     calculate the print preview window height to show the entire page
    /// </summary>
    /// <returns></returns>
    private int PreviewDisplayHeight()
    {
        double displayHeight = PrintDocument.DefaultPageSettings.Bounds.Height
                               + 3 * PrintDocument.DefaultPageSettings.HardMarginX;

        return (int)(displayHeight * PrintPreviewZoom);
    }

    /// <summary>
    ///     Invoke any provided cell owner draw routines
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnCellOwnerDraw(DgvCellDrawingEventArgs e)
    {
        OwnerDraw?.Invoke(this, e);
    }

    /// <summary>
    ///     Given a row and column, get the current grid cell style, including our local
    ///     overrides
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    protected DataGridViewCellStyle GetStyle(DataGridViewRow row, DataGridViewColumn col)
    {
        // set initial default
        var colstyle = row.Cells[col.Index].InheritedStyle.Clone();

        // check for our override
        if (ColumnStyles.ContainsKey(col.Name)) colstyle = ColumnStyles[col.Name];

        // check for alternating row override
        if (0 != (row.Index & 1) && AlternatingRowColumnStyles.ContainsKey(col.Name))
            colstyle = AlternatingRowColumnStyles[col.Name];

        return colstyle;
    }

    /// <summary>
    ///     Skim the colstoprint list for a column name and return it's index
    /// </summary>
    /// <param name="colname">Name of column to find</param>
    /// <returns>index of column</returns>
    protected int GetColumnIndex(string colname)
    {
        var i = 0;
        foreach (DataGridViewColumn col in _colstoprint)
            if (col.Name != colname)
                i++;
            else
                break;

        // catch unknown column names
        if (i >= _colstoprint.Count) throw new Exception("Unknown Column Name: " + colname);

        return i;
    }

    #endregion

    #endregion
}