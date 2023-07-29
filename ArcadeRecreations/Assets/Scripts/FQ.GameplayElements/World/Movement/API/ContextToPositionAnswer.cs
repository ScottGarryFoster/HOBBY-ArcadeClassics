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
        /// </summary>
        NoMovementNeeded,
        
        /// <summary>
        /// There is no valid movement to move the player.
        /// </summary>
        NoValidMovement,
    }
}