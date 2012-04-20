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
    public abstract class UpdateTimer
    {
        protected List<UpdateListener> fixedListeners = new List<UpdateListener>();
        protected List<UpdateListener> fullSpeedListeners = new List<UpdateListener>();

        protected Clock clock = new Clock();
        protected SystemTimer systemTimer;

        protected Int64 fixedFrequency;
        protected Int64 maxDelta;
        protected int maxFrameSkip;
        protected Int64 framerateCap = 0; //The amount of time between frames for the framerate cap.

        protected bool started = false;

        private int fixedUpdateIndex = -1;

        /// <summary>
        /// Create a new UpdateTimer. The SystemMesssageListener field specifies
        /// an UpdateListener that processes the OS message loop. This will be
        /// called for every fixed and full speed update to process messages as
        /// much as possible.
        /// </summary>
        /// <param name="systemTimer">The SystemTimer to get high performance time measurements from.</param>
        /// <param name="systemMessageListener">The UpdateListener that processses system messages.</param>
        public UpdateTimer(SystemTimer systemTimer)
        {
            this.systemTimer = systemTimer;
            fixedFrequency = 1000000 / 60;
            maxDelta = 100000;
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
            int index = fixedListeners.IndexOf(listener);
            fixedListeners.RemoveAt(index);
            //Adjust the iteration index backwards if the element being removed is before or on the index.
            //This way nothing gets skipped.
            if (index != -1 && index <= fixedUpdateIndex)
            {
                --fixedUpdateIndex;
            }
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
        public abstract bool startLoop();

        /// <summary>
        /// Stops the loop.
        /// </summary>
        public void stopLoop()
        {
            started = false;
        }

        /// <summary>
        /// Reset the last time to be the current time. Call after a long delay to avoid frame skipping.
        /// </summary>
        public abstract void resetLastTime();

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
        /// is greater than this value it will be clamped. The default is 100000 ms
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
                    return 1000000 / framerateCap;
                }
                return 0;
            }
            set
            {
                if (value > 0)
                {
                    framerateCap = 1000000 / value;
                }
                else
                {
                    framerateCap = 0;
                }
            }
        }

        public Int64 ElapsedTime
        {
            get
            {
                return systemTimer.getCurrentTime();
            }
        }

        /// <summary>
        /// Fire a fixed update.
        /// </summary>
        protected virtual void fireFixedUpdate(long time)
        {
            clock.setTimeMicroseconds(time);

            //Iterate manually, this way fixedListeners can be added/removed during the iteration of this loop.
            //If a listener is removed the fixedUpdateIndex will be adjusted if needed.
            for (fixedUpdateIndex = 0; fixedUpdateIndex < fixedListeners.Count; fixedUpdateIndex++)
            {
                fixedListeners[fixedUpdateIndex].sendUpdate(clock);
            }
            fixedUpdateIndex = -1;
        }

        /// <summary>
        /// Fire a full speed update.
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last full speed update in seconds.</param>
        protected virtual void fireFullSpeedUpdate(Int64 deltaTime)
        {
            clock.setTimeMicroseconds(deltaTime);
            foreach (UpdateListener fullSpeedListener in fullSpeedListeners)
            {
                fullSpeedListener.sendUpdate(clock);
            }
        }

        /// <summary>
        /// Fire the loopStaring event.
        /// </summary>
        protected virtual void fireLoopStarted()
        {
            foreach (UpdateListener fixedListener in fixedListeners)
            {
                fixedListener.loopStarting();
            }
            foreach (UpdateListener fullSpeedListener in fullSpeedListeners)
            {
                fullSpeedListener.loopStarting();
            }
        }

        /// <summary>
        /// Fire the exceededMaxDelta event.
        /// </summary>
        protected virtual void fireExceededMaxDelta()
        {
            foreach (UpdateListener fixedListener in fixedListeners)
            {
                fixedListener.exceededMaxDelta();
            }
            foreach (UpdateListener fullSpeedListener in fullSpeedListeners)
            {
                fullSpeedListener.exceededMaxDelta();
            }
        }
    }
}
