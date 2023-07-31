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
        /// <param name="arrowTileProvider">Provides the tiles to use in the visuals. </param>
        /// <returns>A reference to the created object. </returns>
        GameObject AddVisualisationObject(
            GameObject prefab, 
            Tilemap scanTilemap, 
            IArrowTileProvider arrowTileProvider);
    }
}