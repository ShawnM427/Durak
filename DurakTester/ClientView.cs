using Durak.Client;
using Durak.Common;
using Durak.Common.Cards;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakTester
{
    public partial class frmClientView : Form
    {
        private class MessageInput
        {
            public MessagePacketDefinitions.PayloadParamType Type;
            public Control InputControl;
        }

        GameClient myClient;
        List<MessageInput> myInputs;
        DiscoveredServerCollection myDicoveredServers;

        /// <summary>
        /// Gets or sets the client for this client viewer
        /// </summary>
        public GameClient Client
        {
            get { return myClient; }
            set { SetClient(value); }
        }

        public frmClientView()
        {
            InitializeComponent();

            myInputs = new List<MessageInput>();
            myDicoveredServers = new DiscoveredServerCollection();

            object[] values = Enum.GetValues(typeof(MessageType)).OfType<object>().ToArray();
            cmbMessageType.Items.AddRange(values);
            cmbMessageType.SelectedIndex = 0;
        }

        public void SetClient(GameClient client)
        {
            myClient = client;

            gsvClientState.SetState(myClient.LocalState);

            myClient.OnDisconnected += ClientDisconnected; ;
            myClient.OnServerDiscovered += ClientDiscoveredServer;
            myClient.OnConnected += ClientConnected;
            myClient.OnHandCardAdded += ClientCardAdded;
            myClient.OnHandCardRemoved += ClientCardRemoved;
            myClient.Run();
            myClient.DiscoverServers();
        }

        private void ClientCardRemoved(object sender, PlayingCard e)
        {
            cmbCards.Items.Remove(e);
        }

        private void ClientCardAdded(object sender, PlayingCard e)
        {
            cmbCards.Items.Add(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
    
            myClient.Stop();
        }

        private void ClientConnected(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
            btnSend.Enabled = true;
        }

        private void ClientDiscoveredServer(object sender, ServerTag tag)
        {
            myDicoveredServers.AddItem(tag);
        }

        private void ClientDisconnected(object sender, EventArgs e)
        {
            cmbCards.Items.Clear();

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            btnSend.Enabled = true;
        }
        
        private void btnSend_Click(object sender, EventArgs e)
        {
            byte clientId = (byte)myClient.PlayerId;
            
            if (myClient != null)
            {
                NetOutgoingMessage msg = myClient.PrepareDebugMessage();

                ComboBox box;
                NumericUpDown nud;
                TextBox text;

                msg.Write((byte)((MessageType)cmbMessageType.SelectedItem));

                for (int index = 0; index < myInputs.Count; index++)
                {
                    switch (myInputs[index].Type)
                    {
                        case MessagePacketDefinitions.PayloadParamType.Boolean:
                            box = (ComboBox)myInputs[index].InputControl;
                            msg.Write((bool)box.SelectedItem);
                            msg.WritePadBits();
                            break;

                        case MessagePacketDefinitions.PayloadParamType.BotDifficulty:
                            TrackBar control = (TrackBar)myInputs[index].InputControl;
                            msg.Write((byte)(control.Value));
                            break;

                        case MessagePacketDefinitions.PayloadParamType.Byte:
                            nud = (NumericUpDown)myInputs[index].InputControl;
                            msg.Write((byte)nud.Value);
                            break;

                        case MessagePacketDefinitions.PayloadParamType.CardRank:
                            box = (ComboBox)myInputs[index].InputControl;
                            msg.Write((byte)((CardRank)box.SelectedItem));
                            break;

                        case MessagePacketDefinitions.PayloadParamType.CardSuit:
                            box = (ComboBox)myInputs[index].InputControl;
                            msg.Write((byte)((CardSuit)box.SelectedItem));
                            break;

                        case MessagePacketDefinitions.PayloadParamType.Integer:
                            nud = (NumericUpDown)myInputs[index].InputControl;
                            msg.Write((int)nud.Value);
                            break;

                        case MessagePacketDefinitions.PayloadParamType.ServerState:
                            box = (ComboBox)myInputs[index].InputControl;
                            msg.Write((byte)((ServerState)box.SelectedItem));
                            break;

                        case MessagePacketDefinitions.PayloadParamType.String:
                            text = (TextBox)myInputs[index].InputControl;
                            msg.Write(text.Text);
                            break;

                        case MessagePacketDefinitions.PayloadParamType.PlayerID:
                            msg.Write((byte)myClient.PlayerId);
                            break;

                        default:
                            break;
                    }
                }

                myClient.SendMessageDebug(msg);
            }
        }
        
        private void cmbMessageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageType type = (MessageType)cmbMessageType.SelectedItem;
            grpMessageParams.Controls.Clear();
            myInputs.Clear();

            MessagePacketDefinitions.PayloadParameter[] currentParams = MessagePacketDefinitions.GetParams(type);

            int counter = 0;

            for (int index = 0; index < currentParams.Length; index++)
            {
                if (currentParams[index].ParamType != MessagePacketDefinitions.PayloadParamType.PlayerID)
                {
                    Label label = new Label();
                    label.Text = currentParams[index].Name;

                    Control selector;

                    switch (currentParams[index].ParamType)
                    {
                        case MessagePacketDefinitions.PayloadParamType.Boolean:
                            ComboBox tfSelector = new ComboBox();
                            tfSelector.DropDownStyle = ComboBoxStyle.DropDownList;
                            tfSelector.Items.Add(true);
                            tfSelector.Items.Add(false);
                            tfSelector.SelectedIndex = 0;
                            selector = tfSelector;
                            break;

                        case MessagePacketDefinitions.PayloadParamType.BotDifficulty:
                            TrackBar bar = new TrackBar();
                            bar.Minimum = 0;
                            bar.Maximum = 255;
                            bar.TickFrequency = 1;
                            selector = bar;
                            break;

                        case MessagePacketDefinitions.PayloadParamType.Byte:
                            NumericUpDown nud = new NumericUpDown();
                            nud.Minimum = 0;
                            nud.Maximum = 255;
                            selector = nud;
                            break;

                        case MessagePacketDefinitions.PayloadParamType.CardRank:
                            ComboBox rnkSelector = new ComboBox();
                            rnkSelector.DropDownStyle = ComboBoxStyle.DropDownList;
                            rnkSelector.Items.AddRange(Enum.GetValues(typeof(CardRank)).OfType<object>().ToArray());
                            rnkSelector.SelectedIndex = 0;
                            selector = rnkSelector;
                            break;

                        case MessagePacketDefinitions.PayloadParamType.CardSuit:
                            ComboBox suitSelector = new ComboBox();
                            suitSelector.DropDownStyle = ComboBoxStyle.DropDownList;
                            suitSelector.Items.AddRange(Enum.GetValues(typeof(CardSuit)).OfType<object>().ToArray());
                            suitSelector.SelectedIndex = 0;
                            selector = suitSelector;
                            break;

                        case MessagePacketDefinitions.PayloadParamType.Integer:
                            NumericUpDown nudInt = new NumericUpDown();
                            selector = nudInt;
                            break;

                        case MessagePacketDefinitions.PayloadParamType.ServerState:
                            ComboBox stateSelector = new ComboBox();
                            stateSelector.DropDownStyle = ComboBoxStyle.DropDownList;
                            stateSelector.Items.AddRange(Enum.GetValues(typeof(ServerState)).OfType<object>().ToArray());
                            stateSelector.SelectedIndex = 0;
                            selector = stateSelector;
                            break;

                        case MessagePacketDefinitions.PayloadParamType.String:
                            TextBox textIn = new TextBox();
                            selector = textIn;
                            break;

                        default:
                            selector = new Panel();
                            break;
                    }

                    label.Top = 24 + 24 * counter;
                    label.Left = 5;

                    selector.Top = 24 + 24 * counter;
                    selector.Left = 24 + label.Right + 5;

                    grpMessageParams.Controls.Add(label);
                    grpMessageParams.Controls.Add(selector);

                    myInputs.Add(new MessageInput() { Type = currentParams[index].ParamType, InputControl = selector });

                    counter++;
                }
                else
                    myInputs.Add(new MessageInput() { Type = MessagePacketDefinitions.PayloadParamType.PlayerID, InputControl = null });
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (myClient != null)
            {
                myClient.Disconnect();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (myClient != null)
            {
                if (myDicoveredServers.Count > 0)
                {
                    myClient.ConnectTo(myDicoveredServers[0]);
                }
            }
        }

        private void btnDiscover_Click(object sender, EventArgs e)
        {
            if (myClient != null)
            {
                myClient.DiscoverServers();
            }
        }
    }
}
