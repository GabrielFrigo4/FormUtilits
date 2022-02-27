namespace VisualStudioControl;
public class DarkModeStartArgs : EventArgs
{
    public Color Main { get; private set; }
    public Color Other { get; private set; }
    public bool Enabled { get; private set; }

    public DarkModeStartArgs(Color main, Color other, bool enabled)
    {
        Main = main;
        Other = other;
        Enabled = enabled;
    }
}