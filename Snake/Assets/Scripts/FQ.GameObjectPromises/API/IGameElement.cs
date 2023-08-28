using System;
using UnityEngine;

namespace FQ.GameObjectPromises
{
    /// <summary>
    /// Foundational element for everything created by the game as part of the scene.
    /// </summary>
    public interface IGameElement
    {
        /// <summary>
        /// Called when the game element is to be destroyed to clean up anything.
        /// </summary>
        /// <remarks>Only reacted to by <see cref="IGameElement"/>. </remarks>
        Action DestroyGameElement { get; }
        
        /// <summary>
        /// Called when a game element believes the session has started.
        /// </summary>
        /// <remarks>Called by a <see cref="IGameElement"/>, not reacted to by <see cref="IGameElement"/>. </remarks>
        Action StartTrigger { get; set; }
        
        /// <summary>
        /// Called when a game element believes the session has ended.
        /// </summary>
        /// <remarks>Called by a <see cref="IGameElement"/>, not reacted to by <see cref="IGameElement"/>. </remarks>
        Action EndTrigger { get; set; }
        
        /// <summary>
        /// Called when the element should reset it's state to the start of the session.
        /// </summary>
        /// <remarks>Only reacted to by <see cref="IGameElement"/>. </remarks>
        Action ResetElement { get; }
    }
}