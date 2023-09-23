using System.Linq;
using FQ.GameElementCommunication;
using FQ.GameObjectPromises;
using FQ.Libraries.Randomness;
using FQ.Logger;
using UnityEngine;

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
        protected IRandom RandomGenerator;

        /// <summary>
        /// Finds World Info in the Scene.
        /// </summary>
        /// <remarks>Internal only for testing purposes. </remarks>
        protected IWorldInfoFromTilemapFinder WorldInfoFromTilemapFinder;

        /// <summary>
        /// Attempts to find ElementCommunication in the Scene
        /// </summary>
        /// <remarks>Internal only for testing purposes. </remarks>
        protected IElementCommunicationFinder ElementCommunicationFinder;
        
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
        private IPlayerStatusBasics playerStatus;
        
        protected override void BaseStart()
        {
            this.areActive = true;

            SetupAndAcquireSafeArea();
            AcquirePlayerStatus();
            MoveToRandomValidLocation();
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
        /// Links up the Food to the current player status such as location.
        /// This allows the food not to spawn where the player is.
        /// </summary>
        private void AcquirePlayerStatus()
        {
            IElementCommunication communication = null;
            if (this.ElementCommunicationFinder == null)
            {
                communication = TryToFindElementCommunication();
            }
            else
            { 
                communication = this.ElementCommunicationFinder.FindElementCommunication();
                
            }
            
            if (communication == null)
            {
                Log.Error($"{typeof(PlayerStatus)}: " +
                          $"No {nameof(ElementCommunication)}. " +
                          $"Cannot stop food spawning on player.");
                return;
            }

            this.playerStatus = communication.PlayerStatus;
        }
        
        /// <summary>
        /// Will attempt to find <see cref="IElementCommunication"/> in the scene.
        /// </summary>
        /// <returns><see cref="IElementCommunication"/> if any is in the scene. </returns>
        private IElementCommunication TryToFindElementCommunication()
        {
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
             if (controller == null)
             {
                 Log.Error($"{typeof(PlayerStatus)}: " +
                             $"No Object with GameController. " +
                             $"Cannot stop food spawning on player.");
                 return null;
             }
            
            return controller.GetComponent<ElementCommunication>();
        }

        /// <summary>
        /// Moves to a random but also valid location for food.
        /// </summary>
        private void MoveToRandomValidLocation()
        {
            if (!SafeAreaIsValid())
            {
                return;
            }

            while (true)
            {
                int max = this.safeArea.Length;
                int i = this.RandomGenerator.Range(0, max);
                transform.position = this.safeArea[i];
            
                if (!CurrentPositionIsWherePlayerIs())
                {
                    break;
                }
            }

        }

        /// <summary>
        /// Determines if Safe Area is valid to use.
        /// </summary>
        /// <returns>True means valid and it can be used.</returns>
        private bool SafeAreaIsValid()
        {
            bool isNull = this.safeArea == null;
            
            bool isEmpty = false;
            if (!isNull)
            {
                isEmpty = !this.safeArea.Any();
            }
            
            return !isNull && !isEmpty;
        }

        /// <summary>
        /// Looks for the communication with the player to ensure the current location is equal to the current
        /// location.
        /// </summary>
        /// <returns>True means the current location is where the player is. </returns>
        private bool CurrentPositionIsWherePlayerIs()
        {
            if (this.playerStatus == null)
            {
                return false;
            }
            
            Vector3 position = transform.position;
            Vector2Int tile = new Vector2Int((int)position.x, (int)position.y);
            
            return this.playerStatus.PlayerLocation.Any(x => x == tile);
        }

        /// <summary>
        /// Grabs and sets up the safe area which is the area we can spawn.
        /// </summary>
        private void SetupAndAcquireSafeArea()
        {
            EnsureWorldInfoFromTilemapFinderIsNotNull();
            
            IWorldInfoFromTilemap worldInfo = this.WorldInfoFromTilemapFinder.FindWorldInfo();
            if (worldInfo == null)
            {
                Debug.LogWarning($"{typeof(SnakeFood)}: World info null");
                return;
            }
            
            this.safeArea = worldInfo.GetTravelableArea();
            if (this.safeArea == null)
            {
                Debug.LogWarning($"{typeof(SnakeFood)}: safeArea null. Will not move Food.");
            }
        }

        /// <summary>
        /// Ensures the <see cref="WorldInfoFromTilemapFinder"/> is never <c>null</c> by using a default if <c>null</c>.
        /// </summary>
        private void EnsureWorldInfoFromTilemapFinderIsNotNull()
        {
            this.WorldInfoFromTilemapFinder ??= new SnakeWorldInfoFromTilemapFinder();
        }
    }
}