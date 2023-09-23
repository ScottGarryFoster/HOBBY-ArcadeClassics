using FQ.Libraries.StandardTypes;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Provides information about the world based on tilemaps.
    /// </summary>
    public interface IWorldInfoFromTilemap
    {
        /// <summary>
        /// Gets loop information based on the given input.
        /// </summary>
        /// <param name="location">Location to test. </param>
        /// <param name="direction">Direction the Snake is moving in. </param>
        /// <returns>Answer as to whether there are loops. </returns>
        CollisionPositionAnswer GetLoop(Vector2Int location, MovementDirection direction);

        /// <summary>
        /// Get the area which provided the player was not there, would be travelable.
        /// </summary>
        /// <returns>All the tiles which would be travelable. </returns>
        Vector3Int[] GetTravelableArea();
    }
}