using System;
using System.Collections.Generic;
using FQ.BehaviourProviders;
using FQ.GameObjectPromises;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FQ.OverallControllers
{
    /// <summary>
    /// Controls a single session, think a single round of a game.
    /// </summary>
    public class SessionController : GameElement, ISessionController
    {
        /// <summary>
        /// Provides behaviours for the Snake Game.
        /// </summary>
        private ISnakeGameBehaviours snakeGameBehaviours;

        /// <summary>
        /// Prefab stored for the snake food.
        /// </summary>
        private GameElement snakeFoodPrefab;
        
        /// <summary>
        /// Ability to create game objects.
        /// </summary>
        private IObjectCreation objectCreation;

        /// <summary>
        /// Created objects in the scene.
        /// </summary>
        private List<GameElement> objectsInTheScene;
        
        /// <summary>
        /// Created objects in the scene.
        /// </summary>
        private List<GameElement> objectsCreatedDuringTheScene;
        
        protected override void BaseStart()
        {
            this.objectCreation = new GameObjectCreation();
            this.objectsInTheScene = new List<GameElement>();
            this.objectsCreatedDuringTheScene = new List<GameElement>();

            Scene snake = SceneManager.GetSceneByName("MainSnakeGame");
            SceneManager.SetActiveScene(snake);
            
            this.snakeGameBehaviours = new SnakeGameBehaviours(new GameObjectCreation());
            var player = this.snakeGameBehaviours.CreateNewBehaviour(SnakeGameElements.Player);
            player.StartTrigger += OnStartTrigger;
            player.EndTrigger += OnEndTrigger;
            this.objectsInTheScene.Add(player);

            this.snakeFoodPrefab = this.snakeGameBehaviours.GetPrefab(SnakeGameElements.Food);
        }

        private void OnStartTrigger()
        {
            var go = this.objectCreation.Instantiate(this.snakeFoodPrefab);
            this.objectsCreatedDuringTheScene.Add(go.GetComponent<GameElement>());
        }
        
        private void OnEndTrigger()
        {
            this.objectsCreatedDuringTheScene.ForEach(x => x.DestroyGameElement?.Invoke());
            this.objectsCreatedDuringTheScene.ForEach(x => GameObject.Destroy(x.gameObject));
            this.objectsCreatedDuringTheScene.Clear();
            this.objectsInTheScene.ForEach(x => x.ResetElement?.Invoke());
        }
    }
}