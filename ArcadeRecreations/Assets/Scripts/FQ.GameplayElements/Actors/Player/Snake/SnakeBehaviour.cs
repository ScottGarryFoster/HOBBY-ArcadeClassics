using System;
using UnityEngine;

namespace FQ.GameplayElements
{
    public class SnakeBehaviour : ISnakeBehaviour
    {
        /// <summary>
        /// Pieces of the Snake's body which count as the tail.
        /// </summary>
        public SnakeTail[] SnakeTailPieces { get; private set; }
        
        /// <summary>
        /// Parent object.
        /// </summary>
        private GameObject parent;
        
        public SnakeBehaviour(GameObject gameObject)
        {
            this.parent = gameObject != null
                ? gameObject
                : throw new ArgumentNullException(
                    $"{typeof(SnakeBehaviour)}: {nameof(gameObject)} must not be null.");
        }
        
        /// <summary>
        /// Called when the object begins life.
        /// </summary>
        public void Start()
        {
            
        }

        /// <summary>
        /// Updates the game object.
        /// </summary>
        /// <param name="timeDelta">The time between frames. </param>
        public void Update(float timeDelta)
        {
            
        }
    }
}