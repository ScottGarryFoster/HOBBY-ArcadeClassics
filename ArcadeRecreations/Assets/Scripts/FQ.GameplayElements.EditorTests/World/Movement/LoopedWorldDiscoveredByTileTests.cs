using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.GameplayElements.EditorTests
{
    public class LoopedWorldDiscoveredByTileTests
    {
        private LoopedWorldDiscoveredByTile testClass;

        [SetUp]
        public void Setup()
        {
            this.testClass = new LoopedWorldDiscoveredByTile();
        }
        
        [Test]
        public void CalculateLoops_ReturnsFalse_WhenGivenTilemapIsNullTest()
        {
            // Arrange
            Tilemap testTilemap = null;
            
            // Act
            bool actual = this.testClass.CalculateLoops(null, out _);
            
            // Assert
            Assert.IsFalse(actual);
        }
        
        

        public Tilemap GetTestBorderTileMap()
        {
            Tilemap returnTilemap = null;
            
            GameObject borderObject = GetTestBorderObject();
            if (borderObject != null)
            {
                returnTilemap = borderObject.GetComponent<Tilemap>();
            }

            return returnTilemap;
        }

        private GameObject GetTestBorderObject()
        {
            var gridGameObject = Resources.Load<GameObject>("TestResources/World/TestGrid");
            for (int i = 0; i < gridGameObject.transform.childCount; ++i)
            {
                Transform child = gridGameObject.transform.GetChild(i);
                if (child.name == "Border")
                {
                    return child.gameObject;
                }
            }

            return null;
        }
    }
}