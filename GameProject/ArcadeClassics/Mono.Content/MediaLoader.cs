using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Content;

/// <summary>
/// Loads the given media of the given type.
/// </summary>
public class MediaLoader : IMediaLoader
{
    /// <summary>
    /// Controls and links to assets.
    /// </summary>
    private IContentManager contentManager;

    private Dictionary<string, MonoTexture2D> texture2Ds;

    public MediaLoader(IContentManager contentManager)
    {
        this.contentManager = contentManager ?? throw new ArgumentNullException(
            $"{typeof(MediaLoader)}: {nameof(contentManager)} cannot be null");

        this.texture2Ds = new Dictionary<string, MonoTexture2D>();
    }

    /// <summary>
    /// Loads the given media name of the given type.
    /// </summary>
    /// <param name="key">Key of the media. </param>
    /// <typeparam name="T">
    /// Type of the media.
    /// Keep in mind <see cref="Texture2D"/> might be the origin of <see cref="ITexture2D"/>.
    /// </typeparam>
    /// <returns>Media of the given type or null when it does not exist.</returns>
    public MonoTexture2D LoadTexture2D(string key)
    {
        if (this.texture2Ds.TryGetValue(key, out MonoTexture2D value))
        {
            return value;
        }
        else
        {
            try
            {
                MonoTexture2D newTexture = new MonoTexture2D(this.contentManager, key);
                if (newTexture.IsLoaded)
                {
                    this.texture2Ds.Add(key, newTexture);
                    return newTexture;
                }
            }
            catch (Exception e)
            {
                // TODO: Add logger.
                Console.WriteLine($"{typeof(MediaLoader)}: Could not load: {key}. Exception: {e}");
            }
        }

        return null;
    }

    /// <summary>
    /// Loads the given media name of the given type.
    /// </summary>
    /// <param name="key">Key of the media. </param>
    /// <typeparam name="T">
    /// Type of the media.
    /// Keep in mind <see cref="Texture2D"/> might be the origin of <see cref="ITexture2D"/>.
    /// </typeparam>
    /// <returns>Media of the given type or null when it does not exist.</returns>
    public ITexture2D LoadITexture2D(string key)
    {
        return LoadTexture2D(key);
    }
}