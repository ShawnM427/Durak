/*
 * CardBox.cs - The CardBox Class
 *
 * @author      Ryan Schuette
 * @version     1.0
 * @since       23 Feb 2016
 */

using System;
using System.Drawing;
using System.Windows.Forms;

using Durak.Common.Cards;
using System.Drawing.Imaging;

namespace Durak
{
    /// <summary>
    /// A control to use in a windows forms application that displays a playing card.
    /// </summary>
    public partial class CardBox: UserControl
    {

        #region FIELDS AND PROPERTIES

        /// <summary>
        /// Stores the card object
        /// </summary>
        private PlayingCard myCard;
        /// <summary>
        /// Stores the image of the card being rendered
        /// </summary>
        private Image myImage;
        /// <summary>
        /// Card Property:
        /// Sets / gets the underlying Card object
        /// </summary>
        public PlayingCard Card
        {
            get { return myCard; }
            set
            {
                myCard = value;

                if (myCard != null)
                    myImage = myCard.GetCardImage();

                Invalidate();
            }
        }

        /// <summary>
        /// Suit Property:
        /// Sets / gets the underlying Card objects suit
        /// </summary>
        public CardSuit Suit
        {
            get { return Card.Suit; }
            set
            {
                Card.Suit = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Rank Property:
        /// Sets / gets the underlying Card objects rank
        /// </summary>
        public CardRank Rank
        {
            get { return Card.Rank; }
            set
            {
                Card.Rank = value;
                Invalidate();
            }
        }

        /// <summary>
        /// FaceUp Property:
        /// Sets / gets the underlying Card objects FaceUp property
        /// </summary>
        public bool FaceUp
        {
            get { return Card.FaceUp; }
            set
            {
                // if value is different than the underlying card's FaceUp property
                if (myCard.FaceUp != value) // then the card is flipping over
                {
                    myCard.FaceUp = value; // change the card's FaceUp property
                    Invalidate();

                    // if there is an event handler for CardFlipped in the client program
                    CardFlipped?.Invoke(this, new EventArgs()); // call it
                }
            }
        }

        /// <summary>
        /// CardOrientation Property:
        /// Sets / gets the orientation of the card.
        /// if setting this property changes the state of control, swaps
        /// the height and width of the control and updates the image.
        /// </summary>
        private Orientation myOrientation;
        /// <summary>
        /// Gets or sets the orientation of the card
        /// </summary>
        public Orientation CardOrientation
        {
            get { return myOrientation; }
            set
            {
                // if value is different than myOrientation
                if (myOrientation != value)
                {
                    myOrientation = value; // change the orientation
                    // swap the height and width of the control
                    this.Size = new Size(Size.Height, Size.Width);
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the low level winforms create parameters, adding transparency
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constuctor (Default):
        /// Constructs the control with a new card, oriented vertically
        /// </summary>
        public CardBox()
        {
            InitializeComponent(); // required method for designer support
            myOrientation = Orientation.Vertical; // set the orietation to vertical
            myCard = new PlayingCard(); // create a new underlying card
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        /// <summary>
        /// Constructor (PlayingCard, Orientation): Constructs the control using parameters
        /// </summary>
        /// <param name="card">Underlying PlayingCard object</param>
        /// <param name="orientation">Orientation enumeration. Vertical by default</param>
        public CardBox(PlayingCard card, Orientation orientation = Orientation.Vertical) : this()
        {
            myOrientation = orientation; // set the orientation
            myCard = card; // set the underlying card
        }
        #endregion

        #region EVENTS AND EVENT HANDLERS
        
        /// <summary>
        /// An event the client program can handle when the card flips face up/down
        /// </summary>
        public event EventHandler CardFlipped;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        /// <summary>
        /// Overrides the onPaint event, rendering this card
        /// </summary>
        /// <param name="e">The event arguments with the graphics device to render with</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (myCard != null && myCard.GetCardImage() != null)
            {
                Color disabledColor = Color.FromArgb(Enabled ? 0 : 128, 128, 128, 128);

                using (Brush brush = new SolidBrush(disabledColor))
                {
                    e.Graphics.DrawImage(myCard.GetCardImage(), 0, 0, Width, Height);
                    e.Graphics.FillRectangle(brush, 0, 0, Width, Height);
                }
            }
        }

        #endregion

        #region OTHER METHODS

        /// <summary>
        /// ToString: Overrides System.Object.ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return myCard.ToString();
        }


        #endregion

    }
}
