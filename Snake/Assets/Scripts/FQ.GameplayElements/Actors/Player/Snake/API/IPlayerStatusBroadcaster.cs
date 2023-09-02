using System;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// The ability to broadcast player status.
    /// </summary>
    public interface IPlayerStatusBroadcaster
    {
        /// <summary>
        /// Update where the player is considered to be.
        /// </summary>
        Action<Vector2Int[]> UpdatePlayerLocation { get; set; }
    }
}