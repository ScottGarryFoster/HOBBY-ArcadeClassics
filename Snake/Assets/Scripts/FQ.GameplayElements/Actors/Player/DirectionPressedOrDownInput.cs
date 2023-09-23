using System;
using FQ.GameplayInputs;
using FQ.Libraries.StandardTypes;

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
        /// <exception cref="System.Exception">When setup is unsuccessful or not called first. </exception>
        public bool PressingInputInDirection(MovementDirection direction)
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

        /// <summary>
        /// Converts a <see cref="MovementDirection"/> to a <see cref="GameplayButton"/>.
        /// </summary>
        /// <param name="direction">Direction to convert. </param>
        /// <returns>The equivalent <see cref="GameplayButton"/>. </returns>
        /// <exception cref="NotImplementedException">
        /// Upon a <see cref="MovementDirection"/> not having an equivalent.
        /// </exception>
        private GameplayButton GameplayButtonFromDirection(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Down: return GameplayButton.DirectionDown;
                case MovementDirection.Up: return GameplayButton.DirectionUp;
                case MovementDirection.Left: return GameplayButton.DirectionLeft;
                case MovementDirection.Right: return GameplayButton.DirectionRight;
                default:
                    throw new NotImplementedException($"{typeof(DirectionPressedOrDownInput)}: " +
                        $"{nameof(GameplayButtonFromDirection)} not Implemented {direction.ToString()}.");
            }
        }
    }
}