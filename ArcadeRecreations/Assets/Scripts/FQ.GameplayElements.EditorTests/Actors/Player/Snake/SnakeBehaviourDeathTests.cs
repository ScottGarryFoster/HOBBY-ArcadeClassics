using System.Linq;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeBehaviourDeathTests
    {
        private ISnakeBehaviour snakeBehaviour;
        private GameObject playerObject;
        private StubObjectCreation stubObjectCreation;
        private Mock<IGameplayInputs> mockGameplayInputs;

        /// <summary>
        /// A tick amount to advance the update which will not advance the movement.
        /// </summary>
        private const float SafeFloatTick = 0.0001f;
        
        [SetUp]
        public void Setup()
        {
            this.playerObject = new GameObject();
            this.stubObjectCreation = new StubObjectCreation();
            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            var concreteSnakeBehaviour = new SnakeBehaviour(
                this.playerObject,
                this.stubObjectCreation,
                this.mockGameplayInputs.Object)
            {
                MovementSpeed = 1
            };
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            concreteSnakeBehaviour.snakeTailPrefab = snakeTailPrefab;

            this.snakeBehaviour = concreteSnakeBehaviour;
            
            // All the tests for position require start to have occured.
            this.snakeBehaviour.Start();
        }

        [TearDown]
        public void Teardown()
        {
            this.stubObjectCreation.CreatedGameObjects.ForEach(Object.DestroyImmediate);
            this.stubObjectCreation.CreatedGameObjects.Clear();
        }

        [Test]
        public void OnCollisionWithTail_TriggerEnd_WhenTailTriggeredWith2DEnterTest()
        {
            // Arrange
            bool didTriggerEnd = false;
            this.snakeBehaviour.EndTrigger += () => { didTriggerEnd = true; };
            
            this.snakeBehaviour.Start();

            // Act 
            RunInToTail(this.playerObject, this.mockGameplayInputs, this.snakeBehaviour);
            RunMovementUpdateCycle();

            // Assert
            Assert.IsTrue(didTriggerEnd);
        }
        
        [Test]
        public void ResetElement_ReturnsToTheLocationAfterStart_WhenInvokedAfterMovementTest()
        {
            // Arrange
            this.snakeBehaviour.Start();

            Vector3 expectedLocation = CopyVector3(this.playerObject.transform.position);
            
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionUp)).Returns(true);
            RunMovementUpdateCycle();
            RunMovementUpdateCycle();
            
            Vector3 newLocation = CopyVector3(this.playerObject.transform.position);
            Assert.AreNotEqual(expectedLocation, newLocation, "Player did not move");

            // Act 
            this.snakeBehaviour.ResetElement?.Invoke();
            Vector3 actual = CopyVector3(this.playerObject.transform.position);

            // Assert
            Assert.AreEqual(expectedLocation, actual, "Player did not return");
        }
        
        [Test]
        public void ResetElement_ReducesTheTailSizeToDefault_WhenInvokedAfterGrowingTest()
        {
            // Arrange
            this.snakeBehaviour.Start();

            // Eat
            EatFood(this.playerObject, this.mockGameplayInputs, this.snakeBehaviour);
            RunMovementUpdateCycle();
            Vector3 firstFoodEaten = CopyVector3(this.playerObject.transform.position);

            // Eat
            EatFood(this.playerObject, this.mockGameplayInputs, this.snakeBehaviour);
            RunMovementUpdateCycle();
            Vector3 secondFoodEaten = CopyVector3(this.playerObject.transform.position);
            
            RunMovementUpdateCycle();
            
            int originalActive = this.snakeBehaviour.SnakeTailPieces.Count(x => x.gameObject.activeSelf);
            Assert.AreEqual(2, originalActive, "Did not eat food and grow tail.");

            // Act 
            this.snakeBehaviour.ResetElement?.Invoke();

            // Assert
            int numberActive = this.snakeBehaviour.SnakeTailPieces.Count(x => x.gameObject.activeSelf);
            Assert.Zero(numberActive, "Still active tail pieces.");
        }
        
        [Test]
        public void StartTrigger_StartTriggerIsCalledTwice_WhenResetElementIsInvokedTest()
        {
            // Arrange
            this.mockGameplayInputs.Setup(
                x => x.KeyPressed(GameplayButton.DirectionDown)).Returns(true);

            int expected = 2;
            int didTrigger = 0;
            this.snakeBehaviour.StartTrigger += () => { ++didTrigger; };
            RunMovementUpdateCycle();
            
            this.snakeBehaviour.ResetElement?.Invoke();

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.AreEqual(expected, didTrigger);
        }
        
        #region Process Methods
        
        private GameObject SetupTailInFrontOfPlayer(
            GameObject player, 
            Mock<IGameplayInputs> mockGI, 
            out Collider2D collider2D)
        {
            Vector3 currentPosition = CopyVector3(player.transform.position);
            var foodGo = CreateGameObject(position: new Vector3(0, currentPosition.y + 1, 0), tag: "SnakeTail");
            collider2D = CreateTriggerBoxCollider(foodGo);

            mockGI.Setup(x => x.KeyPressed(GameplayButton.DirectionUp))
                .Returns(true);

            return foodGo;
        }
        
        private void RunInToTail(GameObject player, Mock<IGameplayInputs> mockGI, ISnakeBehaviour behaviour)
        {
            SetupTailInFrontOfPlayer(this.playerObject, this.mockGameplayInputs, out Collider2D collider2D);
            behaviour.OnTriggerEnter2D(collider2D);
            behaviour.OnTriggerStay2D(collider2D);
        }
        
        private GameObject SetupSnakeToEatFood(
            GameObject player, 
            Mock<IGameplayInputs> mockGI, 
            out Collider2D collider2D)
        {
            Vector3 currentPosition = CopyVector3(player.transform.position);
            var foodGo = CreateGameObject(position: new Vector3(0, currentPosition.y + 1, 0), tag: "SnakeFood");
            //foodGo.AddComponent<TestFood>();
            collider2D = CreateTriggerBoxCollider(foodGo);

            mockGI.Setup(x => x.KeyPressed(GameplayButton.DirectionUp))
                .Returns(true);

            return foodGo;
        }

        private void EatFood(GameObject player, Mock<IGameplayInputs> mockGI, ISnakeBehaviour behaviour)
        {
            SetupSnakeToEatFood(this.playerObject, this.mockGameplayInputs, out Collider2D collider2D);
            behaviour.OnTriggerEnter2D(collider2D);
            behaviour.OnTriggerStay2D(collider2D);
        }
        
        #endregion
        
        #region Helper Methods

        private Vector3 CopyVector3(Vector3 toCopy)
        {
            return new Vector3(toCopy.x, toCopy.y, toCopy.z);
        }

        private void RunMovementUpdateCycle()
        {
            this.snakeBehaviour.Update(this.snakeBehaviour.MovementSpeed);
        }
        
        private void RunUpdateCycleButDoNotTriggerMovement()
        {
            this.snakeBehaviour.Update(SafeFloatTick);
        }

        private void MockKeyInput(KeyPressMethod method, GameplayButton button, bool press = true)
        {
            switch (method)
            {
                case KeyPressMethod.KeyDown:
                    this.mockGameplayInputs.Setup(
                        x => x.KeyDown(button)).Returns(press);
                    break;
                case KeyPressMethod.KeyPressed:
                    this.mockGameplayInputs.Setup(
                        x => x.KeyPressed(button)).Returns(press);
                    break;
                case KeyPressMethod.KeyUp:
                    this.mockGameplayInputs.Setup(
                        x => x.KeyUp(button)).Returns(press);
                    break;
            }
        }
        
        private GameObject CreateGameObject(Vector3 position, string tag = "")
        {
            var newGO = new GameObject();
            newGO.transform.position = position;

            if (string.IsNullOrWhiteSpace(tag))
            {
                tag = "SnakeFood";
            }
            
            newGO.tag = tag;

            return newGO;
        }

        private BoxCollider2D CreateTriggerBoxCollider(GameObject go)
        {
            var bc2d = go.AddComponent<BoxCollider2D>();
            bc2d.size = new Vector2(0.9f, 0.9f);
            bc2d.isTrigger = true;

            return bc2d;
        }
        
        private void AddFullCollider(GameObject gameObject)
        {
            CreateTriggerBoxCollider(gameObject);
            var ridgedBody = gameObject.AddComponent<Rigidbody2D>();
            ridgedBody.gravityScale = 0;
        }
        
        #endregion
    }
}