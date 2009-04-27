using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    /// <summary>
    /// This class is an abstract base class for a timer.
    /// </summary>
    public abstract class UpdateTimer
    {
        List<UpdateListener> fixedListeners = new List<UpdateListener>();
        List<UpdateListener> fullSpeedListeners = new List<UpdateListener>();

        Clock clock = new Clock();

        public UpdateTimer()
        {
            FixedFrequency = 1.0 / 60.0;
            MaxDelta = 0.1;
            MaxFrameSkip = 7;
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
        /// Pass true to this function to tell this timer to process the
        /// platform's message loop if the platform requires that and the loop
        /// thread is the same thread the windows are running on.
        /// </summary>
        /// <param name="process">True to have this timer process the platform's message loop.</param>
        public abstract void processMessageLoop(bool process);

        /// <summary>
        /// Starts the loop iterating at the set update frequency.  This function will return
        /// once the loop is stopped.
        /// </summary>
        public abstract bool startLoop();

        /// <summary>
        /// Stops the loop.
        /// </summary>
        public abstract void stopLoop();

        /// <summary>
        /// Sets the frequency of the fixed updates in seconds. The default is
        /// 1/60 (60Hz).
        /// </summary>
        public abstract double FixedFrequency { get; set; }

        /// <summary>
        /// Set the maximum delta that the timer can report. If the true delta
        /// is greater than this value it will be clamped. The default is 0.1
        /// (10 fps). This will cause the simulation to run slow if it is
        /// running at less than the max delta.
        /// </summary>
        public abstract double MaxDelta { get; set; }

        /// <summary>
        /// Sets the maximum number of frames that can be skipped in the full
        /// speed updater if the fixed updater is running slow. The default is
        /// 7.
        /// </summary>
        public abstract int MaxFrameSkip { get; set; }

        /// <summary>
        /// Fire a fixed update.
        /// </summary>
        protected void fireFixedUpdate()
        {
            clock.setTimeSeconds(FixedFrequency);
            foreach (UpdateListener fixedListener in fixedListeners)
            {
                fixedListener.sendUpdate(clock);
            }
        }

        /// <summary>
        /// Fire a full speed update.
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last full speed update in seconds.</param>
        protected void fireFullSpeedUpdate(double deltaTime)
        {
            clock.setTimeSeconds(deltaTime);
            foreach (UpdateListener fullSpeedListener in fullSpeedListeners)
            {
                fullSpeedListener.sendUpdate(clock);
            }
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
        }
    }
}
