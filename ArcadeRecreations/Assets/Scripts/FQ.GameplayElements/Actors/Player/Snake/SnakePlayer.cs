using System;
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
        public SnakeTail[] SnakeTailPieces { get; private set; }

        /// <summary>
        /// How big the given tail may get. This does not include the head.
        /// </summary>
        public const int MaxTailSize = 199;
        
        /// <summary>
        /// Prefab for the Snake's body.
        /// </summary>
        /// <remarks>Internal for testing. </remarks>
        [SerializeField]
        internal SnakeTail snakeTailPrefab;
        
        /// <summary>
        /// Logic for an actor which moves.
        /// </summary>
        private IMovingActor movingActor;

        /// <summary>
        /// Handles direction inputs from the input IO.
        /// </summary>
        private IDirectionInput directionInput;
        
        /// <summary>
        /// Used to create a slower movement tick.
        /// </summary>
        private float deltaDelay;
        
        /// <summary>
        /// Current direction player is moving in.
        /// </summary>
        private Direction currentDirection;
        
        /// <summary>
        /// True means input has been received.
        /// </summary>
        private bool receivedInput;

        private int snakeTailLength;
        
        protected override void ProtectedStart()
        {
            this.currentDirection = Direction.Down;
            this.receivedInput = false;

            this.movingActor = new MovingActor();
            this.movingActor.Setup(this.transform, movement: 1);

            this.directionInput = new DirectionPressedOrDownInput();
            this.directionInput.Setup(this.gameplayInputs);

            SetupTail();
        }

        protected override void ProtectedUpdate()
        {
            UpdateNewInputInAllDirections();
            
            this.deltaDelay += Time.deltaTime;
            if (this.deltaDelay >= MovementSpeed)
            {
                this.deltaDelay -= MovementSpeed;

                if (this.receivedInput)
                {
                    UpdateTail();
                    this.movingActor.MoveActor(this.currentDirection);
                }
            }
        }

        protected override void TriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("SnakeFood"))
            {
                Debug.Log("Eat");
                other.gameObject.tag = "Untagged";
                ++snakeTailLength;
            }
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
                SnakeTail snakeTail = Instantiate(snakeTailPrefab);
                this.SnakeTailPieces[i] = snakeTail;

                this.SnakeTailPieces[i].transform.position = this.transform.position;
                this.SnakeTailPieces[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Tests the directions for turning. Will update the direction if another is found.
        /// </summary>
        private void UpdateNewInputInAllDirections()
        {
            foreach (Direction direction in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                UpdateNewInputDirectionInDirection(direction);
            }
        }

        /// <summary>
        /// Updates <see cref="currentDirection"/> and <see cref="receivedInput"/> if the given <see cref="Direction"/>
        /// is a correct fit for the new direction.
        /// </summary>
        /// <param name="direction">Direction to test turning in. </param>
        private void UpdateNewInputDirectionInDirection(Direction direction)
        {
            Direction counterDirection = GetCounterDirection(direction);
            if (DetermineIfDirectionShouldUpdate(direction, counterDirection))
            {
                this.currentDirection = direction;
                this.receivedInput = true;
            }
        }

        /// <summary>
        /// Figures out if the direction should change based on user inputs and state.
        /// </summary>
        /// <param name="direction">Direction suggested to turn. </param>
        /// <param name="counterDirection">Counter direction to the given. </param>
        /// <returns>True means the given direction is likely a good direction to move in. </returns>
        private bool DetermineIfDirectionShouldUpdate(Direction direction, Direction counterDirection)
        {
            bool haveNotMoved = !this.receivedInput;
            bool areNotMovingCounter = this.currentDirection != counterDirection;
            bool arePressingDirection = this.directionInput.PressingInputInDirection(direction);

            return arePressingDirection && (haveNotMoved || areNotMovingCounter);
        }

        /// <summary>
        /// Figures out the direction opposite the given direction.
        /// </summary>
        /// <param name="direction">Direction to test. </param>
        /// <returns>A direction opposite to given. </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented instance of <see cref="Direction"/>.
        /// </exception>
        private Direction GetCounterDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down: return Direction.Up;
                case Direction.Up: return Direction.Down;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                default:
                    throw new NotImplementedException($"{typeof(SnakePlayer)}: " +
                        $"{nameof(GetCounterDirection)} requires implementation for {direction.ToString()}.");
            }
        }
        
        private void UpdateTail()
        {
            Debug.Log("Move");
            if (this.snakeTailLength == 2)
            {
                SnakeTailPieces[1].gameObject.SetActive(true);
                SnakeTailPieces[1].gameObject.transform.position = SnakeTailPieces[0].gameObject.transform.position;
            }
            
            if (this.snakeTailLength >= 1)
            {
                SnakeTailPieces[0].gameObject.SetActive(true);
                SnakeTailPieces[0].gameObject.transform.position = this.transform.position;
            }
        }
    }
}