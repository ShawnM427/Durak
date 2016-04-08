/**
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

namespace Durak
{
    /// <summary>
    /// A control to use in a windows forms application that displays a playing card.
    /// </summary>
    public partial class CardBox: UserControl
    {

        #region FIELDS AND PROPERTIES

        /// <summary>
        /// Card Property:
        /// Sets / gets the underlying Card object
        /// </summary>
        private PlayingCard myCard;
        public PlayingCard Card
        {
            get { return myCard; }
            set
            {
                myCard = value;
                if (myCard != null)
                    pbMyPictureBox.Image = myCard.GetCardImage(); // Update the card image
                else
                    pbMyPictureBox.Image = null;
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
                UpdateCardImage(); // update card image
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
                UpdateCardImage(); // update card image
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

                    UpdateCardImage(); // update the card image (back or front)

                    // if there is an event handler for CardFlipped in the client program
                    if (CardFlipped != null)
                        CardFlipped(this, new EventArgs()); // call it
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
                    UpdateCardImage(); // update the card image
                }
            }
        }

        /// <summary>
        /// UpdateCardImage helper method:
        /// Sets the picturebox image using the underlying
        /// card and the orientation
        /// </summary>
        private void UpdateCardImage()
        {
            // set the image using the underlying card
            pbMyPictureBox.Image = myCard.GetCardImage();

            // if the orientation is horizontal
            if (myOrientation == Orientation.Horizontal)
            {
                // rotate the image
                pbMyPictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
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
            UpdateCardImage();
        }

        /// <summary>
        /// Constructor (PlayingCard, Orientation): Constructs the control using parameters
        /// </summary>
        /// <param name="card">Underlying PlayingCard object</param>
        /// <param name="orientation">Orientation enumeration. Vertical by default</param>
        public CardBox(PlayingCard card, Orientation orientation = Orientation.Vertical)
        {
            InitializeComponent(); // required method for designer support
            myOrientation = orientation; // set the orientation
            myCard = card; // set the underlying card
        }
        #endregion

        #region EVENTS AND EVENT HANDLERS

        /// <summary>
        /// An event handler for the load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardBox_Load(object sender, EventArgs e)
        {
            UpdateCardImage(); // update card image
        }

        /// <summary>
        /// An event the client program can handle when the card flips face up/down
        /// </summary>
        public event EventHandler CardFlipped;

        /// <summary>
        /// An event the client program can handle when the user clicks the control
        /// </summary>
        new public event EventHandler Click;

        /// <summary>
        /// An event handler for the user clicking the picture box control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbMyPictureBox_Click(object sender, EventArgs e)
        {
            if (Click != null) // if there is a handler for clicking the control in the client program
                Click(this, e); // call it
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
