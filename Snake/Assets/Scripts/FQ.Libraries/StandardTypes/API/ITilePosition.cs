using UnityEngine;

namespace FQ.Libraries.StandardTypes
{
    /// <summary>
    /// Represents the position on a tile grid.
    /// </summary>
    public interface ITilePosition
    {
        /// <summary>
        /// Position on the grid.
        /// </summary>
        Vector3Int Position { get; }
    }
}