using System;
using System.Collections.Generic;
using FQ.Libraries.StandardTypes;
using FQ.TilemapTools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Provides information about the world based on tilemaps.
    /// </summary>
    public class WorldInfoInfoFromTilemap : MonoBehaviour, IWorldInfoFromTilemap
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
        private Dictionary<Vector2Int, Dictionary<MovementDirection, CollisionPositionAnswer>> loops;
        
        /// <summary>
        /// Cached information about where the player can move.
        /// </summary>
        private Vector3Int[] travelableArea;

        private void Start()
        {
            ILoopedWorldDiscoveredByTile loopDiscoverer = new LoopedWorldDiscoveredByTile();
            loopDiscoverer.CalculateLoops(scanTileMap, borderTile, out loops);

            GetTravelableArea();
        }

        /// <summary>
        /// Gets loop information based on the given input.
        /// </summary>
        /// <param name="location">Location to test. </param>
        /// <param name="direction">Direction the Snake is moving in. </param>
        /// <returns>Answer as to whether there are loops. </returns>
        public CollisionPositionAnswer GetLoop(Vector2Int location, MovementDirection direction)
        {
            CollisionPositionAnswer answer = new()
            {
                Answer = ContextToPositionAnswer.NoValidMovement,
            };
            
            if (loops.TryGetValue(location, out Dictionary<MovementDirection, CollisionPositionAnswer> locationValue))
            {
                if (locationValue.TryGetValue(direction, out CollisionPositionAnswer value))
                {
                    answer = value;
                }
            }

            return answer;
        }
        
        public Vector3Int[] GetTravelableArea()
        {
            if (travelableArea != null)
            {
                return travelableArea;
            }
            
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Vector3 originalLocation = playerObject.transform.position;
                Vector3Int location = new((int) originalLocation.x, (int) originalLocation.y, (int) originalLocation.z);
                IDetectSingleTileArea detector = new DetectSingleTileArea();
                this.travelableArea = detector.DetectTilesOfTheSameTypeTouching(scanTileMap, null, location);
            }

            return this.travelableArea;
        }

        /// <summary>
        /// Reduces the precision (floats) of the vector to the top left position.
        /// </summary>
        /// <param name="given">Vector to reduce. </param>
        /// <returns>Result with 0 points of precision. </returns>
        private Vector3 ReduceToTopLeft(Vector3 given)
        {
            given.x = (int) given.x;
            given.y = (int) given.y;
            given.z = (int) given.z;
            return given;
        }
    }
}