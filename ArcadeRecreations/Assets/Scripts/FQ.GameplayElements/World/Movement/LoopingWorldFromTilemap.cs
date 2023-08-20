using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Provides information about loops based on tilemaps in the world.
    /// </summary>
    public class LoopingWorldFromTilemap : MonoBehaviour, ILoopingWorldFromTilemap
    {
        /// <summary>
        /// Tilemap to Scan.
        /// </summary>
        [SerializeField]
        private Tilemap scanTileMap;

        /// <summary>
        /// Border tile player will move into.
        /// </summary>
        [SerializeField]
        private Tile borderTile;

        /// <summary>
        /// Loops to lookup.
        /// </summary>
        private Dictionary<Vector2Int, Dictionary<Direction, CollisionPositionAnswer>> loops;
        
        private void Start()
        {
            ILoopedWorldDiscoveredByTile loopDiscoverer = new LoopedWorldDiscoveredByTile();
            loopDiscoverer.CalculateLoops(scanTileMap, borderTile, out loops);
        }

        /// <summary>
        /// Gets loop information based on the given input.
        /// </summary>
        /// <param name="location">Location to test. </param>
        /// <param name="direction">Direction the Snake is moving in. </param>
        /// <returns>Answer as to whether there are loops. </returns>
        public CollisionPositionAnswer GetLoop(Vector2Int location, Direction direction)
        {
            CollisionPositionAnswer answer = new()
            {
                Answer = ContextToPositionAnswer.NoValidMovement,
            };
            
            if (loops.TryGetValue(location, out Dictionary<Direction, CollisionPositionAnswer> locationValue))
            {
                if (locationValue.TryGetValue(direction, out CollisionPositionAnswer value))
                {
                    answer = value;
                }
            }

            return answer;
        }
    }
}