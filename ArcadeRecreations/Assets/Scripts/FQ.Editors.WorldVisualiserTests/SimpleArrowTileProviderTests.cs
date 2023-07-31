using System;
using FQ.Libraries;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FQ.Editors.WorldVisualiserTests
{
    public class SimpleArrowTileProviderTests
    {
        private const string NotAPrefab = "InvalidPrefabName";
        private const string ValidPrefabPrefix = "Editor/LoopVisualiser/ArrowTiles/TileArrows_";
        
        [Test]
        public void OnConstruction_ThrowArgumentNullException_WhenPrefabPrefixIsNullTest()
        {
            // Arrange
            string prefabPrefix = null;
            
            // Act
            bool didThrow = false;
            try
            {
                new SimpleArrowTileProvider(prefabPrefix);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(ArgumentNullException)}");
        }
        
        [Test]
        public void OnConstruction_ThrowArgumentNullException_WhenPrefabPrefixIsWhitespaceTest()
        {
            // Arrange
            string prefabPrefix = "  ";
            
            // Act
            bool didThrow = false;
            try
            {
                new SimpleArrowTileProvider(prefabPrefix);
            }
            catch (ArgumentNullException)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(ArgumentNullException)}");
        }
        
        [Test]
        public void OnConstruction_ThrowInvalidParameter_WhenPrefabIsNotAPrefabTest()
        {
            // Arrange
            string prefabPrefix = NotAPrefab;
            
            // Act
            bool didThrow = false;
            try
            {
                new SimpleArrowTileProvider(prefabPrefix);
            }
            catch (InvalidParameter)
            {
                didThrow = true;
            }
            
            // Assert
            Assert.IsTrue(didThrow, $"Did not throw {nameof(InvalidParameter)}");
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile0_WhenGivenEntranceAllDirectionsTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Down | 
                ArrowDirection.Left | 
                ArrowDirection.Right | 
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 0;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile20_WhenGivenExitAllDirectionsTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Down | 
                ArrowDirection.Left | 
                ArrowDirection.Right | 
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 20;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile1_WhenGivenEntranceAndDownLeftUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Down | 
                ArrowDirection.Left |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 1;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile21_WhenGivenExitAndDownLeftUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Down | 
                ArrowDirection.Left |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 21;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile2_WhenGivenEntranceAndLeftRightUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Right | 
                ArrowDirection.Left |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 2;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile22_WhenGivenExitAndLeftRightUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Right | 
                ArrowDirection.Left |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 22;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile3_WhenGivenEntranceAndDownRightUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Down | 
                ArrowDirection.Right |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 3;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile23_WhenGivenExitAndDownRightUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Down | 
                ArrowDirection.Right |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 23;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile4_WhenGivenEntranceAndDownRightLeftTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Down | 
                ArrowDirection.Right |
                ArrowDirection.Left;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 4;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile24_WhenGivenExitAndDownRightLeftTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Down | 
                ArrowDirection.Right |
                ArrowDirection.Left;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 24;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile5_WhenGivenEntranceAndLeftUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Left |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 5;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile25_WhenGivenExitAndLeftUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Left |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 25;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile6_WhenGivenEntranceAndRightUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Right |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 6;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose); 
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile26_WhenGivenExitAndRightUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Right |
                ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 26;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile7_WhenGivenEntranceAndRightDownTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Right |
                ArrowDirection.Down;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 7;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile27_WhenGivenExitAndRightDownTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Right |
                ArrowDirection.Down;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 27;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile8_WhenGivenEntranceAndLeftDownTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Left |
                ArrowDirection.Down;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 8;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile28_WhenGivenExitAndLeftDownTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Left |
                ArrowDirection.Down;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 28;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile9_WhenGivenEntranceAndLeftRightTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Left |
                ArrowDirection.Right;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 9;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile29_WhenGivenExitAndLeftRightTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Left |
                ArrowDirection.Right;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 29;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile10_WhenGivenEntranceAndUpDownTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Up |
                ArrowDirection.Down;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 10;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile30_WhenGivenExitAndUpDownTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection =
                ArrowDirection.Up |
                ArrowDirection.Down;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 30;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile11_WhenGivenEntranceAndRightTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection = ArrowDirection.Right;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 11;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile31_WhenGivenExitAndRightTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection = ArrowDirection.Right;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 31;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile12_WhenGivenEntranceAndDownTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection = ArrowDirection.Down;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 12;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile32_WhenGivenExitAndDownTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection = ArrowDirection.Down;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 32;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile13_WhenGivenEntranceAndLeftTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection = ArrowDirection.Left;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 13;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile33_WhenGivenExitAndLeftTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection = ArrowDirection.Left;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 33;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile14_WhenGivenEntranceAndUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection = ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopEntrance;

            int tile = 14;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
        
        [Test]
        public void GetArrowTile_ReturnsTile34_WhenGivenExitAndUpTest()
        {
            // Arrange
            IArrowTileProvider testClass = new SimpleArrowTileProvider(ValidPrefabPrefix);

            ArrowDirection givenDirection = ArrowDirection.Up;
            var givenPurpose = ArrowPurpose.LoopExit;

            int tile = 34;
            var expectedTile = Resources.Load<Tile>($"{ValidPrefabPrefix}{tile}");
            
            // Act
            Tile actual = testClass.GetArrowTile(givenDirection, givenPurpose);
            
            // Assert
            Assert.AreEqual(expectedTile, actual);
        }
    }
}