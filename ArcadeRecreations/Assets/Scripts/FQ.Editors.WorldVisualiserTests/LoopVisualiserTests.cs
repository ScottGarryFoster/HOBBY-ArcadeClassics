using System;
using System.Collections.Generic;
using FQ.Libraries;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace FQ.Editors.WorldVisualiserTests
{
    public class LoopVisualiserTests
    {
        private const string BorderTileLocation = "TestResources/World/TestBasicChecker-A/TestBasicChecker-A-Tile";
        
        private ILoopVisualiser testClass;
        
        /// <summary>
        /// Add to this list to tear down these objects after tests.
        /// </summary>
        private List<GameObject> tearDownObjects; 
        
        [SetUp]
        public void Setup()
        {
            this.testClass = new LoopVisualiser();
            this.tearDownObjects = new List<GameObject>();
        }

        [TearDown]
        public void Teardown()
        {
            foreach (var obj in tearDownObjects)
            {
                Object.DestroyImmediate(obj);
            }
        }
        
        [Test]
        public void AddVisualisationObject_ThrowsArgumentNullException_WhenPrefabGivenIsNullTest()
        {
            // Arrange
            GameObject givenPrefab = null;
            Tilemap givenTilemap = GetTestBorderTileMap("TestResources/World/TestGrid");
            var givenBorderTile = Resources.Load<Tile>(BorderTileLocation);
            var provider = new Mock<IArrowTileProvider>();
            
            // Act
            bool didThrow = false;
            try
            {
                this.testClass.AddVisualisationObject(givenPrefab, givenTilemap, givenBorderTile, provider.Object);
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
            var givenBorderTile = Resources.Load<Tile>(BorderTileLocation);
            var provider = new Mock<IArrowTileProvider>();
            
            // Act
            bool didThrow = false;
            try
            {
                this.testClass.AddVisualisationObject(givenPrefab, givenTilemap, givenBorderTile, provider.Object);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(ArgumentNullException)}");
        }
        
        [Test]
        public void AddVisualisationObject_ThrowsArgumentNullException_WhenBorderTileMapIsNullTest()
        {
            // Arrange
            GameObject givenPrefab = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            Tilemap givenTilemap = GetTestBorderTileMap("TestResources/World/TestGrid");
            Tile givenBorderTile = null;
            var provider = new Mock<IArrowTileProvider>();
            
            // Act
            bool didThrow = false;
            try
            {
                this.testClass.AddVisualisationObject(givenPrefab, givenTilemap, givenBorderTile, provider.Object);
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
            var givenBorderTile = Resources.Load<Tile>(BorderTileLocation);
            IArrowTileProvider provider = null;
            
            // Act
            bool didThrow = false;
            try
            {
                this.testClass.AddVisualisationObject(givenPrefab, givenTilemap, givenBorderTile, provider);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(ArgumentNullException)}");
        }
        
        [Test]
        public void AddVisualisationObject_ProvidesAtLeastOneArrow_WhenGivenTestTileMapAndArrowProviderTest()
        {
            // Arrange
            GameObject givenPrefab = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            Tilemap givenTilemap = GetTestBorderTileMap("TestResources/World/TestGrid");
            var givenBorderTile = Resources.Load<Tile>(BorderTileLocation);
            var provider = new SimpleArrowTileProvider("Editor/LoopVisualiser/ArrowTiles/TileArrows_"); 
            
            // Act
            GameObject actual = this.testClass.AddVisualisationObject(
                givenPrefab, givenTilemap, givenBorderTile, provider);
            this.tearDownObjects.Add(actual);

            // Assert
            Assert.IsNotNull(actual, "No created object.");
            Tilemap extractedActualTilemap = ExtractTilemap(actual);
            Assert.IsNotNull(extractedActualTilemap, "No tilemap in created object.");
            TileBase topLeft = GetTopLeftTile(extractedActualTilemap);
            Assert.IsNotNull(topLeft, "No Visual.");
        }
        
        [Test]
        public void AddVisualisationObject_SetsEntrancesForBasicTestMap_WhenTestGridIsGivenTest()
        {
            // Arrange
            GameObject givenPrefab = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            Tilemap givenTilemap = GetTestBorderTileMap("TestResources/World/TestGrid");
            var givenBorderTile = Resources.Load<Tile>(BorderTileLocation);
            var provider = new Mock<IArrowTileProvider>();
            
            Tile mockLeft = new();
            provider.Setup(x => x.GetArrowTile(ArrowDirection.Left, ArrowPurpose.LoopEntrance)).Returns(mockLeft);
            
            Tile mockRight = new();
            provider.Setup(x => x.GetArrowTile(ArrowDirection.Right, ArrowPurpose.LoopEntrance)).Returns(mockRight);
            
            Tile mockUp = new();
            provider.Setup(x => x.GetArrowTile(ArrowDirection.Up, ArrowPurpose.LoopEntrance)).Returns(mockUp);
            
            Tile mockDown = new();
            provider.Setup(x => x.GetArrowTile(ArrowDirection.Down, ArrowPurpose.LoopEntrance)).Returns(mockDown);

            // Act
            GameObject actual = this.testClass.AddVisualisationObject(
                givenPrefab, givenTilemap, givenBorderTile, provider.Object);
            this.tearDownObjects.Add(actual);

            // Assert
            Assert.IsNotNull(actual, "No created object.");
            Tilemap extractedActualTilemap = ExtractTilemap(actual);
            
            Assert.AreEqual(mockLeft, extractedActualTilemap.GetTile(new Vector3Int(-3, -2)));
            Assert.AreEqual(mockLeft, extractedActualTilemap.GetTile(new Vector3Int(-3, -1)));
            Assert.AreEqual(mockLeft, extractedActualTilemap.GetTile(new Vector3Int(-3, -0)));
            Assert.AreEqual(mockLeft, extractedActualTilemap.GetTile(new Vector3Int(-3, 1)));
            
            Assert.AreEqual(mockRight, extractedActualTilemap.GetTile(new Vector3Int(2, -2)));
            Assert.AreEqual(mockRight, extractedActualTilemap.GetTile(new Vector3Int(2, -1)));
            Assert.AreEqual(mockRight, extractedActualTilemap.GetTile(new Vector3Int(2, 0)));
            Assert.AreEqual(mockRight, extractedActualTilemap.GetTile(new Vector3Int(2, 1)));
            
            Assert.AreEqual(mockUp, extractedActualTilemap.GetTile(new Vector3Int(-2, -3)));
            Assert.AreEqual(mockUp, extractedActualTilemap.GetTile(new Vector3Int(-1, -3)));
            Assert.AreEqual(mockUp, extractedActualTilemap.GetTile(new Vector3Int(0, -3)));
            Assert.AreEqual(mockUp, extractedActualTilemap.GetTile(new Vector3Int(1, -3)));
            
            Assert.AreEqual(mockDown, extractedActualTilemap.GetTile(new Vector3Int(-2, 2)));
            Assert.AreEqual(mockDown, extractedActualTilemap.GetTile(new Vector3Int(-1, 2)));
            Assert.AreEqual(mockDown, extractedActualTilemap.GetTile(new Vector3Int(0, 2)));
            Assert.AreEqual(mockDown, extractedActualTilemap.GetTile(new Vector3Int(1, 2)));
        }
        
        [Test]
        public void AddVisualisationObject_SetsEntrancesForLoopArrangements_WhenTestGridIsGivenTest()
        {
            // Arrange
            GameObject givenPrefab = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            Tilemap givenTilemap = GetTestBorderTileMap("TestResources/World/TestGrid-LoopArrangements");
            var givenBorderTile = Resources.Load<Tile>(BorderTileLocation);
            var provider = new SimpleArrowTileProvider("Editor/LoopVisualiser/ArrowTiles/TileArrows_");
            
            Tilemap expectedTilemap = GetTestBorderTileMap("TestResources/World/TestGrid-LoopArrangements-Answer");
            TileBase[][] expectedTiles = ExtractTiles(expectedTilemap);

            // Act
            GameObject actual = this.testClass.AddVisualisationObject(
                givenPrefab, givenTilemap, givenBorderTile, provider);
            this.tearDownObjects.Add(actual);

            // Assert
            Assert.IsNotNull(actual, "No created object.");
            Tilemap extractedActualTilemap = ExtractTilemap(actual);
            TileBase[][] actualTiles = ExtractTiles(extractedActualTilemap);
            
            Vector3Int size = extractedActualTilemap.size;
            
            Assert.AreEqual(expectedTiles.Length, actualTiles.Length);
            for (int x = 0; x < size.x; ++x)
            {
                Assert.AreEqual(expectedTiles[x].Length, actualTiles[x].Length);
                for (int y = 0; y < size.y; ++y)
                {
                    if (!IsEntrance(actualTiles[x][y]))
                    {
                        continue;
                    }
                    
                    Assert.AreEqual(expectedTiles[x][y], actualTiles[x][y]);
                }
            }
        }

        private bool IsEntrance(TileBase tileBase)
        {
            string tilePrefab = "TileArrows_";
            string nameNoPrefab = tilePrefab.Replace(tilePrefab, "");

            if (int.TryParse(nameNoPrefab, out int value))
            {
                return value <= 18;
            }

            return false;
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

        /// <remark>
        /// Note the X Y do not match because the origin will no begin at 0.
        /// This is why Unity does not give you an array.
        /// </remark>
        private TileBase[][] ExtractTiles(Tilemap tilemap)
        {
            Vector3Int origin = tilemap.origin;
            Vector2Int tileIndex = new Vector2Int(origin.x, origin.y);
            Vector3Int size = tilemap.size;
            var returnMap = new TileBase[size.x][];
            for (int x = 0; x < size.x; ++x)
            {
                returnMap[x] = new TileBase[size.y];
                for (int y = 0; y < size.y; ++y)
                {
                    returnMap[x][y] = tilemap.GetTile(new Vector3Int(tileIndex.x, tileIndex.y));
                    tileIndex.y++;
                }

                tileIndex.y = origin.y;
                tileIndex.x++;
            }

            return returnMap;
        }

        private Tilemap CreateBlankTilemap(Vector3Int origin, Vector3Int size)
        {
            GameObject obj = new GameObject("Blank");
            this.tearDownObjects.Add(obj);

            Tilemap returnMap = obj.AddComponent<Tilemap>();
            returnMap.size = size;
            returnMap.origin = origin;

            return returnMap;
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

        private ArrowTilesStruct GetArrowsFromProvider(IArrowTileProvider provider, ArrowPurpose purpose)
        {
            ArrowTilesStruct tiles = new();
            tiles.L = provider.GetArrowTile(ArrowDirection.Left, purpose);
            tiles.R = provider.GetArrowTile(ArrowDirection.Right, purpose);
            tiles.U = provider.GetArrowTile(ArrowDirection.Up, purpose);
            tiles.D = provider.GetArrowTile(ArrowDirection.Down, purpose);
            tiles.LR = provider.GetArrowTile(ArrowDirection.Left | ArrowDirection.Right, purpose);
            tiles.UD = provider.GetArrowTile(ArrowDirection.Up | ArrowDirection.Down, purpose);
            tiles.LU = provider.GetArrowTile(ArrowDirection.Left | ArrowDirection.Up, purpose);
            tiles.LD = provider.GetArrowTile(ArrowDirection.Left | ArrowDirection.Down, purpose);
            tiles.RU = provider.GetArrowTile(ArrowDirection.Right | ArrowDirection.Up, purpose);
            tiles.RD = provider.GetArrowTile(ArrowDirection.Right | ArrowDirection.Down, purpose);
            tiles.LRD = provider.GetArrowTile(ArrowDirection.Left |ArrowDirection.Right | ArrowDirection.Down, purpose);
            tiles.LDU = provider.GetArrowTile(ArrowDirection.Left |ArrowDirection.Down | ArrowDirection.Up, purpose);
            tiles.LUR = provider.GetArrowTile(ArrowDirection.Left |ArrowDirection.Up | ArrowDirection.Right, purpose);
            tiles.URD = provider.GetArrowTile(ArrowDirection.Up |ArrowDirection.Right | ArrowDirection.Down, purpose);
            tiles.LRDU = provider.GetArrowTile(
                ArrowDirection.Up | ArrowDirection.Left | ArrowDirection.Right | ArrowDirection.Down, purpose);

            return tiles;
        }
        
        #endregion

        /// <summary>
        /// Stores all directions for ease of naming
        /// </summary>
        public struct ArrowTilesStruct
        {
            /// <summary>
            /// Left.
            /// </summary>
            public Tile L;

            /// <summary>
            /// Right.
            /// </summary>
            public Tile R;

            /// <summary>
            /// Up.
            /// </summary>
            public Tile U;

            /// <summary>
            /// Down.
            /// </summary>
            public Tile D;

            /// <summary>
            /// Left Right.
            /// </summary>
            public Tile LR;

            /// <summary>
            /// Up Down.
            /// </summary>
            public Tile UD;

            /// <summary>
            /// Left Up.
            /// </summary>
            public Tile LU;

            /// <summary>
            /// Left Down.
            /// </summary>
            public Tile LD;

            /// <summary>
            /// Right Up.
            /// </summary>
            public Tile RU;

            /// <summary>
            /// Right Down.
            /// </summary>
            public Tile RD;

            /// <summary>
            /// Left Right Down.
            /// </summary>
            public Tile LRD;

            /// <summary>
            /// Left Down Up.
            /// </summary>
            public Tile LDU;

            /// <summary>
            /// Left Up Right.
            /// </summary>
            public Tile LUR;

            /// <summary>
            /// Up Right Down.
            /// </summary>
            public Tile URD;

            /// <summary>
            /// Left Right Down Up.
            /// </summary>
            public Tile LRDU;
        }
    }
}