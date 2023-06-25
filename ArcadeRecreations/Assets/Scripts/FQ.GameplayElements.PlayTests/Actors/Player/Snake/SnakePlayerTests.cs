using System.Collections;
using FQ.GameplayInputs;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace FQ.GameplayElements.PlayTests
{
    public class SnakePlayerTests
    {
        private GameObject playerObject;
        private Mock<IGameplayInputs> mockGameplayInputs;
        private SnakePlayer snakePlayer;
        
        public void Setup()
        {
            this.playerObject = new GameObject();
            this.snakePlayer = this.playerObject.AddComponent<SnakePlayer>();
            
            // The only reason Movement Speed is internal is to speed up tests
            // We speed up Time delta and slow down frames.
            this.snakePlayer.movementSpeed = 0.025f;
            Time.maximumDeltaTime = 0.0001f;

            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            this.snakePlayer.gameplayInputs = this.mockGameplayInputs.Object;
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            this.snakePlayer.snakeTailPrefab = snakeTailPrefab;
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(this.playerObject);
            
            // Ensure to reset frames to its default
            Time.maximumDeltaTime = Time.fixedDeltaTime;
        }
        
        #region Position with Keypress
        
        [UnityTest]
        public IEnumerator Update_PlayerRemainsStill_WhenNoKeyPressedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesDownOneUnit_WhenKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInDown_WhenKeyPressIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInDown_WhenKeyPressedBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesUpOneUnit_WhenKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInUp_WhenKeyPressIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInUp_WhenKeyPressedBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesLeftOneUnit_WhenKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInLeft_WhenKeyPressIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInLeft_WhenKeyPressedBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();

            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(true);
            yield return new WaitForSeconds(this.snakePlayer.MovementSpeed / 2);
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionLeft)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesRightOneUnit_WhenKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInRight_WhenKeyPressIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }

        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInRight_WhenKeyPressedBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionRight)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        #region Position with KeyDown

        [UnityTest]
        public IEnumerator Update_PlayerMovesDownOneUnit_WhenKeyDownIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInDown_WhenKeyDownIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInDown_WhenKeyDownBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionDown)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesUpOneUnit_WhenKeyDownIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInUp_WhenKeyDownIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInUp_WhenKeyDownBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionUp)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesLeftOneUnit_WhenKeyDownIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInLeft_WhenKeyDownIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInLeft_WhenKeyDownBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionLeft)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesRightOneUnit_WhenKeyDownIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.snakePlayer.transform.position;
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInRight_WhenKeyDownIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }

        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInRight_WhenKeyDownBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(GameplayButton.DirectionRight)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #region MoveRightThenLeft
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveLeft_WhenMovingRightAndKeyDownFirstAndKeyPressedSecondTest()
        {
            Setup();

            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;
            yield return new WaitForEndOfFrame();

            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionRight);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionRight, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionLeft);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveLeft_WhenMovingRightAndKeyPressedFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionRight);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionRight, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionLeft);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveLeft_WhenMovingRightAndKeyPressedFirstAndKeyPressedSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionRight);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionRight, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionLeft);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveLeft_WhenMovingRightAndKeyDownFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionRight);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionRight, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionLeft);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        #region MoveLeftThenRight
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveRight_WhenMovingLeftAndKeyDownFirstAndKeyPressedSecondTest()
        {
            Setup();

            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;
            yield return new WaitForEndOfFrame();

            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionLeft);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionLeft, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionRight);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveRight_WhenMovingLeftAndKeyPressedFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionLeft);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionLeft, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionRight);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveRight_WhenMovingLeftAndKeyPressedFirstAndKeyPressedSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionLeft);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionLeft, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionRight);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveRight_WhenMovingLeftAndKeyDownFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.x -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionLeft);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionLeft, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionRight);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        #region MoveDownThenUp
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveUp_WhenMovingDownAndKeyDownFirstAndKeyPressedSecondTest()
        {
            Setup();

            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;
            yield return new WaitForEndOfFrame();

            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionDown);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionDown, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionUp);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveUp_WhenMovingDownAndKeyPressedFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionDown);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionDown, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionUp);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveUp_WhenMovingDownAndKeyPressedFirstAndKeyPressedSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionDown);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionDown, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionUp);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveUp_WhenMovingDownAndKeyDownFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y -= 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionDown);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionDown, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionUp);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        #region MoveUpThenDown
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveDown_WhenMovingUpAndKeyDownFirstAndKeyPressedSecondTest()
        {
            Setup();

            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;
            yield return new WaitForEndOfFrame();

            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionUp);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionUp, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionDown);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveDown_WhenMovingUpAndKeyPressedFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionUp);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionUp, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionDown);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveDown_WhenMovingUpAndKeyPressedFirstAndKeyPressedSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyPressed;
            KeyPressMethod secondMethod = KeyPressMethod.KeyPressed;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionUp);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionUp, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionDown);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerDoesNotMoveDown_WhenMovingUpAndKeyDownFirstAndKeyDownSecondTest()
        {
            Setup();
            
            // Arrange
            KeyPressMethod firstMethod = KeyPressMethod.KeyDown;
            KeyPressMethod secondMethod = KeyPressMethod.KeyDown;
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.snakePlayer.transform.position);
            expectedPosition.y += 2;

            MockKeyInput(firstMethod, GameplayButton.DirectionUp);
            yield return RunUpdateCycle();
            MockKeyInput(firstMethod, GameplayButton.DirectionUp, press: false);
            MockKeyInput(secondMethod, GameplayButton.DirectionDown);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.snakePlayer.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion

        #endregion

        #region SnakeTail

        [UnityTest]
        public IEnumerator Setup_Creates199TailPieces_WhenGivenAPrefabTest()
        {
            Setup();
            
            // Arrange
            int pieces = SnakePlayer.MaxTailSize;

            // Act
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.IsNotNull(this.snakePlayer.SnakeTailPieces);
            Assert.AreEqual(pieces, this.snakePlayer.SnakeTailPieces.Length);
            for (int i = 0; i < pieces; ++i)
            {
                Assert.IsNotNull(this.snakePlayer.SnakeTailPieces[i]);
            }
        }
        
        [UnityTest]
        public IEnumerator Setup_PlacesTailWithHeadPiece_WhenGivenAPrefabTest()
        {
            Setup();
            
            // Arrange
            var givenPosition = new Vector3(1, 2, 3);
            this.playerObject.transform.position = givenPosition;

            // Act
            yield return new WaitForEndOfFrame();

            // Assert
            for (int i = 0; i < SnakePlayer.MaxTailSize; ++i)
            {
                Assert.AreEqual(
                    this.snakePlayer.SnakeTailPieces[i].transform.position, 
                    givenPosition);
            }
        }
        
        [UnityTest]
        public IEnumerator Setup_UnenablesTheTailPieces_WhenGivenAPrefabTest()
        {
            Setup();
            
            // Arrange

            // Act
            yield return new WaitForEndOfFrame();

            // Assert
            for (int i = 0; i < SnakePlayer.MaxTailSize; ++i)
            {
                Assert.IsFalse(this.snakePlayer.SnakeTailPieces[i].gameObject.activeSelf);
            }
        }

        #endregion
        
        #region Helper Methods
        
        private Vector3 CopyVector3(Vector3 toCopy)
        {
            return new Vector3(toCopy.x, toCopy.y, toCopy.z);
        }

        private object RunUpdateCycle()
        {
            return new WaitForSeconds(this.snakePlayer.MovementSpeed);
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
        
        #endregion
    }

    public enum KeyPressMethod
    {
        KeyDown,
        
        KeyUp,
        
        KeyPressed,
    }
}