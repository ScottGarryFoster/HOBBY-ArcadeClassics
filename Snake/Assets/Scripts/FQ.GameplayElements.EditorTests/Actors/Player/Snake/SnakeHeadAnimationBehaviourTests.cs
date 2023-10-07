using System;
using FQ.GameElementCommunication;
using FQ.Libraries.StandardTypes;
using Moq;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using Range = NUnit.Framework.RangeAttribute;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeHeadAnimationBehaviourTests
    {
        private Mock<IPlayerStatusBasics> mockPlayerStatusBasics;
        private Mock<IWorldInfoFromTilemap> mockWorldInfoFromTilemap;
        private Mock<ICollectableStatusBasics> mockCollectableStatusBasics;
        private Mock<ITilePosition> mockTilePosition;
        private Mock<ISnakeHeadAnimationUserCustomisation> mockUserCustomisation;
        private ISnakeHeadAnimationBehaviour testClass;
        
        [SetUp]
        public void Setup()
        {
            this.mockPlayerStatusBasics = new Mock<IPlayerStatusBasics>();
            this.mockCollectableStatusBasics = new Mock<ICollectableStatusBasics>();
            this.mockWorldInfoFromTilemap = new Mock<IWorldInfoFromTilemap>();
            this.mockTilePosition = new Mock<ITilePosition>();
            this.mockUserCustomisation = new Mock<ISnakeHeadAnimationUserCustomisation>();
            this.testClass = new SnakeHeadAnimationBehaviour(
                this.mockPlayerStatusBasics.Object, 
                this.mockWorldInfoFromTilemap.Object,
                this.mockCollectableStatusBasics.Object,
                this.mockTilePosition.Object,
                this.mockUserCustomisation.Object);
            
            CollisionPositionAnswer border = new()
            {
                Answer = ContextToPositionAnswer.NoValidMovement,
            };
            this.mockWorldInfoFromTilemap.Setup(x => x.GetLoop(It.IsAny<Vector2Int>(), It.IsAny<MovementDirection>()))
                .Returns(border);

            // We will test by default a distance of 3. This is inline as test fixtures will use it.
            this.mockUserCustomisation.Setup(x => x.EatDistance).Returns(3);
        }

        #region Construction
        
        [Test]
        public void OnConstruction_ArgumentNullExceptionIsThrown_WhenElementCommunicationIsNullTest()
        {
            // Arrange
            IPlayerStatusBasics given = null;
            
            // Act
            bool didThrow = false;
            try
            {
                new SnakeHeadAnimationBehaviour(
                    playerCommunication: given, 
                    this.mockWorldInfoFromTilemap.Object,
                    this.mockCollectableStatusBasics.Object,
                    this.mockTilePosition.Object,
                    this.mockUserCustomisation.Object);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow);
        }
        
        [Test]
        public void OnConstruction_ArgumentNullExceptionIsThrown_WhenWorldInfoFromTilemapIsNullTest()
        {
            // Arrange
            IWorldInfoFromTilemap given = null;
            
            // Act
            bool didThrow = false;
            try
            {
                new SnakeHeadAnimationBehaviour(
                    this.mockPlayerStatusBasics.Object,
                    worldInfo: given,
                    this.mockCollectableStatusBasics.Object,
                    this.mockTilePosition.Object,
                    this.mockUserCustomisation.Object);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow);
        }
        
        [Test]
        public void OnConstruction_ArgumentNullExceptionIsThrown_WhenCollectableStatusBasicsIsNullTest()
        {
            // Arrange
            ICollectableStatusBasics given = null;
            
            // Act
            bool didThrow = false;
            try
            {
                new SnakeHeadAnimationBehaviour(
                    this.mockPlayerStatusBasics.Object,
                    this.mockWorldInfoFromTilemap.Object,
                    given,
                    this.mockTilePosition.Object,
                    this.mockUserCustomisation.Object);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow);
        }
        
        [Test]
        public void OnConstruction_ArgumentNullExceptionIsThrown_WhenTilePositionIsNullTest()
        {
            // Arrange
            ITilePosition given = null;
            
            // Act
            bool didThrow = false;
            try
            {
                new SnakeHeadAnimationBehaviour(
                    this.mockPlayerStatusBasics.Object,
                    this.mockWorldInfoFromTilemap.Object,
                    this.mockCollectableStatusBasics.Object,
                    tilePosition: given,
                    this.mockUserCustomisation.Object);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow);
        }
        
        [Test]
        public void OnConstruction_ArgumentNullExceptionIsThrown_WhenUserCustomisationIsNullTest()
        {
            // Arrange
            SnakeHeadAnimationUserCustomisation given = null;
            
            // Act
            bool didThrow = false;
            try
            {
                new SnakeHeadAnimationBehaviour(
                    this.mockPlayerStatusBasics.Object,
                    this.mockWorldInfoFromTilemap.Object,
                    this.mockCollectableStatusBasics.Object,
                    this.mockTilePosition.Object,
                    userCustomisation: given);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow);
        }
        
        #endregion
        
        #region DirectionParam

        [Test]
        public void OnPlayerDetailsUpdated_CallsParamDirectionWith2_WhenPlayerDirectionIsDownTest()
        {
            // Arrange
            int expected = 2;
            
            MovementDirection given = MovementDirection.Down;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);
            
            int actual = -1;
            this.testClass.ParamDirection += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_CallsParamDirectionWith6_WhenPlayerDirectionIsRightTest()
        {
            // Arrange
            int expected = 6;
            
            MovementDirection given = MovementDirection.Right;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);
            
            int actual = -1;
            this.testClass.ParamDirection += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_CallsParamDirectionWith4_WhenPlayerDirectionIsLeftTest()
        {
            // Arrange
            int expected = 4;
            
            MovementDirection given = MovementDirection.Left;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);
            
            int actual = -1;
            this.testClass.ParamDirection += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_CallsParamDirectionWith8_WhenPlayerDirectionIsUpTest()
        {
            // Arrange
            int expected = 8;
            
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);
            
            int actual = -1;
            this.testClass.ParamDirection += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        #endregion
        
        #region Mouth Openning
        
        [Test]
        public void OnPlayerDetailsUpdated_OpensMouth_WhenFoodIsDirectlyInFrontTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int positionAbovePlayer = new(givenStartPosition.x, givenStartPosition.y + distance);
            MockASingleCollectableAtLocation(positionAbovePlayer);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_ClosesMouth_WhenNoFoodIsInFrontTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_OpensMouthIsOnlyCalledOnce_WhenPlayerDetailsUpdatedTwiceAndLastUpdateWasOpenTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int positionAbovePlayer = new(givenStartPosition.x, givenStartPosition.y + distance);
            MockASingleCollectableAtLocation(positionAbovePlayer);

            int occurrence = 0;
            bool actual = false;
            this.testClass.ParamMouth += d => 
            { 
                actual = d;
                ++occurrence;
            };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(actual);
            Assert.AreEqual(1, occurrence);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_CloseMouthIsOnlyCalledOnce_WhenPlayerDetailsUpdatedTwiceAndLastUpdateWasCloseTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            int occurrence = 0;
            bool actual = false;
            this.testClass.ParamMouth += d => 
            { 
                actual = d;
                ++occurrence;
            };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsFalse(actual);
            Assert.AreEqual(1, occurrence);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_DoesNotOpenMouth_WhenATailPieceDirectlyBlocksCollectableTest(
            [Range(1, 2, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int[] givenPlayerTail = {new(givenStartPosition.x, givenStartPosition.y + distance)};
            this.mockPlayerStatusBasics.Setup(x => x.PlayerLocation).Returns(givenPlayerTail);
            
            Vector2Int positionAbovePlayer = new(givenStartPosition.x, givenStartPosition.y + distance + 1);
            MockASingleCollectableAtLocation(positionAbovePlayer);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_DoesNotOpenMouth_WhenANonLoopingBorderIsInFrontOfPlayerTest(
            [Range(1, 2, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int givenPlayerTail = new(givenStartPosition.x, givenStartPosition.y + distance);
            CollisionPositionAnswer border = new()
            {
                Answer = ContextToPositionAnswer.NoMovementNeeded
            };
            this.mockWorldInfoFromTilemap.Setup(x => x.GetLoop(givenPlayerTail, MovementDirection.Up.Opposite()))
                .Returns(border);
            
            Vector2Int positionAbovePlayer = new(givenStartPosition.x, givenStartPosition.y + distance + 1);
            MockASingleCollectableAtLocation(positionAbovePlayer);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_OpensMouth_WhenBorderIsLoopAndOthersideIsFoodTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);
            
            Vector2Int givenPlayerTail = new(givenStartPosition.x, givenStartPosition.y + distance);
            Vector2Int foodPosition = new(10, 10);
            CollisionPositionAnswer border = new()
            {
                Answer = ContextToPositionAnswer.NewPositionIsCorrect,
                NewPosition = foodPosition,
                NewDirection = given,
            };
            this.mockWorldInfoFromTilemap.Setup(x => x.GetLoop(givenPlayerTail, given.Opposite()))
                .Returns(border);
            
            MockASingleCollectableAtLocation(foodPosition);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test, Timeout(100)]
        public void OnPlayerDetailsUpdated_DoesNothingAndDoesNotTimeout_WhenBorderLoopCreatedAnActualLoopTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);
            
            Vector2Int firstLoop = new(givenStartPosition.x, givenStartPosition.y + distance);
            Vector2Int nextloop = new(10, 10);
            CollisionPositionAnswer border = new()
            {
                Answer = ContextToPositionAnswer.NewPositionIsCorrect,
                NewPosition = nextloop,
                NewDirection = given,
            };
            this.mockWorldInfoFromTilemap.Setup(x => x.GetLoop(firstLoop, given.Opposite()))
                .Returns(border);
            
            CollisionPositionAnswer secondAnswer = new()
            {
                Answer = ContextToPositionAnswer.NewPositionIsCorrect,
                NewPosition = firstLoop,
                NewDirection = given,
            };
            this.mockWorldInfoFromTilemap.Setup(x => x.GetLoop(nextloop, given.Opposite()))
                .Returns(secondAnswer);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsFalse(actual);
        }
        
        #region Test Direction
        
        [Test]
        public void OnPlayerDetailsUpdated_OpensMouth_WhenPlayerFacesUpAndFoodIsAboveTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int positionAbovePlayer = new(givenStartPosition.x, givenStartPosition.y + distance);
            MockASingleCollectableAtLocation(positionAbovePlayer);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_OpensMouth_WhenPlayerFacesDownAndFoodIsBelowTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Down;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int positionAbovePlayer = new(givenStartPosition.x, givenStartPosition.y - distance);
            MockASingleCollectableAtLocation(positionAbovePlayer);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_OpensMouth_WhenPlayerFacesLeftAndFoodIsToTheLeftTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Left;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int positionAbovePlayer = new(givenStartPosition.x - distance, givenStartPosition.y);
            MockASingleCollectableAtLocation(positionAbovePlayer);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_OpensMouth_WhenPlayerFacesRightAndFoodIsToTheRightTest(
            [Range(1, 3, 1)] int distance)
        {
            // Arrange
            MovementDirection given = MovementDirection.Right;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int positionAbovePlayer = new(givenStartPosition.x + distance, givenStartPosition.y);
            MockASingleCollectableAtLocation(positionAbovePlayer);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_OpensMouth_WhenFoodDistanceIsSetToFourAndFoodIsFourSpacesAwayTest()
        {
            // Arrange
            int givenDistance = 4;
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int positionAbovePlayer = new(givenStartPosition.x, givenStartPosition.y + givenDistance);
            MockASingleCollectableAtLocation(positionAbovePlayer);

            this.mockUserCustomisation.Setup(x => x.EatDistance).Returns(givenDistance);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void OnPlayerDetailsUpdated_DoesNotOpenMouth_WhenFoodDistanceIsSetToThreeAndFoodIsFourSpacesAwayTest()
        {
            // Arrange
            int givenDistance = 3;
            MovementDirection given = MovementDirection.Up;
            this.mockPlayerStatusBasics.Setup(x => x.PlayerDirection).Returns(given);

            Vector3Int givenStartPosition = new(1, 2);
            this.mockTilePosition.Setup(x => x.Position).Returns(givenStartPosition);

            Vector2Int positionAbovePlayer = new(givenStartPosition.x, givenStartPosition.y + givenDistance + 1);
            MockASingleCollectableAtLocation(positionAbovePlayer);

            this.mockUserCustomisation.Setup(x => x.EatDistance).Returns(givenDistance);
            
            bool actual = false;
            this.testClass.ParamMouth += d => { actual = d; };
            
            // Act
            this.mockPlayerStatusBasics.Raise(x => x.PlayerDetailsUpdated -= null, EventArgs.Empty);

            // Assert
            Assert.IsFalse(actual);
        }
        
        #endregion

        private void MockASingleCollectableAtLocation(Vector2Int positionAbovePlayer)
        {
            Vector2Int[] collectable = {positionAbovePlayer};
            this.mockCollectableStatusBasics.Setup(x => x.GetCollectableLocation()).Returns(collectable);
        }

        #endregion
    }
}