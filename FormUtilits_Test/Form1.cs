using FormUtilits.Themes;

namespace FormUtilits_Test;
public partial class Form1 : Form
{
    internal FormTheme FormTheme { get; private set; }
    public Form1()
    {
        FormTheme = new FormTheme(this);
        InitializeComponent();
        FormTheme.Init();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeMode(ThemeMode.Dark);
        Form2 form2 = new Form2();
        form2.Show();
        FormTheme.SetThemeModeForm(form2, ThemeMode.System);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeMode(ThemeMode.Light);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeMode(ThemeMode.System);
    }
}
