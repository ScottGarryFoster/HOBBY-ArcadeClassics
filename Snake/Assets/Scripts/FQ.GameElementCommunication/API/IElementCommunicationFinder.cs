namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Attempts to find ElementCommunication in the Scene
    /// </summary>
    public interface IElementCommunicationFinder
    {
        /// <summary>
        /// Will attempt to find Element communication.
        /// </summary>
        /// <returns>The first ElementCommunication found in the scene or null if none are found. </returns>
        IElementCommunication FindElementCommunication();
    }
}