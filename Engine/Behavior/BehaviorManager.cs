using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine.ObjectManagement;
using Logging;
using Engine.Renderer;

namespace Engine
{
    /// <summary>
    /// This class updates and manages behaviors.
    /// </summary>
    public class BehaviorManager : SimElementManager, UpdateListener
    {
        private List<List<Behavior>> updatePhaseOrder = new List<List<Behavior>>();
        private Dictionary<String, List<Behavior>> updatePhases = new Dictionary<String, List<Behavior>>();
        private EventManager eventManager;
        private BehaviorFactory behaviorFactory;
        private String name;
        private UpdateTimer timer;
        private List<LateLinkEntry> lateLinkActions = new List<LateLinkEntry>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventManager">The event manager to process on updates.</param>
        public BehaviorManager(String name, UpdateTimer timer, EventManager eventManager)
        {
            this.eventManager = eventManager;
            this.timer = timer;
            timer.addUpdateListener(this);
            this.name = name;
            behaviorFactory = new BehaviorFactory(this);
        }

        public void Dispose()
        {
            timer.removeUpdateListener(this);
        }

        /// <summary>
        /// Update all behaviors owned by this manager that are currently active.
        /// </summary>
        /// <param name="clock">The clock with info about the last update.</param>
        public void updateBehaviors(Clock clock)
        {
            try
            {
                foreach (Behavior behavior in activeBehaviors())
                {
                    behavior.update(clock, eventManager);
                }
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

        /// <summary>
        /// Add an update phase.
        /// </summary>
        /// <param name="name"></param>
        internal void addUpdatePhase(String name)
        {
            List<Behavior> updatePhase = new List<Behavior>();
            updatePhaseOrder.Add(updatePhase);
            updatePhases.Add(name, updatePhase);
        }

        /// <summary>
        /// Activate a behavior so it can recieve updates.
        /// </summary>
        /// <param name="name">The name of the behavior to activate.</param>
        internal void activateBehavior(Behavior behavior)
        {
            List<Behavior> updatePhase;
            if(updatePhases.TryGetValue(behavior.UpdatePhase, out updatePhase))
            {
                updatePhase.Add(behavior);
            }
            else
            {
                Log.Error("Could not add behavior '{0}' from SimObject '{1}' to update phase '{2}' because it was not found. This behavior will not update.", behavior.Name, behavior.Owner.Name, behavior.UpdatePhase);
            }
        }

        /// <summary>
        /// Deactivate a behavior so it no longer recieves updates.
        /// </summary>
        /// <param name="name">The name of the behavior to deactivate.</param>
        internal void deactivateBehavior(Behavior behavior)
        {
            List<Behavior> updatePhase;
            if (updatePhases.TryGetValue(behavior.UpdatePhase, out updatePhase))
            {
                updatePhase.Remove(behavior);
            }
        }

        /// <summary>
        /// Get the BehaviorFactory for this manager.
        /// </summary>
        /// <returns></returns>
        internal BehaviorFactory getBehaviorFactory()
        {
            return behaviorFactory;
        }

        /// <summary>
        /// Render all active behaviors debug info.
        /// </summary>
        /// <param name="debugDrawing"></param>
        internal void renderDebugInfo(DebugDrawingSurface debugDrawing)
        {
            foreach (Behavior behavior in activeBehaviors())
            {
                behavior.drawDebugInfo(debugDrawing);
            }
        }

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
            //Have to reverse lookup from the phases defined, this isn't really performance critical so its a bit wonky
            foreach(var phase in updatePhaseOrder)
            {
                //Reverse lookup from dictionary, the list and dictionary are always in sync
                definition.addUpdatePhase(new BehaviorUpdatePhase(updatePhases.First(i => i.Value == phase).Key));
            }
            return definition;
        }

        public void sendUpdate(Clock clock)
        {
            PerformanceMonitor.start("Behavior Update");
            updateBehaviors(clock);
            PerformanceMonitor.stop("Behavior Update");
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }

        /// <summary>
        ///  A list of late link callbacks, only use this from the BehaviorFactory and nowhere else.
        ///  These are fired at the end of the link phase, but since most behaviors won't have one
        ///  we don't override a function and instead register these callbacks. The BehaviorFactory
        ///  will call these actions and clear them after its link phase runs. BlacklistExceptions will
        ///  be handled from these like the other phases.
        /// </summary>
        internal IEnumerable<LateLinkEntry> LateLinkActions
        {
            get
            {
                return lateLinkActions;
            }
        }

        /// <summary>
        /// Clear all lateLinkActions, only call from BehaviorFactory.
        /// </summary>
        internal void clearLateLinkActions()
        {
            lateLinkActions.Clear();
        }

        /// <summary>
        /// Add a late link action only call from Behavior.
        /// </summary>
        /// <param name="action"></param>
        internal void addLateLinkAction(LateLinkEntry entry)
        {
            lateLinkActions.Add(entry);
        }

        private IEnumerable<Behavior> activeBehaviors()
        {
            int updateIndex;
            foreach (var phase in updatePhaseOrder)
            {
                updateIndex = 0;
                while (updateIndex < phase.Count)
                {
                    yield return phase[updateIndex++];
                }
            }
        }
    }
}
