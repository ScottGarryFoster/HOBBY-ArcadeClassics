using System;
using System.Collections;
using System.Linq;
using FQ.GameObjectPromises;
using Mono.Collections.Generic;
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
        /// <summary>
        /// True means active.
        /// </summary>
        private bool areActive;

        /// <summary>
        /// The area we may spawn.
        /// </summary>
        private Vector3Int[] safeArea;
        
        protected override void BaseStart()
        {
            this.areActive = true;

            SetupAndAcquireSafeArea();
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
        /// Collects the world information if in the scene.
        /// </summary>
        /// <returns>World Info or Null if not found. </returns>
        private IWorldInfoFromTilemap GetWorldInfo()
        {
            GameObject[] borders = GameObject.FindGameObjectsWithTag("SnakeBorder");
            GameObject border = borders.FirstOrDefault(x => x.name == "Border");
            if (border == null)
            {
                return null;
            }

            return border.GetComponent<WorldInfoInfoFromTilemap>();
        }

        /// <summary>
        /// Moves to a random but also valid location for food.
        /// </summary>
        private void MoveToRandomValidLocation()
        {
            if (this.safeArea == null)
            {
                return;
            }
            
            int max = this.safeArea.Length;
            int i = Random.Range(0, max);
            transform.position = this.safeArea[i];
        }

        protected override void BaseOnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                areActive = false;
            }
        }
        
        /// <summary>
        /// Grabs and sets up the safe area which is the area we can spawn.
        /// </summary>
        private void SetupAndAcquireSafeArea()
        {
            IWorldInfoFromTilemap worldInfo = GetWorldInfo();
            if (worldInfo == null)
            {
                Debug.LogError($"{typeof(SnakeFood)}: World info null");
                return;
            }

            this.safeArea = worldInfo.GetTravelableArea();
            if (this.safeArea == null)
            {
                Debug.LogError($"{typeof(SnakeFood)}: safeArea null");
            }
        }
    }
}