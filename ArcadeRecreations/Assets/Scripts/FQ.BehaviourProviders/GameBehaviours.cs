using System;
using FQ.GameObjectPromises;
using UnityEngine;

namespace FQ.BehaviourProviders
{
    /// <summary>
    /// Creates behaviours for the game.
    /// </summary>
    public class GameBehaviours
    {
        /// <summary>
        /// Ability to create game objects.
        /// </summary>
        private readonly IObjectCreation objectCreation;

        protected GameBehaviours(IObjectCreation objectCreation)
        {
            this.objectCreation = objectCreation ?? throw new ArgumentNullException(
                $"{typeof(SnakeGameBehaviours)}: {nameof(objectCreation)} must not be null");
        }
        
        /// <summary>
        /// Creates prefab in the scene in the form of <see cref="GameElement"/>s.
        /// </summary>
        /// <param name="gameElement">Prefab to create. </param>
        /// <returns><see cref="GameElement"/> of the prefab. </returns>
        protected GameElement CreateGameElementFromPrefab(GameElement gameElement)
        {
            var go = objectCreation.Instantiate(gameElement);
            return go.GetComponent<GameElement>();
        }
        
        /// <summary>
        /// Creates prefab in the scene in the form of <see cref="GameElement"/>s.
        /// </summary>
        /// <param name="prefabLocation">Location of the prefab. </param>
        /// <returns><see cref="GameElement"/> of the prefab. </returns>
        protected GameElement GetPrefabFromName(string prefabLocation)
        {
            return Resources.Load<GameElement>(prefabLocation);
        }
    }
}