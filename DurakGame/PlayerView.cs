using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DurakGame.Properties;
using Durak.Common;
using Durak.Client;

namespace DurakGame
{
    public partial class PlayerView : UserControl
    {
        private bool hasControl;
        private bool isReady;
        private Player myPlayer;

        public bool HasControl
        {
            get { return hasControl; }
            set
            {
                if (hasControl != value)
                {
                    hasControl = value;

                    if (hasControl)
                    {
                        imgReady.Click += ReadyClicked;
                    }
                    else
                    {
                        imgReady.Click -= ReadyClicked;
                    }
                }
            }
        }
        public bool IsReady
        {
            get { return isReady; }
            set
            {
                if (hasControl)
                {
                    if (isReady)
                    {
                        imgReady.Image = Resources.ready;
                    }
                    else
                    {
                        imgReady.Image = Resources.notReady;
                    }
                }
            }
        }
        public Player Player
        {
            get { return myPlayer; }
            set
            {
                myPlayer = value;

                if (myPlayer != null)
                {
                    if (myPlayer.IsBot)
                    {
                        imgPlayerType.Image = Resources.bot;

                        if (Client.IsHost)
                        {
                            imgReady.Image = Resources.delete;
                            imgReady.Click += ReadyClicked;
                        }
                    }
                    else if (myPlayer.PlayerId != Client.PlayerId)
                        imgPlayerType.Image = Resources.netIcon;
                    else
                        imgPlayerType.Image = Resources.silhoutte;

                    lblPlayerName.Text = myPlayer.Name;
                }
            }
        }
        public GameClient Client
        {
            get;
            set;
        }

        public event EventHandler<bool> OnReadinessChanged;

        private void ReadyClicked(object sender, EventArgs e)
        {
            if (myPlayer.IsBot && Client.IsHost)
            {
                Client.RequestKick(myPlayer, "Host kick bot");
            }
            if (HasControl)
            {
                isReady = !isReady;

                if (isReady)
                    imgReady.Image = Resources.ready;
                else
                    imgReady.Image = Resources.notReady;

                OnReadinessChanged?.Invoke(this, isReady);
            }
        }

        public PlayerView()
        {
            InitializeComponent();
        }
    }
}
