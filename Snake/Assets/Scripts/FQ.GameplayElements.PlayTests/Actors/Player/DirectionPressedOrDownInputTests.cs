using System;
using FQ.GameplayInputs;
using FQ.Libraries.StandardTypes;
using Moq;
using NUnit.Framework;

namespace FQ.GameplayElements.PlayTests
{
    public class DirectionPressedOrDownInputTests
    {
        private Mock<IGameplayInputs> mockGameplayInput;
        private IDirectionInput testClass;
        
        [SetUp]
        public void Setup()
        {
            this.testClass = new DirectionPressedOrDownInput();
            this.mockGameplayInput = new Mock<IGameplayInputs>();
        }
        
        [Test]
        public void Setup_ThrowsArgumentNullException_WhenGameplayInputsIsNullTest()
        {
            // Arrange
            IGameplayInputs nullInput = null;

            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.testClass.Setup(nullInput);
            });
        }
        
        [Test]
        public void Setup_DoesNotThrowArgumentNullException_WhenGameplayInputsIsNotNullTest()
        {
            // Arrange
            var mockGameplayInputs = new Mock<IGameplayInputs>();

            // Act
            this.testClass.Setup(mockGameplayInputs.Object);
        }

        [Test]
        public void PressingInputInDirection_ThrowSystemException_WhenSetupIsNotCalled()
        {
            // Arrange

            // Act Assert
            Assert.Throws<Exception>(() =>
            {
                this.testClass.PressingInputInDirection(MovementDirection.Down);
            });
        }
        
        [Test]
        public void PressingInputInDirection_ThrowSystemException_WhenSetupThrowsSystemExceptionCalled()
        {
            // Arrange
            bool didThrow = false;
            try
            {
                this.testClass.Setup(gameplayInputs: null);
            }
            catch
            {
                didThrow = true;
            }
            Assert.IsTrue(didThrow, "Setup did not throw. The test requires setup to fail.");
            
            // Act Assert
            Assert.Throws<Exception>(() =>
            {
                testClass.PressingInputInDirection(MovementDirection.Down);
            });
        }

        [Test]
        public void PressingInputInDirection_ReturnsFalse_WhenDirectionButtonIsNotPressedTest(
            [Values(MovementDirection.Down, MovementDirection.Right, MovementDirection.Left, MovementDirection.Up)] MovementDirection direction)
        {
            // Arrange
            this.testClass.Setup(this.mockGameplayInput.Object);

            // Act
            bool actual = this.testClass.PressingInputInDirection(direction);

            // Assert
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnsTrue_WhenDirectionButtonPressedTest(
            [Values(MovementDirection.Down, MovementDirection.Right, MovementDirection.Left, MovementDirection.Up)] MovementDirection direction)
        {
            // Arrange
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(
                x => x.KeyPressed(GameplayButtonFromDirection(direction))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(direction);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndPressedIsDownTest()
        {
            // Arrange
            var givenDirection = MovementDirection.Down;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyPressed(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(MovementDirection.Left);
            bool actual2 = this.testClass.PressingInputInDirection(MovementDirection.Right);
            bool actual3 = this.testClass.PressingInputInDirection(MovementDirection.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndPressedIsLeftTest()
        {
            // Arrange
            var givenDirection = MovementDirection.Left;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyPressed(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(MovementDirection.Down);
            bool actual2 = this.testClass.PressingInputInDirection(MovementDirection.Right);
            bool actual3 = this.testClass.PressingInputInDirection(MovementDirection.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndPressedIsRightTest()
        {
            // Arrange
            var givenDirection = MovementDirection.Right;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyPressed(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(MovementDirection.Down);
            bool actual2 = this.testClass.PressingInputInDirection(MovementDirection.Left);
            bool actual3 = this.testClass.PressingInputInDirection(MovementDirection.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndPressedIsUpTest()
        {
            // Arrange
            var givenDirection = MovementDirection.Up;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyPressed(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(MovementDirection.Down);
            bool actual2 = this.testClass.PressingInputInDirection(MovementDirection.Left);
            bool actual3 = this.testClass.PressingInputInDirection(MovementDirection.Right);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnsTrue_WhenDirectionButtonDownTest(
            [Values(MovementDirection.Down, MovementDirection.Right, MovementDirection.Left, MovementDirection.Up)] MovementDirection direction)
        {
            // Arrange
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(
                x => x.KeyDown(GameplayButtonFromDirection(direction))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(direction);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionDownIsNotGivenAndPressedIsDownTest()
        {
            // Arrange
            var givenDirection = MovementDirection.Down;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyDown(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(MovementDirection.Left);
            bool actual2 = this.testClass.PressingInputInDirection(MovementDirection.Right);
            bool actual3 = this.testClass.PressingInputInDirection(MovementDirection.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndDownIsLeftTest()
        {
            // Arrange
            var givenDirection = MovementDirection.Left;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyDown(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(MovementDirection.Down);
            bool actual2 = this.testClass.PressingInputInDirection(MovementDirection.Right);
            bool actual3 = this.testClass.PressingInputInDirection(MovementDirection.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndDownIsRightTest()
        {
            // Arrange
            var givenDirection = MovementDirection.Right;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyDown(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(MovementDirection.Down);
            bool actual2 = this.testClass.PressingInputInDirection(MovementDirection.Left);
            bool actual3 = this.testClass.PressingInputInDirection(MovementDirection.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndDownIsUpTest()
        {
            // Arrange
            var givenDirection = MovementDirection.Up;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyDown(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(MovementDirection.Down);
            bool actual2 = this.testClass.PressingInputInDirection(MovementDirection.Left);
            bool actual3 = this.testClass.PressingInputInDirection(MovementDirection.Right);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }

        GameplayButton GameplayButtonFromDirection(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Down: return GameplayButton.DirectionDown;
                case MovementDirection.Left: return GameplayButton.DirectionLeft;
                case MovementDirection.Right: return GameplayButton.DirectionRight;
                case MovementDirection.Up: return GameplayButton.DirectionUp;
                default:
                    return GameplayButton.None;
            }
        }
    }
}