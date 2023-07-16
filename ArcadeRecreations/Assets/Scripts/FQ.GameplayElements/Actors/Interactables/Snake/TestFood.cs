using System;
using System.Collections;
using FQ.GameObjectPromises;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FQ.GameplayElements.EditorTests
{
    /// <summary>
    /// Exists to remove the collider similar to actual food.
    /// </summary>
    public class TestFood : GameElement
    {
        private bool areActive;

        protected override void BaseStart()
        {
            this.areActive = true;
        }

        protected override void BaseFixedUpdate()
        {
            if (!this.areActive)
            {
                int x = Random.Range(-15, 15);
                int y = Random.Range(-8, 8);
                transform.position = new Vector2(x, y);
                this.areActive = true;
                tag = "SnakeFood";
            }
        }

        protected override void BaseOnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                areActive = false;
            }
        }
    }
}