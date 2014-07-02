using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Threading;

namespace PCPlatform
{
    public class PCUpdateTimer : UpdateTimer
    {
        private OSMessagePump messagePump = new NullMessagePump();
        private Int64 frameStartTime;

        public PCUpdateTimer(SystemTimer timer)
            :base(timer)
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
            Int64 totalTime = 0;
            Int64 lastTime = systemTimer.getCurrentTime();
            Int64 totalFrameTime;

            messagePump.loopStarting();

            while (started)
            {
		        if(!messagePump.processMessages())
		        {
			        frameStartTime = systemTimer.getCurrentTime();
			        deltaTime = frameStartTime - lastTime;

			        if (deltaTime > maxDelta)
			        {
				        deltaTime = maxDelta;
				        fireExceededMaxDelta();
			        }
			        totalTime += deltaTime;
			        if (totalTime > fixedFrequency * maxFrameSkip)
			        {
				        totalTime = fixedFrequency * maxFrameSkip;
			        }

			        //Frame skipping
                    Int64 fixedStartTime = frameStartTime - totalTime + fixedFrequency;
			        while (totalTime >= fixedFrequency)
			        {
				        fireFixedUpdate(fixedStartTime, fixedFrequency);
                        fixedStartTime += fixedFrequency;
				        totalTime -= fixedFrequency;
			        }

			        fireFullSpeedUpdate(frameStartTime, deltaTime);

			        lastTime = frameStartTime;

			        //cap the framerate if required
			        totalFrameTime = systemTimer.getCurrentTime() - frameStartTime;
                    //while (totalFrameTime < framerateCap)
                    //{
                    //    Thread.Sleep((int)((framerateCap - totalFrameTime) / 1000));
                    //    totalFrameTime = systemTimer.getCurrentTime() - frameStartTime;
                    //}
		        }
            }
            messagePump.loopCompleted();
            return true;
        }

        /// <summary>
        /// Reset the last time to be the current time. Call after a long delay to avoid frame skipping.
        /// </summary>
        public override void resetLastTime()
        {
            frameStartTime = systemTimer.getCurrentTime();
        }

        public OSMessagePump MessagePump
        {
            get
            {
                return messagePump;                     
            }
            set
            {
                messagePump = value;
            }
        }
    }
}
