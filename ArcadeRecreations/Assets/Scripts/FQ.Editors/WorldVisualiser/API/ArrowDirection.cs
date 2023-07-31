using System;

namespace FQ.Editors
{
    /// <summary>
    /// Direction for arrow visualisations.
    /// </summary>
    [Flags]
    public enum ArrowDirection
    {
        None = 0,
        
        /// <summary>
        /// Upwards.
        /// </summary>
        Up = 1,
        
        /// <summary>
        /// Downwards.
        /// </summary>
        Down = 2,
        
        /// <summary>
        /// Leftwards.
        /// </summary>
        Left = 4,
        
        /// <summary>
        /// Rightwards.
        /// </summary>
        Right = 8,
    }
}