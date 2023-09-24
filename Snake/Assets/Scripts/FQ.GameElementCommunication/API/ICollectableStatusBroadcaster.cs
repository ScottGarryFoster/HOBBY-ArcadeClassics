using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// The methods a collectable (or controller) will broadcast their status with.
    /// </summary>
    public interface ICollectableStatusBroadcaster
    {
        /// <summary>
        /// Updates where collectables within the given bucket exist.
        /// This overrides any previous provided information.
        /// </summary>
        /// <param name="bucket">The broad type of collectable. </param>
        /// <param name="locations">The location of the collectable. </param>
        /// <exception cref="System.ArgumentNullException">
        /// List of <see cref="Vector2Int"/> locations is null.
        /// Send in an empty list to wipe the list.
        /// </exception>
        void UpdateCollectableLocation(CollectableBucket bucket, Vector2Int[] locations);
    }
}