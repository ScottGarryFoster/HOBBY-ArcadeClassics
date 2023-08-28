namespace FQ.GameplayInputs
{
    /// <summary>
    /// Interaction with Gameplay Inputs.
    /// </summary>
    public interface IGameplayInputs
    {
        /// <summary>
        /// Determines if the gameplay button is pressed.
        /// </summary>
        /// <param name="gameplayButton">Gameplay button to test for. </param>
        /// <returns>True means button is pressed. </returns>
        bool KeyPressed(GameplayButton gameplayButton);

        /// <summary>
        /// Determines if the gameplay button was released.
        /// </summary>
        /// <param name="gameplayButton">Gameplay button to test for. </param>
        /// <returns>True means button was pressed last frame. </returns>
        bool KeyUp(GameplayButton gameplayButton);

        /// <summary>
        /// Determines if the gameplay button is down and only fires the first frame.
        /// </summary>
        /// <param name="gameplayButton">Gameplay button to test for. </param>
        /// <returns>True means button was pressed this frame. </returns>
        bool KeyDown(GameplayButton gameplayButton);
    }
}