using System;
using FQ.Libraries.StandardTypes;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Moves an actor in a given direction.
    /// </summary>
    public class MovingActor : IMovingActor
    {
        /// <summary>
        /// Transform to use when moving the actor.
        /// </summary>
        private Transform actorTransform;

        /// <summary>
        /// Distance to move every full movement.
        /// </summary>
        private int movementUnit;

        /// <summary>
        /// True means actor is setup.
        /// </summary>
        private bool haveSetup;
        
        /// <summary>
        /// Sets up the moving actor, to be run once.
        /// </summary>
        /// <param name="actor">Actor <see cref="Transform"/> to move. </param>
        /// <param name="movement">Movement unit. </param>
        /// <exception cref="ArgumentNullException"><see cref="Transform"/> given is null. </exception>
        /// <exception cref="ArgumentNullException">Given unit is Zero or less. </exception>
        public void Setup(Transform actor, int movement)
        {
            this.actorTransform = actor ?? throw new ArgumentNullException(
                $"{typeof(MovingActor)}: {actor} must not be null.");
            
            this.movementUnit = movement > 0 ? movement : throw new ArgumentNullException(
                $"{typeof(MovingActor)}: {movement} must be above 0.");
            
            this.haveSetup = true;
        }

        /// <summary>
        /// Move the actor in the given direction.
        /// Must run Setup before this.
        /// </summary>
        /// <param name="currentDirection">Current direction to move the actor. </param>
        /// <exception cref="Exception">Throws if Setup not called successfully first. </exception>
        public void MoveActor(MovementDirection currentDirection)
        {
            if (!haveSetup)
            {
                throw new Exception($"{typeof(MovingActor)}: Must setup Moving actor before moving.");
            }
            
            Vector3 position = this.actorTransform.position;
            switch (currentDirection)
            {
                case MovementDirection.Down:
                    position.y -= movementUnit;
                    break;
                case MovementDirection.Up:
                    position.y += movementUnit;
                    break;
                case MovementDirection.Left:
                    position.x -= movementUnit;
                    break;
                case MovementDirection.Right:
                    position.x += movementUnit;
                    break;
            }

            this.actorTransform.position = position;
        }
    }
}