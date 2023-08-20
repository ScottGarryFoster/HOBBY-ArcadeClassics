using System;
using System.Collections.Generic;
using FQ.GameplayElements;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace FQ.Editors
{
    /// <summary>
    /// Manages and creates the Loop visualiser.
    /// </summary>
    public class LoopVisualiser : ILoopVisualiser
    {
        /// <summary>
        /// Adds a new visualisation layer using the data of the given border.
        /// </summary>
        /// <param name="prefab">Prefab to use when making the visualisation. </param>
        /// <param name="scanTilemap">Tilemap to find the loops. </param>
        /// <param name="borderTile">Border tile to search for. </param>
        /// <param name="arrowTileProvider">Provides the tiles to use in the visuals. </param>
        /// <returns>A reference to the created object. </returns>
        public GameObject AddVisualisationObject(
            GameObject prefab,
            Tilemap scanTilemap,
            Tile borderTile,
            IArrowTileProvider arrowTileProvider)
        {
            ValidateParametersForNewVisualisation(prefab, scanTilemap, borderTile, arrowTileProvider);
            
            bool createdEmptyTilemap = CreatePrefab(prefab, out GameObject newObject, out Tilemap tilemap);
            if (!createdEmptyTilemap)
            {
                Debug.LogError($"{typeof(LoopVisualiser)}: No tile map in prefab: {prefab.name}.");
                return null;
            }
            
            Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loops =
                CalculateLoops(scanTilemap, borderTile);
            IdentifyAllEntrancesAndExitsInTilemap(
                loops, 
                out Dictionary<Vector2Int, ArrowDirection> entrances,
                out Dictionary<Vector2Int, ArrowDirection> exits);

            UpdateTilemapWithDiscoveredAnswers(arrowTileProvider, tilemap, entrances, exits);

            return newObject;
        }

        /// <summary>
        /// Calculates loops on the given tilemap.
        /// </summary>
        /// <param name="scanTilemap">Tilemap to scan. </param>
        /// <param name="borderTile">Border tile to use for entrances and exits. </param>
        /// <returns>All the information for loops. </returns>
        private Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> CalculateLoops(Tilemap scanTilemap, Tile borderTile)
        {
            ILoopedWorldDiscoveredByTile loopingAnswers = new LoopedWorldDiscoveredByTile();
            loopingAnswers.CalculateLoops(
                scanTilemap,
                borderTile,
                out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer);

            return loopAnswer;
        }

        /// <summary>
        /// Updates the tilemap with Entrances and exits discovered in the lists.
        /// </summary>
        /// <param name="arrowTileProvider">Arrow tiles to use when updating. </param>
        /// <param name="tilemap">Tilemap to update tiles on. </param>
        /// <param name="entrances">Location and <see cref="ArrowDirection"/> for entrances. </param>
        /// <param name="exits">Location and <see cref="ArrowDirection"/> for exits. </param>
        private void UpdateTilemapWithDiscoveredAnswers(
            IArrowTileProvider arrowTileProvider, 
            Tilemap tilemap,
            Dictionary<Vector2Int, ArrowDirection> entrances,
            Dictionary<Vector2Int, ArrowDirection> exits)
        {
            SetTilesInMapToArrowsGiven(arrowTileProvider, entrances, ArrowPurpose.LoopEntrance, tilemap);
            SetTilesInMapToArrowsGiven(arrowTileProvider, exits, ArrowPurpose.LoopExit, tilemap);
        }

        /// <summary>
        /// Sets tile to the <see cref="ArrowDirection"/> version given in the dictionary location.
        /// Will not set if there is no arrow in <see cref="IArrowTileProvider"/>.
        /// </summary>
        /// <param name="arrowTileProvider">Provider to find arrows. </param>
        /// <param name="arrows">Arrows to set, with location and direction. </param>
        /// <param name="purpose"><see cref="ArrowPurpose"/> to filter for arrows. </param>
        /// <param name="tilemap">Tilemap to update tiles. </param>
        private void SetTilesInMapToArrowsGiven(
            IArrowTileProvider arrowTileProvider, 
            Dictionary<Vector2Int, ArrowDirection> arrows,
            ArrowPurpose purpose,
            Tilemap tilemap)
        {
            foreach (var arrow in arrows)
            {
                var location = new Vector3Int(arrow.Key.x, arrow.Key.y);
                ArrowDirection givenDirection = arrow.Value;
                Tile tile = arrowTileProvider.GetArrowTile(givenDirection, purpose);

                if (tile == null)
                {
                    continue;
                }
                
                tilemap.SetTile(location, tile);
            }
        }

        /// <summary>
        /// Finds all the entrances and exits from <see cref="loopAnswer"/> and
        /// extracts the <see cref="ArrowDirection"/>s for both Entrances and Exits.
        /// </summary>
        /// <param name="loopAnswer">Answers to use for entrances and exits. </param>
        /// <param name="entrances">Extracted entrances. </param>
        /// <param name="exits">Extracted exits. </param>
        private void IdentifyAllEntrancesAndExitsInTilemap(
            Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer,
            out Dictionary<Vector2Int, ArrowDirection> entrances,
            out Dictionary<Vector2Int, ArrowDirection> exits)
        {
            exits = new Dictionary<Vector2Int, ArrowDirection>();
            entrances = new Dictionary<Vector2Int, ArrowDirection>();
            foreach (var location in loopAnswer)
            {
                DiscoverEntrancesAndExitsOnTile(location.Key, loopAnswer, entrances, exits);
            }
        }

        /// <summary>
        /// Discovers all the entrances and exits which are on the given tile.
        /// If there is an entrance on the tile there should be an exit and this will identify both.
        /// </summary>
        /// <param name="location">Location to discover and update loop information. </param>
        /// <param name="loopAnswer">Source of information for loops. </param>
        /// <param name="entrances">Entrance answers to add to. </param>
        /// <param name="exits">Exit answers to add to. </param>
        private void DiscoverEntrancesAndExitsOnTile(Vector2Int location, Dictionary<Vector2Int,
                                                         Dictionary<Direction, CollisionPositionAnswer>> loopAnswer,
                                                     Dictionary<Vector2Int, ArrowDirection> entrances,
                                                     Dictionary<Vector2Int, ArrowDirection> exits)
        {
            if (loopAnswer.TryGetValue(location, out Dictionary<Direction, CollisionPositionAnswer> loopValue))
            {
                foreach (var currentDirection in loopValue)
                {
                    if (currentDirection.Value.Answer != ContextToPositionAnswer.NewPositionIsCorrect)
                    {
                        continue;
                    }

                    AddOrUpdateNewArrow(entrances, 
                            location, currentDirection.Key);
                    AddOrUpdateNewArrow(exits, 
                        currentDirection.Value.NewPosition, currentDirection.Value.NewDirection);
                }
            }
        }

        /// <summary>
        /// Adds or updates the given <see cref="Direction"/> arrow in the the output given.
        /// </summary>
        /// <param name="arrowOutput">The list of locations and <see cref="ArrowDirection"/> to update. </param>
        /// <param name="location">Location to update. </param>
        /// <param name="direction">Direction to add or update. </param>
        private void AddOrUpdateNewArrow(
                Dictionary<Vector2Int, ArrowDirection> arrowOutput, 
                Vector2Int location,
                Direction direction)
        {
            ArrowDirection exitDirection = ConvertDirectionToArrowDirection(direction);
            if (arrowOutput.ContainsKey(location))
            {
                arrowOutput[location] |= exitDirection;
            }
            else
            {
                arrowOutput.Add(location, exitDirection);
            }
        }

        /// <summary>
        /// Creates the prefab and extracts the tilemap.
        /// </summary>
        /// <param name="prefab">Prefab to create. </param>
        /// <param name="newObject">New object created. </param>
        /// <param name="tilemap">Tilemap extracted. </param>
        /// <returns>True means tilemap found. </returns>
        private bool CreatePrefab(GameObject prefab, out GameObject newObject, out Tilemap tilemap)
        {
            newObject = Object.Instantiate(prefab);
            tilemap = newObject.GetComponentInChildren<Tilemap>();

            bool hasTilemap = tilemap != null;
            if (!hasTilemap)
            {
                Object.DestroyImmediate(newObject);
            }
            
            return hasTilemap;
        }

        /// <summary>
        /// Converts a <see cref="Direction"/> to an <see cref="ArrowDirection"/>.
        /// </summary>
        /// <param name="direction"><see cref="Direction"/> to convert. </param>
        /// <returns><see cref="ArrowDirection"/> converted. </returns>
        /// <exception cref="NotImplementedException">
        /// Thrown if there is no equivalent.
        /// </exception>
        private ArrowDirection ConvertDirectionToArrowDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down: return ArrowDirection.Down;
                case Direction.Left: return ArrowDirection.Left;
                case Direction.Right: return ArrowDirection.Right;
                case Direction.Up: return ArrowDirection.Up;
                default:
                    throw new NotImplementedException($"{nameof(ConvertDirectionToArrowDirection)}: " +
                                                  $"{nameof(direction)} does not have a " +
                                                  $"{nameof(ArrowDirection)} equivalent.");
            }
        }

        /// <summary>
        /// Validates that a visualization may take place with the given parameters.
        /// </summary>
        /// <param name="prefab">Prefab to create. </param>
        /// <param name="scanTilemap">Tile map to scan. </param>
        /// <param name="borderTile">Border map to compare. </param>
        /// <param name="arrowTileProvider"><see cref="IArrowTileProvider"/> to give tiles for visualization. </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if not valid.
        /// </exception>
        private void ValidateParametersForNewVisualisation(GameObject prefab, 
                                                           Tilemap scanTilemap,
                                                           Tile borderTile,
                                                           IArrowTileProvider arrowTileProvider)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException($"{typeof(LoopVisualiser)}: {nameof(prefab)} must not be null.");
            }

            if (scanTilemap == null)
            {
                throw new ArgumentNullException($"{typeof(LoopVisualiser)}: {nameof(scanTilemap)} must not be null.");
            }
            
            if (borderTile == null)
            {
                throw new ArgumentNullException($"{typeof(LoopVisualiser)}: {nameof(borderTile)} must not be null.");
            }

            if (arrowTileProvider == null)
            {
                throw new ArgumentNullException($"{typeof(LoopVisualiser)}: {nameof(arrowTileProvider)} must not be null.");
            }
        }
    }
}