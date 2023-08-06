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
            ValidateParametersForNewVisualisation(prefab, scanTilemap, arrowTileProvider);

            ILoopedWorldDiscoveredByTile loopingAnswers = new LoopedWorldDiscoveredByTile();
            if (!loopingAnswers.CalculateLoops(
                    scanTilemap,
                    borderTile,
                    out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer))
            {
                
            }

            GameObject newObject = Object.Instantiate(prefab);
            Tilemap tilemap = newObject.GetComponentInChildren<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogError($"No tile map in prefab.");
                return null;
            }
            Debug.Log($"Tilemap: {tilemap.name}");
            for (int x = scanTilemap.origin.x; x < scanTilemap.size.x; ++x)
            {
                for (int y = scanTilemap.origin.y; y < scanTilemap.size.y; ++y)
                {
                    var location = new Vector2Int(x, y);
                    if (loopAnswer.TryGetValue(location, out Dictionary<Direction, CollisionPositionAnswer> loopValue))
                    {
                        bool didSet = false;
                        ArrowDirection givenDirection = ArrowDirection.None;
                        foreach (var currentDirection in loopValue)
                        {
                            if (currentDirection.Value.Answer == ContextToPositionAnswer.NewPositionIsCorrect)
                            {
                                didSet = true;
                                if (givenDirection == ArrowDirection.None)
                                {
                                    givenDirection = ConvertDirectionToArrowDirection(currentDirection.Key);
                                }
                                else
                                {
                                    givenDirection |= ConvertDirectionToArrowDirection(currentDirection.Key);
                                }
                            }
                        }

                        if (didSet)
                        {
                            /*var arrowTile1 = Resources.Load<Tile>("Editor/LoopVisualiser/ArrowTiles/TileArrows_0");
                            GameObject newObject1 = Object.Instantiate(prefab);
                            Tilemap tilemap1 = newObject1.GetComponentInChildren<Tilemap>();
                            if (tilemap1 != null)
                            {
                
                                tilemap1.SetTile(new Vector3Int(x, y), arrowTile1);
                            }

                            return null;*/
                            //var arrowTile = Resources.Load<Tile>("Editor/LoopVisualiser/ArrowTiles/TileArrows_0");
                            TileChangeData change = new()
                            {
                                position = new Vector3Int(x, y),
                                tile = arrowTileProvider.GetArrowTile(givenDirection, ArrowPurpose.LoopEntrance)
                            };
                            
                            tilemap.SetTile(new Vector3Int(x, y), arrowTileProvider.GetArrowTile(givenDirection, ArrowPurpose.LoopEntrance));
                            Debug.Log($"Set: {change.tile.name} at {change.position.x}, {change.position.y}." +
                                      $"{givenDirection}");
                            //tilemap.RefreshTile(change.position);
                        }
                    }
                }
            }
            
            return newObject;
        }

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

        private void ValidateParametersForNewVisualisation(GameObject prefab, Tilemap scanTilemap,
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

            if (arrowTileProvider == null)
            {
                throw new ArgumentNullException($"{typeof(LoopVisualiser)}: {nameof(arrowTileProvider)} must not be null.");
            }
        }
    }
}