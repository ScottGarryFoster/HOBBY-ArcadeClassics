using System;
using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Basics about the Player.
    /// </summary>
    public class PlayerStatus : IPlayerStatus, IPlayerBroadcaster
    {
        /// <summary>
        /// Every tile which is counted as 'player'.
        /// </summary>
        public Vector2Int[] PlayerLocation { get; private set; }

        /// <summary>
        /// Provide a new location. This is every location considered "player".
        /// </summary>
        /// <param name="location">New location. </param>
        /// <exception cref="System.ArgumentNullException">
        /// location is null.
        /// </exception>
        public void UpdateLocation(Vector2Int[] location)
        {
            Debug.Log($"{typeof(PlayerStatus)}");
        }
    }
}