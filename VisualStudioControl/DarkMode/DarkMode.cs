using System.Runtime.InteropServices;

namespace VisualStudioControl
{
    public static class DarkMode
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string? pszSubIdList);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }

        public static bool UseImmersiveDarkMode(Form form, bool enabled = true, Func<Control, Color, Color, bool>? newFuncs = null)
        {
            Color main, other;
            if (enabled)
            {
                main = Color.FromArgb(23,23,23);
                other = Color.White;
            }
            else
            {
                main = Color.WhiteSmoke;
                other = Color.Black;
            }

            #region control
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

                if(myControl is VisualStudioTabControl)
                {
                    if (enabled)
                    {
                        ((VisualStudioTabControl)myControl).Theme = VisualStudioTabControlTheme.Dark;
                    }
                    else
                    {
                        ((VisualStudioTabControl)myControl).Theme = VisualStudioTabControlTheme.Light;
                    }      
                }
                else if (myControl is IDarkMode)
                {
                    IDarkMode mode = (IDarkMode)myControl;
                    if (enabled)
                    {
                        mode.DarkMode();
                    }
                    else
                    {
                        mode.WhiteMode();
                    }
                }
                else if (newFuncs != null)
                {
                    if(!newFuncs.Invoke(myControl, main, other))
                    {
                        myControl.BackColor = main;
                        myControl.ForeColor = other;
                    }
                }
                else
                {
                    myControl.BackColor = main;
                    myControl.ForeColor = other;
                }

                foreach (Control subC in myControl.Controls)
                {
                    UpdateColorControls(subC);
                }
            }
            #endregion

            #region form
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

            #endregion

            #region controlBox
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
            #endregion

            return false;
        }
    }
}
