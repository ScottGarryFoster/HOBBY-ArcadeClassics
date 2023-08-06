using System;
using System.Collections.Generic;
using FQ.Libraries;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.Editors
{
    /// <summary>
    /// Provides Loop visualisations.
    /// </summary>
    public class SimpleArrowTileProvider : IArrowTileProvider
    {
        /// <summary>
        /// The number of tiles per purpose set.
        /// </summary>
        private const int TilesPerSet = 20;
        
        /// <summary>
        /// All the loop arrow prefabs organised and preloaded.
        /// </summary>
        private readonly Dictionary<ArrowPurpose, Dictionary<ArrowDirection, Tile>> loopArrows;

        /// <summary>
        /// Creates arrow tile prefabs from given string.
        /// </summary>
        /// <param name="arrowTilePrefabPrefix">
        /// The name of prefabs for arrows until the number.
        /// Supposes arrows are in sets and then each purpose occurs one after another.
        /// </param>
        /// <exception cref="ArgumentNullException">If a prefab does not exist. </exception>
        public SimpleArrowTileProvider(string arrowTilePrefabPrefix)
        {
            if (string.IsNullOrWhiteSpace(arrowTilePrefabPrefix))
            {
                throw new ArgumentNullException();
            }

            this.loopArrows = new Dictionary<ArrowPurpose, Dictionary<ArrowDirection, Tile>>();
            CacheArrows(arrowTilePrefabPrefix);
        }
        
        /// <summary>
        /// Returns an arrow tile matching the given.
        /// </summary>
        /// <param name="directions">Directions to show. </param>
        /// <param name="purpose">Purpose of the visualisation. </param>
        /// <returns>A <see cref="Tile"/> or null if no tile exists. </returns>
        public Tile GetArrowTile(ArrowDirection directions, ArrowPurpose purpose)
        {
            if (this.loopArrows.TryGetValue(purpose, out Dictionary<ArrowDirection, Tile> value))
            {
                if (value.TryGetValue(directions, out Tile tile))
                {
                    return tile;
                }
            }

            return null;
        }

        /// <summary>
        /// Preloads and caches the arrow prefabs.
        /// </summary>
        /// <param name="arrowTilePrefabs"></param>
        private void CacheArrows(string arrowTilePrefabs)
        {
            foreach (int purpose in Enum.GetValues(typeof(ArrowPurpose)))
            {
                var arrowPurpose = (ArrowPurpose) purpose;
                Dictionary<ArrowDirection, Tile> currentPurpose = new();
                this.loopArrows.Add(arrowPurpose, currentPurpose);

                int actualTileValue = purpose * TilesPerSet;
                FindAndAddTilePrefabsToCache(arrowTilePrefabs, actualTileValue, currentPurpose);
            }
        }

        /// <summary>
        /// Adds the arrow tiles to the cache.
        /// </summary>
        /// <param name="arrowTilePrefabs">The prefix for the prefabs. </param>
        /// <param name="actualTileValue">The number to start counting the tile prefabs from. </param>
        /// <param name="currentPurposeSet">Where to add prefabs to. </param>
        /// <exception cref="ArgumentNullException">If a prefab does not exist. </exception>
        private void FindAndAddTilePrefabsToCache(string arrowTilePrefabs, int actualTileValue, Dictionary<ArrowDirection, Tile> currentPurposeSet)
        {
            for (int tile = 0; tile < TilesPerSet; ++tile)
            {
                string prefabName = $"{arrowTilePrefabs}{actualTileValue++}";
                Tile arrow = Resources.Load<Tile>(prefabName);
                if (arrow == null)
                {
                    throw new InvalidParameter(
                        $"{typeof(SimpleArrowTileProvider)}: Prefab is invalid. Name: {prefabName}.");
                }

                ArrowDirection direction = GetDirection(tile);
                if (direction != ArrowDirection.None)
                {
                    currentPurposeSet.Add(direction, arrow);
                }
            }
        }

        /// <summary>
        /// Returns the direction for the tile number in a set.
        /// </summary>
        /// <param name="tile">Tile number in a set. </param>
        /// <returns>Direction(s) which relate to the tile number. </returns>
        private ArrowDirection GetDirection(int tile)
        {
            switch (tile)
            {
                case 0: return ArrowDirection.Down | ArrowDirection.Left | ArrowDirection.Right | ArrowDirection.Up;
                case 1: return ArrowDirection.Down | ArrowDirection.Right | ArrowDirection.Up;
                case 2: return ArrowDirection.Left | ArrowDirection.Right | ArrowDirection.Up;
                case 3: return ArrowDirection.Down | ArrowDirection.Left | ArrowDirection.Up;
                case 4: return ArrowDirection.Down | ArrowDirection.Right | ArrowDirection.Left;
                case 5: return ArrowDirection.Right | ArrowDirection.Up;
                case 6: return ArrowDirection.Left | ArrowDirection.Up;
                case 7: return ArrowDirection.Left | ArrowDirection.Down;
                case 8: return ArrowDirection.Right | ArrowDirection.Down;
                case 9: return ArrowDirection.Left | ArrowDirection.Right;
                case 10: return ArrowDirection.Up | ArrowDirection.Down;
                case 11: return ArrowDirection.Left;
                case 12: return ArrowDirection.Down;
                case 13: return ArrowDirection.Right;
                case 14: return ArrowDirection.Up;
            }

            return ArrowDirection.None;
        }
    }
}