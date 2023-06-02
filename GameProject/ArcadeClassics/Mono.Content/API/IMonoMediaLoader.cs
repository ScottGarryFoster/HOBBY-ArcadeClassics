namespace Mono.Content;

/// <summary>
/// Loads the given media of the given type in Mono-form.
/// </summary>
public interface IMonoMediaLoader
{
    /// <summary>
    /// Loads the given media name of the given type.
    /// </summary>
    /// <param name="key">Key of the media. </param>
    /// <typeparam name="T">
    /// Type of the media.
    /// Keep in mind <see cref="Texture2D"/> might be the origin of <see cref="ITexture2D"/>.
    /// </typeparam>
    /// <returns>Media of the given type or null when it does not exist.</returns>
    MonoTexture2D LoadTexture2D(string key);
}