using System;
using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Basics about the Player.
    /// </summary>
    public class PlayerStatus : IPlayerStatus
    {
        /// <summary>
        /// Every tile which is counted as 'player'.
        /// </summary>
        public Vector2Int[] PlayerLocation { get; private set; }

        public PlayerStatus()
        {
            PlayerLocation = Array.Empty<Vector2Int>();
        }
        
        /// <summary>
        /// Provide a new location. This is every location considered "player".
        /// </summary>
        /// <param name="location">New location. </param>
        /// <exception cref="System.ArgumentNullException">
        /// location is null.
        /// </exception>
        public void UpdateLocation(Vector2Int[] location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(
                    $"{typeof(PlayerStatus)}: {nameof(location)} must not be null.");
            }
            
            PlayerLocation = location;
        }
    }
}