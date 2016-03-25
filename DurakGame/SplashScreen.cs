using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakGame
{
    public partial class frmSplashScreen : Form
    {
        public frmSplashScreen()
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            this.BackColor = this.pictureBox1.BackColor;
            this.TransparencyKey = this.pictureBox1.BackColor;

            Timer t = new Timer();
            t.Interval = 5000;
            t.Tick += TimerTicked;
            t.Start();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // We empty this to have a transparent background
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TimerTicked(sender, e);
        }

        private void TimerTicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}
