namespace Mono.Content;

/// <summary>
/// Controls and links to assets.
/// </summary>
public interface IContentManager
{
    /// <summary>
    /// The root directory for the content manager.
    /// </summary>
    string RootDirectory { get; set; }
    
    /// <summary>
    /// Load an asset from the given location.
    /// </summary>
    /// <param name="name">Name of the asset. </param>
    /// <typeparam name="T">Type of the asset. </typeparam>
    /// <returns>The asset of the given type. </returns>
    /// <exception cref="System.IO.FileNotFoundException">
    /// Thrown if the file is not in the content.
    /// </exception>
    T Load<T>(string name);
}