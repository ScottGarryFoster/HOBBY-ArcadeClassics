using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Provides information about loops based on tilemaps in the world.
    /// </summary>
    public interface ILoopingWorldFromTilemap
    {
        /// <summary>
        /// Gets loop information based on the given input.
        /// </summary>
        /// <param name="location">Location to test. </param>
        /// <param name="direction">Direction the Snake is moving in. </param>
        /// <returns>Answer as to whether there are loops. </returns>
        CollisionPositionAnswer GetLoop(Vector2Int location, Direction direction);
    }
}