namespace FQ.GameplayElements
{
    /// <summary>
    /// Finds World Info in the Scene.
    /// </summary>
    public interface IWorldInfoFromTilemapFinder
    {
        /// <summary>
        /// Attempts to find <see cref="IWorldInfoFromTilemap"/> in the Scene if possible.
        /// </summary>
        /// <returns>The first <see cref="IWorldInfoFromTilemap"/> found or <c>null</c> if none exists. </returns>
        IWorldInfoFromTilemap FindWorldInfo();
    }
}