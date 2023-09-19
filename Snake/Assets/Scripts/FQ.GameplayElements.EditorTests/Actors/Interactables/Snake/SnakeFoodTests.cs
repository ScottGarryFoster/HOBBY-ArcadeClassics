using FQ.GameElementCommunication;
using FQ.Libraries.Randomness;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeFoodTests
    {
        private const string NotValidPlayerTag = "SnakeFood";
        private const string ValidPlayerTag = "Player";
        private const string SnakeFoodTag = "SnakeFood";
        private const string NotSnakeFoodTag = "Untagged";
        
        private GameObject gameObject;
        private TestSnakeFood testClass;
        private Mock<IRandom> mockRandomNumbers;
        private Mock<IWorldInfoFromTilemapFinder> mockWorldInfoFromTilemapFinder;
        private Mock<IWorldInfoFromTilemap> mockWorldInfoFromTilemap;
        private Mock<IElementCommunicationFinder> mockElementCommunicationFinder;
        
        [SetUp]
        public void Setup()
        {
            this.gameObject = new GameObject();
            this.testClass = this.gameObject.AddComponent<TestSnakeFood>();

            this.mockRandomNumbers = new Mock<IRandom>();
            this.mockWorldInfoFromTilemapFinder = new Mock<IWorldInfoFromTilemapFinder>();
            this.mockWorldInfoFromTilemap = new Mock<IWorldInfoFromTilemap>();
            this.mockWorldInfoFromTilemapFinder.Setup(x => x.FindWorldInfo()).Returns(mockWorldInfoFromTilemap.Object);
            this.mockElementCommunicationFinder = new Mock<IElementCommunicationFinder>();
            
            this.testClass.SetRandomGenerator(this.mockRandomNumbers.Object);
            this.testClass.SetWorldInfoFromTilemapFinder(this.mockWorldInfoFromTilemapFinder.Object);
            this.testClass.SetElementCommunicationFinder(this.mockElementCommunicationFinder.Object);
            
            Vector3Int[] area = new[] {new Vector3Int(0, 1)};
            this.mockRandomNumbers.Setup(x => x.Range(0, 1)).Returns(0);
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);
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
            player.tag = ValidPlayerTag;
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
            player.tag = ValidPlayerTag;
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
        
        [Test]
        public void BaseFixedUpdateToMovePlayer_DoesNotMoveFood_WhenOnTriggerEnter2DIsCalledWithoutPlayerTest()
        {
            // Arrange
            this.testClass.PublicStart();
            Vector3 expectedPosition = CopyVector(this.testClass.gameObject.transform.position);

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = NotValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expectedPosition, actual);
        }
        
        [Test]
        public void BaseFixedUpdateToMovePlayer_SetsTagToSnakeFood_WhenBaseFixedUpdateToMovePlayerIsCalledAfterCollisionWithPlayerTest()
        {
            // Arrange
            string expectedTag = SnakeFoodTag;
            this.testClass.gameObject.tag = NotSnakeFoodTag;
            this.testClass.PublicStart();

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = ValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expectedTag, this.testClass.gameObject.tag);
        }
        
        [Test]
        public void BaseFixedUpdateToMovePlayer_MovesToRandomLocationInSafeArea_WhenCollidedWithPlayerTest()
        {
            // Arrange
            Vector3Int expected = new Vector3Int(5, 6);
            Vector3Int[] area = new[] {new Vector3Int(1, 2), expected, new Vector3Int(3, 4)};
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);
            this.mockRandomNumbers.Setup(x => x.Range(0, area.Length)).Returns(1);
            
            this.testClass.PublicStart();

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = ValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expected.x, (int)actual.x);
            Assert.AreEqual(expected.y, (int)actual.y);
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

            public void SetRandomGenerator(IRandom newValue)
            {
                this.randomGenerator = newValue;
            }

            public void SetWorldInfoFromTilemapFinder(IWorldInfoFromTilemapFinder newValue)
            {
                this.worldInfoFromTilemapFinder = newValue;
            }

            public void SetElementCommunicationFinder(IElementCommunicationFinder newValue)
            {
                this.elementCommunicationFinder = newValue;
            }
        }
        
        
    }
}