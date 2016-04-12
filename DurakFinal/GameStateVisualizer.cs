using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Durak.Common;

namespace DurakCommon
{
    public partial class GameStateVisualizer : UserControl
    {
        private GameState myState;
        
        public GameStateVisualizer()
        {
            InitializeComponent();

            if (dgvMainView != null)
            {
                dgvMainView.Columns.Add("Name", "Name");
                dgvMainView.Columns[0].Width = Width / 2;
                dgvMainView.Columns.Add("Value", "Value");
                dgvMainView.Columns[1].Width = Width / 2;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (dgvMainView.Columns.Count == 2)
            {
                dgvMainView.Columns[0].Width = Width / 2;
                dgvMainView.Columns[1].Width = Width / 2;
            }
        }

        public void SetState(GameState state)
        {
            myState = state;

            StateParameter[] states = state.GetParameterCollection();

            foreach (StateParameter parameter in states)
            {
                dgvMainView.Rows.Add(parameter.Name, parameter.RawValue);
            }

            myState.OnStateChangedUnSilenceable += StateChanged;
            myState.OnCleared += StateCleared;
        }

        private void StateCleared(object sender, EventArgs e)
        {
            dgvMainView.Rows.Clear();
        }

        private void StateChanged(object sender, StateParameter e)
        {
            bool exists = false;

            for (int index = 0; index < dgvMainView.Rows.Count; index++)
            {
                if (dgvMainView.Rows[index].Cells[0].Value == e.Name)
                {
                    dgvMainView.Rows[index].Cells[1].Value = e.RawValue;
                    exists = true;
                    break;
                }
            }

            if (!exists)
                dgvMainView.Rows.Add(e.Name, e.RawValue);
        }
    }
}
