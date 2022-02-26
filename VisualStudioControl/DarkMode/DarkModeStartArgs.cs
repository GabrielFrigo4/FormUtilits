namespace VisualStudioControl;
public class DarkModeStartArgs : EventArgs
{
    public Color Main { get; private set; }
    public Color Other { get; private set; }
    public bool Enable { get; private set; }

    public DarkModeStartArgs(Color main, Color other, bool enable)
    {
        Main = main;
        Other = other;
        Enable = enable;
    }
}