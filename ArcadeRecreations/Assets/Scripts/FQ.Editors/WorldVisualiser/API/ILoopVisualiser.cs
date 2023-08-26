using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.Editors
{
    /// <summary>
    /// Manages and creates the Loop visualiser.
    /// </summary>
    public interface ILoopVisualiser
    {
        /// <summary>
        /// Adds a new visualisation layer using the data of the given border.
        /// </summary>
        /// <param name="prefab">Prefab to use when making the visualisation. </param>
        /// <param name="scanTilemap">Tilemap to find the loops. </param>
        /// <param name="borderTile">Border tile to search for. </param>
        /// <param name="arrowTileProvider">Provides the tiles to use in the visuals. </param>
        /// <param name="elementsToShow">Describes the world loop elements to display in the object. </param>
        /// <returns>A reference to the created object. </returns>
        GameObject AddVisualisationObject(
            GameObject prefab, 
            Tilemap scanTilemap, 
            Tile borderTile,
            IArrowTileProvider arrowTileProvider,
            WorldLoopElementsToShow elementsToShow);
    }
}