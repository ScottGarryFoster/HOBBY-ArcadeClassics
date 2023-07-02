using System;
using System.Collections;
using UnityEngine;

namespace FQ.GameplayElements.EditorTests
{
    /// <summary>
    /// Exists to remove the collider similar to actual food.
    /// </summary>
    public class TestFood : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && CompareTag("SnakeFood"))
            {
                Debug.Log("TestFood");
            }
        }
    }
}