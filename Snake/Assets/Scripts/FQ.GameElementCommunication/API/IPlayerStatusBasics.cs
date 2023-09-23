using System;
using FQ.Libraries.StandardTypes;
using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Basics about the Player.
    /// </summary>
    public interface IPlayerStatusBasics
    {
        /// <summary>
        /// Called whenever the player details are updated.
        /// </summary>
        public event EventHandler PlayerDetailsUpdated;
        
        /// <summary>
        /// Every tile which is counted as 'player'
        /// </summary>
        Vector2Int[] PlayerLocation { get; }
        
        /// <summary>
        /// Last known player direction.
        /// </summary>
        MovementDirection PlayerDirection { get; }
    }
}