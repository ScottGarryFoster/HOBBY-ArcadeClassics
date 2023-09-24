using System;
using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Basics on locations and information on Collectable items.
    /// </summary>
    public interface ICollectableStatusBasics
    {
        /// <summary>
        /// Called whenever the collectable details are updated.
        /// </summary>
        public event EventHandler CollectableDetailsUpdated;
        
        /// <summary>
        /// Gets all reported collectable locations.
        /// </summary>
        /// <returns>Either the locations of the collectables or an empty array as none yet reported. </returns>
        Vector2Int[] GetCollectableLocation();
        
        /// <summary>
        /// Gets collectable of the given bucket.
        /// </summary>
        /// <param name="bucket">Bucket to search for. </param>
        /// <returns>Either the locations of the collectables or an empty array as none yet reported. </returns>
        Vector2Int[] GetCollectableLocation(CollectableBucket bucket);
    }
}