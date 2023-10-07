using System;

namespace FQ.Libraries.StandardTypes
{
    /// <summary>
    /// Describes direction on the 2D Plane.
    /// </summary>
    public enum MovementDirection
    {
        /// <summary>
        /// Upwards.
        /// </summary>
        Up,
        
        /// <summary>
        /// Downwards.
        /// </summary>
        Down,
        
        /// <summary>
        /// Leftwards.
        /// </summary>
        Left,
        
        /// <summary>
        /// Rightwards.
        /// </summary>
        Right,
    }
    
    public static class MovementDirectionExtensions
    {
        /// <summary>
        /// Finds the opposite direction of the given.
        /// </summary>
        /// <param name="movementDirection">Direction to find opposite of. </param>
        /// <returns>Opposite direction. </returns>
        /// <exception cref="NotImplementedException">
        /// Not implemented direction.
        /// </exception>
        public static MovementDirection Opposite(this MovementDirection movementDirection)
        {
            switch (movementDirection)
            {
                case MovementDirection.Up: return MovementDirection.Down;
                case MovementDirection.Down: return MovementDirection.Up;
                case MovementDirection.Left: return MovementDirection.Right;
                case MovementDirection.Right: return MovementDirection.Left;
                default:
                    throw new NotImplementedException($"{nameof(movementDirection)} not implemented as an opposite.");
            }
        }
    }
}