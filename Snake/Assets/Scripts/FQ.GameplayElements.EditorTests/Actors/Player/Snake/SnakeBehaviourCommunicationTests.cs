using System;
using System.Collections;
using System.Linq;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
using FQ.Libraries.StandardTypes;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;
using Object = UnityEngine.Object;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeBehaviourCommunicationTests
    {
        private ISnakeBehaviour snakeBehaviour;
        private GameObject playerObject;
        private StubObjectCreation stubObjectCreation;
        private Mock<IGameplayInputs> mockGameplayInputs;
        private Mock<IWorldInfoFromTilemap> mockWorldInfo;

        /// <summary>
        /// A tick amount to advance the update which will not advance the movement.
        /// </summary>
        private const float SafeFloatTick = 0.0001f;
        
        [SetUp]
        public void Setup()
        {
            this.playerObject = new GameObject();
            this.stubObjectCreation = new StubObjectCreation();
            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            this.mockWorldInfo = new Mock<IWorldInfoFromTilemap>();
            var concreteSnakeBehaviour = new SnakeBehaviour(
                this.playerObject,
                this.stubObjectCreation,
                this.mockGameplayInputs.Object,
                this.mockWorldInfo.Object)
            {
                MovementSpeed = 1
            };
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            concreteSnakeBehaviour.snakeTailPrefab = snakeTailPrefab;

            this.snakeBehaviour = concreteSnakeBehaviour;

            // By default ensures no loop is found.
            this.mockWorldInfo.Setup(x => x.GetLoop(It.IsAny<Vector2Int>(), It.IsAny<MovementDirection>()))
                .Returns(new CollisionPositionAnswer()
                {
                    Answer = ContextToPositionAnswer.NoValidMovement,
                });
            
            // All the tests for position require start to have occured.
            this.snakeBehaviour.Start();
        }

        [TearDown]
        public void Teardown()
        {
            this.stubObjectCreation.CreatedGameObjects.ForEach(Object.DestroyImmediate);
            this.stubObjectCreation.CreatedGameObjects.Clear();
        }
        
        #region Tests with Movement
        
        [Test]
        public void Update_DoesNotThrowException_WhenNoMethodIsGivenToUpdatePlayerLocationTest()
        {
            // Arrange
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);

            // Act
            RunMovementUpdateCycle();
        }
        
        [Test]
        public void Update_FiresUpdatePlayerLocation_WhenKeyPressIsDownTest()
        {
            // Arrange
            Vector2 exactPosition = this.playerObject.transform.position;
            exactPosition.y--;
            Vector2Int expectedPosition = new((int)exactPosition.x, (int)exactPosition.y);
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);

            bool didSend = false;
            Vector2Int[] actual = null;
            this.snakeBehaviour.UpdatePlayerLocation += (Vector2Int[] input) =>
            {
                didSend = true;
                actual = input;
            };

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.IsTrue(didSend, "Did not update.");
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length, $"Expected: 1 | Actual {actual.Length}");
            Assert.AreEqual(expectedPosition, actual[0], $"Expected: {expectedPosition} | Actual {actual[0]}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInDown_WhenKeyPressIsReleasedTest()
        {
            // Arrange
            Vector2 exactPosition = CopyVector3(this.playerObject.transform.position);
            exactPosition.y -= 2;
            Vector2Int expectedPosition = new((int)exactPosition.x, (int)exactPosition.y);
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(false);
            
            bool didSend = false;
            Vector2Int[] actual = null;
            this.snakeBehaviour.UpdatePlayerLocation += (Vector2Int[] input) =>
            {
                didSend = true;
                actual = input;
            };

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.IsTrue(didSend, "Did not update.");
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length, $"Expected: 1 | Actual {actual.Length}");
            Assert.AreEqual(expectedPosition, actual[0], $"Expected: {expectedPosition} | Actual {actual[0]}");
        }
        
        [Test]
        public void CollideWithFood_CausesUpdatePlayerLocationToContainTail_WhenTailShouldBeCreatedTest()
        {
            // Arrange
            Vector2 exactPosition = CopyVector3(this.playerObject.transform.position);
            exactPosition.y += 2;
            Vector2Int expectedPosition = new((int)exactPosition.x, (int)exactPosition.y);
            Vector2Int expectedTailPosition = new((int)exactPosition.x, (int)exactPosition.y - 1);
            
            this.snakeBehaviour.Start();
            EatFood(this.playerObject, this.mockGameplayInputs, this.snakeBehaviour);
            
            bool didSend = false;
            Vector2Int[] actual = null;
            this.snakeBehaviour.UpdatePlayerLocation += (Vector2Int[] input) =>
            {
                didSend = true;
                actual = input;
            };
            
            // Act
            // One to eat, One to Move and Keep the tail where it is.
            RunMovementUpdateCycle();
            RunMovementUpdateCycle();

            // Assert
            Assert.IsTrue(didSend, "Did not update.");
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length, $"Expected: 2 | Actual {actual.Length}");
            Assert.AreEqual(expectedPosition, actual[0], $"Expected: {expectedPosition} | Actual {actual[0]}");
            Assert.AreEqual(expectedTailPosition, actual[1], $"Expected: {expectedTailPosition} | Actual {actual[1]}");
        }
         
        #endregion

        
        
        #region Helper Methods
        
        private Vector3 CopyVector3(Vector3 toCopy)
        {
            return new Vector3(toCopy.x, toCopy.y, toCopy.z);
        }

        private void RunMovementUpdateCycle()
        {
            this.snakeBehaviour.Update(this.snakeBehaviour.MovementSpeed);
        }
        
        private void RunUpdateCycleButDoNotTriggerMovement()
        {
            this.snakeBehaviour.Update(SafeFloatTick);
        }

        private void MockKeyInput(KeyPressMethod method, GameplayButton button, bool press = true)
        {
            switch (method)
            {
                case KeyPressMethod.KeyDown:
                    this.mockGameplayInputs.Setup(
                        x => x.KeyDown(button)).Returns(press);
                    break;
                case KeyPressMethod.KeyPressed:
                    this.mockGameplayInputs.Setup(
                        x => x.KeyPressed(button)).Returns(press);
                    break;
                case KeyPressMethod.KeyUp:
                    this.mockGameplayInputs.Setup(
                        x => x.KeyUp(button)).Returns(press);
                    break;
            }
        }
        
        private GameObject CreateGameObject(Vector3 position, string tag = "")
        {
            var newGO = new GameObject();
            newGO.transform.position = position;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                newGO.tag = "SnakeFood";
            }

            return newGO;
        }

        private BoxCollider2D CreateTriggerBoxCollider(GameObject go)
        {
            var bc2d = go.AddComponent<BoxCollider2D>();
            bc2d.size = new Vector2(0.9f, 0.9f);
            bc2d.isTrigger = true;

            return bc2d;
        }
        
        private void AddFullCollider(GameObject gameObject)
        {
            CreateTriggerBoxCollider(gameObject);
            var ridgedBody = gameObject.AddComponent<Rigidbody2D>();
            ridgedBody.gravityScale = 0;
        }
        
        private void EatFood(GameObject player, Mock<IGameplayInputs> mockGI, ISnakeBehaviour behaviour)
        {
            SetupSnakeToEatFood(this.playerObject, this.mockGameplayInputs, out Collider2D collider2D);
            behaviour.OnTriggerEnter2D(collider2D);
            behaviour.OnTriggerStay2D(collider2D);
        }
        
        private GameObject SetupSnakeToEatFood(
            GameObject player, 
            Mock<IGameplayInputs> mockGI, 
            out Collider2D collider2D)
        {
            Vector3 currentPosition = CopyVector3(player.transform.position);
            var foodGo = CreateGameObject(position: new Vector3(0, currentPosition.y + 1, 0), tag: "SnakeFood");
            //foodGo.AddComponent<SnakeFood>();
            collider2D = CreateTriggerBoxCollider(foodGo);

            mockGI.Setup(x => x.KeyPressed(GameplayButton.DirectionUp))
                .Returns(true);

            return foodGo;
        }
        
        #endregion
    }
}