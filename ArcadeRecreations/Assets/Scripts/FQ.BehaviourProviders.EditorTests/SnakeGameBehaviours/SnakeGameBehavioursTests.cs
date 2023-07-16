using System;
using NUnit.Framework;
using UnityEngine;
using FQ.BehaviourProviders;
using FQ.GameObjectPromises;
using FQ.GameplayElements;
using FQ.GameplayElements.EditorTests;
using UnityEditor;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace FQ.BehaviourProviders.EditorTests
{
    public class SnakeGameBehavioursTests
    {
        private StubObjectCreation stubObjectCreation;
        private ISnakeGameBehaviours testClass;
        
        [SetUp]
        public void Setup()
        {
            this.stubObjectCreation = new StubObjectCreation();
            this.testClass = new SnakeGameBehaviours(this.stubObjectCreation);
        }

        [TearDown]
        public void Teardown()
        {
            foreach (var obj in this.stubObjectCreation.CreatedGameObjects)
            {
                if (obj != null)
                {
                    Object.DestroyImmediate(obj);
                }
            }
        }
        
        [Test]
        public void OnConstruction_ThrowsNullArgumentException_WhenNullObjectCreationGivenTest()
        {
            // Arrange
            IObjectCreation given = null;
            
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SnakeGameBehaviours(given);
            });
            Assert.IsTrue(true);
        }
        
        [Test]
        public void CreateNewBehaviour_CreatesSnakePlayer_WhenGivenSnakeGameElementsPlayerTest()
        {
            // Arrange
            var go = new GameObject();
            SnakePlayer expected = go.AddComponent<SnakePlayer>();
            
            int snakePlayersBefore = Object.FindObjectsOfType<SnakePlayer>().Length;
            int expectedSnakePlayers = snakePlayersBefore + 1;
            
            // Act
            GameElement actual = this.testClass.CreateNewBehaviour(SnakeGameElements.Player);

            // Assert
            int actualSnakePlayers = Object.FindObjectsOfType<SnakePlayer>().Length;
            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(expectedSnakePlayers, actualSnakePlayers);
            
            // Teardown
            Object.DestroyImmediate(go);
        }
        
        [Test]
        public void GetPrefab_CreatesSnakePlayer_WhenGivenSnakeGameElementsPlayerTest()
        {
            // Arrange
            var go = new GameObject();
            SnakePlayer expected = go.AddComponent<SnakePlayer>();
            
            int expectedSnakePlayers = Object.FindObjectsOfType<SnakePlayer>().Length;

            // Act
            GameElement actual = this.testClass.GetPrefab(SnakeGameElements.Player);

            // Assert
            int actualSnakePlayers = Object.FindObjectsOfType<SnakePlayer>().Length;
            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(expectedSnakePlayers, actualSnakePlayers);
            
            // Teardown
            Object.DestroyImmediate(go);
        }
        
        [Test]
        public void CreateNewBehaviour_CreatesTestFood_WhenGivenSnakeGameElementsTestFoodTest()
        {
            // Arrange
            var go = new GameObject();
            var expected = go.AddComponent<TestFood>();
            
            int objectsBefore = Object.FindObjectsOfType<TestFood>().Length;
            int expectedObjects = objectsBefore + 1;

            // Act
            IGameElement actual = this.testClass.CreateNewBehaviour(SnakeGameElements.Food);

            // Assert
            int actualObjects = Object.FindObjectsOfType<TestFood>().Length;
            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(expectedObjects, actualObjects);

            // Teardown
            Object.DestroyImmediate(go);
        }
        
        [Test]
        public void GetPrefab_CreatesTestFood_WhenGivenSnakeGameElementsTestFoodTest()
        {
            // Arrange
            var go = new GameObject();
            var expected = go.AddComponent<TestFood>();
            
            int expectedObjects = Object.FindObjectsOfType<TestFood>().Length;

            // Act
            IGameElement actual = this.testClass.GetPrefab(SnakeGameElements.Food);

            // Assert
            int actualObjects = Object.FindObjectsOfType<TestFood>().Length;
            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(expectedObjects, actualObjects);

            // Teardown
            Object.DestroyImmediate(go);
        }
    }
}