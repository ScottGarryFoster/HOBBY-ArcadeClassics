using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace FQ.GameObjectPromises.PlayTests
{
    public class GameElementTests
    {
        [UnityTest]
        public IEnumerator WaitingForUpdateCycle_CausesProtectedUpdateToBeCalled_WhenTimeIsEnoughTest()
        {
            // Arrange
            var testObject = Object.Instantiate(new GameObject());
            var testElement = testObject.AddComponent<TestGameElement>();

            // Act
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.IsTrue(testElement.testUpdateRun);
            
            // Teardown
            Object.DestroyImmediate(testObject);
        }
        
        [UnityTest]
        public IEnumerator WaitingForFixedUpdateCycle_CausesProtectedFixedUpdateToBeCalled_WhenTimeIsEnoughTest()
        {
            // Arrange
            var testObject = Object.Instantiate(new GameObject());
            var testElement = testObject.AddComponent<TestGameElement>();

            // Act
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();

            // Assert
            Assert.IsTrue(testElement.testFixedUpdateRun);
            
            // Teardown
            Object.DestroyImmediate(testObject);
        }
        
        [UnityTest]
        public IEnumerator WaitingForLateUpdateCycle_CausesLateUpdateToBeCalled_WhenTimeIsEnoughTest()
        {
            // Arrange
            var testObject = Object.Instantiate(new GameObject());
            var testElement = testObject.AddComponent<TestGameElement>();

            // Act
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.IsTrue(testElement.testLateUpdateRun);
            
            // Teardown
            Object.DestroyImmediate(testObject);
        }
        
        [UnityTest]
        public IEnumerator WaitingStartCycle_CausesStartCalled_WhenTimeIsEnoughTest()
        {
            // Arrange
            var testObject = Object.Instantiate(new GameObject());
            var testElement = testObject.AddComponent<TestGameElement>();

            // Act
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.IsTrue(testElement.testStartRun);
            
            // Teardown
            Object.DestroyImmediate(testObject);
        }
        
        [UnityTest]
        public IEnumerator MovingTriggerableInRange_CausesOnTriggerEnter2D_WhenTriggerMovesInToRangeTest()
        {
            // Arrange
            var testObject = Object.Instantiate(new GameObject());
            var testElement = testObject.AddComponent<TestGameElement>();

            // Act
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.IsTrue(testElement.testStartRun);
            
            // Teardown
            Object.DestroyImmediate(testObject);
        }
    }

    public class TestGameElement : GameElement
    {
        public bool testUpdateRun;   
        public bool testFixedUpdateRun;   
        public bool testLateUpdateRun;   
        public bool testStartRun;
        public Collider2D testOnTriggerEnter2D = null;
        public Collider2D testOnTriggerStay2D = null;
        public Collider2D testOnTriggerExit2D = null;

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected override void BaseUpdate()
        {
            testUpdateRun = true;
        }
        
        /// <summary>
        /// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
        /// </summary>
        protected override void BaseFixedUpdate()
        {
            testFixedUpdateRun = true;
        }
        
        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// </summary>
        protected override void BaseLateUpdate()
        {
            testLateUpdateRun = true;
        }
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just
        /// before any of the Update methods are called the first time.
        /// </summary>
        protected override void BaseStart()
        {
            testStartRun = true;
        }
        
        /// <summary>
        /// Sent when another object enters a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        protected override void BaseOnTriggerEnter2D(Collider2D other)
        {
            testOnTriggerEnter2D = other;
        }
        
        /// <summary>
        /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        protected override void BaseOnTriggerStay2D(Collider2D other)
        {
            testOnTriggerStay2D = other;
        }
        
        /// <summary>
        /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision. </param>
        protected override void BaseOnTriggerExit2D(Collider2D other)
        {
            testOnTriggerExit2D = other;
        }
    }
}