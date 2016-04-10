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
        private GameClient myClient;

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
                isReady = value;
                DetermineReadyImage();
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
                            imgReady.Click += ReadyClicked;
                        }
                    }
                    else if (myPlayer.PlayerId != Client.PlayerId)
                        imgPlayerType.Image = Resources.netIcon;
                    else
                        imgPlayerType.Image = Resources.silhoutte;

                    lblPlayerName.Text = myPlayer.Name;
                    DetermineReadyImage();
                }
            }
        }
        public GameClient Client
        {
            get { return myClient; }
            set
            {
                if (myClient != null)
                    myClient.OnPlayerReady -= OnPlayerReady;

                myClient = value;
                myClient.OnPlayerReady += OnPlayerReady;
            }
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

                Client.SetReadiness(IsReady);

                OnReadinessChanged?.Invoke(this, isReady);
            }
        }


        private void OnPlayerReady(object sender, Player e)
        {
            if (e == Player)
                IsReady = Player.IsReady;
        }

        private void DetermineReadyImage()
        {
            if (!Player.IsHost)
                imgReady.Image = IsReady ? Resources.ready : Resources.notReady;
            else
                imgReady.Image = null;
        }

        public PlayerView()
        {
            InitializeComponent();
        }
    }
}
