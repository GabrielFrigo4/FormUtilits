using FormUtilits.Themes;

namespace FormUtilits_Test;
public partial class Form1 : Form
{
    internal FormTheme FormTheme { get; private set; }
    public Form1()
    {
        FormTheme = new(this);
        InitializeComponent();
        FormTheme.AddSetTheme(FormVisualStudioControl.Utilits.SetTheme_VisualStudio);
        FormTheme.AddSetTheme(WinFormsMDI2.Utilits.SetTheme_WinFormsMDI2);
        FormTheme.Init();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeModeAsync(FormThemeMode.Dark);
        Form2 form2 = new();
        form2.Show();
        await Task.Run(() => FormTheme.SetThemeModeFormAsync(form2, FormThemeMode.Dark));
    }

    private async void button2_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeModeAsync(FormThemeMode.Light);
        Form2 form2 = new();
        form2.Show();
        await Task.Run(() => FormTheme.SetThemeModeFormAsync(form2, FormThemeMode.Light));
    }

    private async void button3_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeModeAsync(FormThemeMode.System);
        Form2 form2 = new();
        form2.Show();
        await Task.Run(() => FormTheme.SetThemeModeFormAsync(form2, FormThemeMode.System));
    }

    private void button4_Click(object sender, EventArgs e)
    {
        var myForm = new Form2();
        WinFormsMDI2.MdiThemeMode mode = WinFormsMDI2.MdiThemeMode.Light;
        if (FormTheme.IsDark)
        {
            mode = WinFormsMDI2.MdiThemeMode.Dark;
            FormTheme.SetThemeModeForm(myForm, FormThemeMode.Dark);
        }
        else if (FormTheme.IsLight)
        {
            mode = WinFormsMDI2.MdiThemeMode.Light;
            FormTheme.SetThemeModeForm(myForm, FormThemeMode.Light);
        }
        var myMdi = mdiControl1.CreateMdiWinWithForm(myForm).MdiTheme = mode;
    }
}
