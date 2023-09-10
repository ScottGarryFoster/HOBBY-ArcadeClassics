using NUnit.Framework;
using UnityEngine;

namespace FQ.GameElementCommunication.EditorTests
{
    public class ElementCommunicationFinderTests
    {
        [Test]
        public void FindElementCommunication_ReturnsNull_WhenNoGameObjectWithGameControllerTagExistsTest()
        {
            // Arrange
            DestroyAllGameControllers();

            IElementCommunicationFinder testClass = new ElementCommunicationFinder();
            IElementCommunication expected = null;
            
            // Act
            IElementCommunication actual = testClass.FindElementCommunication();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FindElementCommunication_ReturnsNull_WhenThereIsNoElementCommunicationOnGameControllerGameObjectTest()
        {
            // Arrange
            DestroyAllGameControllers();
            GameObject controllerGameObject = new();
            controllerGameObject.tag = ElementCommunicationFinder.GameControllerTag;

            IElementCommunicationFinder testClass = new ElementCommunicationFinder();
            IElementCommunication expected = null;
            
            // Act
            IElementCommunication actual = testClass.FindElementCommunication();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FindElementCommunication_ReturnsOneInScene_WhenOneIsFoundWithTheTagOnGameControllerTest()
        {
            // Arrange
            DestroyAllGameControllers();
            GameObject controllerGameObject = new();
            controllerGameObject.tag = ElementCommunicationFinder.GameControllerTag;
            IElementCommunication expected = controllerGameObject.AddComponent<ElementCommunication>();

            IElementCommunicationFinder testClass = new ElementCommunicationFinder();
            
            // Act
            IElementCommunication actual = testClass.FindElementCommunication();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private void DestroyAllGameControllers()
        {
            GameObject[] gameControllers = GameObject
                .FindGameObjectsWithTag(ElementCommunicationFinder.GameControllerTag);
            foreach (GameObject gameController in gameControllers)
            {
                Object.DestroyImmediate(gameController);
            }
        }
    }
}