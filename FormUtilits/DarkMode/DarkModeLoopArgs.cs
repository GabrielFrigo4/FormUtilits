namespace FormUtilits.DarkMode;
public class DarkModeLoopArgs : EventArgs
{
    public Control MyControl { get; private set; }
    public Color Main { get; private set; }
    public Color Other { get; private set; }
    public bool Enabled { get; private set; }
    public bool SetTheme { get; set; }
    public bool Stop { get; set; }

    public DarkModeLoopArgs(Control myControl, Color main, Color other, bool enabled)
    {
        MyControl = myControl;
        Main = main;
        Other = other;
        Enabled = enabled;
        SetTheme = false;
        Stop = false;
    }
}