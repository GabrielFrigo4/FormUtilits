namespace VisualStudioControl;
public class DarkModeLoopArgs : EventArgs
{
    public Control MyControl { get; private set; }
    public Color Main { get; private set; }
    public Color Other { get; private set; }
    public bool Enable { get; private set; }
    public bool Stop { get; set; }

    public DarkModeLoopArgs(Control myControl, Color main, Color other, bool enable)
    {
        MyControl = myControl;
        Main = main;
        Other = other;
        Enable = enable;
        Stop = false;
    }
}