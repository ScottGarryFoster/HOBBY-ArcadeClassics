using System;
using System.Collections;
using System.Linq;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeBehaviourTests
    {
        private ISnakeBehaviour snakeBehaviour;
        private GameObject playerObject;
        private StubObjectCreation stubObjectCreation;
        private Mock<IGameplayInputs> mockGameplayInputs;
        
        [SetUp]
        public void Setup()
        {
            this.playerObject = new GameObject();
            this.stubObjectCreation = new StubObjectCreation();
            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            var concreteSnakeBehaviour = new SnakeBehaviour(
                this.playerObject, 
                this.stubObjectCreation, 
                this.mockGameplayInputs.Object,
                null);
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            concreteSnakeBehaviour.snakeTailPrefab = snakeTailPrefab;

            this.snakeBehaviour = concreteSnakeBehaviour;
        }

        [TearDown]
        public void Teardown()
        {
            this.stubObjectCreation.CreatedGameObjects.ForEach(Object.DestroyImmediate);
            this.stubObjectCreation.CreatedGameObjects.Clear();
        }

        [Test]
        public void OnConstruction_ThrowsNullArgumentException_WhenNullGameObjectGivenTest()
        {
            // Arrange
            GameObject given = null;
            
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SnakeBehaviour(given, this.stubObjectCreation, this.mockGameplayInputs.Object, null);
            });
            Assert.IsTrue(true);
        }
        
        [Test]
        public void OnConstruction_ThrowsNullArgumentException_WhenObjectCreationIsNullTest()
        {
            // Arrange
            IObjectCreation given = null;
            
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SnakeBehaviour(new GameObject(), given, this.mockGameplayInputs.Object, null);
            });
        }
        
        [Test]
        public void OnConstruction_ThrowsNullArgumentException_WhenInputIsNullTest()
        {
            // Arrange
            IGameplayInputs given = null;
            
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SnakeBehaviour(new GameObject(), this.stubObjectCreation, given, null);
            });
        }

        #region HelperMethods

        private void RunMovementUpdateCycle()
        {
            this.snakeBehaviour.Update(this.snakeBehaviour.MovementSpeed);
        }
        
        private void AddFullCollider(GameObject gameObject)
        {
            CreateTriggerBoxCollider(gameObject);
            var ridgedBody = gameObject.AddComponent<Rigidbody2D>();
            ridgedBody.gravityScale = 0;
        }
        
        private BoxCollider2D CreateTriggerBoxCollider(GameObject go)
        {
            var bc2d = go.AddComponent<BoxCollider2D>();
            bc2d.size = new Vector2(0.9f, 0.9f);
            bc2d.isTrigger = true;

            return bc2d;
        }
        
        private GameObject SetupSnakeToEatFood(
            GameObject player, 
            Mock<IGameplayInputs> mockGI, 
            out Collider2D collider2D)
        {
            Vector3 currentPosition = CopyVector3(player.transform.position);
            var foodGo = CreateGameObject(position: new Vector3(0, currentPosition.y + 1, 0), tag: "SnakeFood");
            foodGo.AddComponent<TestFood>();
            collider2D = CreateTriggerBoxCollider(foodGo);

            mockGI.Setup(x => x.KeyPressed(GameplayButton.DirectionUp))
                .Returns(true);

            return foodGo;
        }
        
        private Vector3 CopyVector3(Vector3 toCopy)
        {
            return new Vector3(toCopy.x, toCopy.y, toCopy.z);
        }

        private GameObject CreateGameObject(Vector3 position, string tag = "")
        {
            GameObject newGameObject = this.stubObjectCreation.Instantiate(new GameObject());
            newGameObject.transform.position = position;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                newGameObject.tag = "SnakeFood";
            }

            return newGameObject;
        }

        #endregion
    }
}