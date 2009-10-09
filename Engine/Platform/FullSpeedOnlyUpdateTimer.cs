using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Engine.Platform
{
    public class FullSpeedOnlyUpdateTimer : ManagedUpdateTimer
    {
        public FullSpeedOnlyUpdateTimer(SystemTimer systemTimer, UpdateListener systemMessageListener)
            :base(systemTimer, systemMessageListener)
        {

        }

        public override bool startLoop()
        {
            if (!systemTimer.initialize())
            {
                return false;
            }

            started = true;
            fireLoopStarted();

            Int64 deltaTime;
            //Int64 totalTime = 0;
            Int64 frameStartTime;
            Int64 lastTime = systemTimer.getCurrentTime();
            //Int64 totalFrameTime;

            while (started)
            {
                frameStartTime = systemTimer.getCurrentTime();
                deltaTime = frameStartTime - lastTime;

                if (deltaTime > maxDelta)
                {
                    deltaTime = maxDelta;
                    fireExceededMaxDelta();
                }
                //totalTime += deltaTime;
                //if (totalTime > fixedFrequency * maxFrameSkip)
                //{
                //    totalTime = fixedFrequency * maxFrameSkip;
                //}

                //while (totalTime >= fixedFrequency)
                //{
                    fireFixedUpdate(deltaTime);
                    //totalTime -= fixedFrequency;
                //}

                fireFullSpeedUpdate(deltaTime);

                lastTime = frameStartTime;

                //cap the framerate if required
                //totalFrameTime = systemTimer.getCurrentTime() - frameStartTime;
                //while (totalFrameTime < framerateCap)
                //{
                //    Thread.Sleep((int)((framerateCap - totalFrameTime) / 1000));
                //    totalFrameTime = systemTimer.getCurrentTime() - frameStartTime;
                //}
            }
            return true;
        }
    }
}
