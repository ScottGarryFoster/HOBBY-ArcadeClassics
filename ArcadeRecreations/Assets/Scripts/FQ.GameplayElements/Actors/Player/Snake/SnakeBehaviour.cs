using System;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
using UnityEngine;
using Object = System.Object;

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
        /// How fast the actor moves each time the actor moves.
        /// </summary>
        public float MovementSpeed { get; set; }
        
        /// <summary>
        /// Prefab for the Snake's body.
        /// </summary>
        /// <remarks>Internal for testing. </remarks>
        internal SnakeTail snakeTailPrefab;
        
        /// <summary>
        /// Parent object.
        /// </summary>
        private readonly GameObject parent;

        /// <summary>
        /// Ability to create game objects.
        /// </summary>
        private readonly IObjectCreation objectCreation;

        /// <summary>
        /// Ability to take input from the Player.
        /// </summary>
        private readonly IGameplayInputs gameplayInputs;
        
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

        private bool growingLag;
        
        public SnakeBehaviour(GameObject gameObject, IObjectCreation objectCreation, IGameplayInputs gameplayInputs)
        {
            this.parent = gameObject != null
                ? gameObject
                : throw new ArgumentNullException(
                    $"{typeof(SnakeBehaviour)}: {nameof(gameObject)} must not be null.");
            
            this.objectCreation = objectCreation ?? throw new ArgumentNullException(
                    $"{typeof(SnakeBehaviour)}: {nameof(objectCreation)} must not be null.");
            
            this.gameplayInputs = gameplayInputs ?? throw new ArgumentNullException(
                $"{typeof(SnakeBehaviour)}: {nameof(gameplayInputs)} must not be null.");
        }
        
        /// <summary>
        /// Called when the object begins life.
        /// </summary>
        public void Start()
        {
            this.currentDirection = Direction.Down;
            this.receivedInput = false;

            this.movingActor = new MovingActor();
            this.movingActor.Setup(this.parent.transform, movement: 1);

            this.directionInput = new DirectionPressedOrDownInput();
            this.directionInput.Setup(this.gameplayInputs);
            
            SetupTail();
        }

        /// <summary>
        /// Updates the game object.
        /// </summary>
        /// <param name="timeDelta">The time between frames. </param>
        public void Update(float timeDelta)
        {
            UpdateNewInputInAllDirections();
            
            this.deltaDelay += timeDelta;
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

        /// <summary>
        /// Occurs on a Trigger Entered in 2D Space.
        /// </summary>
        /// <param name="collider2D">Other collider. </param>
        public void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.CompareTag("SnakeFood"))
            {
                collider2D.gameObject.tag = "Untagged";
                this.growingLag = true;
            }
        }

        /// <summary>
        /// Occurs on Trigger Exited in 2D Spaced.
        /// </summary>
        /// <param name="collider2D">Other collider. </param>
        public void OnTriggerExit2D(Collider2D collider2D)
        {
            
        }

        /// <summary>
        /// Occurs every frame a Trigger remains collided.
        /// </summary>
        /// <param name="collider2D">Other collider. </param>
        public void OnTriggerStay2D(Collider2D collider2D)
        {
            
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
        
        private void UpdateTail()
        {
            if (this.snakeTailLength >= 1)
            {
                for (int i = this.snakeTailLength - 1; i > 0; --i)
                {
                    SnakeTailPieces[i].gameObject.SetActive(true);
                    SnakeTailPieces[i].gameObject.transform.position =
                        SnakeTailPieces[i - 1].gameObject.transform.position;
                }

                SnakeTailPieces[0].gameObject.SetActive(true);
                SnakeTailPieces[0].gameObject.transform.position = this.parent.transform.position;
            }

            if (this.growingLag)
            {
                ++snakeTailLength;
                this.growingLag = false;
            }
        }
    }
}