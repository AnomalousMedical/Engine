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
        protected List<UpdateListener> updateListeners = new List<UpdateListener>();
        private Dictionary<String, UpdateListenerWithBackgrounding> multiThreadedWorkerListeners = new Dictionary<string, UpdateListenerWithBackgrounding>();

        protected Clock clock = new Clock();
        protected SystemTimer systemTimer;

        protected Int64 maxDelta;
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
            maxDelta = 100000;
        }

        /// <summary>
        /// Add an update listener to get updates from the fixed updater part of the timer.
        /// </summary>
        /// <param name="listener">The listener to add</param>
        public void addUpdateListener(UpdateListener listener)
        {
            updateListeners.Add(listener);
        }

        /// <summary>
        /// Remove an update listener from the fixed updater part of the timer.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void removeUpdateListener(UpdateListener listener)
        {
            int index = updateListeners.IndexOf(listener);
            if (index != -1)
            {
                updateListeners.RemoveAt(index);
                //Adjust the iteration index backwards if the element being removed is before or on the index.
                //This way nothing gets skipped.
                if (index <= fixedUpdateIndex)
                {
                    --fixedUpdateIndex;
                }
            }
        }

        /// <summary>
        /// Add a listener that will be updated as fast a possible by the loop.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void addBackgroundUpdateListener(String name, BackgroundUpdateListener listener)
        {
            try
            {
                UpdateListenerWithBackgrounding bgListenerWorker = multiThreadedWorkerListeners[name];
                bgListenerWorker.addBackgroundListener(listener);
            }
            catch (KeyNotFoundException)
            {
                Log.Warning("Could not find background worker supporting update named '{0}'. Be sure to add the update listeners that support background tasks before the background tasks. Listener will not update.", name);
            }
        }

        /// <summary>
        /// Remove a listener from the full speed loop.
        /// </summary>
        /// <param name="listener">The listener to remove</param>
        public void removeBackgroundUpdateListener(String name, BackgroundUpdateListener listener)
        {
            try
            {
                UpdateListenerWithBackgrounding bgListenerWorker = multiThreadedWorkerListeners[name];
                bgListenerWorker.removeBackgroundListener(listener);
            }
            catch (KeyNotFoundException)
            {
                //This is ok, the foreground task may have been removed first
            }
        }

        /// <summary>
        /// Add a listener that will be updated as fast a possible by the loop. Also will enable background tasks to run at the same time
        /// as this listener. Those can be added using addBackgroundUpdateListener and giving the same name given here.
        /// You must add the main thread update listener before adding the updates that will run in the background.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void addUpdateListenerWithBackgrounding(String name, UpdateListener listener)
        {
            UpdateListenerWithBackgrounding bgListenerWorker = new UpdateListenerWithBackgrounding(listener);
            multiThreadedWorkerListeners.Add(name, bgListenerWorker);
            addUpdateListener(bgListenerWorker);
        }

        /// <summary>
        /// Remove a listener from the full speed loop.
        /// </summary>
        /// <param name="listener">The listener to remove</param>
        public void removeUpdateListenerWithBackgrounding(String name, UpdateListener listener)
        {
            UpdateListenerWithBackgrounding bgListenerWorker = multiThreadedWorkerListeners[name];
            removeUpdateListener(bgListenerWorker);
            bgListenerWorker.Dispose();
            multiThreadedWorkerListeners.Remove(name);
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
        /// Reset the last time to be the current time. Call after a long delay to avoid falling way behind.
        /// </summary>
        public abstract void resetLastTime();

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

        /// <summary>
        /// Get the elapsed time in microseconds
        /// </summary>
        public Int64 ElapsedTime
        {
            get
            {
                return systemTimer.getCurrentTime();
            }
        }

        /// <summary>
        /// Fire an update.
        /// </summary>
        protected virtual void fireUpdate(Int64 currentTimeMicro, Int64 deltaTimeMicro)
        {
            clock.setTimeMicroseconds(currentTimeMicro, deltaTimeMicro);

            //Iterate manually, this way fixedListeners can be added/removed during the iteration of this loop.
            //If a listener is removed the fixedUpdateIndex will be adjusted if needed.
            for (fixedUpdateIndex = 0; fixedUpdateIndex < updateListeners.Count; fixedUpdateIndex++)
            {
                updateListeners[fixedUpdateIndex].sendUpdate(clock);
            }
            fixedUpdateIndex = -1;
        }

        /// <summary>
        /// Fire the loopStaring event.
        /// </summary>
        protected virtual void fireLoopStarted()
        {
            foreach (UpdateListener listener in updateListeners)
            {
                listener.loopStarting();
            }
        }

        /// <summary>
        /// Fire the exceededMaxDelta event.
        /// </summary>
        protected virtual void fireExceededMaxDelta()
        {
            foreach (UpdateListener listener in updateListeners)
            {
                listener.exceededMaxDelta();
            }
        }
    }
}
