using System;
using System.Collections;
using System.Linq;
using FQ.GameElementCommunication;
using FQ.GameObjectPromises;
using FQ.Libraries.Randomness;
using Mono.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Exists to remove the collider similar to actual food.
    /// </summary>
    public class SnakeFood : GameElement
    {
        /// <summary>
        /// Random number generator.
        /// </summary>
        /// <remarks>Internal only for testing purposes. </remarks>
        internal IRandom randomGenerator;

        /// <summary>
        /// Finds World Info in the Scene.
        /// </summary>
        /// <remarks>Internal only for testing purposes. </remarks>
        internal IWorldInfoFromTilemapFinder worldInfoFromTilemapFinder;

        /// <summary>
        /// Attempts to find ElementCommunication in the Scene
        /// </summary>
        /// <remarks>Internal only for testing purposes. </remarks>
        internal IElementCommunicationFinder elementCommunicationFinder;
        
        /// <summary>
        /// True means active.
        /// </summary>
        private bool areActive;

        /// <summary>
        /// The area we may spawn.
        /// </summary>
        private Vector3Int[] safeArea;

        /// <summary>
        /// Basics about the Player this session.
        /// </summary>
        private IPlayerStatus playerStatus;
        
        protected override void BaseStart()
        {
            this.areActive = true;

            //SetupAndAcquireSafeArea();
            //AcquirePlayerStatus();
            //MoveToRandomValidLocation();
        }

        protected override void BaseFixedUpdate()
        {
            if (!this.areActive)
            {
                MoveToRandomValidLocation();

                this.areActive = true;
                tag = "SnakeFood";
            }
        }
        
        protected override void BaseOnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                areActive = false;
            }
        }
        
        /// <summary>
        /// Collects the world information if in the scene.
        /// </summary>
        /// <returns>World Info or Null if not found. </returns>
        private IWorldInfoFromTilemap GetWorldInfo()
        {
            throw new NotImplementedException();
            // GameObject[] borders = GameObject.FindGameObjectsWithTag("SnakeBorder");
            // GameObject border = borders.FirstOrDefault(x => x.name == "Border");
            // if (border == null)
            // {
            //     return null;
            // }
            //
            // return border.GetComponent<WorldInfoInfoFromTilemap>();
        }
        
        /// <summary>
        /// Links up the Food to the current player status such as location.
        /// This allows the food not to spawn where the player is.
        /// </summary>
        private void AcquirePlayerStatus()
        {
            throw new NotImplementedException();
            // GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            // if (controller == null)
            // {
            //     Debug.LogError($"{typeof(PlayerStatus)}: " +
            //                    $"No Object with GameController. " +
            //                    $"Cannot stop food spawning on player.");
            //     return;
            // }
            //
            // ElementCommunication communication = controller.GetComponent<ElementCommunication>();
            // if (communication == null)
            // {
            //     Debug.LogError($"{typeof(PlayerStatus)}: " +
            //                    $"No {nameof(ElementCommunication)}. " +
            //                    $"Cannot stop food spawning on player.");
            //     return;
            // }
            //
            // this.playerStatus = communication.PlayerStatus;
        }

        /// <summary>
        /// Moves to a random but also valid location for food.
        /// </summary>
        private void MoveToRandomValidLocation()
        {
            Vector3 position = gameObject.transform.position;
            gameObject.transform.position = new Vector3(position.x + 1, position.y);
            // if (this.safeArea == null)
            // {
            //     return;
            // }
            //
            // while (true)
            // {
            //     int max = this.safeArea.Length;
            //     int i = Random.Range(0, max);
            //     transform.position = this.safeArea[i];
            //
            //     if (!CurrentPositionIsWherePlayerIs())
            //     {
            //         break;
            //     }
            // }

        }

        /// <summary>
        /// Looks for the communication with the player to ensure the current location is equal to the current
        /// location.
        /// </summary>
        /// <returns>True means the current location is where the player is. </returns>
        private bool CurrentPositionIsWherePlayerIs()
        {
            throw new NotImplementedException();
            // if (this.playerStatus == null)
            // {
            //     return false;
            // }
            //
            // Vector3 position = transform.position;
            // Vector2Int tile = new Vector2Int((int)position.x, (int)position.y);
            //
            // return this.playerStatus.PlayerLocation.Any(x => x == tile);
        }

        /// <summary>
        /// Grabs and sets up the safe area which is the area we can spawn.
        /// </summary>
        private void SetupAndAcquireSafeArea()
        {
            throw new NotImplementedException();
            // IWorldInfoFromTilemap worldInfo = GetWorldInfo();
            // if (worldInfo == null)
            // {
            //     Debug.LogError($"{typeof(SnakeFood)}: World info null");
            //     return;
            // }
            //
            // this.safeArea = worldInfo.GetTravelableArea();
            // if (this.safeArea == null)
            // {
            //     Debug.LogError($"{typeof(SnakeFood)}: safeArea null");
            // }
        }
    }
}