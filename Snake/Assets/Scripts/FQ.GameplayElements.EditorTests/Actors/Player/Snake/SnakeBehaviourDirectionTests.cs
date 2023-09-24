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
    public class SnakeBehaviourDirectionTests
    {
        private ISnakeBehaviour snakeBehaviour;
        private GameObject playerObject;
        private StubObjectCreation stubObjectCreation;
        private Mock<IGameplayInputs> mockGameplayInputs;

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
            var concreteSnakeBehaviour = new SnakeBehaviour(
                this.playerObject,
                this.stubObjectCreation,
                this.mockGameplayInputs.Object,
                worldInfoInfo: null)
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
        
        #region Handles nothing attached to Direction Action
        
        [Test]
        public void UpdatePlayerDirection_HandlesNothingAttached_WhenButtonPressMovesDownTest()
        {
            // Arrange
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);

            // Act
            RunMovementUpdateCycle();
        }
        
        [Test]
        public void UpdatePlayerDirection_HandlesNothingAttached_WhenButtonPressMovesLeftTest()
        {
            // Arrange
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(true);

            // Act
            RunMovementUpdateCycle();
        }
        
        [Test]
        public void UpdatePlayerDirection_HandlesNothingAttached_WhenButtonPressMovesRightTest()
        {
            // Arrange
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);

            // Act
            RunMovementUpdateCycle();
        }
        
        [Test]
        public void UpdatePlayerDirection_HandlesNothingAttached_WhenButtonPressMovesUpTest()
        {
            // Arrange
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);

            // Act
            RunMovementUpdateCycle();
        }
        
        #endregion
        
        #region New Position is Sent
        
        [Test]
        public void UpdatePlayerDirection_SendsDown_WhenButtonPressMovesDownTest()
        {
            // Arrange
            MovementDirection expected = MovementDirection.Down;
            
            bool updatedDirection = false;
            MovementDirection actual = MovementDirection.Up;
            this.snakeBehaviour.UpdatePlayerDirection += direction =>
            {
                updatedDirection = true;
                actual = direction;
            };
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.IsTrue(updatedDirection);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void UpdatePlayerDirection_SendsLeft_WhenButtonPressMovesLeftTest()
        {
            // Arrange
            MovementDirection expected = MovementDirection.Left;
            
            bool updatedDirection = false;
            MovementDirection actual = MovementDirection.Up;
            this.snakeBehaviour.UpdatePlayerDirection += direction =>
            {
                updatedDirection = true;
                actual = direction;
            };
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.IsTrue(updatedDirection);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void UpdatePlayerDirection_SendsRight_WhenButtonPressMovesRightTest()
        {
            // Arrange
            MovementDirection expected = MovementDirection.Right;
            
            bool updatedDirection = false;
            MovementDirection actual = MovementDirection.Up;
            this.snakeBehaviour.UpdatePlayerDirection += direction =>
            {
                updatedDirection = true;
                actual = direction;
            };
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.IsTrue(updatedDirection);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void UpdatePlayerDirection_SendsUp_WhenButtonPressMovesUpTest()
        {
            // Arrange
            MovementDirection expected = MovementDirection.Up;
            
            bool updatedDirection = false;
            MovementDirection actual = MovementDirection.Up;
            this.snakeBehaviour.UpdatePlayerDirection += direction =>
            {
                updatedDirection = true;
                actual = direction;
            };
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.IsTrue(updatedDirection);
            Assert.AreEqual(expected, actual);
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