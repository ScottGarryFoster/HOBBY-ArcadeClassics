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

        // Start is called before the first frame update
        void Start()
        {
            this.originalPosition = this.transform.position;
            this.gridPosition = new Vector2Int(0, 0);

            this.currentDirection = Direction.Down;
            this.receivedInput = false;

            this.movingActor = new MovingActor();
            this.movingActor.Setup(this.transform, movement: 1);
        }

        // Update is called once per frame
        void Update()
        {
            if (this.gameplayInputs.KeyPressed(EGameplayButton.DirectionDown))
            {
                this.currentDirection = Direction.Down;
                this.receivedInput = true;
            }
            else if (this.gameplayInputs.KeyPressed(EGameplayButton.DirectionUp))
            {
                this.currentDirection = Direction.Up;
                this.receivedInput = true;
            }
            else if (this.gameplayInputs.KeyPressed(EGameplayButton.DirectionLeft))
            {
                this.currentDirection = Direction.Left;
                this.receivedInput = true;
            }
            else if (this.gameplayInputs.KeyPressed(EGameplayButton.DirectionRight))
            {
                this.currentDirection = Direction.Right;
                this.receivedInput = true;
            }
            
            if (this.gameplayInputs.KeyDown(EGameplayButton.DirectionDown))
            {
                this.currentDirection = Direction.Down;
                this.receivedInput = true;
            }
            else if (this.gameplayInputs.KeyDown(EGameplayButton.DirectionUp))
            {
                this.currentDirection = Direction.Up;
                this.receivedInput = true;
            }
            else if (this.gameplayInputs.KeyDown(EGameplayButton.DirectionLeft))
            {
                this.currentDirection = Direction.Left;
                this.receivedInput = true;
            }
            else if (this.gameplayInputs.KeyDown(EGameplayButton.DirectionRight))
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