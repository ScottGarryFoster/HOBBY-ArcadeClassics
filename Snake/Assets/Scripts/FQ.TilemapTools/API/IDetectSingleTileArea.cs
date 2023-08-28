using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.TilemapTools
{
    /// <summary>
    /// Used to detect where a single tile is within a given tilemap.
    /// </summary>
    public interface IDetectSingleTileArea
    {
        /// <summary>
        /// Finds all the tiles touching this tile upon the tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to detect this. Note the limits (size) are defined by this! </param>
        /// <param name="type">Type to detect. </param>
        /// <param name="location">Location to begin. </param>
        /// <returns>All the location that touch the given tile which are like the given tile. </returns>
        Vector3Int[] DetectTilesOfTheSameTypeTouching(Tilemap tilemap, TileBase type, Vector3Int location);
    }
}