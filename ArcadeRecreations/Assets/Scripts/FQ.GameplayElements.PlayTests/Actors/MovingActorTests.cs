using System;
using FQ.GameplayElements;
using NUnit.Framework;
using UnityEngine;
using Object = System.Object;

namespace FQ.GameplayElements.PlayTests
{
    public class MovingActorTests
    {
        private IMovingActor testClass;
        private Transform testTransform;
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            this.testClass = new MovingActor();
            this.testGameObject = new GameObject();
            this.testTransform = this.testGameObject.transform;
        }

        [TearDown]
        public void Teardown()
        {
            UnityEngine.Object.DestroyImmediate(this.testGameObject);
        }
        
        [Test]
        public void MoveActor_ThrowsException_WhenSetupNotCalledTest()
        {
            // Arrange
            
            // Act Assert
            Assert.Throws<Exception>(() =>
            {
                this.testClass.MoveActor(Direction.Down);
            });
        }
        
        [Test]
        public void MoveActor_ThrowException_WhenSetupThrowsExceptionTest()
        {
            // Arrange
            try
            {
                this.testClass.Setup(this.testTransform, movement: -1);
            }
            catch (Exception e)
            {
                
            }

            // Act Assert
            Assert.Throws<Exception>(() =>
            {
                this.testClass.MoveActor(Direction.Down);
            });
        }

        [Test]
        public void Setup_ThrowsArgumentNullException_WhenTransformIsNullTest()
        {
            // Arrange
            Transform given = null;
            int movement = 1;

            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.testClass.Setup(given, movement);
            });
        }
        
        [Test]
        public void Setup_ThrowsArgumentNullException_WhenMovementIsZeroTest()
        {
            // Arrange
            int movement = 0;

            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.testClass.Setup(this.testTransform, movement);
            });
        }
        
        [Test]
        public void Setup_ThrowsArgumentNullException_WhenMovementIsBelowZeroTest()
        {
            // Arrange
            int movement = -1;

            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.testClass.Setup(this.testTransform, movement);
            });
        }
        
        [Test]
        public void MoveActor_DoesNotThrowException_WhenSetupCorrectlyTest()
        {
            // Arrange
            this.testClass.Setup(this.testTransform, movement: 1);
            
            // Act Assert
            this.testClass.MoveActor(Direction.Down);
        }

        [Test]
        public void MoveActor_DecreasesYValue_WhenDirectionIsDownTest(
            [NUnit.Framework.Range(1, 10, 1)] int unit)
        {
            // Arrange
            this.testClass.Setup(this.testTransform, movement: unit);
            
            // Act
            this.testClass.MoveActor(Direction.Down);
            
            // Assert
            Assert.AreEqual(-unit, this.testTransform.position.y);
        }
        
        [Test]
        public void MoveActor_DecreasesYValueMultipleTimes_WhenDirectionIsDownMultipleTimesTest(
            [NUnit.Framework.Range(1, 10, 1)] int unit)
        {
            // Arrange
            int expected = unit * -3;
            this.testClass.Setup(this.testTransform, movement: unit);
            
            // Act
            this.testClass.MoveActor(Direction.Down);
            this.testClass.MoveActor(Direction.Down);
            this.testClass.MoveActor(Direction.Down);

            // Assert
            Assert.AreEqual(expected, this.testTransform.position.y);
        }
        
        [Test]
        public void MoveActor_IncreasesYValue_WhenDirectionIsUpTest(
            [NUnit.Framework.Range(1, 10, 1)] int unit)
        {
            // Arrange
            this.testClass.Setup(this.testTransform, movement: unit);
            
            // Act
            this.testClass.MoveActor(Direction.Up);
            
            // Assert
            Assert.AreEqual(unit, this.testTransform.position.y);
        }
        
        [Test]
        public void MoveActor_IncreasesYValueMultipleTimes_WhenDirectionIsUpMultipleTimesTest(
            [NUnit.Framework.Range(1, 10, 1)] int unit)
        {
            // Arrange
            int expected = unit * 3;
            this.testClass.Setup(this.testTransform, movement: unit);
            
            // Act
            this.testClass.MoveActor(Direction.Up);
            this.testClass.MoveActor(Direction.Up);
            this.testClass.MoveActor(Direction.Up);

            // Assert
            Assert.AreEqual(expected, this.testTransform.position.y);
        }
        
        [Test]
        public void MoveActor_DecreaseXValue_WhenDirectionIsUpTest(
            [NUnit.Framework.Range(1, 10, 1)] int unit)
        {
            // Arrange
            this.testClass.Setup(this.testTransform, movement: unit);
            
            // Act
            this.testClass.MoveActor(Direction.Left);
            
            // Assert
            Assert.AreEqual(-unit, this.testTransform.position.x);
        }
        
        [Test]
        public void MoveActor_DecreaseXValueMultipleTimes_WhenDirectionIsLeftMultipleTimesTest(
            [NUnit.Framework.Range(1, 10, 1)] int unit)
        {
            // Arrange
            int expected = unit * -3;
            this.testClass.Setup(this.testTransform, movement: unit);
            
            // Act
            this.testClass.MoveActor(Direction.Left);
            this.testClass.MoveActor(Direction.Left);
            this.testClass.MoveActor(Direction.Left);

            // Assert
            Assert.AreEqual(expected, this.testTransform.position.x);
        }
        
        [Test]
        public void MoveActor_IncreaseXValue_WhenDirectionIsRightTest(
            [NUnit.Framework.Range(1, 10, 1)] int unit)
        {
            // Arrange
            this.testClass.Setup(this.testTransform, movement: unit);
            
            // Act
            this.testClass.MoveActor(Direction.Right);
            
            // Assert
            Assert.AreEqual(unit, this.testTransform.position.x);
        }
        
        [Test]
        public void MoveActor_IncreaseXValueMultipleTimes_WhenDirectionIsRightMultipleTimesTest(
            [NUnit.Framework.Range(1, 10, 1)] int unit)
        {
            // Arrange
            int expected = unit * 3;
            this.testClass.Setup(this.testTransform, movement: unit);
            
            // Act
            this.testClass.MoveActor(Direction.Right);
            this.testClass.MoveActor(Direction.Right);
            this.testClass.MoveActor(Direction.Right);

            // Assert
            Assert.AreEqual(expected, this.testTransform.position.x);
        }
        
        [Test]
        public void MoveActor_DoesNotAffectZ_WhenMovingInAnyDirectionTest()
        {
            // Arrange
            int expectedZ = 4;
            this.testClass.Setup(this.testTransform, movement: 1);
            this.testTransform.position = new Vector3(2, 3, expectedZ);
            
            // Act
            this.testClass.MoveActor(Direction.Up);
            Vector3 actualUp = CopyVector3(this.testTransform.position);
            this.testClass.MoveActor(Direction.Left);
            Vector3 actualLeft = CopyVector3(this.testTransform.position);
            this.testClass.MoveActor(Direction.Right);
            Vector3 actualRight = CopyVector3(this.testTransform.position);
            this.testClass.MoveActor(Direction.Down);
            Vector3 actualDown = CopyVector3(this.testTransform.position);

            // Assert
            Assert.AreEqual(expectedZ, actualUp.z);
            Assert.AreEqual(expectedZ, actualLeft.z);
            Assert.AreEqual(expectedZ, actualRight.z);
            Assert.AreEqual(expectedZ, actualDown.z);
        }

        /// <summary>
        /// Ensures the C# object referencing does not cloud testing.
        /// </summary>
        /// <param name="original">Original vector to copy. </param>
        /// <returns>Copied Vector3. </returns>
        private Vector3 CopyVector3(Vector3 original)
        {
            return new Vector3(original.x, original.y, original.z);
        }
    }
}