using System;
using FQ.GameObjectPromises;
using UnityEngine;

namespace FQ.GameplayElements
{
    public class SnakeBehaviour : ISnakeBehaviour
    {
        /// <summary>
        /// How big the given tail may get. This does not include the head.
        /// </summary>
        public const int MaxTailSize = 199;
        
        /// <summary>
        /// Pieces of the Snake's body which count as the tail.
        /// </summary>
        public SnakeTail[] SnakeTailPieces { get; private set; }
        
        /// <summary>
        /// Prefab for the Snake's body.
        /// </summary>
        /// <remarks>Internal for testing. </remarks>
        internal SnakeTail snakeTailPrefab;
        
        /// <summary>
        /// Parent object.
        /// </summary>
        private GameObject parent;

        /// <summary>
        /// Ability to create game objects.
        /// </summary>
        private IObjectCreation objectCreation;
        
        public SnakeBehaviour(GameObject gameObject, IObjectCreation objectCreation)
        {
            this.parent = gameObject != null
                ? gameObject
                : throw new ArgumentNullException(
                    $"{typeof(SnakeBehaviour)}: {nameof(gameObject)} must not be null.");
            
            this.objectCreation = objectCreation ?? throw new ArgumentNullException(
                    $"{typeof(SnakeBehaviour)}: {nameof(objectCreation)} must not be null.");
        }
        
        /// <summary>
        /// Called when the object begins life.
        /// </summary>
        public void Start()
        {
            SetupTail();
        }

        /// <summary>
        /// Updates the game object.
        /// </summary>
        /// <param name="timeDelta">The time between frames. </param>
        public void Update(float timeDelta)
        {
            
        }
        
        private void SetupTail()
        {
            if (this.snakeTailPrefab == null)
            {
                Debug.Log($"{typeof(SnakePlayer)}:" +
                          $"{nameof(this.snakeTailPrefab)} was null. Not setting up tail.");
                return;
            }

            this.SnakeTailPieces = new SnakeTail[MaxTailSize];
            for (int i = 0; i < MaxTailSize; ++i)
            {
                SnakeTail snakeTail = this.objectCreation.Instantiate(snakeTailPrefab);
                this.SnakeTailPieces[i] = snakeTail;

                this.SnakeTailPieces[i].transform.position = this.parent.transform.position;
                this.SnakeTailPieces[i].gameObject.SetActive(false);
            }
        }
    }
}