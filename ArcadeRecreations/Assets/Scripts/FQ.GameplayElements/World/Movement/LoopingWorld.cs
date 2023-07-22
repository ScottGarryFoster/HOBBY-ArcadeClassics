using System;
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
        /// <param name="newPosition">The new position they should take. </param>
        /// <param name="newDirection">The new direction they should be in. </param>
        /// <returns>Upon returning a position this gives context as how to use it or whether it failed. </returns>
        public ContextToPositionAnswer FindNewPositionForPlayer(
            Vector3 currentPosition,
            Direction currentDirection,
            out Vector3 newPosition,
            out Direction newDirection)
        {
            throw new NotImplementedException();
        }
    }
}