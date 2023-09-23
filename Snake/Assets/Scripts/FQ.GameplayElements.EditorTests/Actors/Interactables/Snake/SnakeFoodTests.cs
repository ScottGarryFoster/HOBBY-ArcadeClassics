using System;
using System.Collections.Generic;
using FQ.GameElementCommunication;
using FQ.Libraries.Randomness;
using FQ.Logger;
using FQ.OverallControllers;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
        private Mock<IElementCommunication> mockElementCommunication;
        private Mock<IPlayerStatus> mockPlayerStatus;

        /// <summary>
        /// Random by default works like this:
        /// It will provide a different answer each time upto three times.
        /// Then it will repeat.
        /// If you want to know the answer re-mock it. The default is only to avoid
        /// clutter in the tests.
        /// </summary>
        private int randomCounter = -1;
        
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
            
            Vector3Int[] area = new[] {new Vector3Int(0, 1), new Vector3Int(1, 2), new Vector3Int(2, 3)};
            this.mockRandomNumbers.Setup(x => x.Range(0, It.IsAny<int>())).Returns<int, int>((minInclusive, maxExclusive) =>
            {
                if (++randomCounter >= area.Length)
                {
                    randomCounter = 0;
                }

                return randomCounter;
            });
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);

            this.mockElementCommunication = new Mock<IElementCommunication>();
            this.mockElementCommunicationFinder.Setup(x => x.FindElementCommunication())
                .Returns(mockElementCommunication.Object);

            this.mockPlayerStatus = new Mock<IPlayerStatus>();
            this.mockElementCommunication.Setup(x => x.PlayerStatus).Returns(this.mockPlayerStatus.Object);

            var locationNotIntersecting = Array.Empty<Vector2Int>();
            this.mockPlayerStatus.Setup(x => x.PlayerLocation).Returns(locationNotIntersecting);
            
            Log.TestMode();
        }

        [TearDown]
        public void Teardown()
        {
        }
        
        #region Handles Bad Input
        
        [Test, Timeout(1000)]
        public void PublicUpdate_HandlesUpdate_WhenWorldInfoFromTilemapFinderIsNullTest()
        {
            // Arrange
            this.testClass.SetWorldInfoFromTilemapFinder(null);
            this.testClass.PublicStart();

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = ValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();
        }
        
        [Test, Timeout(1000)]
        public void PublicUpdate_HandlesUpdate_WhenNoTravelableAreaFoundTest()
        {
            // Arrange
            Vector3Int[] given = null;
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(given);
            
            this.testClass.PublicStart();

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = ValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();
        }
        
        [Test, Timeout(1000)]
        public void PublicUpdate_HandlesUpdate_WhenNoWorldInfoIsFoundTest()
        {
            // Arrange
            IWorldInfoFromTilemap given = null;
            this.mockWorldInfoFromTilemapFinder.Setup(x => x.FindWorldInfo()).Returns(given);
            
            this.testClass.PublicStart();

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = ValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();
        }
        
        [Test, Timeout(1000)]
        public void BaseFixedUpdateToMovePlayer_HandlesUpdate_WhenElementCommunicationFinderIsNotFoundTest()
        {
            // Arrange
            IElementCommunicationFinder given = null;
            this.testClass.SetElementCommunicationFinder(given);

            this.testClass.PublicStart();
            
            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = ValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();
        }
        
        [Test, Timeout(1000)]
        public void BaseFixedUpdateToMovePlayer_HandlesNull_WhenElementCommunicationIsNullTest()
        {
            // Arrange
            IElementCommunication given = null;
            this.mockElementCommunicationFinder.Setup(x => x.FindElementCommunication()).Returns(given);

            this.testClass.PublicStart();
            
            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = ValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();
        }
        
        /// <summary>
        /// Note this tests both Update and Start as the Expected Position is
        /// before Start, meaning if the position altered in Start this would fail.
        /// </summary>
        [Test, Timeout(1000)]
        public void BaseFixedUpdateToMovePlayer_DoesMoveFood_WhenSafeAreaHasNoEntriesTest()
        {
            // Arrange
            Vector3Int[] area = new List<Vector3Int>().ToArray();
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);
            
            Vector3 expectedPosition = CopyVector(this.testClass.gameObject.transform.position);
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
            Assert.AreEqual(expectedPosition.x, actual.x);
            Assert.AreEqual(expectedPosition.y, actual.y);
        }

        #endregion
        
        #region Update Reactions
        
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
            Vector3 actual = CopyVector(this.testClass.gameObject.transform.position);
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
        
        [Test]
        public void BaseFixedUpdateToMovePlayer_DoesMoveFood_WhenNoSafeAreaFoundTest()
        {
            // Arrange
            Vector3Int[] area = null;
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);
            
            Vector3 expectedPosition = CopyVector(this.testClass.gameObject.transform.position);
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
            Assert.AreEqual(expectedPosition.x, actual.x);
            Assert.AreEqual(expectedPosition.y, actual.y);
        }
        
        [Test, Timeout(1000)]
        public void BaseFixedUpdateToMovePlayer_UsesSecondRandomLocation_WhenFirstLocationIsWherePlayerIsTest()
        {
            // Arrange
            Vector3Int expected = new Vector3Int(3, 4);
            Vector3Int positionOne = new Vector3Int(7, 9);
            Vector3Int[] area = new[] {new Vector3Int(1, 2), positionOne, expected};
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);
            
            // Start will mess up the randomness of all of this let it run first.
            this.testClass.PublicStart();
            
            int[] given = {1, 2};
            int current = 0;
            this.mockRandomNumbers.Setup(x => x.Range(0, area.Length))
                .Returns<int, int>((minInclusive, maxExclusive) => given[current++]);
            
            // Put player in the location:
            Vector2Int[] playerLocation = new[] {new Vector2Int(positionOne.x, positionOne.y)};
            this.mockPlayerStatus.Setup(x => x.PlayerLocation).Returns(playerLocation);

            // Simulate player colliding
            GameObject player = new("Player");
            player.tag = ValidPlayerTag;
            Collider2D collider = player.AddComponent<BoxCollider2D>();
            this.testClass.PublicOnTriggerEnter2D(collider);

            // Act
            this.testClass.PublicUpdate();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expected.x, actual.x);
            Assert.AreEqual(expected.y, actual.y);
        }
        
        #endregion
        
        #region Start Reactions
        
        [Test]
        public void Start_MovesFoodToNewPosition_WhenCalledWithPositionsTest()
        {
            // Arrange
            Vector3Int expectedPositionInt = new Vector3Int(10, 40);
            Vector3Int[] area = new[] {expectedPositionInt};
            this.mockRandomNumbers.Setup(x => x.Range(0, 1)).Returns(0);
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);
            
            // Act
            this.testClass.PublicStart();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Vector3 expected = expectedPositionInt;
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void Start_MovesToRandomLocationInSafeArea_WhenStartedTest()
        {
            // Arrange
            Vector3Int expected = new Vector3Int(5, 6);
            Vector3Int[] area = new[] {new Vector3Int(1, 2), expected, new Vector3Int(3, 4)};
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);
            this.mockRandomNumbers.Setup(x => x.Range(0, area.Length)).Returns(1);
            
            // Act
            this.testClass.PublicStart();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expected.x, (int)actual.x);
            Assert.AreEqual(expected.y, (int)actual.y);
        }
        
        [Test]
        public void Start_DoesMoveFood_WhenNoSafeAreaFoundTest()
        {
            // Arrange
            Vector3Int[] area = null;
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);
            
            Vector3 expectedPosition = CopyVector(this.testClass.gameObject.transform.position);

            // Act
            this.testClass.PublicStart();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expectedPosition.x, actual.x);
            Assert.AreEqual(expectedPosition.y, actual.y);
        }
        
        [Test, Timeout(1000)]
        public void Start_UsesSecondRandomLocation_WhenFirstLocationIsWherePlayerIsTest()
        {
            // Arrange
            Vector3Int expected = new Vector3Int(3, 4);
            Vector3Int positionOne = new Vector3Int(7, 9);
            Vector3Int[] area = new[] {new Vector3Int(1, 2), positionOne, expected};
            this.mockWorldInfoFromTilemap.Setup(x => x.GetTravelableArea()).Returns(area);

            int[] given = {1, 2};
            int current = 0;
            this.mockRandomNumbers.Setup(x => x.Range(0, area.Length))
                .Returns<int, int>((minInclusive, maxExclusive) => given[current++]);
            
            // Put player in the location:
            Vector2Int[] playerLocation = new[] {new Vector2Int(positionOne.x, positionOne.y)};
            this.mockPlayerStatus.Setup(x => x.PlayerLocation).Returns(playerLocation);

            // Act
            this.testClass.PublicStart();

            // Assert
            Vector3 actual = this.testClass.gameObject.transform.position;
            Assert.AreEqual(expected.x, actual.x);
            Assert.AreEqual(expected.y, actual.y);
        }
        
        #endregion

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
                this.RandomGenerator = newValue;
            }

            public void SetWorldInfoFromTilemapFinder(IWorldInfoFromTilemapFinder newValue)
            {
                this.WorldInfoFromTilemapFinder = newValue;
            }

            public void SetElementCommunicationFinder(IElementCommunicationFinder newValue)
            {
                this.ElementCommunicationFinder = newValue;
            }
        }
    }
}