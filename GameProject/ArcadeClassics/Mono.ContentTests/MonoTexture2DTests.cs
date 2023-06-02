using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mono.Content;
using Moq;

namespace Mono.ContentTests;

[TestClass]
public class MonoTexture2DTests
{
    private Mock<IContentManager> mockContentManager;

    [TestInitialize]
    public void Setup()
    {
        this.mockContentManager = new Mock<IContentManager>();
        this.mockContentManager.Object.RootDirectory = "Content";
    }
    
    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void OnConstruction_ThrowsArgumentNullException_WhenContentManagerIsNullTest()
    {
        // Arrange
        IContentManager contentManager = null;

        // Act
        new MonoTexture2D(contentManager, key: "SOMETHING");

        // Assert
    }
    
    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void OnConstruction_ThrowsArgumentNullException_WhenKeyIsNullTest()
    {
        // Arrange
        string contentKey = null;

        // Act
        new MonoTexture2D(this.mockContentManager.Object, contentKey);

        // Assert
    }
}