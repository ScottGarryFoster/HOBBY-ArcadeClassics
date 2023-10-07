using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Holds information about the tail piece.
    /// </summary>
    public class SnakeTail : MonoBehaviour, ISnakeTail
    {
        /// <summary>
        /// True this tail piece comes after a border loop.
        /// </summary>
        public bool TailPieceAfterBorder { get; private set; }
        
        /// <summary>
        /// If this tail piece went through a loop, this is the information.
        /// </summary>
        public CollisionPositionAnswer BorderLoopAnswer { get; private set; }

        /// <summary>
        /// Resets the information outwardly expressed by the interface.
        /// </summary>
        public void ResetMetaInfo()
        {
            TailPieceAfterBorder = false;
        }

        /// <summary>
        /// If this tail piece went through a loop, this is the information.
        /// </summary>
        /// <param name="borderInfo">New border information. </param>
        public void GiveNewBorderInfo(CollisionPositionAnswer borderInfo)
        {
            if (borderInfo.Answer == ContextToPositionAnswer.NoValidMovement)
            {
                return;
            }
            
            TailPieceAfterBorder = true;
            BorderLoopAnswer = borderInfo;
        }
    }
}