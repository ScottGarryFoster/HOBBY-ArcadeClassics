using System;
using System.Linq;
using FQ.GameElementCommunication;
using FQ.GameplayInputs;
using FQ.GameObjectPromises;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Represents a player in the arcade game snake.
    /// </summary>
    public class SnakePlayer : InteractableActor
    {
        /// <summary>
        /// Pieces of the Snake's body which count as the tail.
        /// </summary>
        public SnakeTail[] SnakeTailPieces
        {
            get
            {
                if (this.snakeBehaviour == null)
                {
                    return Array.Empty<SnakeTail>();
                }
                return this.snakeBehaviour.SnakeTailPieces;
            }
        }

        /// <summary>
        /// Prefab for the Snake's body.
        /// </summary>
        /// <remarks>Internal for testing. </remarks>
        [SerializeField]
        internal SnakeTail snakeTailPrefab;

        /// <summary>
        /// The player behaviour behind the Snake Player
        /// </summary>
        private ISnakeBehaviour snakeBehaviour;

        protected override void BaseStart()
        {
            base.BaseStart();
            IWorldInfoFromTilemap worldInfoInfo = GetWorldInfo();
            this.snakeBehaviour =
                new SnakeBehaviour(
                    this.gameObject,
                    new GameObjectCreation(),
                    this.gameplayInputs,
                    worldInfoInfo)
                {
                    MovementSpeed = this.MovementSpeed,
                    snakeTailPrefab = this.snakeTailPrefab,
                };

            HookupStatusToCommunication(this.snakeBehaviour);
            this.snakeBehaviour.StartTrigger += StartTrigger;
            this.snakeBehaviour.EndTrigger += EndTrigger;
            ResetElement += () => { this.snakeBehaviour.ResetElement?.Invoke(); };
            DestroyGameElement += OnDestroyGameElement;
            
            this.snakeBehaviour.Start();
        }

        protected override void BaseUpdate()
        {
            base.BaseUpdate();
            this.snakeBehaviour.Update(Time.deltaTime);
        }
        
        protected override void BaseOnTriggerEnter2D(Collider2D other)
        {
            base.BaseOnTriggerEnter2D(other);
            this.snakeBehaviour.OnTriggerEnter2D(other);
        }
        
        /// <summary>
        /// Called when game element is to be destroyed
        /// </summary>
        private void OnDestroyGameElement()
        {
            this.snakeBehaviour.UpdatePlayerLocation = null;
        }
        
        /// <summary>
        /// Collects the world information if in the scene.
        /// </summary>
        /// <returns>World Info or Null if not found. </returns>
        private IWorldInfoFromTilemap GetWorldInfo()
        {
            GameObject[] borders = GameObject.FindGameObjectsWithTag("SnakeBorder");
            GameObject border = borders.FirstOrDefault(x => x.name == "Border");
            if (border == null)
            {
                return null;
            }

            return border.GetComponent<WorldInfoInfoFromTilemap>();
        }
        
        /// <summary>
        /// Hooks up the ability for the player to broadcast it's status.
        /// </summary>
        /// <param name="behaviour">The behaviour to be broadcasting. </param>
        private void HookupStatusToCommunication(ISnakeBehaviour behaviour)
        {
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            if (controller == null)
            {
                Debug.LogError($"{typeof(SnakePlayer)}: No Object with GameController. Cannot update player status.");
                return;
            }

            ElementCommunication communication = controller.GetComponent<ElementCommunication>();
            if (communication == null)
            {
                Debug.LogError($"{typeof(SnakePlayer)}: No {nameof(ElementCommunication)}. Cannot update player status.");
                return;
            }

            IPlayerStatus playerStatus = communication.PlayerStatus;
            behaviour.UpdatePlayerLocation += playerStatus.UpdateLocation;
            behaviour.UpdatePlayerDirection += playerStatus.UpdatePlayerHeadDirection;
        }
    }
}