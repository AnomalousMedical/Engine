using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    public static class SpriteBuilder
    {
        public static SpriteFrame[] CreateAnimatedSprite(int width, int height, int framesPerRow, int totalFrames)
        {
            var frames = new SpriteFrame[totalFrames];
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            var framesPerColumn = totalFrames / framesPerRow;
            if(totalFrames % framesPerRow > 0)
            {
                ++framesPerColumn;
            }
            float SpriteStepX = (float)width / (framesPerRow * width);
            float SpriteStepY = (float)height / (framesPerColumn * height);

            for (int i = 0; i < totalFrames; ++i)
            {
                int xIndex = i % framesPerRow;
                int yIndex = i / framesPerRow;
                var frame = new SpriteFrame(SpriteStepX * xIndex, SpriteStepY * yIndex, SpriteStepX * (xIndex + 1), SpriteStepY * (yIndex + 1))
                {
                    Attachments = new List<SpriteFrameAttachment>()
                    {
                        SpriteFrameAttachment.FromFramePosition(halfWidth, halfHeight, 0, width, height), //Center
                    }
                };
                frames[i] = frame;
            }

            return frames;
        }
    }
}
