using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.TilemapTools
{
    /// <summary>
    /// Used to detect where a single tile is within a given tilemap.
    /// </summary>
    public class DetectSingleTileArea : IDetectSingleTileArea
    {
        /// <summary>
        /// Finds all the tiles touching this tile upon the tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to detect this. Note the limits (size) are defined by this! </param>
        /// <param name="type">Type to detect. </param>
        /// <param name="location">Location to begin. </param>
        /// <returns>All the location that touch the given tile which are like the given tile. </returns>
        public Vector3Int[] DetectTilesOfTheSameTypeTouching(Tilemap tilemap, TileBase type, Vector3Int location)
        {
            ValidateInputs(tilemap);
            
            string firstTileName = GetTileNameFromTileMap(tilemap, location);
            string givenTileName = GetTileName(type);
            if (!string.Equals(firstTileName, givenTileName))
            {
                return Array.Empty<Vector3Int>();
            }

            return FindAllTilesConnectedOfGivenName(tilemap, givenTileName, location);
        }

        /// <summary>
        /// Finds all the tiles connected to the given tile name connected to the tile at the location.
        /// </summary>
        /// <param name="tilemap">Tilemap to use for searching. </param>
        /// <param name="givenTileName">Tile name to use when searching. </param>
        /// <param name="location">Start location. </param>
        /// <returns>All the locations found connected. </returns>
        private Vector3Int[] FindAllTilesConnectedOfGivenName(
            Tilemap tilemap, 
            string givenTileName, 
            Vector3Int location)
        {
            Stack<Vector3Int> locationsToExplore = new();
            HashSet<Vector3Int> validLocations = new();
            HashSet<Vector3Int> explored = new();
            locationsToExplore.Push(location);
            validLocations.Add(location);
            
            while (locationsToExplore.Any())
            {
                Vector3Int current = locationsToExplore.Pop();
                explored.Add(current);

                Vector3Int north = GetLocationInDirectionClampedToTilemap(CardinalDirection.North, current, tilemap);
                AnalyseNewLocation(north, tilemap, givenTileName, validLocations, locationsToExplore, explored);
                
                Vector3Int south = GetLocationInDirectionClampedToTilemap(CardinalDirection.South, current, tilemap);
                AnalyseNewLocation(south, tilemap, givenTileName, validLocations, locationsToExplore, explored);
                
                Vector3Int east = GetLocationInDirectionClampedToTilemap(CardinalDirection.East, current, tilemap);
                AnalyseNewLocation(east, tilemap, givenTileName, validLocations, locationsToExplore, explored);
                
                Vector3Int west = GetLocationInDirectionClampedToTilemap(CardinalDirection.West, current, tilemap);
                AnalyseNewLocation(west, tilemap, givenTileName, validLocations, locationsToExplore, explored);
            }

            return validLocations.ToArray();
        }

        /// <summary>
        /// Returns a location in the direction but will only ever return a location on the tilemap.
        /// </summary>
        /// <param name="direction">Direction to move in. </param>
        /// <param name="location">Location to move in and perhaps clamp. </param>
        /// <param name="tilemap">Tilemap to use for clamping. </param>
        /// <returns>A valid location in the direction. </returns>
        private Vector3Int GetLocationInDirectionClampedToTilemap(
            CardinalDirection direction, Vector3Int location, Tilemap tilemap)
        {
            MoveLocationInDirection(direction, location, out int x, out int y);
            return ClampLocationToTileMap(tilemap, x, y);
        }

        /// <summary>
        /// Clamp the given location upon the tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to use when clamping. </param>
        /// <param name="x">X cord to clamp. </param>
        /// <param name="y">Y cord to clamp. </param>
        /// <returns>Location clamped to the tilemap. </returns>
        private Vector3Int ClampLocationToTileMap(Tilemap tilemap, int x, int y)
        {
            int minX = tilemap.origin.x;
            int maxX = minX + tilemap.size.x;
            x = x < minX ? minX : x;
            x = x > maxX ? maxX : x;

            int minY = tilemap.origin.y;
            int maxY = minY + tilemap.size.y;
            y = y < minY ? minY : y;
            y = y > maxY ? maxY : y;

            return new Vector3Int(x, y);;
        }

        /// <summary>
        /// Move location in the direction.
        /// </summary>
        /// <param name="direction">Direction to move the location in. </param>
        /// <param name="current">Location to move. </param>
        /// <param name="x">X location in the direction. </param>
        /// <param name="y">Y location in the direction.</param>
        private void MoveLocationInDirection(CardinalDirection direction, Vector3Int current, out int x, out int y)
        {
            x = current.x;
            y = current.y;
            switch (direction)
            {
                case CardinalDirection.North:
                    --y;
                    break;
                case CardinalDirection.South:
                    ++y;
                    break;
                case CardinalDirection.East:
                    --x;
                    break;
                case CardinalDirection.West:
                    ++x;
                    break;
            }
        }

        /// <summary>
        /// Analyses a new location and will add it to list which make sense in terms of exploring the entire map
        /// and figuring out which tiles do not need to be explored.
        /// </summary>
        /// <param name="location">Location to analyse. </param>
        /// <param name="tilemap">Tilemap to use for information. </param>
        /// <param name="givenTileName">The name to compare against. </param>
        /// <param name="validLocations">All valid location. </param>
        /// <param name="locationsToExplore">Locations to explore more in depth. </param>
        /// <param name="explored">Locations explored and no longer need any more info. </param>
        private void AnalyseNewLocation(
            Vector3Int location, 
            Tilemap tilemap, 
            string givenTileName, 
            HashSet<Vector3Int> validLocations, 
            Stack<Vector3Int> locationsToExplore, 
            HashSet<Vector3Int> explored)
        {
            if (explored.Contains(location))
            {
                return;    
            }

            string tileNameOnMap = GetTileNameFromTileMap(tilemap, location);
            if (string.Equals(tileNameOnMap, givenTileName))
            {
                locationsToExplore.Push(location);
                validLocations.Add(location);
            }
            else
            {
                explored.Add(location);
            }
        }

        /// <summary>
        /// Validates the inputs are valid to discover the tiles on map.
        /// </summary>
        /// <param name="tilemap">Tilemap to use. </param>
        /// <exception cref="ArgumentNullException">Given are not valid. </exception>
        private void ValidateInputs(Tilemap tilemap)
        {
            if (tilemap == null)
            {
                throw new ArgumentNullException($"{typeof(DetectSingleTileArea)}: {tilemap} must not be null.");
            }
        }
        
        /// <summary>
        /// Extracts the tile name of the tile at the location. Empty tiles are a <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="tilemap"><see cref="Tilemap"/> to search for the tile. </param>
        /// <param name="location">Location of the tile. </param>
        /// <returns>Name of the <see cref="Tile"/>. Empty tiles are a <see cref="string.Empty"/>. </returns>
        private string GetTileNameFromTileMap(Tilemap tilemap, Vector3Int location)
        {
            return GetTileName(tilemap.GetTile<TileBase>(location));
        }
        
        /// <summary>
        /// Extracts the tile name from the TileBase. Empty tiles are a <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="tileBase">Tile to extract from. </param>
        /// <returns>Name of the <see cref="Tile"/>. Empty tiles are a <see cref="string.Empty"/>. </returns>
        private string GetTileName(TileBase tileBase)
        {
            string returnName = "";
            if (tileBase != null)
            {
                returnName = tileBase.name;
            }

            return returnName;
        }
    }
}