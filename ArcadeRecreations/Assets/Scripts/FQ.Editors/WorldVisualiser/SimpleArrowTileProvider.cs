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

        private Dictionary<ArrowPurpose, Dictionary<ArrowDirection, Tile>> loopArrows;

        public SimpleArrowTileProvider(string arrowTilePrefabs)
        {
            if (string.IsNullOrWhiteSpace(arrowTilePrefabs))
            {
                throw new ArgumentNullException();
            }
            
            this.loopArrows = new Dictionary<ArrowPurpose, Dictionary<ArrowDirection, Tile>>();
            int doubleRowValue = 0;
            int purpose = 0;

            var arrowPurpose = (ArrowPurpose)purpose++;
            Dictionary<ArrowDirection, Tile> currentPurpose = new Dictionary<ArrowDirection, Tile>();
            this.loopArrows.Add(arrowPurpose, currentPurpose);
            
            for (int tile = 0; tile < 39; ++tile)
            {
                if (tile == 20)
                {
                    doubleRowValue = 0;
                    arrowPurpose = (ArrowPurpose)purpose++;
                    currentPurpose = new Dictionary<ArrowDirection, Tile>();
                    this.loopArrows.Add(arrowPurpose, currentPurpose);
                }
                
                string prefabName = $"{arrowTilePrefabs}{tile}";
                Tile arrow = Resources.Load<Tile>(prefabName);
                if (arrow == null)
                {
                    throw new InvalidParameter(
                        $"{typeof(SimpleArrowTileProvider)}: Prefab is invalid. Name: {prefabName}.");
                }

                ArrowDirection direction = GetDirection(doubleRowValue++);
                if (direction != ArrowDirection.None)
                {
                    currentPurpose.Add(direction, arrow);
                }
            }
        }

        private ArrowDirection GetDirection(int tile)
        {
            switch (tile)
            {
                case 0: return ArrowDirection.Down | ArrowDirection.Left | ArrowDirection.Right | ArrowDirection.Up;
                case 1: return ArrowDirection.Down | ArrowDirection.Left | ArrowDirection.Up;
                case 2: return ArrowDirection.Left | ArrowDirection.Right | ArrowDirection.Up;
                case 3: return ArrowDirection.Down | ArrowDirection.Right | ArrowDirection.Up;
                case 4: return ArrowDirection.Down | ArrowDirection.Right | ArrowDirection.Left;
                case 5: return ArrowDirection.Left | ArrowDirection.Up;
                case 6: return ArrowDirection.Right | ArrowDirection.Up;
                case 7: return ArrowDirection.Right | ArrowDirection.Down;
                case 8: return ArrowDirection.Left | ArrowDirection.Down;
                case 9: return ArrowDirection.Left | ArrowDirection.Right;
                case 10: return ArrowDirection.Up | ArrowDirection.Down;
                case 11: return ArrowDirection.Right;
                case 12: return ArrowDirection.Down;
                case 13: return ArrowDirection.Left;
                case 14: return ArrowDirection.Up;
            }

            return ArrowDirection.None;
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
    }
}