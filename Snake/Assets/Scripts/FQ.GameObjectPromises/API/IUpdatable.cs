namespace FQ.GameObjectPromises
{
    /// <summary>
    /// A game object which may be updated.
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Updates the game object.
        /// </summary>
        /// <param name="timeDelta">The time between frames. </param>
        void Update(float timeDelta);
    }
}