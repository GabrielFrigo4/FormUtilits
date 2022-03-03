namespace FormUtilits.DarkMode;
public class DarkModeStartArgs : EventArgs
{
    public Form Form { get; private set; }
    public Color Main { get; private set; }
    public Color Other { get; private set; }
    public bool Enabled { get; private set; }

    public DarkModeStartArgs(Form form, Color main, Color other, bool enabled)
    {
        Form = form;
        Main = main;
        Other = other;
        Enabled = enabled;
    }
}