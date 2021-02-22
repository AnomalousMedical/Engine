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
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public static IEnumerable<SpriteFrame> MakeFramesFromHorizontal(float xStep, float yStep, float width, int numFrames, int start = 0)
        {
            var end = start + numFrames;
            for(var i = start; i < end; ++i)
            {
                var step = i * xStep;
                float x = step % width;
                float y = step / width;
                yield return new SpriteFrame()
                {
                    Left = x,
                    Top = y,
                    Right = x + xStep,
                    Bottom = y + yStep
                };
            }
        }
    }

    public class SpriteAnimation
    {
        public SpriteAnimation(long frameTime, SpriteFrame[] frames)
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
