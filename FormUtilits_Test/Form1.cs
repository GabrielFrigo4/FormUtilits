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

    private async void button1_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeModeAsync(FormThemeMode.Dark);
        for(int i = 0; i < 10; i++)
        {
            Form2 form2 = new Form2();
            form2.Show();
            await Task.Run(() => FormTheme.SetThemeModeFormAsync(form2, FormThemeMode.System));
        }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeModeAsync(FormThemeMode.Light);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        FormTheme.SetThemeModeAsync(FormThemeMode.System);
    }
}
