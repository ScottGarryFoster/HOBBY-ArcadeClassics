using System;
using System.Collections;
using System.Linq;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeBehaviourTailTests
    {
        private ISnakeBehaviour snakeBehaviour;
        private GameObject playerObject;
        private StubObjectCreation stubObjectCreation;
        private Mock<IGameplayInputs> mockGameplayInputs;
        
        [SetUp]
        public void Setup()
        {
            this.playerObject = new GameObject();
            this.stubObjectCreation = new StubObjectCreation();
            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            var t = new SnakeBehaviour(
                this.playerObject, 
                this.stubObjectCreation, 
                this.mockGameplayInputs.Object);
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            t.snakeTailPrefab = snakeTailPrefab;

            this.snakeBehaviour = t;
            /*this.playerObject.tag = "Player";*/
            //AddFullCollider(this.playerObject);
            //this.snakeBehaviour = this.playerObject.AddComponent<SnakePlayer>();
            
            // The only reason Movement Speed is internal is to speed up tests
            // We speed up Time delta and slow down frames.
            /*this.snakePlayer.movementSpeed = 0.025f;
            Time.maximumDeltaTime = 0.0001f;

            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            this.snakePlayer.gameplayInputs = this.mockGameplayInputs.Object;
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            this.snakePlayer.snakeTailPrefab = snakeTailPrefab;*/
        }

        [TearDown]
        public void Teardown()
        {
            this.stubObjectCreation.CreatedGameObjects.ForEach(Object.DestroyImmediate);
            this.stubObjectCreation.CreatedGameObjects.Clear();
        }

        [Test]
        public void OnConstruction_ThrowsNullArgumentException_WhenNullGameObjectGivenTest()
        {
            // Arrange
            GameObject given = null;
            
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SnakeBehaviour(given, this.stubObjectCreation, this.mockGameplayInputs.Object);
            });
        }
        
        [Test]
        public void OnConstruction_ThrowsNullArgumentException_WhenObjectCreationIsNullTest()
        {
            // Arrange
            IObjectCreation given = null;
            
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SnakeBehaviour(new GameObject(), given, this.mockGameplayInputs.Object);
            });
        }
        
        [Test]
        public void OnConstruction_ThrowsNullArgumentException_WhenInputIsNullTest()
        {
            // Arrange
            IGameplayInputs given = null;
            
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SnakeBehaviour(new GameObject(), this.stubObjectCreation, given);
            });
        }
        
        //#region SnakeTail

        [Test]
        public void Start_Creates199TailPieces_WhenGivenAPrefabTest()
        {
            // Arrange
            int pieces = SnakeBehaviour.MaxTailSize;

            // Act
            this.snakeBehaviour.Start();

            // Assert
            Assert.IsNotNull(this.snakeBehaviour.SnakeTailPieces);
            Assert.AreEqual(pieces, this.snakeBehaviour.SnakeTailPieces.Length);
            for (int i = 0; i < pieces; ++i)
            {
                Assert.IsNotNull(this.snakeBehaviour.SnakeTailPieces[i]);
            }
        }
        
        [Test]
        public void Start_PlacesTailWithHeadPiece_WhenGivenAPrefabTest()
        {
            // Arrange
            var givenPosition = new Vector3(1, 2, 3);
            this.playerObject.transform.position = givenPosition;

            // Act
            this.snakeBehaviour.Start();

            // Assert
            for (int i = 0; i < SnakeBehaviour.MaxTailSize; ++i)
            {
                Assert.AreEqual(
                    this.snakeBehaviour.SnakeTailPieces[i].transform.position, 
                    givenPosition);
            }
        }
        
        
        [Test]
        public void Start_UnenablesTheTailPieces_WhenGivenAPrefabTest()
        {
            // Arrange

            // Act
            this.snakeBehaviour.Start();

            // Assert
            Assert.IsFalse(this.snakeBehaviour.SnakeTailPieces.Any(x => x.gameObject.activeSelf));
        }
        
        
        /// <summary>
        /// Should move to food but not grow until another update as Movement speed is not complete.
        /// See <see href="https://docs.unity3d.com/Manual/ExecutionOrder.html"/> for details.
        /// </summary>
        [Test]
        public void CollideWithFood_DoesNotEnableTailPiece_WhenFoodPlacedInFrontOfPlayerAndPlayerMovedInToTest()
        {
            // Arrange
            this.snakeBehaviour.Start();
            EatFood(this.playerObject, this.mockGameplayInputs, this.snakeBehaviour);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.IsFalse(this.snakeBehaviour.SnakeTailPieces.Any(x => x.gameObject.activeSelf));
        }

        
        [Test]
        public void CollideWithFood_CausesATailPieceToBeEnabled_WhenFoodPlacedInFrontOfPlayerAndPlayerOffFoodTest()
        {
            // Arrange
            this.snakeBehaviour.Start();
            EatFood(this.playerObject, this.mockGameplayInputs, this.snakeBehaviour);
            
            // Act
            // One to eat, One to Move and Keep the tail where it is.
            RunMovementUpdateCycle();
            RunMovementUpdateCycle();

            // Assert
            Assert.AreEqual(1, this.snakeBehaviour.SnakeTailPieces.Count(x => x.gameObject.activeSelf));
            Assert.IsTrue(this.snakeBehaviour.SnakeTailPieces[0].gameObject.activeSelf);
        }
        
        
        [Test]
        public void CollideWithFood_TailPieceIsWhereFoodWas_WhenMovedOffFoodTest()
        {
            // Arrange
            this.snakeBehaviour.Start();
            EatFood(this.playerObject, this.mockGameplayInputs, this.snakeBehaviour);
            Vector3 expectedPosition = CopyVector3(this.playerObject.transform.position);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector3 tailPiece = this.snakeBehaviour.SnakeTailPieces[0].gameObject.transform.position;
            Assert.AreEqual(expectedPosition, tailPiece);
        }
        
        
        [Test]
        public void CollideWithFood_TailGrows_WhenMoreFoodIsEatenTest()
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

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector3 tailPiece = this.snakeBehaviour.SnakeTailPieces[0].gameObject.transform.position;
            Assert.AreEqual(secondFoodEaten, tailPiece, $"secondFoodEaten == tailPiece | {secondFoodEaten} == {tailPiece}");
            Vector3 tailPiece2 = this.snakeBehaviour.SnakeTailPieces[1].gameObject.transform.position;
            Assert.AreEqual(firstFoodEaten, tailPiece2, $"firstFoodEaten == tailPiece2 | {firstFoodEaten} == {tailPiece2}");
        }
        
        [Test]
        public void CollideWithFood_TailGrowsMore_WhenMoreFoodIsEatenTest()
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
            
            // Eat
            EatFood(this.playerObject, this.mockGameplayInputs, this.snakeBehaviour);
            RunMovementUpdateCycle();
            Vector3 thirdFoodEaten = CopyVector3(this.playerObject.transform.position);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Vector3 tailPiece = this.snakeBehaviour.SnakeTailPieces[0].gameObject.transform.position;
            Assert.AreEqual(thirdFoodEaten, tailPiece, $"thirdFoodEaten == tailPiece | {thirdFoodEaten} == {tailPiece}");
            Vector3 tailPiece2 = this.snakeBehaviour.SnakeTailPieces[1].gameObject.transform.position;
            Assert.AreEqual(secondFoodEaten, tailPiece2, $"secondFoodEaten == tailPiece2 | {secondFoodEaten} == {tailPiece2}");
            Vector3 tailPiece3 = this.snakeBehaviour.SnakeTailPieces[2].gameObject.transform.position;
            Assert.AreEqual(firstFoodEaten, tailPiece3, $"firstFoodEaten == tailPiece3 | {firstFoodEaten} == {tailPiece3}");
        }

/*
        #region Helper Methods
        
        private Vector3 CopyVector3(Vector3 toCopy)
        {
            return new Vector3(toCopy.x, toCopy.y, toCopy.z);
        }

        private object RunMovementUpdateCycle(IActorActiveStats activeStats)
        {
            return new WaitForSeconds(activeStats.MovementSpeed);
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

            if (!string.IsNullOrWhiteSpace(tag))
            {
                newGO.tag = "SnakeFood";
            }

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
        
        private GameObject SetupSnakeToEatFood(GameObject player, Mock<IGameplayInputs> mockGI)
        {
            Vector3 currentPosition = CopyVector3(player.transform.position);
            var foodGo = CreateGameObject(position: new Vector3(0, currentPosition.y + 1, 0), tag: "SnakeFood");
            foodGo.AddComponent<TestFood>();
            CreateTriggerBoxCollider(foodGo);

            mockGI.Setup(x => x.KeyPressed(GameplayButton.DirectionUp))
                .Returns(true);

            return foodGo;
        }
        
        #endregion*/
        
        private void RunMovementUpdateCycle()
        {
            this.snakeBehaviour.Update(this.snakeBehaviour.MovementSpeed);
        }
        
        private void AddFullCollider(GameObject gameObject)
        {
            CreateTriggerBoxCollider(gameObject);
            var ridgedBody = gameObject.AddComponent<Rigidbody2D>();
            ridgedBody.gravityScale = 0;
        }
        
        private BoxCollider2D CreateTriggerBoxCollider(GameObject go)
        {
            var bc2d = go.AddComponent<BoxCollider2D>();
            bc2d.size = new Vector2(0.9f, 0.9f);
            bc2d.isTrigger = true;

            return bc2d;
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
        
        private Vector3 CopyVector3(Vector3 toCopy)
        {
            return new Vector3(toCopy.x, toCopy.y, toCopy.z);
        }

        private GameObject CreateGameObject(Vector3 position, string tag = "")
        {
            GameObject newGameObject = this.stubObjectCreation.Instantiate(new GameObject());
            newGameObject.transform.position = position;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                newGameObject.tag = tag;
            }

            return newGameObject;
        }
    }
}