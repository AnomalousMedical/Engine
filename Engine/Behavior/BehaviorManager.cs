using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine.ObjectManagement;
using Logging;

namespace Engine
{
    /// <summary>
    /// This class updates and manages behaviors.
    /// </summary>
    public class BehaviorManager : SimElementManager, UpdateListener
    {
        private List<Behavior> activeBehaviors = new List<Behavior>();
        private List<Behavior> blacklistedBehaviors = new List<Behavior>();
        private EventManager eventManager;
        private BehaviorFactory behaviorFactory;
        private String name;
        private UpdateTimer timer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventManager">The event manager to process on updates.</param>
        public BehaviorManager(String name, UpdateTimer timer, EventManager eventManager)
        {
            this.eventManager = eventManager;
            this.timer = timer;
            timer.addFixedUpdateListener(this);
            this.name = name;
            behaviorFactory = new BehaviorFactory(this);
        }

        /// <summary>
        /// Update all behaviors owned by this manager that are currently active.
        /// </summary>
        /// <param name="clock">The clock with info about the last update.</param>
        public void updateBehaviors(Clock clock)
        {
            for (int index = 0; index < activeBehaviors.Count; ++index)
            {
                try
                {
                    updateBehaviorResume(ref index, clock);
                }
                catch (Exception e)
                {
                    Log.Default.sendMessage("A behavior threw an exception: {0}.\n{1}\n{2}.", LogLevel.Error, "Behavior", e.GetType().Name, e.Message, e.StackTrace);
                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                        Log.Default.sendMessage("--Inner exception: {0}.\n{1}\n{2}.", LogLevel.Error, "Behavior", e.GetType().Name, e.Message, e.StackTrace);
                    }
                }
            }
        }

        private void updateBehaviorResume(ref int index, Clock clock)
        {
            while (index < activeBehaviors.Count)
            {
                activeBehaviors[index].update(clock, eventManager);
                ++index;
            }
        }

        /// <summary>
        /// Activate a behavior so it can recieve updates.
        /// </summary>
        /// <param name="name">The name of the behavior to activate.</param>
        public void activateBehavior(Behavior behavior)
        {
            activeBehaviors.Add(behavior);
        }

        /// <summary>
        /// Deactivate a behavior so it no longer recieves updates.
        /// </summary>
        /// <param name="name">The name of the behavior to deactivate.</param>
        public void deactivateBehavior(Behavior behavior)
        {
            activeBehaviors.Remove(behavior);
        }

        internal BehaviorFactory getBehaviorFactory()
        {
            return behaviorFactory;
        }

        #region SimElementManager Members

        public SimElementFactory getFactory()
        {
            return behaviorFactory;
        }

        public Type getSimElementManagerType()
        {
            return typeof(BehaviorManager);
        }

        public string getName()
        {
            return name;
        }

        public SimElementManagerDefinition createDefinition()
        {
            BehaviorManagerDefinition definition = new BehaviorManagerDefinition(name);
            return definition;
        }

        public void Dispose()
        {
            timer.removeFixedUpdateListener(this);
        }

        #endregion

        #region UpdateListener Members

        public void sendUpdate(Clock clock)
        {
            updateBehaviors(clock);
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }

        #endregion
    }
}
