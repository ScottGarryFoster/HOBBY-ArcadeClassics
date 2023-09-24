namespace FQ.GameElementCommunication
{
    /// <summary>
    /// The overall purpose, type or goal of a collectable for the outside world to categorise.
    /// </summary>
    public enum CollectableBucket
    {
        /// <summary>
        /// Offered consistently to the player.
        /// </summary>
        BasicValue,
        
        /// <summary>
        /// Offer which has a time limit or other such limit applied.
        /// </summary>
        LimitedOffer,
    }
}