using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Discovers which tiles should link together using the tilemap given.
    /// </summary>
    public class LoopedWorldDiscoveredByTile : LoopingWorld, ILoopedWorldDiscoveredByTile
    {
        /// <summary>
        /// Calculates the looped positions based on the tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to look for the loop position. </param>
        /// <param name="centerTile">The center of the room. </param>
        /// <param name="widthHeight">The width and height of the room to scan. </param>
        /// <param name="loopAnswer">Answers or discovered loops. </param>
        /// <returns>True means there were no issues. </returns>
        public bool CalculateLoops(
            Tilemap tilemap,
            Vector3Int centerTile,
            Tile borderTile,
            int widthHeight,
            out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer
        )
        {
            bool didCalculate = false;
            loopAnswer = null;
            
            if (tilemap == null)
            {
                Log.Logger.Instance.Error($"{typeof(LoopedWorldDiscoveredByTile)}: " +
                                      $"{nameof(tilemap)} is null and therefore cannot calculate loops.");
                return didCalculate;
            }

            int leftLimit = centerTile.x - widthHeight;
            int rightLimit = centerTile.x + widthHeight;
            int lowerLimit = centerTile.y - widthHeight;
            int upperLimit = centerTile.y + widthHeight;
            Bounds searchArea = new Bounds()
            {
                min = new Vector2(leftLimit, lowerLimit),
                max = new Vector2(rightLimit, upperLimit)
            };
            
            loopAnswer = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>();
            for (int x = leftLimit; x < rightLimit; ++x)
            {
                for (int y = lowerLimit; y < upperLimit; ++y)
                {
                    TileBase tile = tilemap.GetTile(new Vector3Int(x, y, centerTile.z));
                    if (tile == borderTile)
                    {
                        var position = new Vector2Int(x, y);

                        var positionAnswer = new Dictionary<Direction, CollisionPositionAnswer>();
                        loopAnswer.Add(position, positionAnswer);
                        if (FindLoop(
                                Direction.Up, 
                                new Vector3Int(position.x, position.y, centerTile.y),
                                tilemap,
                                tile,
                                searchArea,
                                out CollisionPositionAnswer answer))
                        {
                            positionAnswer.Add(Direction.Down, answer);
                        }
                        
                        if (FindLoop(
                                     Direction.Down, 
                                     new Vector3Int(position.x, position.y, centerTile.y),
                                     tilemap,
                                     tile,
                                     searchArea,
                                     out CollisionPositionAnswer answer2))
                        {
                            positionAnswer.Add(Direction.Up, answer2);
                        }
                        
                        if (FindLoop(
                                     Direction.Left,
                                     new Vector3Int(position.x, position.y, centerTile.y),
                                     tilemap,
                                     tile,
                                     searchArea,
                                     out CollisionPositionAnswer answer3))
                        {
                            positionAnswer.Add(Direction.Right, answer3);
                        }

                        if (FindLoop(
                                Direction.Right, 
                                new Vector3Int(position.x, position.y, centerTile.y),
                                tilemap,
                                tile,
                                searchArea,
                                out CollisionPositionAnswer answer4))
                        {
                            positionAnswer.Add(Direction.Left, answer4);
                        }
                    }
                }
                
            }

            return loopAnswer.Keys.Any();
        }

        private bool FindLoop(
            Direction direction, 
            Vector3Int position,
            Tilemap tilemap,
            TileBase tile,
            Bounds searchArea,
            out CollisionPositionAnswer collisionPositionAnswer)
        {
            collisionPositionAnswer = new CollisionPositionAnswer()
            {
                Answer = ContextToPositionAnswer.NoValidMovement
            };
            bool didFindLoop = false;
            
            if (direction == Direction.Left)
            {
                for (int x = position.x - 1; x < searchArea.min.x; --x)
                {
                    TileBase currentTile = tilemap.GetTile(new Vector3Int(x, position.y, position.z));
                    if (currentTile == tile)
                    {

                        collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                        collisionPositionAnswer.NewDirection = Direction.Right;
                        collisionPositionAnswer.NewPosition = new Vector2Int(x + 1, position.y);
                        didFindLoop = true;
                        break;
                    }
                }
            }
            else if (direction == Direction.Right)
            {
                for (int x = position.x + 1; x < searchArea.max.x; ++x)
                {
                    TileBase currentTile = tilemap.GetTile(new Vector3Int(x, position.y, position.z));
                    if (currentTile == tile)
                    {
                        collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                        collisionPositionAnswer.NewDirection = Direction.Left;
                        collisionPositionAnswer.NewPosition = new Vector2Int(x - 1, position.y);
                        didFindLoop = true;
                        break;
                    }
                }
            }
            else if (direction == Direction.Up)
            {
                for (int y = position.y + 1; y < searchArea.min.y; --y)
                {
                    TileBase currentTile = tilemap.GetTile(new Vector3Int(position.x, y, position.z));
                    if (currentTile == tile)
                    {
                        collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                        collisionPositionAnswer.NewDirection = Direction.Down;
                        collisionPositionAnswer.NewPosition = new Vector2Int(position.x, y + 1);
                        didFindLoop = true;
                        break;
                    }
                }
            }
            else if (direction == Direction.Down)
            {
                for (int y = position.y - 1; y < searchArea.max.y; ++y)
                {
                    TileBase currentTile = tilemap.GetTile(new Vector3Int(position.x, y, position.z));
                    if (currentTile == tile)
                    {
                        collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                        collisionPositionAnswer.NewDirection = Direction.Up;
                        collisionPositionAnswer.NewPosition = new Vector2Int(position.x, y - 1);
                        didFindLoop = true;
                        break;
                    }
                }
            }

            return didFindLoop;
        }


        /// <summary>
        /// Finds a new position for the actor if they collide with the world.
        /// </summary>
        /// <param name="currentPosition">Where the actor is currently. </param>
        /// <param name="currentDirection">The direction they took to get here. </param>
        /// <returns>
        /// Upon attempting to find a new position on collision with the world, this is the answer.
        /// </returns>
        public override CollisionPositionAnswer FindNewPositionForPlayer(
            Vector3 currentPosition,
            Direction currentDirection)
        {
            throw new NotImplementedException();
        }
    }
}