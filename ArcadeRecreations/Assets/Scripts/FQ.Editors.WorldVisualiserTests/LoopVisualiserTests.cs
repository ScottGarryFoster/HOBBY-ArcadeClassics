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
        private const string BorderTileLocation = "TestResources/World/TestBasicChecker-A/TestBasicChecker-A-Tile";
        
        private ILoopVisualiser testClass;
        
        [SetUp]
        public void Setup()
        {
            this.testClass = new LoopVisualiser();
        }
        
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
                //this.testClass.AddVisualisationObject(givenPrefab, givenTilemap, provider.Object);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(ArgumentNullException)}");
        }
        
        [Test]
        public void AddVisualisationObject_ThrowsArgumentNullException_WhenGivenTilemapIsNullTest()
        {
            // Arrange
            GameObject givenPrefab = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            Tilemap givenTilemap = null;
            var provider = new Mock<IArrowTileProvider>();
            
            // Act
            bool didThrow = false;
            try
            {
                //this.testClass.AddVisualisationObject(givenPrefab, givenTilemap, provider.Object);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(ArgumentNullException)}");
        }
        
        [Test]
        public void AddVisualisationObject_ThrowsArgumentNullException_WhenArrowProviderIsNullTest()
        {
            // Arrange
            GameObject givenPrefab = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            Tilemap givenTilemap = GetTestBorderTileMap("TestResources/World/TestGrid");
            IArrowTileProvider provider = null;
            
            // Act
            bool didThrow = false;
            try
            {
                //this.testClass.AddVisualisationObject(givenPrefab, givenTilemap, provider);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(ArgumentNullException)}");
        }
        
        [Test]
        public void AddVisualisationObject_ThrowsArgumentNullException_WhenPrefabGivenIsNullTest2()
        {
            // Arrange
            GameObject givenPrefab = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            Tilemap givenTilemap = GetTestBorderTileMap("TestResources/World/TestGrid");
            var givenBorderTile = Resources.Load<Tile>(BorderTileLocation);
            //var provider = new Mock<IArrowTileProvider>();
            var provider = new SimpleArrowTileProvider("Editor/LoopVisualiser/ArrowTiles/TileArrows_"); 
            
            // Act
            GameObject actual = this.testClass.AddVisualisationObject(
                givenPrefab, givenTilemap, givenBorderTile, provider);

            // Assert
            Assert.IsNotNull(actual, "No created object.");
            Tilemap extractedActualTilemap = ExtractTilemap(actual);
            Assert.IsNotNull(extractedActualTilemap, "No tilemap in created object.");
            TileBase topLeft = GetTopLeftTile(extractedActualTilemap);
            Assert.IsNotNull(topLeft, "No Visual.");
            Assert.Fail($"{topLeft.name}"); 
        }

        private TileBase GetTopLeftTile(Tilemap search)
        {
            for (int x = search.origin.x; x < search.size.x; ++x)
            {
                for (int y = search.origin.y; y < search.size.y; ++y)
                {
                    TileBase tilebase = search.GetTile(new Vector3Int(x, y));
                    if (tilebase != null)
                    {
                        return tilebase;
                    }
                }
            }

            return null;
        }
        
        #region Helper Methods
        
        private Tilemap GetTestBorderTileMap(string map)
        {
            Tilemap returnTilemap = null;
            var gridGameObject = Resources.Load<GameObject>(map);
            GameObject borderObject = GetTestBorderObject(gridGameObject);
            if (borderObject != null)
            {
                returnTilemap = borderObject.GetComponent<Tilemap>();
            }

            return returnTilemap;
        }
        
        private Tilemap ExtractTilemap(GameObject map)
        {
            Tilemap returnTilemap = null;
            
            GameObject borderObject = GetTestBorderObject(map);
            if (borderObject != null)
            {
                returnTilemap = borderObject.GetComponent<Tilemap>();
            }

            return returnTilemap;
        }

        private GameObject GetTestBorderObject(GameObject gridGameObject)
        {
            for (int i = 0; i < gridGameObject.transform.childCount; ++i)
            {
                Transform child = gridGameObject.transform.GetChild(i);
                if (child.name == "Border")
                {
                    return child.gameObject;
                }
                if (child.name == "Tilemap")
                {
                    return child.gameObject;
                }
            }

            return null;
        }
        
        #endregion
    }
}