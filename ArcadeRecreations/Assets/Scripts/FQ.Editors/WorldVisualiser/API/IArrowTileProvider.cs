using UnityEngine.Tilemaps;

namespace FQ.Editors
{
    /// <summary>
    /// Provides Loop visualisations.
    /// </summary>
    public interface IArrowTileProvider
    {
        /// <summary>
        /// Returns an arrow tile matching the given.
        /// </summary>
        /// <param name="directions">Directions to show. </param>
        /// <param name="purpose">Purpose of the visualisation. </param>
        /// <returns>A <see cref="Tile"/> or null if no tile exists. </returns>
        Tile GetArrowTile(ArrowDirection directions, ArrowPurpose purpose);
    }
}