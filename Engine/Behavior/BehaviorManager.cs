using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine.ObjectManagement;

namespace Engine
{
    public class BehaviorManager : SimElementManager
    {
        private List<Behavior> activeBehaviors = new List<Behavior>();
        private List<Behavior> blacklistedBehaviors = new List<Behavior>();
        private EventManager eventManager;
        private BehaviorFactory behaviorFactory = new BehaviorFactory();
        private String name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventManager">The event manager to process on updates.</param>
        public BehaviorManager(String name)
        {
            this.eventManager = EventManager.Instance;
            this.name = name;
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
            
        }

        #endregion
    }
}
