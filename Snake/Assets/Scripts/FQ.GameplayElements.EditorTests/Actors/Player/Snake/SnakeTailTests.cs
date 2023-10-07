using FQ.Libraries.StandardTypes;
using NUnit.Framework;
using UnityEngine;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeTailTests
    {
        private GameObject owningObject;
        private SnakeTail testClass;

        [SetUp]
        public void Setup()
        {
            this.owningObject = new GameObject("Snake Tail");
            this.testClass = this.owningObject.AddComponent<SnakeTail>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(this.owningObject);
        }
        
        [Test]
        public void TailPieceAfterBorder_IsFalse_ByDefaultTest()
        {
            // Arrange
            
            // Act
            bool actual = this.testClass.TailPieceAfterBorder;

            // Assert
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void TailPieceAfterBorder_IsTrue_AfterGivenNewBorderInfoTest()
        {
            // Arrange
            CollisionPositionAnswer given = new()
            {
                Answer = ContextToPositionAnswer.NewPositionIsCorrect,
            };
            this.testClass.GiveNewBorderInfo(given);
            
            // Act
            bool actual = this.testClass.TailPieceAfterBorder;

            // Assert
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void TailPieceAfterBorder_IsFalse_AfterGivenNewBorderInfoButItIsNoValidMovementTest()
        {
            // Arrange
            CollisionPositionAnswer given = new()
            {
                Answer = ContextToPositionAnswer.NoValidMovement,
            };
            this.testClass.GiveNewBorderInfo(given);
            
            // Act
            bool actual = this.testClass.TailPieceAfterBorder;

            // Assert
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void CollisionPositionAnswer_IsGiven_AfterGiveNewBorderInfoCalledWithValidMovementTest()
        {
            // Arrange
            CollisionPositionAnswer given = new()
            {
                Answer = ContextToPositionAnswer.NewPositionIsCorrect,
                NewPosition = new(4, 10),
                NewDirection = MovementDirection.Up,
            };
            this.testClass.GiveNewBorderInfo(given);
            
            // Act
            CollisionPositionAnswer actual = this.testClass.BorderLoopAnswer;

            // Assert
            Assert.AreEqual(given, actual, "Answers are not equal.");
        }
        
        [Test]
        public void CollisionPositionAnswer_RemainsTheSame_AfterGivenNewBorderInfoButItIsNoValidMovementTest()
        {
            // Arrange
            CollisionPositionAnswer expected = new()
            {
                Answer = ContextToPositionAnswer.NewPositionIsCorrect,
                NewPosition = new(4, 10),
                NewDirection = MovementDirection.Up,
            };
            this.testClass.GiveNewBorderInfo(expected);
            
            CollisionPositionAnswer notExpected = new()
            {
                Answer = ContextToPositionAnswer.NoValidMovement,
                NewPosition = new(2, 4),
                NewDirection = MovementDirection.Left,
            };
            this.testClass.GiveNewBorderInfo(notExpected);
            
            // Act
            CollisionPositionAnswer actual = this.testClass.BorderLoopAnswer;

            // Assert
            Assert.AreEqual(expected, actual, "Answers are not equal.");
        }
        
        [Test]
        public void ResetMetaInfo_ResetsTailPieceAfterBorderToFalse_WhenResetMetaInfoIsCalledAfterInfoGivenTest()
        {
            // Arrange
            CollisionPositionAnswer validBorderGiven = new()
            {
                Answer = ContextToPositionAnswer.NewPositionIsCorrect,
                NewPosition = new(4, 10),
                NewDirection = MovementDirection.Up,
            };
            this.testClass.GiveNewBorderInfo(validBorderGiven);
            bool isBorder = this.testClass.TailPieceAfterBorder;
            
            Assert.IsTrue(isBorder, "Original border state is not true.");
            
            // Act
            this.testClass.ResetMetaInfo();

            // Assert
            bool actual = this.testClass.TailPieceAfterBorder;
            Assert.IsFalse(actual);
        }
    }
}