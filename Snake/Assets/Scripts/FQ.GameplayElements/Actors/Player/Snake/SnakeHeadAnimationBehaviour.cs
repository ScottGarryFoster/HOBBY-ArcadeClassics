using System;
using System.Collections.Generic;
using System.Linq;
using FQ.GameElementCommunication;
using FQ.Libraries.StandardTypes;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Provides the correct details and methods to control the animator.
    /// </summary>
    public class SnakeHeadAnimationBehaviour : ISnakeHeadAnimationBehaviour
    {
        /// <summary>
        /// Updates the animation direction.
        /// These are numbers on a keypad.
        /// </summary>
        public Action<int> ParamDirection { get; set; }
        
        /// <summary>
        /// Updates the animation mouth open.
        /// True mean open.
        /// </summary>
        public Action<bool> ParamMouth { get; set; }

        /// <summary>
        /// Basics about the Player.
        /// </summary>
        private readonly IPlayerStatusBasics playerCommunication;

        /// <summary>
        /// Provides information about the world based on tilemaps.
        /// </summary>
        private readonly IWorldInfoFromTilemap worldInfo;

        /// <summary>
        /// Basics on locations and information on Collectable items.
        /// </summary>
        private readonly ICollectableStatusBasics collectableStatus;

        /// <summary>
        /// Represents the position on a tile grid.
        /// </summary>
        private readonly ITilePosition tilePosition;

        /// <summary>
        /// The items given at runtime to the behaviour from the developer.
        /// </summary>
        private readonly ISnakeHeadAnimationUserCustomisation userCustomisation;

        /// <summary>
        /// True means we have updated mouth at least once.
        /// </summary>
        private bool haveSentUpdatedMouth;

        /// <summary>
        /// True means open, false close.
        /// This is only true after a first update and
        /// if this is the true owner of the mouth open parameter.
        /// </summary>
        private bool currentMouthState;

        public SnakeHeadAnimationBehaviour(
            IPlayerStatusBasics playerCommunication,
            IWorldInfoFromTilemap worldInfo,
            ICollectableStatusBasics collectableStatus,
            ITilePosition tilePosition,
            ISnakeHeadAnimationUserCustomisation userCustomisation)
        {
            this.playerCommunication = playerCommunication ?? throw new ArgumentNullException(
                $"{typeof(SnakeHeadAnimationBehaviour)}: {nameof(playerCommunication)} must not be null.");
            
            this.worldInfo = worldInfo ?? throw new ArgumentNullException(
                $"{typeof(SnakeHeadAnimationBehaviour)}: {nameof(worldInfo)} must not be null.");
            
            this.collectableStatus = collectableStatus ?? throw new ArgumentNullException(
                $"{typeof(SnakeHeadAnimationBehaviour)}: {nameof(collectableStatus)} must not be null.");
            
            this.tilePosition = tilePosition ?? throw new ArgumentNullException(
                $"{typeof(SnakeHeadAnimationBehaviour)}: {nameof(tilePosition)} must not be null.");

            this.userCustomisation = userCustomisation ?? throw new ArgumentNullException(
                $"{typeof(SnakeHeadAnimationBehaviour)}: {nameof(userCustomisation)} must not be null.");

            this.playerCommunication.PlayerDetailsUpdated += OnPlayerDetailsUpdated;
        }

        /// <summary>
        /// Occurs when the player updates their details in playerCommunication.
        /// </summary>
        private void OnPlayerDetailsUpdated(object sender, EventArgs e)
        {
            UpdateDirection();
            UpdateMouthOpen();
        }

        /// <summary>
        /// Updates direction for animations.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// If the player direction does not have an animation direction.
        /// </exception>
        private void UpdateDirection()
        {
            switch (this.playerCommunication.PlayerDirection)
            {
                case MovementDirection.Down: this.ParamDirection?.Invoke(2); break;
                case MovementDirection.Left: this.ParamDirection?.Invoke(4); break;
                case MovementDirection.Right: this.ParamDirection?.Invoke(6); break;
                case MovementDirection.Up: this.ParamDirection?.Invoke(8); break;
                default:
                    throw new NotImplementedException(
                        $"{typeof(SnakeHeadAnimationBehaviour)}:" +
                        $" {nameof(this.playerCommunication.PlayerDirection)} is not handled in" +
                        $" {nameof(UpdateDirection)}");
            }
        }
        
        /// <summary>
        /// Updates the mouth open animation state.
        /// </summary>
        private void UpdateMouthOpen()
        {
            bool originalState = this.currentMouthState;
            this.currentMouthState = DiscoverMouthMovement();
            if (originalState != this.currentMouthState || !haveSentUpdatedMouth)
            {
                this.ParamMouth?.Invoke(this.currentMouthState);
                haveSentUpdatedMouth = true;
            }
        }

        /// <summary>
        /// Discovers the state the mouth should currently be in.
        /// </summary>
        /// <returns>True means mouth open, false means mouth closed. </returns>
        private bool DiscoverMouthMovement()
        {
            HashSet<Vector2Int> exploredLocations = new();
            Vector2Int currentPosition = new (this.tilePosition.Position.x, this.tilePosition.Position.y);
            for (int p = 0; p < this.userCustomisation.EatDistance; ++p)
            {
                currentPosition = MoveVectorInDirection(currentPosition, this.playerCommunication.PlayerDirection);
                if (HaveExplored(exploredLocations, currentPosition))
                {
                    return false;
                }
                
                if (PositionIsCollectable(currentPosition))
                {
                    return true;
                }
                
                if (PositionIsPlayer(currentPosition))
                {
                    return false;
                }

                CollisionPositionAnswer borderInfo = AcquireBorderInfoForPosition(currentPosition);
                if (BorderPositionIsBlockingBorder(borderInfo))
                {
                    return false;
                }
                
                if (BorderPositionIsLoop(borderInfo))
                {
                    currentPosition = borderInfo.NewPosition;
                    currentPosition = MoveVectorInDirection(
                        currentPosition, this.playerCommunication.PlayerDirection.Opposite());
                    --p;
                }
            }

            return false;
        }

        /// <summary>
        /// Figures out information about border tiles from the position.
        /// </summary>
        /// <param name="currentPosition">Position to question. </param>
        /// <returns>
        /// Information about the border.
        /// <see cref="ContextToPositionAnswer.NoValidMovement"/> means there is no border,
        /// for other options look at the enum.
        /// </returns>
        private CollisionPositionAnswer AcquireBorderInfoForPosition(Vector2Int currentPosition)
        {
            return this.worldInfo.GetLoop(currentPosition, CorrectDirectionForBorderTiles(this.playerCommunication.PlayerDirection));
        }

        /// <summary>
        /// Discovers if the given border tile is in a loop.
        /// </summary>
        /// <param name="borderInfo">Border info to acquire about. </param>
        /// <returns>True means in a loop. </returns>
        private static bool BorderPositionIsLoop(CollisionPositionAnswer borderInfo)
        {
            return borderInfo.Answer == ContextToPositionAnswer.NewPositionIsCorrect;
        }

        /// <summary>
        /// Discovers if the given border tile is blocking meaning player cannot move through it.
        /// </summary>
        /// <param name="borderInfo">Border info to acquire about. </param>
        /// <returns>True means is blocking. </returns>
        private static bool BorderPositionIsBlockingBorder(CollisionPositionAnswer borderInfo)
        {
            return borderInfo.Answer == ContextToPositionAnswer.NoMovementNeeded;
        }

        /// <summary>
        /// Discovers if the position contains something considered to be the player.
        /// </summary>
        /// <param name="currentPosition">Position to question. </param>
        /// <returns>True means is player. </returns>
        private bool PositionIsPlayer(Vector2Int currentPosition)
        {
            return this.playerCommunication.PlayerLocation.Any(x => x == currentPosition);
        }

        /// <summary>
        /// Determines if the position contains a collectable.
        /// </summary>
        /// <param name="currentPosition">Position to question. </param>
        /// <returns>True means is a collectable. </returns>
        private bool PositionIsCollectable(Vector2Int currentPosition)
        {
            return this.collectableStatus.GetCollectableLocation().Any(x => x == currentPosition);
        }

        /// <summary>
        /// Determines if this is an explored tile given the input is the set of explored tiles.
        /// Will also add the current location as an explored location.
        /// </summary>
        /// <param name="exploredLocations">A given set of explored locations. </param>
        /// <param name="currentPosition">The current position which may be a new location. </param>
        /// <returns>True means this location is not new. </returns>
        private static bool HaveExplored(HashSet<Vector2Int> exploredLocations, Vector2Int currentPosition)
        {
            return !exploredLocations.Add(currentPosition);
        }

        /// <summary>
        /// Up and Down border tiles are flipped.
        /// This corrects this.
        /// </summary>
        /// <param name="direction">Input direction. </param>
        /// <returns>The correct direction to use when polling borders. </returns>
        private MovementDirection CorrectDirectionForBorderTiles(MovementDirection direction)
        {
            if (direction == MovementDirection.Down ||
                direction == MovementDirection.Up)
            {
                return direction.Opposite();
            }

            return direction;
        }

        /// <summary>
        /// Moves the given vector in the given direction.
        /// </summary>
        /// <param name="inputPosition">Input vector direction. </param>
        /// <param name="direction">Direction to move in. </param>
        /// <returns>A vector moved by one in the direction. </returns>
        /// <exception cref="NotImplementedException">Thrown if the direction is not supported. </exception>
        private Vector2Int MoveVectorInDirection(Vector2Int inputPosition, MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Down:
                    --inputPosition.y;
                    break;
                case MovementDirection.Up:
                    ++inputPosition.y;
                    break;
                case MovementDirection.Left:
                    --inputPosition.x;
                    break;
                case MovementDirection.Right:
                    ++inputPosition.x;
                    break;
                default:
                    throw new NotImplementedException(
                        $"{typeof(SnakeHeadAnimationBehaviour)}.{nameof(MoveVectorInDirection)}: " +
                        $"{nameof(direction)} is not implemented: {direction.ToString()}.");
            }

            return inputPosition;
        }
    }
}