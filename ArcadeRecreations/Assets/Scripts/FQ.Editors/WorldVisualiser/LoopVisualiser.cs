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

            var exits = new Dictionary<Vector2Int, ArrowDirection>();
            
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

                                ArrowDirection exitDirection =
                                    ConvertDirectionToArrowDirection(currentDirection.Value.NewDirection);
                                if (exits.ContainsKey(currentDirection.Value.NewPosition))
                                {
                                    exits[currentDirection.Value.NewPosition] |= exitDirection;
                                }
                                else
                                {
                                    exits.Add(currentDirection.Value.NewPosition, exitDirection);
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
                            var l = new Vector3Int(x, y);
                            Tile tile = arrowTileProvider.GetArrowTile(givenDirection, ArrowPurpose.LoopEntrance);

                            if (tile == null)
                            {
                                Debug.Log($"Set LoopEntrance: NULL at {l.x}, {l.y} : {givenDirection}");
                            }
                            else
                            {
                                tilemap.SetTile(l, tile);
                                Debug.Log($"Set LoopEntrance: {tile.name} at {l.x}, {l.y} : {givenDirection}");
                            }

                            //tilemap.RefreshTile(change.position);
                        }
                    }
                }
            }

            foreach (var exit in exits)
            {
                var l = new Vector3Int(exit.Key.x, exit.Key.y);
                ArrowDirection givenDirection = exit.Value;
                Tile tile = arrowTileProvider.GetArrowTile(givenDirection, ArrowPurpose.LoopExit);

                if (tile == null)
                {
                    Debug.Log($"Set LoopExit: NULL at {l.x}, {l.y} : {givenDirection}");
                }
                else
                {
                    tilemap.SetTile(l, tile);
                    Debug.Log($"Set LoopExit: {tile.name} at {l.x}, {l.y} : {givenDirection}");
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