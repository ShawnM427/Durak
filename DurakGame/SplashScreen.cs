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
    /// <summary>
    /// Represents the splash screen form
    /// </summary>
    public partial class frmSplashScreen : Form
    {
        /// <summary>
        /// Creates the splash screen and starts the timer
        /// </summary>
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

        /// <summary>
        /// Invoked when the main picture box is clicked, this will close the splash screen
        /// </summary>
        /// <param name="e">The key press event that contains the key pressed</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            TimerTicked(this, e);
        }

        /// <summary>
        /// Invoked when the main picture box is clicked, this will close the splash screen
        /// </summary>
        /// <param name="sender">The object that is the sender</param>
        /// <param name="e">The empty event arguments</param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TimerTicked(sender, e);
        }

        /// <summary>
        /// Invoked when the splash timer has elapsed
        /// </summary>
        /// <param name="sender">The timer that invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void TimerTicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}
