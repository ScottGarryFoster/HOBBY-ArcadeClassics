using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Basics about the Player.
    /// </summary>
    public interface IPlayerStatusBasics
    {
        /// <summary>
        /// Every tile which is counted as 'player'
        /// </summary>
        Vector2Int[] PlayerLocation { get; }
    }
}