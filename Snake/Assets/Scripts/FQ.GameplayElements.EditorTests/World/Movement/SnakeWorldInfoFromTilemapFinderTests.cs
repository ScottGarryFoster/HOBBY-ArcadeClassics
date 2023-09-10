using NUnit.Framework;
using UnityEngine;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeWorldInfoFromTilemapFinderTests
    {
        [Test]
        public void FindWorldInfo_ReturnsNull_WhenNoGameObjectWithSnakeBorderTagIsInSceneTest()
        {
            // Arrange
            DestroyAllBorderObjects();
            IWorldInfoFromTilemap expected = null;

            IWorldInfoFromTilemapFinder testClass = new SnakeWorldInfoFromTilemapFinder();

            // Act
            IWorldInfoFromTilemap actual = testClass.FindWorldInfo();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FindWorldInfo_ReturnsNull_WhenSnakeBorderDoesNoHaveAChildCalledBorderTest()
        {
            // Arrange
            DestroyAllBorderObjects();
            GameObject snakeBorder = new();
            snakeBorder.tag = SnakeWorldInfoFromTilemapFinder.SnakeBorderTag;
            
            IWorldInfoFromTilemap expected = null;

            IWorldInfoFromTilemapFinder testClass = new SnakeWorldInfoFromTilemapFinder();

            // Act
            IWorldInfoFromTilemap actual = testClass.FindWorldInfo();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FindWorldInfo_ReturnsNull_WhenBorderObjectWithinSnakeBorderDoesNotHaveWorldInfoInfoFromTilemapTest()
        {
            // Arrange
            DestroyAllBorderObjects();
            GameObject snakeBorder = new();
            snakeBorder.tag = SnakeWorldInfoFromTilemapFinder.SnakeBorderTag;
            
            GameObject border = new("Border");
            border.transform.parent = snakeBorder.transform;
            
            IWorldInfoFromTilemap expected = null;

            IWorldInfoFromTilemapFinder testClass = new SnakeWorldInfoFromTilemapFinder();

            // Act
            IWorldInfoFromTilemap actual = testClass.FindWorldInfo();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FindWorldInfo_ReturnsWorldInfoInfoFromTilemap_WhenBorderHasTheComponentTest()
        {
            // Arrange
            DestroyAllBorderObjects();
            GameObject snakeBorder = new();
            snakeBorder.tag = SnakeWorldInfoFromTilemapFinder.SnakeBorderTag;
            
            GameObject border = new("Border");
            border.transform.parent = snakeBorder.transform;
            IWorldInfoFromTilemap expected = border.AddComponent<WorldInfoInfoFromTilemap>();

            IWorldInfoFromTilemapFinder testClass = new SnakeWorldInfoFromTilemapFinder();

            // Act
            IWorldInfoFromTilemap actual = testClass.FindWorldInfo();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        private void DestroyAllBorderObjects()
        {
            GameObject[] gameControllers = GameObject
                .FindGameObjectsWithTag(SnakeWorldInfoFromTilemapFinder.SnakeBorderTag);
            foreach (GameObject gameController in gameControllers)
            {
                Object.DestroyImmediate(gameController);
            }
        }
    }
}