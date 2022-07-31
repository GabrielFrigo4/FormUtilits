namespace FormUtilits.Themes;
public class FormThemeLoopArgs : EventArgs
{
    public Control MyControl { get; private set; }
    public Color Main { get; private set; }
    public Color Other { get; private set; }
    public bool IsLight { get; private set; }
    public bool IsDark { get; private set; }
    public bool SetTheme { get; set; }
    public bool Stop { get; set; }

    public FormThemeLoopArgs(Control myControl, Color main, Color other, bool isLight, bool isDark)
    {
        MyControl = myControl;
        Main = main;
        Other = other;
        IsLight = isLight;
        IsDark = isDark;
        SetTheme = false;
        Stop = false;
    }
}