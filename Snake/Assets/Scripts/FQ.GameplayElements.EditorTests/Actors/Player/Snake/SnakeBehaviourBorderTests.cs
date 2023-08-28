using System;
using System.Collections;
using System.Linq;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;
using Object = UnityEngine.Object;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeBehaviourBorderTests
    {
        private ISnakeBehaviour snakeBehaviour;
        private GameObject playerObject;
        private StubObjectCreation stubObjectCreation;
        private Mock<IGameplayInputs> mockGameplayInputs;
        private Mock<ILoopingWorldFromTilemap> mockWorldInfo;

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
            this.mockWorldInfo = new Mock<ILoopingWorldFromTilemap>();
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
            
            // All the tests for position require start to have occured.
            this.snakeBehaviour.Start();
        }

        [TearDown]
        public void Teardown()
        {
            this.stubObjectCreation.CreatedGameObjects.ForEach(Object.DestroyImmediate);
            this.stubObjectCreation.CreatedGameObjects.Clear();
        }
        
        #region Position with Keypress
        
        [Test]
        public void Update_PlayerDoesNotTeleport_WhenLoopAnswerDoesNotGiveNewPositionTest()
        {
            // Arrange
            var given = ContextToPositionAnswer.NoValidMovement;
            var expectedPosition = new Vector2(1, 0);
            
            Vector2Int incorrect = new(5, 5);
            Vector2Int oneToTheRight = new(1, 0);
            CollisionPositionAnswer answer = new()
            {
                Answer = given,
                NewDirection = Direction.Right,
                NewPosition = incorrect
            };
            this.mockWorldInfo.Setup(x => x.GetLoop(oneToTheRight, Direction.Right))
                .Returns(answer);
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }

        [Test]
        public void Update_PlayerTeleportsToAnswer_WhenLoopAnswerIsGivenTest()
        {
            // Arrange
            Vector2Int expected = new(5, 5);
            var expectedPosition = new Vector2(expected.x, expected.y);
            
            Vector2Int oneToTheRight = new(1, 0);
            CollisionPositionAnswer answer = new()
            {
                Answer = ContextToPositionAnswer.NewPositionIsCorrect,
                NewDirection = Direction.Right,
                NewPosition = expected
            };
            this.mockWorldInfo.Setup(x => x.GetLoop(oneToTheRight, Direction.Right))
                .Returns(answer);
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
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
        
        
        
        #endregion
    }
}