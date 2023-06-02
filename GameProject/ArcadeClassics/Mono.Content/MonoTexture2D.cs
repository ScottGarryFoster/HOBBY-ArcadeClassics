using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Content;

/// <summary>
/// A texture to render in a Mono Game renderer.
/// </summary>
/// <remarks>This is not engineer tested.</remarks>
public class MonoTexture2D : ITexture2D
{
    /// <summary>
    /// True means the texture is loaded.
    /// </summary>
    public bool IsLoaded { get; private set; }

    /// <summary>
    /// Width of the texture.
    /// </summary>
    public int Width
    {
        get
        {
            if (this.texture == null)
            {
                return 0;
            }

            return this.texture.Width;
        }
    }

    /// <summary>
    /// Height of the texture.
    /// </summary>
    public int Height
    {
        get
        {
            if (this.texture == null)
            {
                return 0;
            }

            return this.texture.Height;
        }
    }
    
    /// <summary>
    /// Controls and links to assets.
    /// </summary>
    private readonly IContentManager content;
    
    /// <summary>
    /// A texture to render in a Mono Game renderer.
    /// </summary>
    private Texture2D texture;

    public MonoTexture2D(IContentManager contentManager, string key)
    {
        this.content = contentManager ?? throw new ArgumentNullException(
            $"{typeof(MonoTexture2D)}: {nameof(contentManager)} must not be null. ");
        
        string finalKey = !string.IsNullOrEmpty(key) ? key : throw new ArgumentNullException(
            $"{typeof(MonoTexture2D)}: {nameof(key)} must not be null. ");

        Load(finalKey);
    }
    
    /// <summary>
    /// Loads a texture using the given key.
    /// </summary>
    /// <param name="key">Name of the texture within the content. </param>
    /// <returns>True means is loaded. </returns>
    public bool Load(string key)
    {
        bool loaded = false;
        try
        {
            this.texture = this.content.Load<Texture2D>(key);
            loaded = true;
            this.IsLoaded = true;
        }
        catch
        {
            // TODO: Add logger.
            Console.WriteLine($"{typeof(MonoTexture2D)}: Could not load: {key} of type Texture2D.");
        }

        return loaded;
    }
}