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
    public class SnakeBehaviourPositionTests
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
                this.mockGameplayInputs.Object)
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
        public void Update_PlayerRemainsStill_WhenNoKeyPressedTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
            Assert.IsTrue(true);
        }
        
        [Test]
        public void Update_PlayerMovesDownOneUnit_WhenKeyPressIsDownTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInDown_WhenKeyPressIsReleasedTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInDown_WhenKeyPressedBeforeFullMovementTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);
            RunUpdateCycleButDoNotTriggerMovement();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }

        [Test]
        public void Update_PlayerMovesUpOneUnit_WhenKeyPressIsDownTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInUp_WhenKeyPressIsReleasedTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInUp_WhenKeyPressedBeforeFullMovementTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);
            RunUpdateCycleButDoNotTriggerMovement();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerMovesLeftOneUnit_WhenKeyPressIsDownTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInLeft_WhenKeyPressIsReleasedTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInLeft_WhenKeyPressedBeforeFullMovementTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(true);
            RunUpdateCycleButDoNotTriggerMovement();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerMovesRightOneUnit_WhenKeyPressIsDownTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;
            expectedPosition.x++;
            
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
        public void Update_PlayerKeepsMovingInRight_WhenKeyPressIsReleasedTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }

        [Test]
        public void Update_PlayerKeepsMovingInRight_WhenKeyPressedBeforeFullMovementTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);
            RunUpdateCycleButDoNotTriggerMovement();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        #region Position with KeyDown

        [Test]
        public void Update_PlayerMovesDownOneUnit_WhenKeyDownIsDownTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInDown_WhenKeyDownIsReleasedTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInDown_WhenKeyDownBeforeFullMovementTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(true);
            RunUpdateCycleButDoNotTriggerMovement();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerMovesUpOneUnit_WhenKeyDownIsDownTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInUp_WhenKeyDownIsReleasedTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInUp_WhenKeyDownBeforeFullMovementTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(true);
            RunUpdateCycleButDoNotTriggerMovement();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerMovesLeftOneUnit_WhenKeyDownIsDownTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInLeft_WhenKeyDownIsReleasedTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInLeft_WhenKeyDownBeforeFullMovementTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(true);
            RunUpdateCycleButDoNotTriggerMovement();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerMovesRightOneUnit_WhenKeyDownIsDownTest()
        {
            // Arrange
            Vector2 expectedPosition = this.playerObject.transform.position;
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(true);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerKeepsMovingInRight_WhenKeyDownIsReleasedTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(true);
            RunMovementUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }

        [Test]
        public void Update_PlayerKeepsMovingInRight_WhenKeyDownBeforeFullMovementTest()
        {
            // Arrange
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(true);
            RunUpdateCycleButDoNotTriggerMovement();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(false);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #region MoveRightThenLeft
        
        [Test]
        public void Update_PlayerDoesNotMoveLeft_WhenMovingRightAndKeyDownFirstAndKeyPressedSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionRight);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionRight, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionLeft);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveLeft_WhenMovingRightAndKeyPressedFirstAndKeyDownSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            
            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionRight);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionRight, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionLeft);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveLeft_WhenMovingRightAndKeyPressedFirstAndKeyPressedSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionRight);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionRight, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionLeft);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveLeft_WhenMovingRightAndKeyDownFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionRight);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionRight, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionLeft);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        #region MoveLeftThenRight
        
        [Test]
        public void Update_PlayerDoesNotMoveRight_WhenMovingLeftAndKeyDownFirstAndKeyPressedSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionLeft);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionLeft, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionRight);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveRight_WhenMovingLeftAndKeyPressedFirstAndKeyDownSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionLeft);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionLeft, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionRight);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveRight_WhenMovingLeftAndKeyPressedFirstAndKeyPressedSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionLeft);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionLeft, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionRight);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveRight_WhenMovingLeftAndKeyDownFirstAndKeyDownSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.x -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionLeft);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionLeft, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionRight);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        #region MoveDownThenUp
        
        [Test]
        public void Update_PlayerDoesNotMoveUp_WhenMovingDownAndKeyDownFirstAndKeyPressedSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionDown);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionDown, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionUp);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveUp_WhenMovingDownAndKeyPressedFirstAndKeyDownSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionDown);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionDown, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionUp);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveUp_WhenMovingDownAndKeyPressedFirstAndKeyPressedSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionDown);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionDown, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionUp);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveUp_WhenMovingDownAndKeyDownFirstAndKeyDownSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionDown);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionDown, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionUp);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        #region MoveUpThenDown
        
        [Test]
        public void Update_PlayerDoesNotMoveDown_WhenMovingUpAndKeyDownFirstAndKeyPressedSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionUp);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionUp, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionDown);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveDown_WhenMovingUpAndKeyPressedFirstAndKeyDownSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionUp);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionUp, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionDown);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveDown_WhenMovingUpAndKeyPressedFirstAndKeyPressedSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionUp);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionUp, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionDown);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [Test]
        public void Update_PlayerDoesNotMoveDown_WhenMovingUpAndKeyDownFirstAndKeyDownSecondTest()
        {
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;

            Vector2 expectedPosition = CopyVector3(this.playerObject.transform.position);
            expectedPosition.y += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionUp);
            RunMovementUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionUp, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionDown);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector2 actualPosition = this.playerObject.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion

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

    public enum KeyPressMethod
    {
        KeyDown,
        
        KeyUp,
        
        KeyPressed,
    }
}