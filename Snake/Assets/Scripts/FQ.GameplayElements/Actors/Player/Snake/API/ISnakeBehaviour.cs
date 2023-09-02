using System;
using FQ.GameObjectPromises;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// The player behaviour behind the Snake Player
    /// </summary>
    public interface ISnakeBehaviour : IGameActor, ITriggerCollidable2D, IActorActiveStats
    {
        /// <summary>
        /// Pieces of the Snake's body which count as the tail.
        /// </summary>
        public SnakeTail[] SnakeTailPieces { get; }
        
        /// <summary>
        /// Called when a game element believes the session has started.
        /// </summary>
        Action StartTrigger { get; set; }
        
        /// <summary>
        /// Called when a game element believes the session has ended.
        /// </summary>
        public Action EndTrigger { get; set; }
        
        /// <summary>
        /// Called when the element should reset it's state to the start of the session.
        /// </summary>
        public Action ResetElement { get; set; }
        
        /// <summary>
        /// Update where the player is considered to be.
        /// </summary>
        public Action<Vector2Int[]> UpdatePlayerLocation { get; set; }
    }
}