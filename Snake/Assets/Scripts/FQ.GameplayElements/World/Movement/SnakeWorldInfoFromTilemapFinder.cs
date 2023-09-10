using System.Linq;
using UnityEngine;

namespace FQ.GameplayElements
{
    /// <summary>
    /// Finds World Info in the Scene.
    /// </summary>
    public class SnakeWorldInfoFromTilemapFinder : IWorldInfoFromTilemapFinder
    {
        /// <summary>
        /// The tag used for Snake Border.
        /// </summary>
        public const string SnakeBorderTag = "SnakeBorder";
        
        /// <summary>
        /// Attempts to find <see cref="IWorldInfoFromTilemap"/> in the Scene if possible.
        /// </summary>
        /// <returns>The first <see cref="IWorldInfoFromTilemap"/> found or <c>null</c> if none exists. </returns>
        public IWorldInfoFromTilemap FindWorldInfo()
        {
            IWorldInfoFromTilemap returnWorldInfo = null;
            GameObject snakeBorder = GameObject.FindGameObjectWithTag(SnakeBorderTag);
            if (snakeBorder == null)
            {
                Debug.LogWarning($"{typeof(SnakeWorldInfoFromTilemapFinder)}: " +
                                 $"No tag with {SnakeBorderTag}.");
                return returnWorldInfo;
            }

            for (int i = 0; i < snakeBorder.transform.childCount; ++i)
            {
                GameObject current = snakeBorder.transform.GetChild(i).gameObject;
                if (current.name == "Border")
                {
                    returnWorldInfo = current.GetComponent<WorldInfoInfoFromTilemap>();
                    break;
                }
            }

            if (returnWorldInfo == null)
            {
                Debug.LogWarning($"{typeof(SnakeWorldInfoFromTilemapFinder)}: " +
                                 $"Could not find a child called Border.");
            }

            return returnWorldInfo;
        }
    }
}