using Engine.Platform;

namespace DiligentEngine.RT.Sprites
{
    public interface ISprite
    {
        SpriteFrame GetCurrentFrame();
        void SetAnimation(string animationName);
        void Update(Clock clock);
    }
}