using System;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Provides the correct details and methods to control the animator.
    /// </summary>
    public interface ISnakeHeadAnimationBehaviour
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
    }
}