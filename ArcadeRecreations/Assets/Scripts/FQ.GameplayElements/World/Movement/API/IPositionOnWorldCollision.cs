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
        /// <returns>
        /// Upon attempting to find a new position on collision with the world, this is the answer.
        /// </returns>
        CollisionPositionAnswer FindNewPositionForPlayer(
            Vector3 currentPosition,
            Direction currentDirection);
    }
}