using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Connects elements to broad information they might want.
    /// </summary>
    public class ElementCommunication : MonoBehaviour, IElementCommunication
    {
        /// <summary>
        /// Basics about the Player.
        /// </summary>
        public IPlayerStatus PlayerStatus
        {
            get { return playerStatus ??= new PlayerStatus(); }
        }

        /// <summary>
        /// Stores the player status when created.
        /// </summary>
        private IPlayerStatus playerStatus;

        /// <summary>
        /// Locations and information on Collectable items.
        /// </summary>
        public ICollectableStatus CollectableStatus
        {
            get { return collectableStatus ??= new CollectableStatus(); }
        }
        
        /// <summary>
        /// Stores the Collectable Status status when created.
        /// </summary>
        private ICollectableStatus collectableStatus;
    }
}