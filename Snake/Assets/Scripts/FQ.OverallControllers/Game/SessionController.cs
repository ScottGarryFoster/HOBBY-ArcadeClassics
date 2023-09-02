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
            MovePlayerToPlaceholder(player);
            
            player.StartTrigger += OnStartTrigger;
            player.EndTrigger += OnEndTrigger;
            this.objectsInTheScene.Add(player);

            this.snakeFoodPrefab = this.snakeGameBehaviours.GetPrefab(SnakeGameElements.Food);
        }

        private void OnStartTrigger()
        {
            var go = this.objectCreation.Instantiate(this.snakeFoodPrefab);
            this.snakeFoodPrefab.StartTrigger?.Invoke();
            
            this.objectsCreatedDuringTheScene.Add(go.GetComponent<GameElement>());
        }
        
        private void OnEndTrigger()
        {
            this.objectsCreatedDuringTheScene.ForEach(x => x.DestroyGameElement?.Invoke());
            this.objectsCreatedDuringTheScene.ForEach(x => GameObject.Destroy(x.gameObject));
            this.objectsCreatedDuringTheScene.Clear();
            this.objectsInTheScene.ForEach(x => x.ResetElement?.Invoke());
        }
        
        /// <summary>
        /// Moves the player to the player placeholder.
        /// </summary>
        /// <param name="player">Player to move. </param>
        private void MovePlayerToPlaceholder(GameElement player)
        {
            GameObject playerTag = GameObject.FindGameObjectWithTag("Player");
            if (playerTag != null)
            {
                Vector3 tagPosition = playerTag.transform.position;
                tagPosition = ReduceToTopLeft(tagPosition);
                player.transform.position = tagPosition;
                Destroy(playerTag);
            }
        }

        /// <summary>
        /// Reduces the precision (floats) of the vector to the top left position.
        /// </summary>
        /// <param name="given">Vector to reduce. </param>
        /// <returns>Result with 0 points of precision. </returns>
        private Vector3 ReduceToTopLeft(Vector3 given)
        {
            given.x = (int) given.x;
            given.y = (int) given.y;
            given.z = (int) given.z;
            return given;
        }
    }
}