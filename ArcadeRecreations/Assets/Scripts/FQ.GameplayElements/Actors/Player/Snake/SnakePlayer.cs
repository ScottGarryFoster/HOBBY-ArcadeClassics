using System;
using System.Linq;
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
            ILoopingWorldFromTilemap worldInfo = GetWorldInfo();
            this.snakeBehaviour =
                new SnakeBehaviour(
                    this.gameObject,
                    new GameObjectCreation(),
                    this.gameplayInputs,
                    worldInfo)
                {
                    MovementSpeed = this.MovementSpeed,
                    snakeTailPrefab = this.snakeTailPrefab,
                };
            
            this.snakeBehaviour.StartTrigger += StartTrigger;
            this.snakeBehaviour.EndTrigger += EndTrigger;
            ResetElement += () => { this.snakeBehaviour.ResetElement?.Invoke(); };
            
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
        /// Collects the world information if in the scene.
        /// </summary>
        /// <returns>World Info or Null if not found. </returns>
        private ILoopingWorldFromTilemap GetWorldInfo()
        {
            GameObject[] borders = GameObject.FindGameObjectsWithTag("SnakeBorder");
            GameObject border = borders.FirstOrDefault(x => x.name == "Border");
            if (border == null)
            {
                return null;
            }

            return border.GetComponent<LoopingWorldFromTilemap>();
        }
    }
}