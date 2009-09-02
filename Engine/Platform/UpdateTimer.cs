using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Logging;

namespace Engine.Platform
{
    /// <summary>
    /// This class is an abstract base class for a timer.
    /// </summary>
    public class UpdateTimer
    {
        private List<UpdateListener> fixedListeners = new List<UpdateListener>();
        private List<UpdateListener> fullSpeedListeners = new List<UpdateListener>();
        private UpdateListener systemMessageListener;

        private Clock clock = new Clock();
        private SystemTimer systemTimer;

        Int64 fixedFrequency;
        Int64 maxDelta;
        int maxFrameSkip;
        Int64 framerateCap; //The amount of time between frames for the framerate cap.

        bool started = false;

        /// <summary>
        /// Create a new UpdateTimer. The SystemMesssageListener field specifies
        /// an UpdateListener that processes the OS message loop. This will be
        /// called for every fixed and full speed update to process messages as
        /// much as possible.
        /// </summary>
        /// <param name="systemTimer">The SystemTimer to get high performance time measurements from.</param>
        /// <param name="systemMessageListener">The UpdateListener that processses system messages.</param>
        public UpdateTimer(SystemTimer systemTimer, UpdateListener systemMessageListener)
        {
            this.systemTimer = systemTimer;
            this.systemMessageListener = systemMessageListener;
            fixedFrequency = 1000 / 60;
            maxDelta = 100;
            maxFrameSkip = 7;
        }

        /// <summary>
        /// Add an update listener to get updates from the fixed updater part of the timer.
        /// </summary>
        /// <param name="listener">The listener to add</param>
        public void addFixedUpdateListener(UpdateListener listener)
        {
            fixedListeners.Add(listener);
        }

        /// <summary>
        /// Remove an update listener from the fixed updater part of the timer.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void removeFixedUpdateListener(UpdateListener listener)
        {
            fixedListeners.Remove(listener);
        }

        /// <summary>
        /// Add a listener that will be updated as fast a possible by the loop.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void addFullSpeedUpdateListener(UpdateListener listener)
        {
            fullSpeedListeners.Add(listener);
        }

        /// <summary>
        /// Remove a listener from the full speed loop.
        /// </summary>
        /// <param name="listener">The listener to remove</param>
        public void removeFullSpeedUpdateListener(UpdateListener listener)
        {
            fullSpeedListeners.Remove(listener);
        }

        /// <summary>
        /// Starts the loop iterating at the set update frequency.  This function will return
        /// once the loop is stopped.
        /// </summary>
        public bool startLoop()
        {
            if (!systemTimer.initialize())
            {
                return false;
            }

            started = true;
            fireLoopStarted();
            
            Int64 deltaTime;
            Int64 totalTime = 0;
            Int64 currentTime;
            Int64 lastTime = systemTimer.getCurrentTime();

            while (started)
            {
                currentTime = systemTimer.getCurrentTime();
                deltaTime = currentTime - lastTime;//systemTimer.getDelta();

                ////If the time is faster than the framerate cap sleep for the difference.
                ////This is not exact, but any error will be handled by the rest of the timer.
                //while (deltaTime < framerateTime)
                //{
                //    sleepTime = framerateTime - deltaTime + extraSleepTime;
                //    if (sleepTime < 0.0)
                //    {
                //        sleepTime = 0.0;
                //    }
                //    Thread.Sleep((int)((sleepTime) * 1000));
                //    sleptTime = 0.0;// systemTimer.getDelta();
                //    Log.Debug("Sleep {0} slept {1} startdelta {2} enddelta {3} frametime {4} extrasleep {5}", sleepTime, sleptTime, deltaTime, deltaTime + sleptTime, framerateTime, extraSleepTime);
                //    deltaTime += sleptTime;
                //    sleptThisFrame = true;
                //}

                //Log.Debug("continuing frame");

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

                while (totalTime >= fixedFrequency)
                {
                    fireFixedUpdate();
                    totalTime -= fixedFrequency;
                }

                fireFullSpeedUpdate(deltaTime);

                lastTime = currentTime;

                //If sleep was called adjust the framerate time as required to run at the framerate cap.
                //Sometimes sleep takes too long and adjusting the time this way allows for sleep to take
                //longer but the cap to be maintained.
                //if (sleptThisFrame)
                //{
                //    framerateTime -= deltaTime - framerateCap;
                //    //Make sure the framerate does not go to 0 or negative or the cap will disable.
                //    //If that happens just reset the time to the original cap.
                //    if (framerateTime <= 0.0)
                //    {
                //        framerateTime = framerateCap;
                //    }
                //    sleptThisFrame = false;
                //}
            }
            return true;
        }

        /// <summary>
        /// Stops the loop.
        /// </summary>
        public void stopLoop()
        {
            started = false;
        }

        /// <summary>
        /// Sets the frequency of the fixed updates in milliseconds. The default is
        /// 1/60 (60Hz).
        /// </summary>
        public Int64 FixedFrequency
        {
            get
            {
                return fixedFrequency;
            }
            set
            {
                fixedFrequency = value;
            }
        }

        /// <summary>
        /// Set the maximum delta that the timer can report. If the true delta
        /// is greater than this value it will be clamped. The default is 100ms
        /// (10 fps). This will cause the simulation to run slow if it is
        /// running at less than the max delta.
        /// </summary>
        public Int64 MaxDelta
        {
            get
            {
                return maxDelta;
            }
            set
            {
                maxDelta = value;
            }
        }

        /// <summary>
        /// Sets the maximum number of frames that can be skipped in the full
        /// speed updater if the fixed updater is running slow. The default is
        /// 7.
        /// </summary>
        public int MaxFrameSkip
        {
            get
            {
                return maxFrameSkip;
            }
            set
            {
                maxFrameSkip = value;
            }
        }

        public Int64 FramerateCap
        {
            get
            {
                if (framerateCap > 0)
                {
                    return 1000 / framerateCap;
                }
                return 0;
            }
            set
            {
                if (value > 0)
                {
                    framerateCap = 1000 / value;
                }
                else
                {
                    framerateCap = 0;
                }
            }
        }

        /// <summary>
        /// Fire a fixed update.
        /// </summary>
        protected void fireFixedUpdate()
        {
            clock.setTimeMilliseconds(FixedFrequency);
            foreach (UpdateListener fixedListener in fixedListeners)
            {
                fixedListener.sendUpdate(clock);
            }
            systemMessageListener.sendUpdate(clock);
        }

        /// <summary>
        /// Fire a full speed update.
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last full speed update in seconds.</param>
        protected void fireFullSpeedUpdate(Int64 deltaTime)
        {
            clock.setTimeMilliseconds(deltaTime);
            foreach (UpdateListener fullSpeedListener in fullSpeedListeners)
            {
                fullSpeedListener.sendUpdate(clock);
            }
            systemMessageListener.sendUpdate(clock);
        }

        /// <summary>
        /// Fire the loopStaring event.
        /// </summary>
        protected void fireLoopStarted()
        {
            foreach (UpdateListener fixedListener in fixedListeners)
            {
                fixedListener.loopStarting();
            }
            foreach (UpdateListener fullSpeedListener in fullSpeedListeners)
            {
                fullSpeedListener.loopStarting();
            }
            systemMessageListener.loopStarting();
        }

        /// <summary>
        /// Fire the exceededMaxDelta event.
        /// </summary>
        protected void fireExceededMaxDelta()
        {
            foreach (UpdateListener fixedListener in fixedListeners)
            {
                fixedListener.exceededMaxDelta();
            }
            foreach (UpdateListener fullSpeedListener in fullSpeedListeners)
            {
                fullSpeedListener.exceededMaxDelta();
            }
            systemMessageListener.exceededMaxDelta();
        }
    }
}
