using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FQ.GameplayInputs;
using UnityEngine;

namespace FQ.GameplayElements.Actors.Player
{
    /// <summary>
    /// General player with some actor behaviours.
    /// </summary>
    public class InteractableActor : MonoBehaviour
    {
        internal IGameplayInputs gameplayInputs;

        private Vector2 originalPosition;
        private Vector2Int gridPosition;
        private float deltaDelay;
        
        // Start is called before the first frame update
        void Start()
        {
            this.originalPosition = this.transform.position;
            this.gridPosition = new Vector2Int(0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            deltaDelay += Time.deltaTime;
            if (deltaDelay >= 0.2f)
            {
                deltaDelay -= 0.2f;
                if (this.gameplayInputs.KeyPressed(EGameplayButton.DirectionDown))
                {
                    this.gridPosition.y--;
                }

                this.transform.position = new Vector3(this.gridPosition.x, this.gridPosition.y);
            }
        }
    }

}