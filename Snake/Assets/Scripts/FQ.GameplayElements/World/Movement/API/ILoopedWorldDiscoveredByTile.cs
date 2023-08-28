using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.GameplayElements
{
    public interface ILoopedWorldDiscoveredByTile : IPositionOnWorldCollision
    {
        /// <summary>
        /// Calculates the looped positions based on the tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to look for the loop position. </param>
        /// <param name="borderTile">Tile to look for as the border. </param>
        /// <param name="loopAnswer">Answers or discovered loops. </param>
        /// <returns>True means there were no issues. </returns>
        bool CalculateLoops(
            Tilemap tilemap,
            Tile borderTile,
            out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer
            );
    }
}