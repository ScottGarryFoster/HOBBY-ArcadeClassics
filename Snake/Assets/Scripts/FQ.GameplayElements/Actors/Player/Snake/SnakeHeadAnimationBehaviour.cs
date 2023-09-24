using System;
using FQ.GameElementCommunication;
using FQ.Libraries.StandardTypes;

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

        public SnakeHeadAnimationBehaviour(IPlayerStatusBasics playerCommunication)
        {
            this.playerCommunication = playerCommunication ?? throw new ArgumentNullException(
                $"{typeof(SnakeHeadAnimationBehaviour)}: {nameof(playerCommunication)} must not be null.");

            this.playerCommunication.PlayerDetailsUpdated += OnPlayerDetailsUpdated;
        }

        /// <summary>
        /// Occurs when the player updates their details in playerCommunication.
        /// </summary>
        private void OnPlayerDetailsUpdated(object sender, EventArgs e)
        {
            UpdateDirection();
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
    }
}