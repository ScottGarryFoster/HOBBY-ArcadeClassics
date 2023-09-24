using System;
using FQ.Logger;
using NUnit.Framework;
using UnityEngine;

namespace FQ.GameElementCommunication.EditorTests
{
    public class CollectableStatusTests
    {
        private ICollectableStatus testClass;
        
        [SetUp]
        public void Setup()
        {
            Log.TestMode();
            this.testClass = new CollectableStatus();
        }

        [Test]
        public void GetCollectableLocation_ReturnsEmptyArray_AfterConstructionTest()
        {
            // Arrange
            Vector2Int[] expected = Array.Empty<Vector2Int>();
            
            // Act
            Vector2Int[] actual = this.testClass.GetCollectableLocation();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GetCollectableLocation_ReturnsEmptyArray_WhenGivenABucketTest()
        {
            // Arrange
            Vector2Int[] expected = Array.Empty<Vector2Int>();
            CollectableBucket given = CollectableBucket.BasicValue;
            
            // Act
            Vector2Int[] actual = this.testClass.GetCollectableLocation(given);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpdateCollectableLocation_ThrowsArgumentException_WhenGivenNullLocationTest()
        {
            // Arrange
            Vector2Int[] given = null;

            // Act
            bool didThrow = false;
            try
            {
                this.testClass.UpdateCollectableLocation(CollectableBucket.BasicValue, given);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }

            // Assert
            Assert.IsTrue(didThrow);
        }

        [Test]
        public void GetCollectableLocation_ReturnsGivenLocation_WhenUpdateCollectableLocationIsCalledTest()
        {
            // Arrange
            Vector2Int[] expected = {new (1, 2)};
            this.testClass.UpdateCollectableLocation(CollectableBucket.BasicValue, expected);
            
            // Act
            Vector2Int[] actual = this.testClass.GetCollectableLocation();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GetCollectableLocation_ReturnsAllGivenLocations_WhenUpdateCollectableLocationIsCalledTest()
        {
            // Arrange
            Vector2Int[] expected = {new (1, 2), new(3, 4), new(5, 6)};
            this.testClass.UpdateCollectableLocation(CollectableBucket.BasicValue, expected);
            
            // Act
            Vector2Int[] actual = this.testClass.GetCollectableLocation();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GetCollectableLocation_ReturnsGivenLocation_WhenUpdatedAndRequestedWithTheSameBucketTest(
            [Values(CollectableBucket.BasicValue, CollectableBucket.LimitedOffer)] CollectableBucket givenBucket)
        {
            // Arrange
            Vector2Int[] expected = {new (1, 2)};
            this.testClass.UpdateCollectableLocation(givenBucket, expected);
            
            // Act
            Vector2Int[] actual = this.testClass.GetCollectableLocation(givenBucket);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CollectableDetailsUpdated_DidInvoke_WhenUpdateCollectableLocationIsCalledTest()
        {
            // Arrange
            bool didCall = false;
            this.testClass.CollectableDetailsUpdated += (sender, args) => didCall = true;

            CollectableBucket givenBucket = CollectableBucket.BasicValue;
            Vector2Int[] givenLocation = Array.Empty<Vector2Int>();

            // Act
            this.testClass.UpdateCollectableLocation(givenBucket, givenLocation);
            
            // Assert
            Assert.IsTrue(didCall);
        }
    }
}