using UnityEngine;

namespace FQ.GameObjectPromises
{
    /// <summary>
    /// Game object which maybe triggered.
    /// </summary>
    public interface ITriggerCollidable2D
    {
        /// <summary>
        /// Occurs on a Trigger Entered in 2D Space.
        /// </summary>
        /// <param name="collider2D">Other collider. </param>
        void OnTriggerEnter2D(Collider2D collider2D);

        /// <summary>
        /// Occurs on Trigger Exited in 2D Spaced.
        /// </summary>
        /// <param name="collider2D">Other collider. </param>
        void OnTriggerExit2D(Collider2D collider2D);

        /// <summary>
        /// Occurs every frame a Trigger remains collided.
        /// </summary>
        /// <param name="collider2D">Other collider. </param>
        void OnTriggerStay2D(Collider2D collider2D);
    }
}