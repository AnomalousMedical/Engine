using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    public class SpriteFrame
    {
        public SpriteFrame()
        {

        }

        public SpriteFrame(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public float Left;
        public float Top;
        public float Right;
        public float Bottom;
    }

    public class SpriteAnimation
    {
        public SpriteAnimation(long frameTime, params SpriteFrame[] frames)
        {
            this.frameTime = frameTime;
            this.frames = frames;
            this.duration = frameTime * frames.Length;
        }

        public long duration;
        public long frameTime;
        public SpriteFrame[] frames;
    }

    public class Sprite
    {
        private Dictionary<String, SpriteAnimation> animations;
        private SpriteAnimation current;
        private long frameTime;
        private long duration;
        private int frame;

        public Vector3 BaseScale;

        public Sprite()
            : this(new Dictionary<string, SpriteAnimation>()
            {
                { "default", new SpriteAnimation(1, new SpriteFrame[]{ new SpriteFrame()
                    {
                        Left = 0f,
                        Top = 0f,
                        Right = 1f,
                        Bottom = 1f
                    } }) 
                }
            })
        {

        }

        public Sprite(Dictionary<String, SpriteAnimation> animations)
        {
            this.animations = animations;
            SetAnimation(animations.Keys.First());
        }

        public void SetAnimation(String animationName)
        {
            if(!animations.TryGetValue(animationName, out current))
            {
                current = animations.Values.First();
            }
            frameTime = 0;
            duration = current.duration;
        }

        public void Update(Clock clock)
        {
            frameTime += clock.DeltaTimeMicro;
            frameTime %= duration;
            frame = (int)((float)frameTime / duration * current.frames.Length);
        }

        public SpriteFrame GetCurrentFrame()
        {
            return current.frames[frame];
        }
    }
}
