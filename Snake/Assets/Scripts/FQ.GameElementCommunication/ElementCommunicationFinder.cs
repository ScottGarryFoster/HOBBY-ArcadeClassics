using UnityEngine;

namespace FQ.GameElementCommunication
{
    /// <summary>
    /// Attempts to find ElementCommunication in the Scene
    /// </summary>
    public class ElementCommunicationFinder : IElementCommunicationFinder
    {
        /// <summary>
        /// Tag for the Game Controller
        /// </summary>
        public const string GameControllerTag = "GameController";
        
        /// <summary>
        /// Will attempt to find Element communication.
        /// </summary>
        /// <returns>The first ElementCommunication found in the scene or null if none are found. </returns>
        public IElementCommunication FindElementCommunication()
        {
            IElementCommunication communication = null;
            
            GameObject controller = GameObject.FindGameObjectWithTag(GameControllerTag);
            if (controller == null)
            {
                Debug.LogWarning($"{typeof(ElementCommunicationFinder)}: " +
                               $"No Object with GameController. " +
                               $"Cannot stop food spawning on player.");
                return communication;
            }
            
            communication = controller.GetComponent<ElementCommunication>();
            if (communication == null)
            {
                Debug.LogWarning($"{typeof(ElementCommunicationFinder)}: " +
                                 $"No {nameof(ElementCommunication)}. " +
                                 $"Cannot stop food spawning on player.");
                return communication;
            }

            return communication;
        }
    }
}