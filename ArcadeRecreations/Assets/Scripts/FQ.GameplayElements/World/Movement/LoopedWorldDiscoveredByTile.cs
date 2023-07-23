using System;
using System.Collections.Generic;
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
        /// <param name="loopAnswer">Answers or discovered loops. </param>
        /// <returns>True means there were no issues. </returns>
        public bool CalculateLoops(
            Tilemap tilemap,
            out Dictionary<Vector2, Dictionary<Direction, CollisionPositionAnswer>> loopAnswer
        )
        {
            bool didCalculate = false;
            loopAnswer = null;
            
            /*if (tilemap == null)
            {
                Logger.Instance.Error($"{typeof(LoopedWorldDiscoveredByTile)}: " +
                                      $"{nameof(tilemap)} is null and therefore cannot calculate loops.");
                return didCalculate;
            } */

            throw new NotImplementedException();
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