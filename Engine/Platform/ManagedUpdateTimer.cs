using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Engine.Platform
{
    public class ManagedUpdateTimer : UpdateTimer
    {
        protected UpdateListener systemMessageListener;
        private Int64 frameStartTime;

        public ManagedUpdateTimer(SystemTimer systemTimer, UpdateListener systemMessageListener)
            :base(systemTimer)
        {
            this.systemMessageListener = systemMessageListener;
        }

        /// <summary>
        /// Starts the loop iterating at the set update frequency.  This function will return
        /// once the loop is stopped.
        /// </summary>
        public override bool startLoop()
        {
            if (!systemTimer.initialize())
            {
                return false;
            }

            started = true;
            fireLoopStarted();

            Int64 deltaTime;
            Int64 lastTime = systemTimer.getCurrentTime();
            Int64 totalFrameTime;

            systemTimer.Accurate = framerateCap > 0;

            while (started)
            {
                frameStartTime = systemTimer.getCurrentTime();
                deltaTime = frameStartTime - lastTime;

                if (deltaTime > maxDelta)
                {
                    deltaTime = maxDelta;
                    fireExceededMaxDelta();
                }

                fireUpdate(frameStartTime, deltaTime);

                lastTime = frameStartTime;

                //cap the framerate if required
                totalFrameTime = systemTimer.getCurrentTime() - frameStartTime;
                while (totalFrameTime < framerateCap)
                {
                    Thread.Sleep((int)((framerateCap - totalFrameTime) / 1000));
                    totalFrameTime = systemTimer.getCurrentTime() - frameStartTime;
                }
            }
            return true;
        }

        public override void OnIdle()
        {
            throw new NotImplementedException("Managed Update Timer does not implement OnIdle.");
        }

        /// <summary>
        /// Reset the last time to be the current time. Call after a long delay to avoid frame skipping.
        /// </summary>
        public override void resetLastTime()
        {
            frameStartTime = systemTimer.getCurrentTime();
        }

        /// <summary>
        /// Fire an update.
        /// </summary>
        protected override void fireUpdate(Int64 currentTimeMicro, Int64 deltaTimeMicro)
        {
            base.fireUpdate(currentTimeMicro, deltaTimeMicro);
            systemMessageListener.sendUpdate(clock);
        }

        /// <summary>
        /// Fire the loopStaring event.
        /// </summary>
        protected override void fireLoopStarted()
        {
            base.fireLoopStarted();
            systemMessageListener.loopStarting();
        }

        /// <summary>
        /// Fire the exceededMaxDelta event.
        /// </summary>
        protected override void fireExceededMaxDelta()
        {
            base.fireExceededMaxDelta();
            systemMessageListener.exceededMaxDelta();
        }
    }
}
