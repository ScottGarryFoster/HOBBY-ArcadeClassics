using System;
using FQ.Libraries;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.Editors.WorldVisualiserTests
{
    public class LoopVisualiserTests
    {
        [Test]
        public void AddVisualisationObject_ThrowsArgumentNullException_WhenPrefabGivenIsNullTest()
        {
            // Arrange
            GameObject givenPrefab = null;
            Tilemap givenTilemap = GetTestBorderTileMap("TestResources/World/TestGrid");
            var provider = new Mock<IArrowTileProvider>();
            
            // Act
            bool didThrow = false;
            try
            {
                new LoopVisualiser().AddVisualisationObject(givenPrefab, givenTilemap, provider.Object);
            }
            catch (ArgumentNullException)
            {
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(ArgumentNullException)}");
        }
        
        private Tilemap GetTestBorderTileMap(string map)
        {
            Tilemap returnTilemap = null;
            
            GameObject borderObject = GetTestBorderObject(map);
            if (borderObject != null)
            {
                returnTilemap = borderObject.GetComponent<Tilemap>();
            }

            return returnTilemap;
        }

        private GameObject GetTestBorderObject(string map)
        {
            var gridGameObject = Resources.Load<GameObject>(map);
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