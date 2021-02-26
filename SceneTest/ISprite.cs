using Engine.Platform;

namespace SceneTest
{
    public interface ISprite
    {
        SpriteFrame GetCurrentFrame();
        void SetAnimation(string animationName);
        void Update(Clock clock);
    }
}