using System.Runtime.InteropServices;
using FormUtilits.VisualStudioControl;

namespace FormUtilits.DarkMode;
public class DarkMode
{
    #region System Funcs
    [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string? pszSubIdList);

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

    private bool IsWindows10OrGreater(int build = -1)
    {
        return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
    }
    #endregion

    #region My Private Funcs

    private bool SetControlBox(Form form, bool enabled)
    {
        if (IsWindows10OrGreater(17763))
        {
            var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
            if (IsWindows10OrGreater(18985))
            {
                attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
            }

            int useImmersiveDarkMode = enabled ? 1 : 0;
            return DwmSetWindowAttribute(form.Handle, (int)attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
        }

        return false;
    }

    private void SetMainForm(Form form, Color main, Color other, bool enabled)
    {
        form.BackColor = main;
        form.ForeColor = other;

        if (enabled)
        {
            SetWindowTheme(form.Handle, "DarkMode_Explorer", null);
        }
        else
        {
            SetWindowTheme(form.Handle, "Explorer", null);
        }
    }
    #endregion

    #region My Public Funcs
    public Form MainForm { get; private set; }
    public static bool IsDark { get; private set; }
    public static bool IsInit { get; private set; }

    public DarkMode(Form mainForm)
    {
        MainForm = mainForm;
        DarkModeLoop += SetTheme_Label;
        DarkModeLoop += SetTheme_MenuStrip;
        DarkModeLoop += SetTheme_IDarkMode;
        DarkModeLoop += SetTheme_VisualStudioTabControl;
    }

    public event EventHandler<DarkModeLoopArgs>? DarkModeLoop;
    public event EventHandler<DarkModeStartArgs>? DarkModeStart;

    public void Init(bool dark = true)
    {
        SetDarkMode(dark);
        IsInit = true;
    }

    public bool SetDarkMode(bool enabled = true)
    {
        return SetDarkModeForm(MainForm, enabled);
    }

    public bool SetDarkModeForm(Form form, bool enabled = true)
    {
        if (form == MainForm)
        {
            IsDark = enabled;
        }
        Color main, other;
        if (enabled)
        {
            main = Color.FromArgb(23, 23, 23);
            other = Color.White;
        }
        else
        {
            main = Color.WhiteSmoke;
            other = Color.Black;
        }

        DarkModeStart?.Invoke(this, new DarkModeStartArgs(form, main, other, enabled));

        foreach (Control c in form.Controls)
        {
            UpdateColorControls(c);
        }

        void UpdateColorControls(Control myControl)
        {
            if (enabled)
            {
                SetWindowTheme(myControl.Handle, "DarkMode_Explorer", null);
            }
            else
            {
                SetWindowTheme(myControl.Handle, "Explorer", null);
            }

            DarkModeLoopArgs args = new DarkModeLoopArgs(myControl, main, other, enabled);
            DarkModeLoop?.Invoke(this, args);
            if (args.SetTheme == false)
            {
                myControl.BackColor = main;
                myControl.ForeColor = other;
            }
            if (args.Stop == true)
            {
                return;
            }

            foreach (Control subC in myControl.Controls)
            {
                UpdateColorControls(subC);
            }
        }

        SetMainForm(form, main, other, enabled);
        return SetControlBox(form, enabled);
    }
    #endregion

    #region MyControlFuncs
    void SetTheme_Label(object? sender, DarkModeLoopArgs e)
    {
        if(e.MyControl is Label)
        {
            e.MyControl.BackColor = Color.Transparent;
            e.MyControl.ForeColor = e.Other;
            e.SetTheme = true;
        }
    }

    void SetTheme_MenuStrip(object? sender, DarkModeLoopArgs e)
    {
        void SetForeColorItem(ref ToolStripMenuItem item)
        {
            item.ForeColor = e.Other;
            foreach (ToolStripMenuItem subItem in item.DropDownItems)
            {
                ToolStripMenuItem thisSubItem = subItem;
                SetForeColorItem(ref thisSubItem);
            }
        }

        if (e.MyControl is MenuStrip)
        {
            MenuStrip menuStrip = (MenuStrip)e.MyControl;
            e.MyControl.BackColor = e.Main;
            e.MyControl.ForeColor = e.Other;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new VisualStudioColorTable(e.Enabled));
            foreach(ToolStripMenuItem item in menuStrip.Items)
            {
                ToolStripMenuItem thisItem = item;
                SetForeColorItem(ref thisItem);
            }
            e.SetTheme = true;
        }
    }

    void SetTheme_IDarkMode(object? sender, DarkModeLoopArgs e)
    {
        if (e.MyControl is IDarkMode)
        {
            IDarkMode mode = (IDarkMode)e.MyControl;
            mode.SetMode(sender, e);
            e.SetTheme = true;
        }
    }

    void SetTheme_VisualStudioTabControl(object? sender, DarkModeLoopArgs e)
    {
        if (e.MyControl is VisualStudioTabControl)
        {
            if (e.Enabled)
            {
                ((VisualStudioTabControl)e.MyControl).Theme = VisualStudioTabControlTheme.Dark;
            }
            else
            {
                ((VisualStudioTabControl)e.MyControl).Theme = VisualStudioTabControlTheme.Light;
            }
            e.SetTheme = true;
        }
    }
    #endregion
}