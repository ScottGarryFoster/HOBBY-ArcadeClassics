namespace FQ.GameplayElements
{
    /// <summary>
    /// Describes what the actor is doing.
    /// </summary>
    public interface IActorActiveStats
    {
        /// <summary>
        /// How fast the actor moves each time the actor moves.
        /// </summary>
        float MovementSpeed { get; }
    }
}