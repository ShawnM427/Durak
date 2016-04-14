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
    /// <summary>
    /// A custom control used to draw a player in to lobby view
    /// </summary>
    public partial class PlayerView : UserControl
    {
        /// <summary>
        /// The backing field for HasControl
        /// </summary>
        private bool hasControl;
        /// <summary>
        /// The backing field for IsReady
        /// </summary>
        private bool isReady;
        /// <summary>
        /// The backing field for Player
        /// </summary>
        private Player myPlayer;
        /// <summary>
        /// The backing field for Client
        /// </summary>
        private GameClient myClient;

        /// <summary>
        /// Gets or sets whether the client has control over the player
        /// </summary>
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
        /// <summary>
        /// Gets or sets whether the player this view represents is ready
        /// </summary>
        public bool IsReady
        {
            get { return isReady; }
            set
            {
                isReady = value;
                DetermineReadyImage();
            }
        }
        /// <summary>
        /// Gets or sets the player that this view represents
        /// </summary>
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
        /// <summary>
        /// Gets or sets the client that is rendering this view
        /// </summary>
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

        /// <summary>
        /// Invoked when the IsReady value has changed
        /// </summary>
        public event EventHandler<bool> OnReadinessChanged;
        
        /// <summary>
        /// Creates a new empty player view
        /// </summary>
        public PlayerView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the ready button is clicked
        /// </summary>
        /// <param name="sender">The button that invoked the event</param>
        /// <param name="e">The blank event arguments</param>
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

        /// <summary>
        /// Invoked when the player's readiness has changed
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="e">The player that the event is raised for</param>
        private void OnPlayerReady(object sender, Player e)
        {
            if (e == Player)
                IsReady = Player.IsReady;
        }

        /// <summary>
        /// Determines the ready image for this view
        /// </summary>
        private void DetermineReadyImage()
        {
            if (Player.IsBot)
                imgReady.Image = Client.IsHost ? Resources.delete : null;
            else if (!Player.IsHost)
                imgReady.Image = IsReady ? Resources.ready : Resources.notReady;
            else
                imgReady.Image = null;
        }
    }
}
