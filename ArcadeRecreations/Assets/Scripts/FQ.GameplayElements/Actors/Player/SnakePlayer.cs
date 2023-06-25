using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Represents a player in the arcade game snake.
    /// </summary>
    public class SnakePlayer : InteractableActor
    {
        private float deltaDelay;
        private Direction currentDirection;
        private bool receivedInput;
        
        /// <summary>
        /// Logic for an actor which moves.
        /// </summary>
        private IMovingActor movingActor;

        /// <summary>
        /// Handles direction inputs from the input IO.
        /// </summary>
        private IDirectionInput directionInput;

        
        protected override void ProtectedStart()
        {
            this.currentDirection = Direction.Down;
            this.receivedInput = false;

            this.movingActor = new MovingActor();
            this.movingActor.Setup(this.transform, movement: 1);

            this.directionInput = new DirectionPressedOrDownInput();
            this.directionInput.Setup(this.gameplayInputs);
        }
        
        protected override void ProtectedUpdate()
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