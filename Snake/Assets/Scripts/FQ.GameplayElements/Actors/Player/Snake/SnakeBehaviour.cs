using System;
using System.Collections.Generic;
using System.Linq;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

namespace FQ.GameplayElements
{
    /// <summary>
    /// The player behaviour behind the Snake Player
    /// </summary>
    public class SnakeBehaviour : ISnakeBehaviour, IPlayerStatusBroadcaster
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
        /// Called when a game element believes the session has started.
        /// </summary>
        public Action StartTrigger { get; set; }
        
        /// <summary>
        /// Called when a game element believes the session has ended.
        /// </summary>
        public Action EndTrigger { get; set; }
        
        /// <summary>
        /// Called when the element should reset it's state to the start of the session.
        /// </summary>
        public Action ResetElement { get; set; }
        
        /// <summary>
        /// How fast the actor moves each time the actor moves.
        /// </summary>
        public float MovementSpeed { get; set; }
        
        /// <summary>
        /// Update where the player is considered to be.
        /// </summary>
        public Action<Vector2Int[]> UpdatePlayerLocation { get; set; }
        
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
        /// Provides information about the current map.
        /// </summary>
        private readonly IWorldInfoFromTilemap worldInfoInfo;
        
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
        /// Next direction to move in.
        /// </summary>
        private Direction nextDirection;
        
        /// <summary>
        /// True means input has been received.
        /// </summary>
        private bool receivedInput;

        /// <summary>
        /// Current number of tail pieces.
        /// </summary>
        private int snakeTailLength;

        /// <summary>
        /// True means add to the snake length.
        /// This ensures the snake length is added where the food is and not the head.
        /// </summary>
        private bool growingLag;

        /// <summary>
        /// True means we have told the game that the player has started.
        /// </summary>
        private bool didTriggerStart;

        /// <summary>
        /// The position the player spawned at and should return to on death.
        /// </summary>
        private Vector3 spawnPosition;

        public SnakeBehaviour(
            GameObject gameObject, 
            IObjectCreation objectCreation, 
            IGameplayInputs gameplayInputs,
            IWorldInfoFromTilemap worldInfoInfo)
        {
            this.parent = gameObject != null
                ? gameObject
                : throw new ArgumentNullException(
                    $"{typeof(SnakeBehaviour)}: {nameof(gameObject)} must not be null.");
            
            this.objectCreation = objectCreation ?? throw new ArgumentNullException(
                    $"{typeof(SnakeBehaviour)}: {nameof(objectCreation)} must not be null.");
            
            this.gameplayInputs = gameplayInputs ?? throw new ArgumentNullException(
                $"{typeof(SnakeBehaviour)}: {nameof(gameplayInputs)} must not be null.");

            this.worldInfoInfo = worldInfoInfo;
        }
        
        /// <summary>
        /// Called when the object begins life.
        /// </summary>
        public void Start()
        {
            this.spawnPosition = this.parent.transform.position;
            ResetElement += OnResetElement;
            
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
                    if (!didTriggerStart)
                    {
                        this.StartTrigger?.Invoke();
                        didTriggerStart = true;
                    }
                    
                    UpdateTail();
                    this.movingActor.MoveActor(this.nextDirection);
                    this.currentDirection = this.nextDirection;
                    
                    LoopAroundTheWorld();
                    CommunicateCurrentPosition();
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
            else if (collider2D.CompareTag("SnakeTail"))
            {
                EndTrigger?.Invoke();
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
        /// Reacts when element is to be reset.
        /// </summary>
        private void OnResetElement()
        {
            this.parent.transform.position = this.spawnPosition;
            foreach (SnakeTail snakeTailPiece in this.SnakeTailPieces)
            {
                snakeTailPiece.gameObject.SetActive(false);
            }

            this.snakeTailLength = 0;
            this.didTriggerStart = false;
            this.receivedInput = false;
        }

        /// <summary>
        /// Tests the directions for turning. Will update the direction if another is found.
        /// </summary>
        private void UpdateNewInputInAllDirections()
        {
            foreach (Direction direction in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                if (UpdateNewInputDirectionInDirection(direction))
                {
                    // Update is complete when direction is updated.
                    return;
                }
            }
        }

        /// <summary>
        /// Updates <see cref="currentDirection"/> and <see cref="receivedInput"/> if the given <see cref="Direction"/>
        /// is a correct fit for the new direction.
        /// </summary>
        /// <param name="direction">Direction to test turning in. </param>
        /// <returns>True when found a new direction. </returns>
        private bool UpdateNewInputDirectionInDirection(Direction direction)
        {
            bool foundDirection = false;
            
            Direction counterDirection = GetCounterDirection(direction);
            if (DetermineIfDirectionShouldUpdate(direction, counterDirection))
            {
                this.nextDirection = direction;
                this.receivedInput = true;
                foundDirection = true;
            }

            return foundDirection;
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
            bool wouldNotBeGoingCounter = direction != counterDirection;
            bool arePressingDirection = this.directionInput.PressingInputInDirection(direction);

            return arePressingDirection && (haveNotMoved || (areNotMovingCounter && wouldNotBeGoingCounter));
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
        
        /// <summary>
        /// Creates and sets up the tail.
        /// </summary>
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
        
        /// <summary>
        /// Updates the tail positions.
        /// </summary>
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
        
        /// <summary>
        /// Checks for loops in the world and teleports the player if there is a loop.
        /// </summary>
        private void LoopAroundTheWorld()
        {
            if (this.worldInfoInfo == null)
            {
                return;
            }
            
            Vector3 position = this.parent.transform.position;
            Vector2Int location = new((int) position.x, (int) position.y);
            
            Direction giveDirection = this.currentDirection;
            if (giveDirection == Direction.Down || giveDirection == Direction.Up)
            {
                giveDirection = GetCounterDirection(giveDirection);
            }

            CollisionPositionAnswer answer = this.worldInfoInfo.GetLoop(location, giveDirection);
            if (answer.Answer == ContextToPositionAnswer.NewPositionIsCorrect)
            {
                this.parent.transform.position = new Vector3(answer.NewPosition.x, answer.NewPosition.y);
            }
        }
        
        /// <summary>
        /// Communicates the current position of the player and tail pieces.
        /// </summary>
        private void CommunicateCurrentPosition()
        {
            List<Vector2Int> playerPosition = new();
            playerPosition.Add(GetTilePosition(this.parent.transform.position));
            foreach (var tailPiece in SnakeTailPieces.Where(x => x.isActiveAndEnabled))
            {
                playerPosition.Add(GetTilePosition(tailPiece.transform.position));
            }
            
            UpdatePlayerLocation?.Invoke(playerPosition.ToArray());
        }

        /// <summary>
        /// Returns the tile position from the transform exact position.
        /// </summary>
        /// <param name="transformPosition">A transform float based Vector position. </param>
        /// <returns>The location as a tile location. </returns>
        private Vector2Int GetTilePosition(Vector3 transformPosition)
        {
            return new Vector2Int((int) transformPosition.x, (int) transformPosition.y);
        }
    }
}