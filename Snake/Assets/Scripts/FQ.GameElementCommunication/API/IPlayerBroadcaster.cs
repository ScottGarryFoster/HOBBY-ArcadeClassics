using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// The methods the player will broadcast their status with.
    /// </summary>
    public interface IPlayerBroadcaster
    {
        /// <summary>
        /// Provide a new location. This is every location considered "player".
        /// </summary>
        /// <param name="location">New location. </param>
        /// <exception cref="System.ArgumentNullException">
        /// location is null.
        /// </exception>
        void UpdateLocation(Vector2Int[] location);
    }
}