using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Upon attempting to find a new position on collision with the world, this is the answer.
    /// </summary>
    public struct CollisionPositionAnswer
    {
        /// <summary>
        /// Upon giving a position this is the reason why.
        /// </summary>
        public ContextToPositionAnswer Answer;

        /// <summary>
        /// The new position they should take.
        /// </summary>
        public Vector3 NewPosition;

        /// <summary>
        /// The new direction they should be in.
        /// </summary>
        public Direction NewDirection;
    }
}