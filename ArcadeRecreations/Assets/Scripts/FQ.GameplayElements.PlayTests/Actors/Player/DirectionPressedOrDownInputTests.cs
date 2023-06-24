using System;
using FQ.GameplayInputs;
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
                this.testClass.PressingInputInDirection(Direction.Down);
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
                testClass.PressingInputInDirection(Direction.Down);
            });
        }

        [Test]
        public void PressingInputInDirection_ReturnsFalse_WhenDirectionButtonIsNotPressedTest(
            [Values(Direction.Down, Direction.Right, Direction.Left, Direction.Up)] Direction direction)
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
            [Values(Direction.Down, Direction.Right, Direction.Left, Direction.Up)] Direction direction)
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
            var givenDirection = Direction.Down;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyPressed(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(Direction.Left);
            bool actual2 = this.testClass.PressingInputInDirection(Direction.Right);
            bool actual3 = this.testClass.PressingInputInDirection(Direction.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndPressedIsLeftTest()
        {
            // Arrange
            var givenDirection = Direction.Left;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyPressed(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(Direction.Down);
            bool actual2 = this.testClass.PressingInputInDirection(Direction.Right);
            bool actual3 = this.testClass.PressingInputInDirection(Direction.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndPressedIsRightTest()
        {
            // Arrange
            var givenDirection = Direction.Right;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyPressed(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(Direction.Down);
            bool actual2 = this.testClass.PressingInputInDirection(Direction.Left);
            bool actual3 = this.testClass.PressingInputInDirection(Direction.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndPressedIsUpTest()
        {
            // Arrange
            var givenDirection = Direction.Up;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyPressed(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(Direction.Down);
            bool actual2 = this.testClass.PressingInputInDirection(Direction.Left);
            bool actual3 = this.testClass.PressingInputInDirection(Direction.Right);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnsTrue_WhenDirectionButtonDownTest(
            [Values(Direction.Down, Direction.Right, Direction.Left, Direction.Up)] Direction direction)
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
            var givenDirection = Direction.Down;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyDown(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(Direction.Left);
            bool actual2 = this.testClass.PressingInputInDirection(Direction.Right);
            bool actual3 = this.testClass.PressingInputInDirection(Direction.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndDownIsLeftTest()
        {
            // Arrange
            var givenDirection = Direction.Left;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyDown(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(Direction.Down);
            bool actual2 = this.testClass.PressingInputInDirection(Direction.Right);
            bool actual3 = this.testClass.PressingInputInDirection(Direction.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndDownIsRightTest()
        {
            // Arrange
            var givenDirection = Direction.Right;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyDown(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(Direction.Down);
            bool actual2 = this.testClass.PressingInputInDirection(Direction.Left);
            bool actual3 = this.testClass.PressingInputInDirection(Direction.Up);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }
        
        [Test]
        public void PressingInputInDirection_ReturnFalse_WhenDirectionPressedIsNotGivenAndDownIsUpTest()
        {
            // Arrange
            var givenDirection = Direction.Up;
            this.testClass.Setup(this.mockGameplayInput.Object);
            this.mockGameplayInput.Setup(x => 
                x.KeyDown(GameplayButtonFromDirection(givenDirection))).Returns(true);

            // Act
            bool actual = this.testClass.PressingInputInDirection(Direction.Down);
            bool actual2 = this.testClass.PressingInputInDirection(Direction.Left);
            bool actual3 = this.testClass.PressingInputInDirection(Direction.Right);

            // Assert
            Assert.IsFalse(actual);
            Assert.IsFalse(actual2);
            Assert.IsFalse(actual3);
        }

        EGameplayButton GameplayButtonFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down: return EGameplayButton.DirectionDown;
                case Direction.Left: return EGameplayButton.DirectionLeft;
                case Direction.Right: return EGameplayButton.DirectionRight;
                case Direction.Up: return EGameplayButton.DirectionUp;
                default:
                    return EGameplayButton.None;
            }
        }
    }
}