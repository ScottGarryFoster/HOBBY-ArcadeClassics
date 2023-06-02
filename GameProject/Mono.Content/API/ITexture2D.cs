namespace Mono.Content;

/// <summary>
/// Represents a renderable texture.
/// </summary>
public interface ITexture2D
{
    /// <summary>
    /// True means the texture is loaded.
    /// </summary>
    bool IsLoaded { get; }
    
    /// <summary>
    /// Width of the texture.
    /// </summary>
    int Width { get; set; }
    
    /// <summary>
    /// Height of the texture.
    /// </summary>
    int Height { get; set; }

    /// <summary>
    /// Loads a texture using the given key.
    /// </summary>
    /// <param name="key">Name of the texture within the content. </param>
    /// <returns>True means is loaded. </returns>
    bool Load(string key);
}