using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Content;

public interface ISprite
{
    /// <summary>
    /// Texture key on the sprite.
    /// </summary>
    string Key { get; }
    
    /// <summary>
    /// World Location of the Sprite.
    /// </summary>
    Vector2 Position { get; set; }
    
    /// <summary>
    /// Width and Height of the Sprite.
    /// </summary>
    Vector2 WidthHeight { get; set; }
    
    /// <summary>
    /// Area to render the texture.
    /// </summary>
    Rectangle TextureSource { get; set; }
    
    /// <summary>
    /// Tint the Sprite is.
    /// <see cref="Color.White"/> is default.
    /// </summary>
    Color TintColor { get; set; }

    /// <summary>
    /// Simple rotation value.
    /// </summary>
    float Rotation { get; set; }

    /// <summary>
    /// Origin point for rotation.
    /// </summary>
    Vector2 OriginForRotation { get; set; }
    
    /// <summary>
    /// Scale value.
    /// </summary>
    Vector2 Scale { get; set; }
    
    /// <summary>
    /// Current sprite effect to draw.
    /// <see cref="SpriteEffects.FlipHorizontally"/> and <see cref="SpriteEffects.FlipVertically"/>
    /// are controlled here.
    /// </summary>
    SpriteEffects Effect { get; set; }
    
    /// <summary>
    /// The render layer.
    /// </summary>
    float Layer { get; }
    
    /// <summary>
    /// Loads a texture from key.
    /// </summary>
    /// <param name="key">Key for the given texture. </param>
    void LoadTextureFromKey(string key);
}