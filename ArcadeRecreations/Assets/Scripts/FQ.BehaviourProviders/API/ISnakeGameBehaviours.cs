using System;
using FQ.GameObjectPromises;

namespace FQ.BehaviourProviders
{
    /// <summary>
    /// Provides behaviours for the Snake Game.
    /// </summary>
    public interface ISnakeGameBehaviours
    {
        /// <summary>
        /// Creates a new behaviour in the scene.
        /// </summary>
        /// <param name="element">The type of element to create. </param>
        /// <returns>A reference to the created element. </returns>
        GameElement CreateNewBehaviour(SnakeGameElements element);
        
        /// <summary>
        /// Returns only the prefab of the element,
        /// useful for performance reason to avoid a full init in the middle
        /// of action. This allows you to cache the resource lookups.
        /// </summary>
        /// <param name="element">The type of element to return. </param>
        /// <returns>A reference to the an element to create. </returns>
        GameElement GetPrefab(SnakeGameElements element);
    }
}