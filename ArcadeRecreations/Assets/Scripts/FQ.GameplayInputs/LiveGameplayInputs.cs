using System;
using UnityEngine;

namespace FQ.GameplayInputs
{
    /// <summary>
    /// Supplies Gameplay Inputs rather than direct inputs.
    /// </summary>
    /// <remarks>Not tested and only a paper implementation.</remarks>
    public class LiveGameplayInputs : MonoBehaviour, IGameplayInputs
    {
        private KeyCode directionDown;
        private KeyCode directionUp;
        private KeyCode directionLeft;
        private KeyCode directionRight;

        private void Start()
        {
            this.directionDown = KeyCode.S;
            this.directionUp = KeyCode.W;
            this.directionLeft = KeyCode.A;
            this.directionRight = KeyCode.D;
        }

        /// <summary>
        /// Determines if the gameplay button is pressed.
        /// </summary>
        /// <param name="gameplayButton">Gameplay button to test for. </param>
        /// <returns>True means button is pressed. </returns>
        public bool KeyPressed(EGameplayButton gameplayButton)
        {
            KeyCode gameplayAsKeycode = GetKeyAsGameplay(gameplayButton);
            if (gameplayAsKeycode == KeyCode.None)
            {
                return false;
            }

            return Input.GetKey(gameplayAsKeycode);
        }

        /// <summary>
        /// Determines if the gameplay button was released.
        /// </summary>
        /// <param name="gameplayButton">Gameplay button to test for. </param>
        /// <returns>True means button was pressed last frame. </returns>
        public bool KeyUp(EGameplayButton gameplayButton)
        {
            KeyCode gameplayAsKeycode = GetKeyAsGameplay(gameplayButton);
            if (gameplayAsKeycode == KeyCode.None)
            {
                return false;
            }

            return Input.GetKeyUp(gameplayAsKeycode);
        }

        /// <summary>
        /// Determines if the gameplay button is down and only fires the first frame.
        /// </summary>
        /// <param name="gameplayButton">Gameplay button to test for. </param>
        /// <returns>True means button was pressed this frame. </returns>
        public bool KeyDown(EGameplayButton gameplayButton)
        {
            KeyCode gameplayAsKeycode = GetKeyAsGameplay(gameplayButton);
            if (gameplayAsKeycode == KeyCode.None)
            {
                return false;
            }

            return Input.GetKeyDown(gameplayAsKeycode);
        }
        
        /// <summary>
        /// Returns the Keycode which relates to the given Gameplay key.
        /// </summary>
        /// <param name="gameplayButton">Gameplay button to convert. </param>
        /// <returns>Keycode which relates, or <see cref="KeyCode.None"/> if not supported. </returns>
        private KeyCode GetKeyAsGameplay(EGameplayButton gameplayButton)
        {
            switch (gameplayButton)
            {
                case EGameplayButton.None: return KeyCode.None;
                case EGameplayButton.DirectionUp: return KeyCode.W;
                case EGameplayButton.DirectionDown: return KeyCode.S;
                case EGameplayButton.DirectionLeft: return KeyCode.A;
                case EGameplayButton.DirectionRight: return KeyCode.D;
            }

            return KeyCode.None;
        }
    }
}