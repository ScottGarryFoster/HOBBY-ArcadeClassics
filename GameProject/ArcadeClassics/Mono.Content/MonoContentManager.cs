using System;
using Microsoft.Xna.Framework.Content;

namespace Mono.Content;

/// <summary>
/// Interface in front of <see cref="ContentManager"/>.
/// </summary>
/// <remarks>This is not engineer tested.</remarks>
public class MonoContentManager : IContentManager
{
    /// <summary>
    /// The root directory for the content manager.
    /// </summary>
    public string RootDirectory
    {
        get => this.contentManager.RootDirectory;
        set => this.contentManager.RootDirectory = value;
    }

    /// <summary>
    /// Monogame version of the content Manager;
    /// </summary>
    private ContentManager contentManager;
    
    public MonoContentManager(ContentManager contentManager)
    {
        this.contentManager = contentManager ?? throw new ArgumentNullException(
            $"{typeof(MonoContentManager)}: " +
            $"{nameof(this.contentManager)} must not be null");
    }
    
    /// <summary>
    /// Load an asset from the given location.
    /// </summary>
    /// <param name="name">Name of the asset. </param>
    /// <typeparam name="T">Type of the asset. </typeparam>
    /// <returns>The asset of the given type. </returns>
    /// <exception cref="System.IO.FileNotFoundException">
    /// Thrown if the file is not in the content.
    /// </exception>
    public T Load<T>(string name)
    {
        return this.contentManager.Load<T>(name);
    }
}