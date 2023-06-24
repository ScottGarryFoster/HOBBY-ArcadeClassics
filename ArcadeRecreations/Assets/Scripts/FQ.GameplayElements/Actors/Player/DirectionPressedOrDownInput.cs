using System;
using FQ.GameplayInputs;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Handles direction inputs from the input IO.
    /// </summary>
    public class DirectionPressedOrDownInput : IDirectionInput
    {
        /// <summary>
        /// Interaction with Gameplay Inputs.
        /// </summary>
        private IGameplayInputs gameplayInputs;

        /// <summary>
        /// True means setup run.
        /// </summary>
        private bool haveRunSetup;
        
        /// <summary>
        /// Inputs from the user, interfaced behind gameplay meanings.
        /// </summary>
        /// <param name="gameplayInputs">Interaction with Gameplay Inputs. </param>
        /// <exception cref="System.ArgumentNullException"><see cref="IGameplayInputs"/> is null. </exception>
        public void Setup(IGameplayInputs gameplayInputs)
        {
            this.gameplayInputs = gameplayInputs ?? throw new ArgumentNullException(
                $"{typeof(DirectionPressedOrDownInput)}: {nameof(gameplayInputs)} must not be null.");

            this.haveRunSetup = true;
        }

        /// <summary>
        /// Determines if the direction is pressed this frame.
        /// </summary>
        /// <param name="direction">Direction to test. </param>
        /// <returns>True means direction is pressed. </returns>
        public bool PressingInputInDirection(Direction direction)
        {
            if (!this.haveRunSetup)
            {
                throw new System.Exception(
                    $"{typeof(DirectionPressedOrDownInput)}: {nameof(Setup)} must be called first.");
            }

            bool pressed = this.gameplayInputs.KeyPressed(GameplayButtonFromDirection(direction));
            if (!pressed)
            {
                pressed = this.gameplayInputs.KeyDown(GameplayButtonFromDirection(direction));
            }
            
            return pressed;
        }

        private EGameplayButton GameplayButtonFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down: return EGameplayButton.DirectionDown;
                case Direction.Up: return EGameplayButton.DirectionUp;
                case Direction.Left: return EGameplayButton.DirectionLeft;
                case Direction.Right: return EGameplayButton.DirectionRight;
                default:
                    throw new NotImplementedException($"{typeof(DirectionPressedOrDownInput)}: " +
                        $"{nameof(GameplayButtonFromDirection)} not Implemented {direction.ToString()}.");
            }
        }
    }
}