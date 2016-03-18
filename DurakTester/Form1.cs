using Durak.Client;
using Durak.Common;
using Durak.Common.Cards;
using Durak.Server;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakTester
{
    public partial class Form1 : Form
    {
        GameServer myServer;
        GameClient[] myClients;
        ClientButtonsTag[] myButtonTags;
        IPAddress myServerAddress;
        List<MessageInput> myInputs;

        public Form1()
        {
            InitializeComponent();

            myInputs = new List<MessageInput>();

            myClients = new GameClient[4];
            myButtonTags = new ClientButtonsTag[4];

            for (int index = 0; index < myClients.Length; index ++)
            {
                Button clientStartButton = new Button();
                clientStartButton.Text = "Start";
                clientStartButton.Click += btnClientStart_Click;
                clientStartButton.Tag = index;
                clientStartButton.Left = rtbServerOutput.Right + 5;
                clientStartButton.Top = rtbServerOutput.Top + (24 * index);

                Button clientEndButton = new Button();
                clientEndButton.Text = "Stop";
                clientEndButton.Enabled = false;
                clientEndButton.Click += btnClientStop_Click;
                clientEndButton.Left = clientStartButton.Right + 5;
                clientEndButton.Top = clientStartButton.Top;

                Button clientConnectButton = new Button();
                clientConnectButton.Text = "Connect";
                clientConnectButton.Enabled = false;
                clientConnectButton.Click += btnClientConnect_Click;
                clientConnectButton.Left = clientEndButton.Right + 5;
                clientConnectButton.Top = clientEndButton.Top;

                ClientButtonsTag tag = new ClientButtonsTag()
                {
                    Start = clientStartButton,
                    End = clientEndButton,
                    Connect = clientConnectButton,
                    ClientIndex = index
                };

                myButtonTags[index] = tag;

                clientStartButton.Tag = tag;
                clientEndButton.Tag = tag;
                clientConnectButton.Tag = tag;

                this.Controls.Add(clientStartButton);
                this.Controls.Add(clientEndButton);
                this.Controls.Add(clientConnectButton);

                cmbFromClient.Items.Add(index);
            }

            cmbFromClient.SelectedIndex = 0;

            object[] values = Enum.GetValues(typeof(MessageType)).OfType<object>().ToArray();
            cmbMessageType.Items.AddRange(values);
            cmbMessageType.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnInitServer_Click(object sender, EventArgs e)
        {
            myServer = new GameServer();
            myServer.SetOutput(rtbServerOutput);
            myServer.Run();

            if (myServerAddress == null)
                myServerAddress = myServer.IP;

            btnInitServer.Enabled = false;
            btnKillServer.Enabled = true;
        }

        private void btnKillServer_Click(object sender, EventArgs e)
        {
            myServer.Stop();
            myServer = null;

            btnInitServer.Enabled = true;
            btnKillServer.Enabled = false;
        }

        private void btnClientDisconnect_Click(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;

            if (buttonSender != null)
            {
                ClientButtonsTag tag = buttonSender.Tag as ClientButtonsTag;

                if (tag != null)
                {
                    int index = tag.ClientIndex;

                    if (myClients[index] != null)
                    {
                        myClients[index].Disconnect();
                    }

                    tag.Connect.Enabled = false;
                }
            }
        }

        private void btnClientConnect_Click(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;

            if (buttonSender != null)
            {
                ClientButtonsTag tag = buttonSender.Tag as ClientButtonsTag;

                if (tag != null)
                {
                    int index = tag.ClientIndex;

                    if (myClients[index] != null)
                    {
                        myClients[index].ConnectTo(myServerAddress);

                        tag.Connect.Enabled = false;
                    }
                    
                }
            }
        }

        private void btnClientStop_Click(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;

            if (buttonSender != null)
            {
                ClientButtonsTag tag = buttonSender.Tag as ClientButtonsTag;

                if (tag != null)
                {
                    int index = tag.ClientIndex;

                    if (myClients[index] != null)
                    {
                        myClients[index].Terminate();
                        myClients[index] = null;
                    }

                    tag.Start.Enabled = true;
                    tag.End.Enabled = false;
                    tag.Connect.Enabled = false;
                }
            }
        }

        private void btnClientStart_Click(object sender, EventArgs e)
        {
            Button buttonSender = sender as Button;

            if (buttonSender != null)
            {
                ClientButtonsTag tag = buttonSender.Tag as ClientButtonsTag;

                if (tag != null)
                {
                    int index = tag.ClientIndex;

                    if (myClients[index] != null)
                        myClients[index].Terminate();

                    ClientTag cTag = new ClientTag("Player " + index);

                    myClients[index] = new GameClient(cTag);
                    myClients[index].Tag = tag;
                    myClients[index].OnDisconnected += Form1_OnDisconnected;
                    myClients[index].OnConnectionFailed += Form1_OnConnectionFailed;
                    myClients[index].OnConnectionTimedOut += Form1_OnConnectionTimedOut;
                    myClients[index].Initialize();
                    myClients[index].Run();
                    myClients[index].DiscoverServers();

                    tag.Start.Enabled = false;
                    tag.End.Enabled = true;
                    tag.Connect.Enabled = true;
                }
            }
        }

        private void Form1_OnConnectionTimedOut(object sender, EventArgs e)
        {
            MessageBox.Show("Connection timed out");
        }

        private void Form1_OnConnectionFailed(object sender, string e)
        {
            ClientButtonsTag tag = (sender as GameClient).Tag as ClientButtonsTag;
            tag.Connect.Enabled = true;
        }

        private void Form1_OnDisconnected(object sender, EventArgs e)
        {
            ClientButtonsTag tag = (sender as GameClient).Tag as ClientButtonsTag;
            tag.Connect.Enabled = true;
        }

        private class ClientButtonsTag
        {
            public Button Start;
            public Button End;
            public Button Connect;
            public int ClientIndex;
        }

        private void cmbMessageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageType type = (MessageType)cmbMessageType.SelectedItem;
            grpMessageParams.Controls.Clear();
            myInputs.Clear();

            MessagePacketDefinitions.PayloadParameter[] currentParams = MessagePacketDefinitions.GetParams(type);

            for(int index = 0; index < currentParams.Length; index ++)
            {
                Label label = new Label();
                label.Text = currentParams[index].Name;

                Control selector;

                switch(currentParams[index].ParamType)
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
                        ComboBox difSelector = new ComboBox();
                        difSelector.DropDownStyle = ComboBoxStyle.DropDownList;
                        difSelector.Items.Add(0);
                        difSelector.Items.Add(1);
                        difSelector.Items.Add(2);
                        difSelector.Items.Add(3);
                        difSelector.SelectedIndex = 0;
                        selector = difSelector;
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

                label.Top = 24 + 24 * index;
                label.Left = 5;

                selector.Top = 24 + 24 * index;
                selector.Left = 24 + label.Right + 5;

                grpMessageParams.Controls.Add(label);
                grpMessageParams.Controls.Add(selector);

                myInputs.Add(new MessageInput() { Type = currentParams[index].ParamType, InputControl = selector });
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int clientId = (int)cmbFromClient.SelectedItem;

            GameClient client = myClients[clientId];

            if (client != null)
            {
                NetOutgoingMessage msg = client.PrepareDebugMessage();

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
                            box = (ComboBox)myInputs[index].InputControl;
                            msg.Write((byte)box.SelectedItem);
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

                        default:
                            break;
                    }
                }

                client.SendMessageDebug(msg);
            }
        }

        private class MessageInput
        {
            public MessagePacketDefinitions.PayloadParamType Type;
            public Control InputControl;
        }
    }
}
