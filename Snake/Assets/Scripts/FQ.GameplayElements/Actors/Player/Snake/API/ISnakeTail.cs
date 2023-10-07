namespace FQ.GameplayElements
{
    /// <summary>
    /// Holds information about the tail piece.
    /// </summary>
    public interface ISnakeTail
    {
        /// <summary>
        /// True this tail piece comes after a border loop.
        /// </summary>
        bool TailPieceAfterBorder { get; }
        
        /// <summary>
        /// If this tail piece went through a loop, this is the information.
        /// </summary>
        CollisionPositionAnswer BorderLoopAnswer { get; }

        /// <summary>
        /// Resets the information outwardly expressed by the interface.
        /// </summary>
        void ResetMetaInfo();
    }
}