using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FQ.GameplayElements.EditorTests
{
    /// <summary>
    /// Exists to remove the collider similar to actual food.
    /// </summary>
    public class TestFood : MonoBehaviour
    {
        private bool areActive;

        private void Start()
        {
            this.areActive = true;
        }

        private void FixedUpdate()
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                areActive = false;
            }
        }
    }
}