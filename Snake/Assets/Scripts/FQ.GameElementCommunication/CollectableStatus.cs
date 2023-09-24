using System;
using System.Collections.Generic;
using System.Linq;
using FQ.Logger;
using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Locations and information on Collectable items.
    /// </summary>
    public class CollectableStatus : ICollectableStatus
    {
        /// <summary>
        /// Called whenever the collectable details are updated.
        /// </summary>
        public event EventHandler CollectableDetailsUpdated;

        /// <summary>
        /// Contains all the locations for collectables in the scene.
        /// </summary>
        private readonly Dictionary<CollectableBucket, Vector2Int[]> collectionLocations;

        public CollectableStatus()
        {
            this.collectionLocations = new Dictionary<CollectableBucket, Vector2Int[]>();
            foreach (int i in Enum.GetValues(typeof(CollectableBucket)))
            {
                this.collectionLocations.Add((CollectableBucket)i, Array.Empty<Vector2Int>());
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
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
        public void UpdateCollectableLocation(CollectableBucket bucket, Vector2Int[] locations)
        {
            EnsureLocationsIsNotNullOrThrow(locations);

            // Note this relies on the construction adding all enum values.
            this.collectionLocations[bucket] = locations;
            
            this.CollectableDetailsUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets all reported collectable locations.
        /// </summary>
        /// <returns>Either the locations of the collectables or an empty array as none yet reported. </returns>
        public Vector2Int[] GetCollectableLocation()
        {
            return this.collectionLocations.Values.SelectMany(x => x).ToArray();
        }

        /// <summary>
        /// Gets collectable of the given bucket.
        /// </summary>
        /// <param name="bucket">Bucket to search for. </param>
        /// <returns>Either the locations of the collectables or an empty array as none yet reported. </returns>
        public Vector2Int[] GetCollectableLocation(CollectableBucket bucket)
        {
            // Note this is only secure due to construction including all enum options
            // and never allowing null in the dictionary.
            return this.collectionLocations[bucket];
        }
        
        /// <summary>
        /// Checks Variable is not null otherwise throw as though in <see cref="UpdateCollectableLocation"/>.
        /// </summary>
        /// <param name="locations">Input to check. </param>
        /// <exception cref="System.ArgumentNullException">
        /// List of <see cref="Vector2Int"/> locations is null.
        /// Send in an empty list to wipe the list.
        /// </exception>
        private void EnsureLocationsIsNotNullOrThrow(Vector2Int[] locations)
        {
            if (locations == null)
            {
                Log.Error($"{nameof(locations)} cannot be null. " +
                          "Send in empty list if you intend to empty the collection.",
                    typeof(CollectableStatus).ToString(),
                    nameof(UpdateCollectableLocation));
                throw new ArgumentNullException(
                    $"{typeof(CollectableStatus)}: {nameof(locations)} cannot be null. " +
                    $"Send in empty list if you intend to empty the collection.");
            }
        }
    }
}