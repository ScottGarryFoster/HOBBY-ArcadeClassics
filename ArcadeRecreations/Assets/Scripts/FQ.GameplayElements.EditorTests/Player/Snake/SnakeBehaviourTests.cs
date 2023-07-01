using System;
using System.Collections;
using System.Linq;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace FQ.GameplayElements.EditorTests
{
    public class SnakeBehaviourTests
    {
        private ISnakeBehaviour testClass;
        private GameObject playerObject;
        
        [SetUp]
        public void Setup()
        {
            this.playerObject = new GameObject();
            this.testClass = new SnakeBehaviour(this.playerObject);
            
            /*this.playerObject.tag = "Player";
            AddFullCollider(this.playerObject);
            this.snakePlayer = this.playerObject.AddComponent<SnakePlayer>();
            
            // The only reason Movement Speed is internal is to speed up tests
            // We speed up Time delta and slow down frames.
            this.snakePlayer.movementSpeed = 0.025f;
            Time.maximumDeltaTime = 0.0001f;

            this.mockGameplayInputs = new Mock<IGameplayInputs>();
            this.snakePlayer.gameplayInputs = this.mockGameplayInputs.Object;
            
            SnakeTail snakeTailPrefab = Resources.Load<SnakeTail>("Actors/Snake/SnakeTail");
            this.snakePlayer.snakeTailPrefab = snakeTailPrefab;*/
        }

        [Test]
        public void OnConstruction_ThrowsNullArgumentException_WhenNullGameObjectGivenTest()
        {
            // Arrange
            GameObject given = null;
            
            // Act Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SnakeBehaviour(given);
            });
        }
        
        //#region SnakeTail

        [Test]
        public void Setup_Creates199TailPieces_WhenGivenAPrefabTest()
        {
            // Arrange
            int pieces = SnakePlayer.MaxTailSize; // Unlink this

            // Act
            this.testClass.Start();

            // Assert
            // Will need to initiate this differently probably with a mock.
            Assert.IsNotNull(this.testClass.SnakeTailPieces);
            Assert.AreEqual(pieces, this.testClass.SnakeTailPieces.Length);
            for (int i = 0; i < pieces; ++i)
            {
                Assert.IsNotNull(this.testClass.SnakeTailPieces[i]);
            }
        }
        
        /*[UnityTest]
        public IEnumerator Setup_PlacesTailWithHeadPiece_WhenGivenAPrefabTest()
        {
            Setup();
            
            // Arrange
            var givenPosition = new Vector3(1, 2, 3);
            this.playerObject.transform.position = givenPosition;

            // Act
            yield return new WaitForEndOfFrame();

            // Assert
            for (int i = 0; i < SnakePlayer.MaxTailSize; ++i)
            {
                Assert.AreEqual(
                    this.snakePlayer.SnakeTailPieces[i].transform.position, 
                    givenPosition);
            }
        }
        
        [UnityTest]
        public IEnumerator Setup_UnenablesTheTailPieces_WhenGivenAPrefabTest()
        {
            Setup();
            
            // Arrange

            // Act
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.IsFalse(this.snakePlayer.SnakeTailPieces.Any(x => x.gameObject.activeSelf));
        }
        
        /// <summary>
        /// Should move to food but not grow until another update as Movement speed is not complete.
        /// See <see href="https://docs.unity3d.com/Manual/ExecutionOrder.html"/> for details.
        /// </summary>
        [UnityTest]
        public IEnumerator CollideWithFood_DoesNotEnableTalkPiece_WhenFoodPlacedInFrontOfPlayerAndPlayerMovedInToTest()
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

            // Act
            yield return RunMovementUpdateCycle(sp);
            yield return new WaitForFixedUpdate();

            // Assert
            Assert.AreEqual(sp.SnakeTailPieces.Count(x => x.gameObject.activeSelf), 0);
            
            // Cleanup
            Object.DestroyImmediate(food);
        }
        
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
    }
}