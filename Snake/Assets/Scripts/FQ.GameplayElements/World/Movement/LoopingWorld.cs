using System;
using FQ.Libraries.StandardTypes;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// A world which gives the same answer for positions at counter positions to one another.
    /// </summary>
    public class LoopingWorld : IPositionOnWorldCollision
    {
        /// <summary>
        /// Finds a new position for the actor if they collide with the world.
        /// </summary>
        /// <param name="currentPosition">Where the actor is currently. </param>
        /// <param name="currentDirection">The direction they took to get here. </param>
        /// <returns>
        /// Upon attempting to find a new position on collision with the world, this is the answer.
        /// </returns>
        public virtual CollisionPositionAnswer FindNewPositionForPlayer(
            Vector2Int currentPosition,
            MovementDirection currentDirection)
        {
            throw new NotImplementedException();
        }
    }
}