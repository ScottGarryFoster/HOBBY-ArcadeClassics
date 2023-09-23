using FQ.Libraries.StandardTypes;
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
        
        /// <summary>
        /// Provide a new direction for the player head.
        /// </summary>
        /// <param name="direction">An updated <see cref="MovementDirection"/>. </param>
        void UpdatePlayerHeadDirection(MovementDirection direction);
    }
}