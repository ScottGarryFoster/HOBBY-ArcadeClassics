using System;
using System.Collections;
using FQ.GameObjectPromises;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Exists to remove the collider similar to actual food.
    /// </summary>
    public class SnakeFood : GameElement
    {
        private bool areActive;

        private Tilemap snakeArea;
        
        protected override void BaseStart()
        {
            this.areActive = true;

            GameObject snakeFoodAreaGO = GameObject.FindGameObjectWithTag("SnakeFoodArea");
            snakeArea = snakeFoodAreaGO.GetComponent<Tilemap>();
        }

        protected override void BaseFixedUpdate()
        {
            if (!this.areActive)
            {
                MoveToRandomValidLocation();

                this.areActive = true;
                tag = "SnakeFood";
            }
        }

        /// <summary>
        /// Moves to a random but also valid location for food.
        /// </summary>
        private void MoveToRandomValidLocation()
        {
            Vector3Int origin = snakeArea.origin;
            Vector3Int size = snakeArea.size;
            while (true)
            {
                int x = Random.Range(origin.x, origin.x + size.x);
                int y = Random.Range(origin.y, origin.y + size.y);
                transform.position = new Vector2(x, y);

                if (snakeArea == null)
                {
                    break;
                }

                if (snakeArea.HasTile(new Vector3Int(x, y)))
                {
                    break;
                }
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