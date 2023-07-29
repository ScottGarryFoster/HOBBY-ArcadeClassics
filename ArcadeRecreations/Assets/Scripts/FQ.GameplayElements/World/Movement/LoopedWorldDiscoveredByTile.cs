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
            loopAnswer = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>();
            if (!ParametersAreValidForCalculation(tilemap))
            {
                return false;
            }
            
            loopAnswer = CalculateLoopsForGivenTilemap(tilemap, centerTile, borderTile, widthHeight);
            return loopAnswer.Keys.Any();
        }

        /// <summary>
        /// Calculates loops for the entire given tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to search. </param>
        /// <param name="centerTile">Center tile of the tilemap. </param>
        /// <param name="borderTile">Border tile to use for loops. </param>
        /// <param name="widthHeight">Width and height of the tilemap. </param>
        /// <returns>All the border tiles and loops. </returns>
        private Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> CalculateLoopsForGivenTilemap(
            Tilemap tilemap, Vector3Int centerTile, Tile borderTile, int widthHeight)
        {
            var loopAnswer = new Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>>();
            Bounds searchArea = ExtractWorldArea(centerTile, widthHeight);
            for (int x = (int) searchArea.min.x; x < (int) searchArea.max.x; ++x)
            {
                for (int y = (int) searchArea.min.y; y < (int) searchArea.max.y; ++y)
                {
                    if (CalculateLoopsAtGivenTile(tilemap, borderTile,
                            new Vector3Int(x, y, (int) centerTile.z), searchArea,
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
        /// <param name="centerTile">Center borderTile of the world. </param>
        /// <param name="widthHeight">Size of the borderTile as a width and height. </param>
        /// <returns>The bounds of the world. </returns>
        private Bounds ExtractWorldArea(Vector3Int centerTile, int widthHeight)
        {
            int leftLimit = centerTile.x - widthHeight;
            int rightLimit = centerTile.x + widthHeight;
            int lowerLimit = centerTile.y - widthHeight;
            int upperLimit = centerTile.y + widthHeight;
            return new Bounds()
            {
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
                Answer = ContextToPositionAnswer.NoValidMovement
            };
            
            Vector3Int previousPosition = new Vector3Int();
            Vector3Int loopPosition = new Vector3Int(position.x, position.y, position.z);
            FindLoopLoopSearchAreaSetup(direction, ref loopPosition);
            while (FindLoopLoopSearchArea(direction, searchArea, ref loopPosition))
            {
                if (IsCurrentTileALoop(
                        loopPosition, 
                        previousPosition, 
                        tilemap, 
                        borderTile, 
                        direction,
                        out collisionPositionAnswer))
                {
                    didFindLoop = true;
                    break;
                };
                previousPosition = loopPosition;
            }
            
            return didFindLoop;
        }

        /// <summary>
        /// Starts the loop to search a given area based.
        /// </summary>
        /// <param name="direction">Direction to search. </param>
        /// <param name="loopPosition">Position to setup. </param>
        private void FindLoopLoopSearchAreaSetup(Direction direction, ref Vector3Int loopPosition)
        {
            FindLoopLoopSearchArea(direction, new Bounds(), ref loopPosition);
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
            switch (direction)
            {
                case Direction.Left:
                    --loopPosition.x;
                    return loopPosition.x < searchArea.min.x;
                case Direction.Right:
                    ++loopPosition.x;
                    return loopPosition.x < searchArea.max.x;
                case Direction.Up:
                    --loopPosition.y;
                    return loopPosition.y < searchArea.min.y;
                case Direction.Down:
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
                collisionPositionAnswer.Answer = ContextToPositionAnswer.NewPositionIsCorrect;
                collisionPositionAnswer.NewDirection = GetOppositeDirection(currentDirection);
                collisionPositionAnswer.NewPosition = new Vector2Int(previousPosition.x, previousPosition.y);
                isLoop = true;
            }

            return isLoop;
        }

        /// <summary>
        /// Returns the opposite direction of the given direction.
        /// </summary>
        /// <param name="direction">Direction to find the opposite. </param>
        /// <returns>Opposite direction of the given. </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented <see cref="Direction"/>.
        /// </exception>
        private Direction GetOppositeDirection(Direction direction)
        {
            switch(direction)
            {
                case Direction.Down: return Direction.Up;
                case Direction.Up: return Direction.Down;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                default:
                    throw new NotImplementedException(
                        $"{typeof(LoopedWorldDiscoveredByTile)}: " +
                        $"No implementation for {nameof(direction)}");
            }
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