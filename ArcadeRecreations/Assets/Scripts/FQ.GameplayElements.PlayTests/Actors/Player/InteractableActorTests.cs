using System.Collections;
using FQ.GameplayElements.Actors.Player;
using FQ.GameplayInputs;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace FQ.GameplayElements.PlayTests.Actors.Player
{
    public class InteractableActorTests
    {
        private GameObject playerObject;
        private Mock<IGameplayInputs> mockGameplayInputs;
        private InteractableActor interactableActor;
        
        public void Setup()
        {
            this.playerObject = new GameObject();
            this.interactableActor = this.playerObject.AddComponent<InteractableActor>();
            
            // The only reason Movement Speed is internal is to speed up tests
            // We speed up Time delta and slow down frames.
            this.interactableActor.movementSpeed = 0.025f;
            Time.maximumDeltaTime = 0.0001f;

            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            this.interactableActor.gameplayInputs = this.mockGameplayInputs.Object;
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
            
            Vector2 expectedPosition = this.interactableActor.transform.position;

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesDownOneUnit_WhenKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionDown)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInDown_WhenKeyPressIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.y -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionDown)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionDown)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInDown_WhenKeyPressedBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionDown)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionDown)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesUpOneUnit_WhenKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionUp)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInUp_WhenKeyPressIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.y += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionUp)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionUp)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInUp_WhenKeyPressedBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionUp)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionUp)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesLeftOneUnit_WhenKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionLeft)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInLeft_WhenKeyPressIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.x -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionLeft)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionLeft)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInLeft_WhenKeyPressedBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();

            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionLeft)).Returns(true);
            yield return new WaitForSeconds(this.interactableActor.MovementSpeed / 2);
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionLeft)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesRightOneUnit_WhenKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionRight)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInRight_WhenKeyPressIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.x += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionRight)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionRight)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }

        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInRight_WhenKeyPressedBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionRight)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionRight)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
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
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionDown)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInDown_WhenKeyDownIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.y -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionDown)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionDown)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInDown_WhenKeyDownBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionDown)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionDown)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesUpOneUnit_WhenKeyDownIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionUp)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInUp_WhenKeyDownIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.y += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionUp)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionUp)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInUp_WhenKeyDownBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.y++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionUp)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionUp)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesLeftOneUnit_WhenKeyDownIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionLeft)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInLeft_WhenKeyDownIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.x -= 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionLeft)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionLeft)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInLeft_WhenKeyDownBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.x--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionLeft)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionLeft)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesRightOneUnit_WhenKeyDownIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionRight)).Returns(true);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInRight_WhenKeyDownIsReleasedTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.x += 2;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionRight)).Returns(true);
            yield return RunUpdateCycle();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionRight)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }

        [UnityTest]
        public IEnumerator Update_PlayerKeepsMovingInRight_WhenKeyDownBeforeFullMovementTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = CopyVector3(this.interactableActor.transform.position);
            expectedPosition.x++;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionRight)).Returns(true);
            yield return new WaitForEndOfFrame();
            this.mockGameplayInputs.Setup(
                x => x.KeyDown(EGameplayButton.DirectionRight)).Returns(false);

            // Act
            yield return RunUpdateCycle();

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition, 
                $"Expected {expectedPosition.ToString()} Actual {actualPosition.ToString()}");
        }
        
        #endregion
        
        private Vector3 CopyVector3(Vector3 toCopy)
        {
            return new Vector3(toCopy.x, toCopy.y, toCopy.z);
        }

        private object RunUpdateCycle()
        {
            return new WaitForSeconds(this.interactableActor.MovementSpeed);
        }
    }
}