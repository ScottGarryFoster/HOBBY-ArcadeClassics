using System;

namespace FQ.Editors
{
    /// <summary>
    /// Describes the choice over which elements to display.
    /// </summary>
    [Flags]
    public enum WorldLoopElementsToShow
    {
        None = 0,
        
        /// <summary>
        /// Display the entrances meaning the tile to enter a loop.
        /// </summary>
        Entrances = 1,
        
        /// <summary>
        /// Display the exists meaning the places the player leaves a loop
        /// </summary>
        Exits = 2,
    }
}