using FormUtilits.VisualStudioControl;
using FormUtilits.DarkMode;

namespace FormUtilits_Test;
public partial class Form1 : Form
{
    DarkMode darkMode;
    public Form1()
    {
        darkMode = new DarkMode(this);
        InitializeComponent();
        darkMode.Init();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        darkMode.SetDarkMode(true);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        darkMode.SetDarkMode(false);
    }
}
