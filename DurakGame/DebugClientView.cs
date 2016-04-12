using Durak.Common;
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
    public partial class DebugClientView : Form
    {
        public DebugClientView()
        {
            InitializeComponent();
        }

        public void SetGameState(GameState state)
        {
            gsvMain.SetState(state);
        }
    }
}
