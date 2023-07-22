using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Helps to determine if there is another position to send the actor upon collision with the world.
    /// </summary>
    public interface IPositionOnWorldCollision
    {
        /// <summary>
        /// Finds a new position for the actor if they collide with the world.
        /// </summary>
        /// <param name="currentPosition">Where the actor is currently. </param>
        /// <param name="currentDirection">The direction they took to get here. </param>
        /// <param name="newPosition">The new position they should take. </param>
        /// <param name="newDirection">The new direction they should be in. </param>
        /// <returns></returns>
        ContextToPositionAnswer FindNewPositionForPlayer(
            Vector3 currentPosition,
            Direction currentDirection, 
            out Vector3 newPosition, 
            out Direction newDirection);
    }
}