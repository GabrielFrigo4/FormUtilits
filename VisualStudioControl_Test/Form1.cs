using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualStudioControl;

namespace VisualStudioControl_Test
{
    public partial class Form1 : Form
    {
        DarkMode darkMode = new DarkMode();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            darkMode.UseImmersiveDarkMode(this, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            darkMode.UseImmersiveDarkMode(this, false);
        }
    }
}
