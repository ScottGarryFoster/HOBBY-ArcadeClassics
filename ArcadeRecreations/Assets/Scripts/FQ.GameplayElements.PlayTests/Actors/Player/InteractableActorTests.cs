using System.Collections;
using FQ.GameplayElements.Actors.Player;
using FQ.GameplayInputs;
using Moq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace FQ.GameplayElements.PlayTests.Actors.Player
{
    public class InteractableActorTests
    {
        private Mock<IGameplayInputs> mockGameplayInputs;
        private InteractableActor interactableActor;
        
        public void Setup()
        {
            var playerObject = new GameObject();
            this.interactableActor = playerObject.AddComponent<InteractableActor>();

            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            this.interactableActor.gameplayInputs = this.mockGameplayInputs.Object;
        }
        
        [UnityTest]
        public IEnumerator Update_PlayerMovesDownOneUnit_WhenUpdateAndKeyPressIsDownTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();
            
            Vector2 expectedPosition = this.interactableActor.transform.position;
            expectedPosition.y--;
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(EGameplayButton.DirectionDown)).Returns(true);

            // Act
            yield return new WaitForSeconds(0.2f);

            // Assert
            Vector2 actualPosition = this.interactableActor.transform.position;
            Assert.AreEqual(expectedPosition, actualPosition);
        }
    }
}