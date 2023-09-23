using System;
using FQ.Libraries.StandardTypes;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace FQ.GameElementCommunication.EditorTests
{
    public class PlayerStatusTests
    {
        private PlayerStatus testClass;
        
        [SetUp]
        public void Setup()
        {
            this.testClass = new PlayerStatus();
        }

        [TearDown]
        public void Teardown()
        {
            
        }

        [Test]
        public void PlayerLocation_IsEmpty_OnConstructionTest()
        {
            // Arrange
            var expected = Array.Empty<Vector2Int>();

            // Act
            
            // Assert
            Assert.AreEqual(expected, this.testClass.PlayerLocation);
        }
        
        [Test]
        public void UpdateLocation_EqualsGiven_WhenNonNullGivenTest()
        {
            // Arrange
            var expected = new Vector2Int[] {new Vector2Int(5, 10)};

            // Act
            this.testClass.UpdateLocation(expected);
            
            // Assert
            Assert.AreEqual(expected, this.testClass.PlayerLocation);
        }
        
        [Test]
        public void UpdateLocation_CausesPlayerDetailsUpdatedToCallTest()
        {
            // Arrange
            bool didCall = false;
            this.testClass.PlayerDetailsUpdated += (sender, args) =>
            {
                didCall = true;
            };

            // Act
            this.testClass.UpdateLocation(Array.Empty<Vector2Int>());
            
            // Assert
            Assert.IsTrue(didCall);
        }
        
        [Test]
        public void UpdateLocation_ThrowsArgumentNullException_WhenNullIsGivenTest()
        {
            // Arrange
            Vector2Int[] given = null;

            // Act
            bool didThrow = false;
            try
            {
                this.testClass.UpdateLocation(given);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow);
        }
 
    
        [Test]
        public void PlayerDirection_CausesPlayerDetailsUpdatedToBeCalledTest()
        {
            // Arrange
            bool didCall = false;
            this.testClass.PlayerDetailsUpdated += (sender, args) =>
            {
                didCall = true;
            };

            // Act
            this.testClass.UpdatePlayerHeadDirection(MovementDirection.Right);
                
            // Assert
            Assert.IsTrue(didCall);
        }
    
        [Test]
        public void PlayerDirection_EqualsLeft_WhenLeftGivenTest()
        {
            // Arrange
            var expected = MovementDirection.Left;

            // Act
            this.testClass.UpdatePlayerHeadDirection(expected);
            
            // Assert
            Assert.AreEqual(expected, this.testClass.PlayerDirection);
        }
        
        [Test]
        public void PlayerDirection_EqualsRight_WhenRightGivenTest()
        {
            // Arrange
            var expected = MovementDirection.Right;

            // Act
            this.testClass.UpdatePlayerHeadDirection(expected);
            
            // Assert
            Assert.AreEqual(expected, this.testClass.PlayerDirection);
        }
        
        [Test]
        public void PlayerDirection_EqualsDown_WhenDownGivenTest()
        {
            // Arrange
            var expected = MovementDirection.Down;

            // Act
            this.testClass.UpdatePlayerHeadDirection(expected);
            
            // Assert
            Assert.AreEqual(expected, this.testClass.PlayerDirection);
        }
        
        [Test]
        public void PlayerDirection_EqualsUp_WhenUpGivenTest()
        {
            // Arrange
            var expected = MovementDirection.Up;

            // Act
            this.testClass.UpdatePlayerHeadDirection(expected);
            
            // Assert
            Assert.AreEqual(expected, this.testClass.PlayerDirection);
        }
    }
}