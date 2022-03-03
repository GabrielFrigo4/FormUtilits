using FormUtilits.VisualStudioControl;
using FormUtilits.DarkMode;

namespace FormUtilits_Test
{
    public partial class Form1 : Form
    {
        DarkMode darkMode;
        public Form1()
        {
            darkMode = new DarkMode(this);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            darkMode.SetDarkMode(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            darkMode.SetDarkMode(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //menuStrip1.Renderer = new ToolStripProfessionalRenderer(new VisualStudioColorTable(DarkMode.IsDark));
        }
    }
}
