using Durak.Common.Cards;

namespace Durak.Server
{
    /// <summary>
    /// Represents a move proposed by an AI module
    /// </summary>
    public struct AIMoveProposal
    {
        /// <summary>
        /// Backing field for Confidence
        /// </summary>
        private float myConfidence;

        /// <summary>
        /// Gets or sets the confidence for this result (in the range of 0-1)
        /// </summary>
        public float Confidence
        {
            get { return myConfidence; }
            set
            {
                myConfidence = value < 0 ? 0 : value > 1 ? 1 : 0;
            }
        }
        /// <summary>
        /// Gets or sets the card proposed to be played
        /// </summary>
        public PlayingCard Move { get; set; }

        /// <summary>
        /// Creates a new AI move proposal
        /// </summary>
        /// <param name="card">The card to play</param>
        /// <param name="confidence">The confidence in this move, from 0-1</param>
        public AIMoveProposal(PlayingCard card, float confidence)
        {
            myConfidence = confidence;
            Move = card;
            Confidence = confidence;
        }

        /// <summary>
        /// Overrides the confidence detection logic to manually set the confidence
        /// </summary>
        /// <param name="confidence">The confidence to set</param>
        public void SetConfidenceOverride(float confidence)
        {
            myConfidence = confidence;
        }
    }
}