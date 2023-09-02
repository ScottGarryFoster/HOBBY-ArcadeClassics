using System;
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
    }
}