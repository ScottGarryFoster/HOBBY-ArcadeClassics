using System;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
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

        private ISnakeBehaviour snakeBehaviour;

        protected override void ProtectedStart()
        {
            this.snakeBehaviour =
                new SnakeBehaviour(
                    this.gameObject,
                    new GameObjectCreation(),
                    this.gameplayInputs)
                {
                    MovementSpeed = 0.25f,
                    snakeTailPrefab = this.snakeTailPrefab,
                };

            this.snakeBehaviour.Start();
        }

        protected override void ProtectedUpdate()
        {
            this.snakeBehaviour.Update(Time.deltaTime);
        }

        protected override void TriggerEnter2D(Collider2D other)
        {
            this.snakeBehaviour.OnTriggerEnter2D(other);
        }
    }
}