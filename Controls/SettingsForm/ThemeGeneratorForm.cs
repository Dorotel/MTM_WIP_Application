using MTM_Inventory_Application.Helpers;

namespace MTM_Inventory_Application.Controls.SettingsForm;

public partial class ThemeGeneratorForm : UserControl
{
    // Default base color for theme generation
    private Color baseColor = Color.FromArgb(0, 123, 255);

    public ThemeGeneratorForm()
    {
        InitializeComponent();
    }

    private void BtnPickBaseColor_Click(object sender, EventArgs e)
    {
        if (colorDialog.ShowDialog() == DialogResult.OK)
        {
            baseColor = colorDialog.Color;
            UpdateThemePreview();
        }
    }

    private void UpdateThemePreview()
    {
        // Generate a theme from the base color and update the preview panel
        var theme = Helper_ThemeGenerator.CreateThemeFromBaseColor(baseColor, txtThemeName.Text.Trim());
        pnlThemePreview.BackColor = theme.FormBackColor;
        pnlThemePreview.ForeColor = theme.FormBackColor.GetBrightness() < 0.5f ? Color.White : Color.Black;
        pnlThemePreview.Controls.Clear();

        // Add sample controls to preview
        var btn = new Button
        {
            Text = "Button",
            BackColor = theme.ButtonBackColor,
            ForeColor = theme.FormBackColor.GetBrightness() < 0.5f ? Color.White : Color.Black,
            Location = new Point(10, 10),
            Width = 80
        };
        var lbl = new Label
        {
            Text = "Label",
            BackColor = theme.HeaderBackColor,
            ForeColor = theme.FormBackColor.GetBrightness() < 0.5f ? Color.White : Color.Black,
            Location = new Point(100, 10),
            Width = 80
        };
        pnlThemePreview.Controls.Add(btn);
        pnlThemePreview.Controls.Add(lbl);
    }

    private void btnGenerateSingleTheme_Click(object sender, EventArgs e)
    {
        txtLog.Clear();
        try
        {
            var theme = Helper_ThemeGenerator.CreateThemeFromBaseColor(baseColor, txtThemeName.Text.Trim());
            var generator = new Helper_ThemeGenerator();
            var genThemeSql = generator.GetType().GetMethod("GenerateThemeSQL",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (genThemeSql != null)
            {
                genThemeSql.Invoke(generator, new object[] { theme });
                txtLog.Text = $"Theme '{theme.Name}' SQL file generated in Generated_Themes.";
                MessageBox.Show($"Theme '{theme.Name}' SQL file generated!", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                txtLog.Text = "Error: Could not find GenerateThemeSQL method.";
            }
        }
        catch (Exception ex)
        {
            var errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            txtLog.Text += $"Error: {errorMsg}\r\n";
            MessageBox.Show("Theme SQL generation failed!\n" + errorMsg, "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}

public class ThemeDefinition
{
    public string Name { get; set; } = "";
    public Color FormBackColor { get; set; }
    public Color InputBackColor { get; set; }
    public Color ButtonBackColor { get; set; }
    public Color HeaderBackColor { get; set; }
    public Color AlternateRowColor { get; set; }
    public Color BorderColor { get; set; }
    public Color AccentColor { get; set; }
    public Color AccentColorLight { get; set; }
    public Color AccentColorDark { get; set; }
    public Color ErrorColor { get; set; }
    public Color WarningColor { get; set; }
    public Color SuccessColor { get; set; }
    public Color InfoColor { get; set; }
}