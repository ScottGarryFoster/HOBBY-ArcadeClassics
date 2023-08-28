using UnityEngine;

namespace FQ.GameObjectPromises
{
    /// <summary>
    /// Game object which maybe collided with.
    /// </summary>
    public interface ICollidable2D
    {
        /// <summary>
        /// Occurs on a collision entered in 2D Space.
        /// </summary>
        /// <param name="other">Other collider. </param>
        void OnCollisionEnter2D(Collision2D other);
        
        /// <summary>
        /// Occurs on a collision exited in 2D Space.
        /// </summary>
        /// <param name="other">Other collider. </param>
        void OnCollisionExit2D(Collision2D other);
        
        /// <summary>
        /// Occurs every frame collisions overlap in 2D Space.
        /// </summary>
        /// <param name="other">Other collider. </param>
        void OnCollisionStay2D(Collision2D other);
    }
}