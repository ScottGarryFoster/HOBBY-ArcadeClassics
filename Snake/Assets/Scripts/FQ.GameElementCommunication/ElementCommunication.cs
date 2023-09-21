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
        private PlayerStatus playerStatus;
    }
}