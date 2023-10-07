namespace FQ.GameplayElements
{
    /// <summary>
    /// The items given at runtime to the behaviour from the developer.
    /// </summary>
    public interface ISnakeHeadAnimationUserCustomisation
    {
        /// <summary>
        /// The distance to open the mouth from in tiles.
        /// </summary>
        int EatDistance { get; }
    }
}