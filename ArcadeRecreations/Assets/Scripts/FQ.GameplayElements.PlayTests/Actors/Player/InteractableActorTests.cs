using System.Collections;
using FQ.GameplayElements;
using Moq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace FQ.GameplayElements.PlayTests
{
    public class InteractableActorTests
    {
        private GameObject playerObject;
        private InteractableActorTestPoly interactableActor;
        
        public void Setup()
        {
            this.playerObject = new GameObject();
            this.interactableActor = this.playerObject.AddComponent<InteractableActorTestPoly>();
            this.interactableActor.movementSpeed = 0.025f;
        }
        
        [UnityTest]
        public IEnumerator AddComponent_CausesProtectedStartToBeCalled_WhenFrameIsCompleteTest()
        {
            Setup();
            
            // Arrange

            // Act
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.IsTrue(this.interactableActor.didRunStart);
        }
        
        [UnityTest]
        public IEnumerator WaitingForUpdateCycle_CausesProtectedUpdateToBeCalled_WhenTimeIsEnoughTest()
        {
            Setup();
            
            // Arrange
            yield return new WaitForEndOfFrame();

            // Act
            yield return RunUpdateCycle();

            // Assert
            Assert.IsTrue(this.interactableActor.didRunUpdate);
        }
        
        
        private object RunUpdateCycle()
        {
            return new WaitForSeconds(this.interactableActor.MovementSpeed);
        }
    }

    public class InteractableActorTestPoly : InteractableActor
    {
        public bool didRunStart { get; private set; }

        public bool didRunUpdate { get; private set; }
        
        protected override void ProtectedStart()
        {
            this.didRunStart = true;
        }
        
        protected override void ProtectedUpdate()
        {
            this.didRunUpdate = true;
        }
    }
}