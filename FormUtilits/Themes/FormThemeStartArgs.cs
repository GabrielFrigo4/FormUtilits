namespace FormUtilits.Themes;
public class FormThemeStartArgs : EventArgs
{
    public Form Form { get; private set; }
    public Color Main { get; private set; }
    public Color Other { get; private set; }
    public bool IsLight { get; private set; }
    public bool IsDark { get; private set; }

    public FormThemeStartArgs(Form form, Color main, Color other, bool isLight, bool isDark)
    {
        Form = form;
        Main = main;
        Other = other;
        IsLight = isLight;
        IsDark = isDark;
    }
}