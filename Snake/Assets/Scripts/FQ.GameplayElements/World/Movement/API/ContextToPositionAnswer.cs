namespace FQ.GameplayElements
{
    /// <summary>
    /// Upon giving a position this is the reason why.
    /// </summary>
    public enum ContextToPositionAnswer
    {
        /// <summary>
        /// The new position given has been correctly calculated.
        /// </summary>
        NewPositionIsCorrect,
        
        /// <summary>
        /// The correct movement is no movement.
        /// Means there is no valid loop but the border does exist.
        /// </summary>
        NoMovementNeeded,
        
        /// <summary>
        /// There is no valid movement to move the player.
        /// Also used for no border in place.
        /// </summary>
        NoValidMovement,
    }
}