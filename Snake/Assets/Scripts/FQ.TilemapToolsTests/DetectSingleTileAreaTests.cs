using System;
using FQ.TilemapTools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.TilemapToolsTests
{
    public class DetectSingleTileAreaTests
    {
        private IDetectSingleTileArea testClass;
        
        [SetUp]
        public void Setup()
        {
            this.testClass = new DetectSingleTileArea();
        }

        [TearDown]
        public void Teardown()
        {
            
        }

        [Test]
        public void DetectTilesOfTheSameTypeTouching_ThrowsNullArgumentException_WhenNullTilemapIsGivenTest()
        {
            // Arrange
            Tilemap given = null;
            TileBase tilebase = Resources.Load<TileBase>("Editor/LoopVisualiser/ArrowTiles/TileArrows_0");
            Vector3Int location = new();
            
            // Act
            bool didThrow = false;
            try
            {
                this.testClass.DetectTilesOfTheSameTypeTouching(given, tilebase, location);
            }
            catch (ArgumentException)
            {
                didThrow = true;
            }

            // Assert
            Assert.IsTrue(didThrow, "Did not throw.");
        }
        
        [Test]
        public void DetectTilesOfTheSameTypeTouching_DoesNotThrowsNullArgumentException_WhenNullTileBaseIsGivenTest()
        {
            // Arrange
            Tilemap tilemap = Resources.Load<Tilemap>("TestResources/World/FoodArea/FoodTestMap1");
            TileBase tilebase = null;
            Vector3Int location = new();
            
            // Act
            bool didThrow = false;
            try
            {
                this.testClass.DetectTilesOfTheSameTypeTouching(tilemap, tilebase, location);
            }
            catch (ArgumentException)
            {
                didThrow = true;
            }

            // Assert
            Assert.IsFalse(didThrow, "Threw Exception.");
        }

        [Test]
        public void DetectTilesOfTheSameTypeTouching_ReturnsEmptyArray_WhenGivenTileIsNotEqualToGivenLocationTest()
        {
            // Arrange
            GameObject go = Resources.Load<GameObject>("TestResources/World/FoodArea/FoodTestMap1");
            Tilemap tilemap = go.GetComponent<Tilemap>();
            TileBase tileBase = Resources.Load<TileBase>("Editor/LoopVisualiser/ArrowTiles/TileArrows_0");
            Vector3Int location = new();

            string tilenameOnMap = GetTileNameFromTilemap(tilemap, location);
            Assert.AreNotEqual(tilenameOnMap, tileBase.name,
                "Tile already equals location");
            
            // Act
            Vector3Int[] actual = this.testClass.DetectTilesOfTheSameTypeTouching(tilemap, tileBase, location);

            // Assert
            Assert.Zero(actual.Length);
        }
        
        [Test]
        public void DetectTilesOfTheSameTypeTouching_ReturnsNineLocations_WhenGivenEmptyNameInTopRightSectionTest()
        {
            // Arrange
            GameObject go = Resources.Load<GameObject>("TestResources/World/FoodArea/FoodTestMap1");
            Tilemap tilemap = go.GetComponent<Tilemap>();
            TileBase tileBase = null;
            Vector3Int location = new();

            // Act
            Vector3Int[] actual = this.testClass.DetectTilesOfTheSameTypeTouching(tilemap, tileBase, location);

            // Assert
            Assert.AreEqual(9, actual.Length);
        }
        
        [Test]
        public void DetectTilesOfTheSameTypeTouching_ReturnsTopRightLocations_WhenGivenEmptyNameInTopRightSectionTest()
        {
            // Arrange
            GameObject go = Resources.Load<GameObject>("TestResources/World/FoodArea/FoodTestMap1");
            Tilemap tilemap = go.GetComponent<Tilemap>();
            TileBase tileBase = null;
            Vector3Int location = new(x: 0, y: -1);

            Vector3Int[] expected = new[]
            {
                new Vector3Int(-1, -2),
                new Vector3Int(-1, -1),
                new Vector3Int(-1, 0),

                new Vector3Int(0, -2),
                new Vector3Int(0, -1),
                new Vector3Int(0, 0),

                new Vector3Int(1, -2),
                new Vector3Int(1, -1),
                new Vector3Int(1, 0),
            };
            
            // Act
            Vector3Int[] actual = this.testClass.DetectTilesOfTheSameTypeTouching(tilemap, tileBase, location);

            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }
        
        [Test]
        public void DetectTilesOfTheSameTypeTouching_ReturnsLargerArea_WhenPlacedInLargerAreaInTestMapTest()
        {
            // Arrange
            GameObject go = Resources.Load<GameObject>("TestResources/World/FoodArea/FoodTestMap1");
            Tilemap tilemap = go.GetComponent<Tilemap>();
            TileBase tileBase = null;
            Vector3Int location = new(x: -6, y: -2);

            Vector3Int[] expected = new[]
            {
                new Vector3Int(-8, -8),
                new Vector3Int(-8, -7),
                new Vector3Int(-8, -6),
                new Vector3Int(-8, -5),
                new Vector3Int(-8, -4),
                new Vector3Int(-8, -3),
                new Vector3Int(-8, -2),
                new Vector3Int(-8, -1),
                new Vector3Int(-8, 0),
                
                new Vector3Int(-7, -8),
                new Vector3Int(-7, -7),
                new Vector3Int(-7, -6),
                new Vector3Int(-7, -5),
                new Vector3Int(-7, -4),
                new Vector3Int(-7, -3),
                new Vector3Int(-7, -2),
                new Vector3Int(-7, -1),
                new Vector3Int(-7, 0),
                
                new Vector3Int(-6, -8),
                new Vector3Int(-6, -7),
                new Vector3Int(-6, -3),
                new Vector3Int(-6, -2),
                new Vector3Int(-6, -1),
                new Vector3Int(-6, 0),
                
                new Vector3Int(-5, -8),
                new Vector3Int(-5, -7),
                new Vector3Int(-5, -3),
                new Vector3Int(-5, -2),
                new Vector3Int(-5, -1),
                new Vector3Int(-5, 0),
                
                new Vector3Int(-4, -8),
                new Vector3Int(-4, -7),
                new Vector3Int(-4, -3),
                new Vector3Int(-4, -2),
                new Vector3Int(-4, -1),
                new Vector3Int(-4, 0),
                
                new Vector3Int(-3, -8),
                new Vector3Int(-3, -7),
                new Vector3Int(-3, -6),
                new Vector3Int(-3, -5),
                new Vector3Int(-3, -4),
                new Vector3Int(-3, -3),
                new Vector3Int(-3, -2),
                new Vector3Int(-3, -1),
                new Vector3Int(-3, 0),
                
                new Vector3Int(-2, -8),
                new Vector3Int(-2, -7),
                new Vector3Int(-2, -6),
                new Vector3Int(-2, -5),
                new Vector3Int(-2, -4),
                
                new Vector3Int(-1, -8),
                new Vector3Int(-1, -5),
                new Vector3Int(-1, -4),
                
                new Vector3Int(0, -8),
                new Vector3Int(0, -5),
                new Vector3Int(0, -4),
                
                new Vector3Int(1, -8),
                new Vector3Int(1, -7),
                new Vector3Int(1, -6),
                new Vector3Int(1, -5),
                new Vector3Int(1, -4),
            };
            
            // Act
            Vector3Int[] actual = this.testClass.DetectTilesOfTheSameTypeTouching(tilemap, tileBase, location);

            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        private string GetTileNameFromTilemap(Tilemap tilemap, Vector3Int location)
        {
            string returnName = "";
            TileBase tileBase = tilemap.GetTile<TileBase>(location);
            if (tileBase != null)
            {
                returnName = tileBase.name;
            }

            return returnName;
        }
    }
}