using DurakGame.Properties;
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
    /// Represents the Settings form to be implemented
    /// </summary>
    public partial class frmSettings : Form
    {
        private bool changesMade = false;

        /// <summary>
        /// Creates the new settings form
        /// </summary>
        public frmSettings()
        {
            InitializeComponent();

            LoadSettingsView();
        }

        /// <summary>
        /// Handles populating the controls with data from the settings
        /// </summary>
        private void LoadSettingsView()
        {
            txtPlayerName.Text = Settings.Default.UserName;

            txtServerName.Text = Settings.Default.DefaultServerName;
            txtServerDescription.Text = Settings.Default.DefaultServerDescription;
            trkBotDifficulty.Value = (int)(Settings.Default.DefaultBotDifficulty * 100);
            chkSimulateBotThink.Checked = Settings.Default.DefaultBotsThink;
            trkNumPlayers.Value = Settings.Default.DefaultMaxPlayers;
            
            changesMade = false;
        }

        /// <summary>
        /// Handles populating the settings from the controls on the form
        /// </summary>
        private bool ApplySettingsView()
        {
            if (VerifyNotEmpty(txtPlayerName, "Player name"))
                Settings.Default.UserName = txtPlayerName.Text;
            else
                return false;

            if (VerifyNotEmpty(txtServerName, "Server name"))
                Settings.Default.DefaultServerName = txtServerName.Text;
            else
                return false;

            if (VerifyNotEmpty(txtServerDescription, "Server description"))
                Settings.Default.DefaultServerDescription = txtServerDescription.Text;
            else
                return false;
            
            Settings.Default.DefaultBotDifficulty = trkBotDifficulty.Value / 100.0f;
            Settings.Default.DefaultBotsThink = chkSimulateBotThink.Checked;
            Settings.Default.DefaultMaxPlayers = trkNumPlayers.Value;

            return true;
        }

        /// <summary>
        /// Utility method to show a dialoge box if an input is empty
        /// </summary>
        /// <param name="textBox">The text input to check</param>
        /// <param name="paramName">The name of the parameter in the message</param>
        /// <returns>True if the input is not empty, false if otherwise</returns>
        private bool VerifyNotEmpty(TextBox textBox, string paramName)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                MessageBox.Show(paramName + " cannot be empty", "Error", MessageBoxButtons.OK);
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Invoked by all settings controls when their value changes.
        /// This 'dirties' the state and will make settigns be marked for updating
        /// </summary>
        /// <param name="sender">The control that invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void InputValueChanged(object sender, EventArgs e)
        {
            changesMade = true;
        }

        /// <summary>
        /// Invoked when the reset default button has been pressed
        /// </summary>
        /// <param name="sender">The button that invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void btnSetDefaults_Click(object sender, EventArgs e)
        {
            Settings.Default.Reset();
            changesMade = true;
            LoadSettingsView();
        }

        /// <summary>
        /// Invoked when the cancel button has been pressed
        /// </summary>
        /// <param name="sender">The button that invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (changesMade)
            {
                DialogResult result = MessageBox.Show("You have unsaved changes, are you sure you want to leave?", "Confirm", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Invoked when the apply button has been pressed
        /// </summary>
        /// <param name="sender">The button that invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to apply these settings?", "Confirm", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ApplySettingsView();
                Settings.Default.Save();
                Close();
            }
        }

        /// <summary>
        /// Invoked when the max players trackbar value has updated,
        /// this will update it's label
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="e">The empty event arguments</param>
        private void NumPlayersTRackUpdated(object sender, EventArgs e)
        {
            InputValueChanged(sender, e);
            lblNumPlayers.Text = trkNumPlayers.Value.ToString();
        }
    }
}
