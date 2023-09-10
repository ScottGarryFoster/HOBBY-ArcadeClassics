namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Connects elements to broad information they might want.
    /// </summary>
    public interface IElementCommunication
    {
        /// <summary>
        /// Basics about the Player.
        /// </summary>
        PlayerStatus PlayerStatus { get; }
    }
}