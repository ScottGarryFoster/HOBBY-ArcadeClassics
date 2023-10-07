using System;
using FQ.GameElementCommunication;
using FQ.Libraries.StandardTypes;
using Moq;
using NUnit.Framework;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeTailAnimationBehaviourTests
    {
        private Mock<IPlayerStatusBasics> mockPlayerStatusBasics;
        private Mock<IWorldInfoFromTilemap> mockWorldInfoFromTilemap;
        private Mock<ITilePosition> mockTilePosition;
        private ISnakeTailAnimationBehaviour testClass;
        
        [SetUp]
        public void Setup()
        {
            this.mockPlayerStatusBasics = new Mock<IPlayerStatusBasics>();
            this.mockWorldInfoFromTilemap = new Mock<IWorldInfoFromTilemap>();
            this.mockTilePosition = new Mock<ITilePosition>();
            
            this.testClass = new SnakeTailAnimationBehaviour(
                this.mockPlayerStatusBasics.Object,
                this.mockWorldInfoFromTilemap.Object,
                this.mockTilePosition.Object);
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
                new SnakeTailAnimationBehaviour(
                    playerCommunication: given, 
                    this.mockWorldInfoFromTilemap.Object,
                    this.mockTilePosition.Object);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow);
        }
        
        [Test]
        public void OnConstruction_ArgumentNullExceptionIsThrown_WhenWorldInfoIsNullTest()
        {
            // Arrange
            IWorldInfoFromTilemap given = null;
            
            // Act
            bool didThrow = false;
            try
            {
                new SnakeTailAnimationBehaviour(
                    this.mockPlayerStatusBasics.Object,
                    worldInfo: given,
                    this.mockTilePosition.Object);
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
                new SnakeTailAnimationBehaviour(
                    this.mockPlayerStatusBasics.Object,
                    this.mockWorldInfoFromTilemap.Object,
                    tilePosition: given);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow);
        }
        
        #endregion

        #region MyRegion

        

        #endregion
    }
}