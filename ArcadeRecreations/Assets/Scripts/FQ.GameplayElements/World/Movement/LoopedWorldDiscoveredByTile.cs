using System;
using System.Collections.Generic;
using System.Linq;
using FQ.Libraries;
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
        /// Values calculated last time loops were calculated.
        /// </summary>
        private Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> lastCalculatedLoops;
        
        /// <summary>
        /// Calculates the looped positions based on the tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to look for the loop position. </param>
        /// <param name="borderTile">Tile to look for as the border. </param>
        /// <param name="loopAnswer">Answers or discovered loops. </param>
        /// <returns>True means there were no issues. </returns>
        public bool CalculateLoops(
            Tilemap tilemap,
            Tile borderTile,
            out Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer
        )
        {
            loopAnswer = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>();
            if (!ParametersAreValidForCalculation(tilemap))
            {
                return false;
            }
            
            loopAnswer = CalculateLoopsForGivenTilemap(tilemap, borderTile);
            this.lastCalculatedLoops = loopAnswer;
            return loopAnswer.Keys.Any();
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
            Vector2Int currentPosition,
            Direction currentDirection)
        {
            if (this.lastCalculatedLoops == null)
            {
                throw new InvalidCallOrder($"{typeof(LoopedWorldDiscoveredByTile)}: " +
                                           $"{nameof(CalculateLoops)} must be called successfully first.");
            }

            var returnAnswer = new CollisionPositionAnswer() { Answer = ContextToPositionAnswer.NoValidMovement };
            if (this.lastCalculatedLoops.TryGetValue(
                    currentPosition, out Dictionary<Direction, CollisionPositionAnswer> directionsAtLocation))
            {
                if (directionsAtLocation.TryGetValue(currentDirection, out CollisionPositionAnswer answer))
                {
                    returnAnswer = answer;
                }
            }

            return returnAnswer;
        }

        /// <summary>
        /// Calculates loops for the entire given tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to search. </param>
        /// <param name="borderTile">Border tile to use for loops. </param>
        /// <returns>All the border tiles and loops. </returns>
        private Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> CalculateLoopsForGivenTilemap(
            Tilemap tilemap, Tile borderTile)
        {
            var loopAnswer = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>();
            Bounds searchArea = ExtractWorldArea(tilemap);
            for (int x = (int) searchArea.min.x; x < (int) searchArea.max.x; ++x)
            {
                for (int y = (int) searchArea.min.y; y < (int) searchArea.max.y; ++y)
                {
                    if (CalculateLoopsAtGivenTile(tilemap, borderTile,
                            new Vector3Int(x, y, (int) tilemap.origin.z), searchArea,
                        out Dictionary<Direction, CollisionPositionAnswer> tileAnswer))
                    {
                        loopAnswer.Add(new Vector2Int(x,y), tileAnswer);
                    }
                }
            }

            return loopAnswer;
        }

        /// <summary>
        /// Calculating loops from the given tile.
        /// </summary>
        /// <param name="tilemap">Tilemap to search for loops. </param>
        /// <param name="borderTile">Border tile to use to search. </param>
        /// <param name="location">Location to start. </param>
        /// <param name="searchArea">Bounds of the tilemap. </param>
        /// <param name="loopAnswer">Answer if successful. </param>
        /// <returns>True means there was a border. </returns>
        private bool CalculateLoopsAtGivenTile(
            Tilemap tilemap, 
            Tile borderTile,
            Vector3Int location, 
            Bounds searchArea, 
            out Dictionary<Direction, CollisionPositionAnswer> loopAnswer)
        {
            loopAnswer = new Dictionary<Direction, CollisionPositionAnswer>();

            TileBase tile = tilemap.GetTile(location);
            if (tile != borderTile)
            {
                return false;
            }
            
            foreach (int i in Enum.GetValues(typeof(Direction)))
            {
                var direction = (Direction) i;
                if (CalculateLoopsAtGivenTileInDirection(
                        direction,
                        location,
                        tilemap,
                        borderTile,
                        searchArea,
                        out CollisionPositionAnswer answer))
                {
                    loopAnswer.Add(direction, answer);
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if the given parameters would be valid when calculating loops for a world.
        /// </summary>
        /// <param name="tilemap">Tilemap to look for the loop position. </param>
        /// <returns>True means valid. </returns>
        private bool ParametersAreValidForCalculation(Tilemap tilemap)
        {
            bool isValid = true;
            if (tilemap == null)
            {
                Log.Logger.Instance.Error($"{typeof(LoopedWorldDiscoveredByTile)}: " +
                                          $"{nameof(tilemap)} is null and therefore cannot calculate loops.");
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Extracts the bounds of the world from the position and size.
        /// </summary>
        /// <param name="tilemap">Tilemap to extract the bounds from. </param>
        /// <returns>The bounds of the world. </returns>
        private Bounds ExtractWorldArea(Tilemap tilemap)
        {
            Vector3Int centerTile = tilemap.origin;
            Vector3Int size = tilemap.size;
            int widthHeight = size.x > size.y ? size.x : size.y;
            
            int leftLimit = centerTile.x - widthHeight;
            int rightLimit = centerTile.x + widthHeight;
            int lowerLimit = centerTile.y - widthHeight;
            int upperLimit = centerTile.y + widthHeight;
            return new Bounds()
            {
                center = centerTile,
                size = size,
                min = new Vector2(leftLimit, lowerLimit),
                max = new Vector2(rightLimit, upperLimit)
            };
        } 
        
        /// <summary>
        /// Calculating loops from the given tile in the direction.
        /// </summary>
        /// <param name="direction">Direction to search in. </param>
        /// <param name="position">Position to start. </param>
        /// <param name="tilemap">Tilemap to search for loops. </param>
        /// <param name="borderTile">Border tile to use to search. </param>
        /// <param name="searchArea">Bounds of the tilemap. </param>
        /// <param name="collisionPositionAnswer">Answer if successful. </param>
        /// <returns>True means there was a loop. </returns>
        private bool CalculateLoopsAtGivenTileInDirection(
            Direction direction, 
            Vector3Int position,
            Tilemap tilemap,
            TileBase borderTile,
            Bounds searchArea,
            out CollisionPositionAnswer collisionPositionAnswer)
        {
            bool didFindLoop = false;
            collisionPositionAnswer = new CollisionPositionAnswer()
            {
                Answer = ContextToPositionAnswer.NoMovementNeeded
            };
            
            Vector3Int loopPosition = new Vector3Int(position.x, position.y, position.z);
            Vector3Int previousPosition = new Vector3Int(loopPosition.x, loopPosition.y, loopPosition.z);

            while (FindLoopLoopSearchArea(direction, searchArea, ref loopPosition))
            {
                didFindLoop = IsCurrentTileALoop(
                    position,
                    loopPosition,
                    previousPosition,
                    tilemap,
                    borderTile,
                    direction,
                    out collisionPositionAnswer);
                if (didFindLoop)
                {
                    break;
                }
                
                if(collisionPositionAnswer.Answer == ContextToPositionAnswer.NoValidMovement)
                {
                    break;
                }
                
                previousPosition = loopPosition;
            }
            
            return didFindLoop;
        }

        /// <summary>
        /// Condition when looping the search area.
        /// </summary>
        /// <param name="direction">Direction to search. </param>
        /// <param name="searchArea">Search bound. </param>
        /// <param name="loopPosition">Position searching currently. </param>
        /// <returns>True means are still searching. </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented <see cref="Direction"/>.
        /// </exception>
        private bool FindLoopLoopSearchArea(Direction direction, Bounds searchArea, ref Vector3Int loopPosition)
        {
            // Keep in mind we search in the opposite direction, search left when given right, up given down.
            switch (direction)
            {
                case Direction.Right:
                    --loopPosition.x;
                    return loopPosition.x > searchArea.min.x;
                case Direction.Left:
                    ++loopPosition.x;
                    return loopPosition.x < searchArea.max.x;
                case Direction.Down:
                    --loopPosition.y;
                    return loopPosition.y > searchArea.min.y;
                case Direction.Up:
                    ++loopPosition.y;
                    return loopPosition.y < searchArea.max.y;
                default:
                    throw new NotImplementedException(
                        $"{typeof(LoopedWorldDiscoveredByTile)}: " +
                        $"No implementation for {nameof(direction)}");
            }
        }

        /// <summary>
        /// Figures out if the current tile is a border loop.
        /// </summary>
        /// <param name="currentPosition">Current position to inspect. </param>
        /// <param name="previousPosition">Previous position to use in the answer. </param>
        /// <param name="tilemap">Tilemap to loop up the position. </param>
        /// <param name="borderTile">Border tile to look for. </param>
        /// <param name="currentDirection">Current direction moving in. </param>
        /// <param name="collisionPositionAnswer">Answer to create. </param>
        /// <returns>True means border tile found and <see cref="CollisionPositionAnswer"/> created. </returns>
        private bool IsCurrentTileALoop(
            Vector3Int originalPosition, 
            Vector3Int currentPosition, 
            Vector3Int previousPosition, 
            Tilemap tilemap, 
            TileBase borderTile,
            Direction currentDirection,
            out CollisionPositionAnswer collisionPositionAnswer)
        {
            bool isLoop = false;
            collisionPositionAnswer = new CollisionPositionAnswer();
            
            TileBase currentTile = tilemap.GetTile(new Vector3Int(currentPosition.x, currentPosition.y, currentPosition.z));
            if (currentTile == borderTile)
            {
                int distance = Math.Abs(currentPosition.x - originalPosition.x) +
                               Math.Abs(currentPosition.y - originalPosition.y);
                if (distance > 2)
                {
                    collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                    collisionPositionAnswer.NewDirection = currentDirection;
                    collisionPositionAnswer.NewPosition = new Vector2Int(previousPosition.x, previousPosition.y);
                    isLoop = true;
                }
                else
                {
                    collisionPositionAnswer.Answer = ContextToPositionAnswer.NoValidMovement;
                }
            }

            return isLoop;
        }
    }
}