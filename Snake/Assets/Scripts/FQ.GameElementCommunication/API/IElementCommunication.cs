namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Connects elements to broad information they might want.
    /// </summary>
    public interface IElementCommunication
    {
        /// <summary>
        /// Basics about the Player and the ability to control that data.
        /// </summary>
        IPlayerStatus PlayerStatus { get; }
        
        /// <summary>
        /// Locations and information on Collectable items.
        /// </summary>
        ICollectableStatus CollectableStatus { get; }
    }
}