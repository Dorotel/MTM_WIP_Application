using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MTM_WIP_Application.Core;

#region Core_AppThemes

public static class Core_AppThemes
{
    #region Theme Definition

    public class AppTheme
    {
        #region Form Properties

        public Color FormBackColor { get; set; }
        public Color FormForeColor { get; set; }
        public Font? FormFont { get; set; }

        #endregion

        #region DataGridViewProperties

        public Color DataGridBackColor { get; set; }
        public BorderStyle DataGridBorderStyle { get; set; }
        public Color DataGridSelectionBackColor { get; set; }
        public Color DataGridSelectionForeColor { get; set; }
        public Color DataGridRowsBackColor { get; set; }
        public Color DataGridAltRowsBackColor { get; set; }
        public Color? DataGridColumnHeadersForeColor { get; set; }
        public Color? DataGridColumnHeadersBackColor { get; set; }
        public Color? DataGridRowHeadersBackColor { get; set; }

        #endregion

        #region Control Specific Properties

        public Color? ButtonBackColor { get; set; }
        public Color? ButtonForeColor { get; set; }
        public Color? TextBoxBackColor { get; set; }
        public Color? TextBoxForeColor { get; set; }
        public Color? LabelForeColor { get; set; }

        #endregion
    }

    #endregion

    #region Theme Registry

    private static readonly Dictionary<string, AppTheme> Themes = new()
    {
        #region DefaultBlackAndWhite

        ["Default (Black and White)"] = new AppTheme
        {
            FormBackColor = Color.White,
            FormForeColor = Color.Black,
            FormFont = null,
            DataGridBackColor = Color.LightGray,
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.Blue,
            DataGridSelectionForeColor = Color.White,
            DataGridRowsBackColor = Color.White,
            DataGridAltRowsBackColor = Color.WhiteSmoke,
            ButtonBackColor = Color.LightGray,
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.Black,
            LabelForeColor = Color.Black
        },

        #endregion

        #region LightBlue

        ["Light Blue"] = new AppTheme
        {
            FormBackColor = Color.AliceBlue,
            FormForeColor = Color.Black,
            FormFont = null,
            DataGridBackColor = Color.LightGray,
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.CornflowerBlue,
            DataGridSelectionForeColor = Color.Black,
            DataGridRowsBackColor = Color.LightBlue,
            DataGridAltRowsBackColor = Color.Azure,
            DataGridColumnHeadersForeColor = Color.White,
            DataGridColumnHeadersBackColor = Color.Black,
            DataGridRowHeadersBackColor = Color.Black,
            ButtonBackColor = Color.LightBlue,
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.Black,
            LabelForeColor = Color.Navy
        },

        #endregion

        #region LightRed

        ["Light Red"] = new AppTheme
        {
            FormBackColor = ColorTranslator.FromHtml("#f4cccc"),
            FormForeColor = Color.Black,
            FormFont = null,
            DataGridBackColor = ColorTranslator.FromHtml("#EEEEEE"),
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = ColorTranslator.FromHtml("#0000ff"),
            DataGridSelectionForeColor = ColorTranslator.FromHtml("#ffffff"),
            DataGridRowsBackColor = ColorTranslator.FromHtml("#FF0000"),
            DataGridAltRowsBackColor = ColorTranslator.FromHtml("#f4cccc"),
            ButtonBackColor = ColorTranslator.FromHtml("#FFCCCC"),
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.Black,
            LabelForeColor = Color.DarkRed
        },

        #endregion

        #region LightGrey

        ["Light Grey"] = new AppTheme
        {
            FormBackColor = ColorTranslator.FromHtml("#EEEEEE"),
            FormForeColor = Color.Black,
            FormFont = null,
            DataGridBackColor = ColorTranslator.FromHtml("#EEEEEE"),
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = ColorTranslator.FromHtml("#434C5B"),
            DataGridSelectionForeColor = ColorTranslator.FromHtml("#F5F3F5"),
            DataGridRowsBackColor = ColorTranslator.FromHtml("#a6a6a6"),
            DataGridAltRowsBackColor = ColorTranslator.FromHtml("#cccccc"),
            ButtonBackColor = ColorTranslator.FromHtml("#cccccc"),
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.Black,
            LabelForeColor = Color.Black
        },

        #endregion

        #region Dark

        ["Dark"] = new AppTheme
        {
            FormBackColor = Color.FromArgb(32, 32, 32),
            FormForeColor = Color.White,
            FormFont = null,
            DataGridBackColor = Color.FromArgb(48, 48, 48),
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.FromArgb(64, 64, 128),
            DataGridSelectionForeColor = Color.White,
            DataGridRowsBackColor = Color.FromArgb(32, 32, 32),
            DataGridAltRowsBackColor = Color.FromArgb(48, 48, 48),
            DataGridColumnHeadersForeColor = Color.White,
            DataGridColumnHeadersBackColor = Color.FromArgb(64, 64, 64),
            DataGridRowHeadersBackColor = Color.FromArgb(64, 64, 64),
            ButtonBackColor = Color.FromArgb(64, 64, 64),
            ButtonForeColor = Color.White,
            TextBoxBackColor = Color.FromArgb(48, 48, 48),
            TextBoxForeColor = Color.White,
            LabelForeColor = Color.White
        },

        #endregion

        #region Green

        ["Green"] = new AppTheme
        {
            FormBackColor = Color.Honeydew,
            FormForeColor = Color.DarkGreen,
            FormFont = null,
            DataGridBackColor = Color.Honeydew,
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.SeaGreen,
            DataGridSelectionForeColor = Color.White,
            DataGridRowsBackColor = Color.Honeydew,
            DataGridAltRowsBackColor = Color.PaleGreen,
            DataGridColumnHeadersForeColor = Color.White,
            DataGridColumnHeadersBackColor = Color.SeaGreen,
            DataGridRowHeadersBackColor = Color.SeaGreen,
            ButtonBackColor = Color.PaleGreen,
            ButtonForeColor = Color.DarkGreen,
            TextBoxBackColor = Color.White,
            TextBoxForeColor = Color.DarkGreen,
            LabelForeColor = Color.SeaGreen
        },

        #endregion

        #region Solarized

        ["Solarized"] = new AppTheme
        {
            FormBackColor = ColorTranslator.FromHtml("#fdf6e3"),
            FormForeColor = ColorTranslator.FromHtml("#657b83"),
            FormFont = null,
            DataGridBackColor = ColorTranslator.FromHtml("#eee8d5"),
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = ColorTranslator.FromHtml("#b58900"),
            DataGridSelectionForeColor = ColorTranslator.FromHtml("#fdf6e3"),
            DataGridRowsBackColor = ColorTranslator.FromHtml("#eee8d5"),
            DataGridAltRowsBackColor = ColorTranslator.FromHtml("#fdf6e3"),
            DataGridColumnHeadersForeColor = ColorTranslator.FromHtml("#fdf6e3"),
            DataGridColumnHeadersBackColor = ColorTranslator.FromHtml("#657b83"),
            DataGridRowHeadersBackColor = ColorTranslator.FromHtml("#657b83"),
            ButtonBackColor = ColorTranslator.FromHtml("#eee8d5"),
            ButtonForeColor = ColorTranslator.FromHtml("#657b83"),
            TextBoxBackColor = ColorTranslator.FromHtml("#fdf6e3"),
            TextBoxForeColor = ColorTranslator.FromHtml("#657b83"),
            LabelForeColor = ColorTranslator.FromHtml("#657b83")
        },

        #endregion

        #region HighContrast

        ["High Contrast"] = new AppTheme
        {
            FormBackColor = Color.Black,
            FormForeColor = Color.Yellow,
            FormFont = null,
            DataGridBackColor = Color.Black,
            DataGridBorderStyle = BorderStyle.Fixed3D,
            DataGridSelectionBackColor = Color.Yellow,
            DataGridSelectionForeColor = Color.Black,
            DataGridRowsBackColor = Color.Black,
            DataGridAltRowsBackColor = Color.Gray,
            DataGridColumnHeadersForeColor = Color.Black,
            DataGridColumnHeadersBackColor = Color.Yellow,
            DataGridRowHeadersBackColor = Color.Yellow,
            ButtonBackColor = Color.Yellow,
            ButtonForeColor = Color.Black,
            TextBoxBackColor = Color.Black,
            TextBoxForeColor = Color.Yellow,
            LabelForeColor = Color.Yellow
        }

        #endregion
    };

    #endregion

    #region Theme Accessors

    public static IEnumerable<string> GetThemeNames()
    {
        return Themes.Keys;
    }

    public static bool ThemeExists(string themeName)
    {
        return Themes.ContainsKey(themeName);
    }

    public static AppTheme GetTheme(string themeName)
    {
        if (Themes.TryGetValue(themeName, out var theme))
            return theme;
        return Themes["Default (Black and White)"];
    }

    public static string GetEffectiveThemeName()
    {
        var themeName = Core_WipAppVariables.WipDataGridTheme ?? "Default (Black and White)";
        if (!Themes.ContainsKey(themeName)) themeName = "Default (Black and White)";
        return themeName;
    }

    public static Color GetThemeColor(string themeName, string propertyName)
    {
        if (!Themes.TryGetValue(themeName, out var theme))
            throw new InvalidOperationException($"Theme '{themeName}' not found.");

        var property = typeof(AppTheme).GetProperty(propertyName)
                       ?? throw new InvalidOperationException($"Property '{propertyName}' not found on AppTheme.");

        var value = property.GetValue(theme);

        if (value is Color color)
            return color;

        throw new InvalidOperationException($"Property '{propertyName}' is not a Color or is null.");
    }

    #endregion
}

#endregion