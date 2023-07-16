using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FQ.GameObjectPromises;
using FQ.GameplayInputs;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// General player with some actor behaviours.
    /// </summary>
    public class InteractableActor : GameElement, IActorActiveStats
    {
        /// <summary>
        /// How fast the actor moves each time the actor moves.
        /// </summary>
        public float MovementSpeed => movementSpeed;

        [SerializeField]
        private LiveGameplayInputs input;

        /// <summary>
        /// How fast the actor moves each time the actor moves.
        /// </summary>
        /// <remarks>Internal for testing purposes. </remarks>
        [SerializeField]
        internal float movementSpeed = 0.2f;

        /// <summary>
        /// Interaction with Gameplay Inputs.
        /// </summary>
        /// <remarks>Injected. </remarks>
        internal IGameplayInputs gameplayInputs;

        /// <summary>
        /// Start called by Unity.
        /// </summary>
        protected override void BaseStart()
        {
            if (gameplayInputs == null)
            {
                gameplayInputs = input;
            }
        }
    }

}