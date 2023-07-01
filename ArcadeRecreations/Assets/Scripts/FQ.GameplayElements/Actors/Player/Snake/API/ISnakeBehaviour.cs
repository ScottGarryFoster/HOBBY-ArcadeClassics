using FQ.GameObjectPromises;

namespace FQ.GameplayElements
{
    /// <summary>
    /// The player behaviour behind the Snake Player
    /// </summary>
    public interface ISnakeBehaviour : IGameActor
    {
        /// <summary>
        /// Pieces of the Snake's body which count as the tail.
        /// </summary>
        public SnakeTail[] SnakeTailPieces { get; }
    }
}