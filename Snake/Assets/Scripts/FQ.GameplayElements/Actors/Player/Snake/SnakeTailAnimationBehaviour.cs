using System;
using FQ.GameElementCommunication;
using FQ.Libraries.StandardTypes;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Behaviour to control the animation state of a tail piece.
    /// </summary>
    public class SnakeTailAnimationBehaviour : ISnakeTailAnimationBehaviour
    {
        /// <summary>
        /// Updates the animation direction.
        /// These are numbers on a keypad.
        /// </summary>
        public Action<int> ParamDirection { get; set; }

        /// <summary>
        /// Basics about the Player.
        /// </summary>
        private IPlayerStatusBasics playerCommunication;

        /// <summary>
        /// Provides information about the world based on tilemaps.
        /// </summary>
        private IWorldInfoFromTilemap worldInfo;

        /// <summary>
        /// Represents the position on a tile grid.
        /// </summary>
        private ITilePosition tilePosition;
        
        public SnakeTailAnimationBehaviour(
            IPlayerStatusBasics playerCommunication,
            IWorldInfoFromTilemap worldInfo,
            ITilePosition tilePosition)
        {
            this.playerCommunication = playerCommunication ?? throw new ArgumentNullException(
                $"{typeof(SnakeTailAnimationBehaviour)}: {nameof(playerCommunication)} must not be null.");
            
            this.worldInfo = worldInfo ?? throw new ArgumentNullException(
                $"{typeof(SnakeTailAnimationBehaviour)}: {nameof(worldInfo)} must not be null.");
            
            this.tilePosition = tilePosition ?? throw new ArgumentNullException(
                $"{typeof(SnakeTailAnimationBehaviour)}: {nameof(tilePosition)} must not be null.");
        }
    }
}