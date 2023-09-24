﻿using System;
using FQ.GameElementCommunication;
using FQ.Libraries.StandardTypes;
using Moq;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeHeadAnimationBehaviourTests
    {
        private Mock<IPlayerStatusBasics> mockPlayerStatusBasics;
        private Mock<IWorldInfoFromTilemap> mockWorldInfoFromTilemap;
        private ISnakeHeadAnimationBehaviour testClass;
        
        [SetUp]
        public void Setup()
        {
            this.mockPlayerStatusBasics = new Mock<IPlayerStatusBasics>();
            this.mockWorldInfoFromTilemap = new Mock<IWorldInfoFromTilemap>();
            this.testClass = new SnakeHeadAnimationBehaviour(
                mockPlayerStatusBasics.Object, 
                mockWorldInfoFromTilemap.Object);
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
                    this.mockWorldInfoFromTilemap.Object);
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
                    worldInfo: given);
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
    }
}