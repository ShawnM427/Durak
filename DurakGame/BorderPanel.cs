using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakGame
{
    public class BorderPanel : Panel
    {
        private float myBorderWidth;
        private Color myBorderColor;
        private bool showBorder;

        public float BorderWidth
        {
            get { return myBorderWidth; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Border width cannot be less than 0");

                myBorderWidth = value;
                Invalidate();
            }
        }
        public Color BorderColor
        {
            get { return myBorderColor; }
            set
            {
                myBorderColor = value;
                Invalidate();
            }
        }
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                showBorder = value;
                Invalidate();
            }
        }

        public BorderPanel()
        {
            myBorderColor = Color.Black;
            BorderWidth = 2;
            ShowBorder = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            if (ShowBorder)
            {
                Pen pen = new Pen(BorderColor, BorderWidth);
                e.Graphics.DrawRectangle(pen, BorderWidth / 2, BorderWidth / 2, Width - BorderWidth, Height - BorderWidth);
            }
        }
    }
}
