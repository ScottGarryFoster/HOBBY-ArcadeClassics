using System;
using UnityEngine;

namespace FQ.Libraries.StandardTypes
{
    /// <summary>
    /// Represents the position on a tile grid.
    /// </summary>
    public class TilePositionFromTransform : ITilePosition
    {
        /// <summary>
        /// Position on the grid.
        /// </summary>
        public Vector3Int Position
        {
            get
            {
                Vector3 position = transform.position;
                return new Vector3Int((int)(position.x - offset.x), (int)(position.y - offset.y), (int)(position.z - offset.z));
            }
        }

        /// <summary>
        /// Holds the position data.
        /// </summary>
        private readonly Transform transform;

        /// <summary>
        /// An offset of the position used at all times to ensure alignment.
        /// </summary>
        private readonly Vector3 offset;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="transform">Holds the position data. </param>
        /// <param name="offset">An offset of the position used at all times to ensure alignment. </param>
        public TilePositionFromTransform(Transform transform, Vector3 offset)
        {
            this.transform = transform;
            this.offset = offset;
        }
    }
}