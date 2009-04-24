using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine
{
    public class BehaviorManager
    {
        private List<Behavior> activeBehaviors = new List<Behavior>();
        private List<Behavior> blacklistedBehaviors = new List<Behavior>();
        private EventManager eventManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventManager">The event manager to process on updates.</param>
        public BehaviorManager(EventManager eventManager)
        {
            this.eventManager = eventManager;
        }

        /// <summary>
        /// Update all behaviors owned by this manager that are currently active.
        /// </summary>
        /// <param name="time">The amount of time since the last update.</param>
        public void updateBehaviors(double time)
        {
            foreach (Behavior behavior in activeBehaviors)
            {
                behavior.update(time, eventManager);
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
    }
}
