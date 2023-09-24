using System.Numerics;
using FQ.Libraries.StandardTypes;
using Transform = UnityEngine.Transform;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Moves an actor in a given direction.
    /// </summary>
    public interface IMovingActor
    {
        /// <summary>
        /// Sets up the moving actor, to be run once.
        /// </summary>
        /// <param name="actor">Actor <see cref="Transform"/> to move. </param>
        /// <param name="movement">Movement unit. </param>
        /// <exception cref="System.ArgumentNullException"><see cref="Transform"/> given is null. </exception>
        /// <exception cref="System.ArgumentNullException">Given unit is Zero or less. </exception>
        void Setup(Transform actor, int movement);

        /// <summary>
        /// Move the actor in the given direction.
        /// Must run Setup before this.
        /// </summary>
        /// <param name="currentDirection">Current direction to move the actor. </param>
        /// <exception cref="System.Exception">Throws if Setup not called successfully first. </exception>
        void MoveActor(MovementDirection currentDirection);
    }
}