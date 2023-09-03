using NUnit.Framework;
using UnityEngine;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeFoodTests
    {
        private GameObject gameObject;
        private TestSnakeFood testClass;
        
        [SetUp]
        public void Setup()
        {
            this.gameObject = new GameObject();
            this.testClass = this.gameObject.AddComponent<TestSnakeFood>();

        }

        [TearDown]
        public void Teardown()
        {
            
        }

        [Test]
        public void BaseFixedUpdate_DoesNothing_WhenNothingCollidesTest()
        {
            // Arrange
            this.testClass.PublicStart();
            Vector3 expectedPosition = CopyVector(this.testClass.gameObject.transform.position);

            // Act
            this.testClass.PublicUpdate();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expectedPosition, actual);
        }

        [Test]
        public void BaseFixedUpdateToMovePlayer_MoveFood_WhenOnTriggerEnter2DIsCalledWithPlayerTest()
        {
            // Arrange
            this.testClass.PublicStart();
            Vector3 expectedPosition = CopyVector(this.testClass.gameObject.transform.position);

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = "Player";
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreNotEqual(expectedPosition, actual);
        }
        
        [Test]
        public void BaseFixedUpdateToMovePlayer_DoesNotMoveFood_WhenBaseFixedUpdateToMovePlayerIsCalledTwiceAfterCollisionWithPlayerTest()
        {
            // Arrange
            this.testClass.PublicStart();

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = "Player";
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);
            
            // The test above confirms this should give a new location
            this.testClass.PublicUpdate();
            Vector3 expectedPosition = CopyVector(this.testClass.gameObject.transform.position);

            // Act
            this.testClass.PublicUpdate();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expectedPosition, actual);
        }

        private Vector3 CopyVector(Vector3 transformPosition)
        {
            return new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
        }

        /// <summary>
        /// Exposes protected methods so you may call MonoBehaviour methods.
        /// </summary>
        private class TestSnakeFood : SnakeFood
        {
            public void PublicStart()
            {
                BaseStart();
            }
            
            public void PublicUpdate()
            {
                BaseFixedUpdate();
            }
            
            public void PublicOnTriggerEnter2D(Collider2D other)
            {
                BaseOnTriggerEnter2D(other);
            }
        }
    }
}