using System;
using UnityEngine;

namespace FQ.GameObjectPromises
{
    /// <summary>
    /// Foundational element for everything created by the game as part of the scene.
    /// </summary>
    public class GameElement : MonoBehaviour, IGameElement
    {
        /// <summary>
        /// Called when the game element is to be destroyed to clean up anything.
        /// </summary>
        /// <remarks>Only reacted to by <see cref="IGameElement"/>. </remarks>
        public Action DestroyGameElement { get; protected set; }
        
        /// <summary>
        /// Called when a game element believes the session has started.
        /// </summary>
        /// <remarks>Called by a <see cref="IGameElement"/>, not reacted to by <see cref="IGameElement"/>. </remarks>
        public Action StartTrigger { get; set; }
        
        /// <summary>
        /// Called when a game element believes the session has ended.
        /// </summary>
        /// <remarks>Called by a <see cref="IGameElement"/>, not reacted to by <see cref="IGameElement"/>. </remarks>
        public Action EndTrigger { get; set; }
        
        /// <summary>
        /// Called when the element should reset it's state to the start of the session.
        /// </summary>
        /// <remarks>Only reacted to by <see cref="IGameElement"/>. </remarks>
        public Action ResetElement { get; protected set; }
        
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            BaseUpdate();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void BaseUpdate()
        {

        }

        /// <summary>
        /// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
        /// </summary>
        private void FixedUpdate()
        {
            BaseFixedUpdate();
        }

        /// <summary>
        /// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
        /// </summary>
        protected virtual void BaseFixedUpdate()
        {
            
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// </summary>
        private void LateUpdate()
        {
            BaseLateUpdate();
        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// </summary>
        protected virtual void BaseLateUpdate()
        {
            
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just
        /// before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            BaseStart();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just
        /// before any of the Update methods are called the first time.
        /// </summary>
        protected virtual void BaseStart()
        {
            
        }

        /// <summary>
        /// Sent when another object enters a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            BaseOnTriggerEnter2D(other);
        }

        /// <summary>
        /// Sent when another object enters a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        protected virtual void BaseOnTriggerEnter2D(Collider2D other)
        {
        }

        /// <summary>
        /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        private void OnTriggerStay2D(Collider2D other)
        {
            BaseOnTriggerStay2D(other);
        }

        /// <summary>
        /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        protected virtual void BaseOnTriggerStay2D(Collider2D other)
        {
            
        }

        /// <summary>
        /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        private void OnTriggerExit2D(Collider2D other)
        {
            BaseOnTriggerExit2D(other);
        }

        /// <summary>
        /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        protected virtual void BaseOnTriggerExit2D(Collider2D other)
        {
            
        }
    }
}