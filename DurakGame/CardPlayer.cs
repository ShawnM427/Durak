using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Durak.Common.Cards;

namespace DurakGame
{
    public partial class CardPlayer : UserControl
    {
        private const float CARD_ASPECT_RATIO = 1.466666666666f;

        private float myCardWidth;
        private float myCardHeight;
        private float myCardOffsetX;
        private float myCardOffsetY;

        private CardCollection myCards;

        public CardCollection Cards
        {
            get { return myCards; }
            set
            {
                if (myCards != null)
                {
                    myCards.OnCardAdded -= OnCardsChanged;
                    myCards.OnCardRemoved -= OnCardsChanged;
                }

                myCards = value;

                if (myCards != null)
                {
                    myCards.OnCardAdded += OnCardsChanged;
                    myCards.OnCardRemoved += OnCardsChanged;
                }
            }
        }

        private void OnCardsChanged(object sender, CardEventArgs e)
        {
            Invalidate();
        }

        public CardPlayer()
        {
            InitializeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            try
            {
                myCardWidth = Width / (float)myCards.Count;
                myCardHeight = myCardWidth * CARD_ASPECT_RATIO;

                myCardOffsetX = (Width - (myCardWidth * myCards.Count)) / 2.0f;
                myCardOffsetY = (Height - myCardHeight) / 2.0f;

                if (myCardHeight > Height)
                {
                    myCardHeight = Height;
                    myCardWidth = myCardHeight * (1 / CARD_ASPECT_RATIO);
                }

                base.OnResize(e);
            }
            catch (Exception ex) { }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (myCards != null)
            {
                for (int index = 0; index < myCards.Count; index++)
                {
                    PlayingCard card = myCards[index];

                    Image i = card.GetCardImage();

                    e.Graphics.DrawImage(i, myCardOffsetX + myCardWidth * index, myCardOffsetY, myCardWidth, myCardHeight);
                }
            }

            base.OnPaint(e);
        }
    }
}
