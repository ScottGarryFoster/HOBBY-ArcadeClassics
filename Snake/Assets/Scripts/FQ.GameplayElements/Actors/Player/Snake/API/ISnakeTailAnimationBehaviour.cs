using System;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Behaviour to control the animation state of a tail piece.
    /// </summary>
    public interface ISnakeTailAnimationBehaviour
    {
        /// <summary>
        /// Updates the animation direction.
        /// These are numbers on a keypad.
        /// </summary>
        public Action<int> ParamDirection { get; set; }
    }
}