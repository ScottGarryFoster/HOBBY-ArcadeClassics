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
            SetupSnakeToEatFood(this.playerObject, this.mockGameplayInputs, out Collider2D collider2D);
            this.snakeBehaviour.OnTriggerEnter2D(collider2D);

            // Act
            RunMovementUpdateCycle();

            // Assert
            Assert.AreEqual(this.snakeBehaviour.SnakeTailPieces.Count(x => x.gameObject.activeSelf), 0);
        }

        /*
        [UnityTest]
        public IEnumerator CollideWithFood_CausesATailPieceToBeEnabled_WhenFoodPlacedInFrontOfPlayerAndPlayerOffFoodTest()
        {
            //Setup();
            GameObject po = new GameObject();
            po.tag = "Player";
            AddFullCollider(po);
            var sp = po.AddComponent<SnakePlayer>();
            
            // The only reason Movement Speed is internal is to speed up tests
            // We speed up Time delta and slow down frames.
            sp.movementSpeed = 0.025f;
            Time.maximumDeltaTime = 0.0001f;

            var mockGameplayInputs2 = new Mock<IGameplayInputs>();
            sp.gameplayInputs = mockGameplayInputs2.Object;
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            sp.snakeTailPrefab = snakeTailPrefab;
            
            // END SETUP
            
            // Arrange
            yield return new WaitForEndOfFrame();
            GameObject foodObject = SetupSnakeToEatFood(po, mockGameplayInputs2);

            // Act
            // One to eat, One to Move and Keep the tail where it is.
            yield return RunMovementUpdateCycle(sp);
            yield return RunMovementUpdateCycle(sp);

            // Assert
            Assert.AreEqual(sp.SnakeTailPieces.Count(x => x.gameObject.activeSelf), 1);
            Assert.IsTrue(sp.SnakeTailPieces[0].gameObject.activeSelf);
            
            // Tearodwn
            Object.DestroyImmediate(foodObject);
        }
        
        [UnityTest]
        public IEnumerator CollideWithFood_TailPieceIsWhereFoodWas_WhenMovedOffFoodTest()
        {
            //Setup();
            GameObject po = new GameObject();
            po.tag = "Player";
            AddFullCollider(po);
            var sp = po.AddComponent<SnakePlayer>();
            
            // The only reason Movement Speed is internal is to speed up tests
            // We speed up Time delta and slow down frames.
            sp.movementSpeed = 0.025f;
            Time.maximumDeltaTime = 0.0001f;

            var mockGameplayInputs2 = new Mock<IGameplayInputs>();
            sp.gameplayInputs = mockGameplayInputs2.Object;
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            sp.snakeTailPrefab = snakeTailPrefab;
            
            // END SETUP
            
            // Arrange
            yield return new WaitForEndOfFrame();
            GameObject food = SetupSnakeToEatFood(po, mockGameplayInputs2);

            // Eat
            yield return RunMovementUpdateCycle(sp);
            Vector3 expectedPosition = CopyVector3(po.transform.position);

            // Act
            yield return RunMovementUpdateCycle(sp);

            // Assert
            Vector3 tailPiece = sp.SnakeTailPieces[0].gameObject.transform.position;
            Assert.AreEqual(tailPiece, expectedPosition);
            
            // Teardown
            Object.DestroyImmediate(food);
        }
        
        [UnityTest]
        public IEnumerator CollideWithFood_TailGrows_WhenMoreFoodIsEatenTest()
        {
            //Setup();
            GameObject po = new GameObject();
            po.tag = "Player";
            AddFullCollider(po);
            var sp = po.AddComponent<SnakePlayer>();
            
            // The only reason Movement Speed is internal is to speed up tests
            // We speed up Time delta and slow down frames.
            sp.movementSpeed = 0.025f;
            Time.maximumDeltaTime = 0.0001f;

            var mockGameplayInputs2 = new Mock<IGameplayInputs>();
            sp.gameplayInputs = mockGameplayInputs2.Object;
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            sp.snakeTailPrefab = snakeTailPrefab;
            
            // END SETUP
            
            // Arrange
            yield return new WaitForEndOfFrame();

            // Eat
            GameObject food = SetupSnakeToEatFood(po, mockGameplayInputs2);
            yield return RunMovementUpdateCycle(sp);
            Vector3 firstFoodEaten = CopyVector3(po.transform.position);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            // Eat
            GameObject food2 = SetupSnakeToEatFood(po, mockGameplayInputs2);
            yield return RunMovementUpdateCycle(sp);
            Vector3 secondFoodEaten = CopyVector3(po.transform.position);

            // Act
            yield return RunMovementUpdateCycle(sp);

            // Assert
            Vector3 tailPiece = sp.SnakeTailPieces[0].gameObject.transform.position;
            Assert.AreEqual(tailPiece, secondFoodEaten, $"secondFoodEaten == tailPiece | {secondFoodEaten} == {tailPiece}");
            Vector3 tailPiece2 = sp.SnakeTailPieces[1].gameObject.transform.position;
            Assert.AreEqual(tailPiece2, firstFoodEaten, $"firstFoodEaten == tailPiece2 | {firstFoodEaten} == {tailPiece2}");
            
            // Teardown
            Object.DestroyImmediate(food);
            Object.DestroyImmediate(food2);
        }

        #endregion
        
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
            foodGo.AddComponent<TestFood>();
            collider2D = CreateTriggerBoxCollider(foodGo);

            mockGI.Setup(x => x.KeyPressed(GameplayButton.DirectionUp))
                .Returns(true);

            return foodGo;
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
                newGameObject.tag = "SnakeFood";
            }

            return newGameObject;
        }
    }
}