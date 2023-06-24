using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FQ.GameplayInputs;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// General player with some actor behaviours.
    /// </summary>
    public class InteractableActor : MonoBehaviour, IActorActiveStats
    {
        /// <summary>
        /// How fast the actor moves each time the actor moves.
        /// </summary>
        public float MovementSpeed => movementSpeed;
        
        /// <summary>
        /// How fast the actor moves each time the actor moves.
        /// </summary>
        [SerializeField]
        internal float movementSpeed = 0.2f;

        private Vector2 originalPosition;
        private Vector2Int gridPosition;
        private float deltaDelay;
        private Direction currentDirection;
        private bool receivedInput;
        
        /// <summary>
        /// Interaction with Gameplay Inputs.
        /// </summary>
        /// <remarks>Injected. </remarks>
        internal IGameplayInputs gameplayInputs;

        /// <summary>
        /// Logic for an actor which moves.
        /// </summary>
        private IMovingActor movingActor;

        /// <summary>
        /// Handles direction inputs from the input IO.
        /// </summary>
        private IDirectionInput directionInput;

        // Start is called before the first frame update
        void Start()
        {
            this.originalPosition = this.transform.position;
            this.gridPosition = new Vector2Int(0, 0);

            this.currentDirection = Direction.Down;
            this.receivedInput = false;

            this.movingActor = new MovingActor();
            this.movingActor.Setup(this.transform, movement: 1);

            this.directionInput = new DirectionPressedOrDownInput();
            this.directionInput.Setup(this.gameplayInputs);
        }

        // Update is called once per frame
        void Update()
        {
            if ((!this.receivedInput || this.currentDirection != Direction.Up) && this.directionInput.PressingInputInDirection(Direction.Down))
            {
                this.currentDirection = Direction.Down;
                this.receivedInput = true;
            }
            else if ((!this.receivedInput || this.currentDirection != Direction.Down) && this.directionInput.PressingInputInDirection(Direction.Up))
            {
                this.currentDirection = Direction.Up;
                this.receivedInput = true;
            }
            else if (this.currentDirection != Direction.Right && this.directionInput.PressingInputInDirection(Direction.Left))
            {
                this.currentDirection = Direction.Left;
                this.receivedInput = true;
            }
            else if (this.currentDirection != Direction.Left && this.directionInput.PressingInputInDirection(Direction.Right))
            {
                this.currentDirection = Direction.Right;
                this.receivedInput = true;
            }
            
            
            this.deltaDelay += Time.deltaTime;
            if (this.deltaDelay >= MovementSpeed)
            {
                this.deltaDelay -= MovementSpeed;

                if (this.receivedInput)
                {
                    this.movingActor.MoveActor(this.currentDirection);
                }
            }
        }
    }

}