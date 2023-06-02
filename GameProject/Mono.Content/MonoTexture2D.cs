using Microsoft.Xna.Framework.Graphics;

namespace Mono.Content;

/// <summary>
/// A texture to render in a Mono Game renderer.
/// </summary>
public class MonoTexture2D : ITexture2D
{
    /// <summary>
    /// True means the texture is loaded.
    /// </summary>
    public bool IsLoaded { get; }
    
    /// <summary>
    /// Width of the texture.
    /// </summary>
    public int Width { get; set; }
    
    /// <summary>
    /// Height of the texture.
    /// </summary>
    public int Height { get; set; }

    private Texture2D texture;

    public MonoTexture2D(string key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(
                $"{typeof(MonoTexture2D)}: {nameof(key)} must not be null. ");
        }

        Load(key);
    }
    
    /// <summary>
    /// Loads a texture using the given key.
    /// </summary>
    /// <param name="key">Name of the texture within the content. </param>
    /// <returns>True means is loaded. </returns>
    public bool Load(string key)
    {
        try
        {
            this.texture = Content.Load<Texture2D>(key);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return false;
    }
}