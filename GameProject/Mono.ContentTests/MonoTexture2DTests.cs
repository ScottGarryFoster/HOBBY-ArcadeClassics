/*using Mono.Content;

namespace Mono.ContentTests;

[TestClass]
public class MonoTexture2DTests
{
    [TestMethod, ExpectedException(typeof(ArgumentNullException))]
    public void OnConstruction_ThrowsArgumentNullException_WhenKeyIsNullTest()
    {
        // Arrange
        string contentKey = null;

        // Act
        new MonoTexture2D(contentKey);

        // Assert
    }
    
    [TestMethod]
    public void OnConstruction_IsLoadedIsFalse_WhenKeyIsNotValidTest()
    {
        // Arrange
        string contentKey = "notvaliidfilename";

        // Act
        ITexture2D testClass = new MonoTexture2D(contentKey);

        // Assert
        Assert.IsFalse(testClass.IsLoaded);
    }
    
    [TestMethod]
    public void OnConstruction_IsLoadedIsTrue_WhenKeyIsValidTest()
    {
        // Arrange
        string contentKey = "Tests/TestImage.png";

        // Act
        ITexture2D testClass = new MonoTexture2D(contentKey);

        // Assert
        Assert.IsTrue(testClass.IsLoaded);
    }
}*/