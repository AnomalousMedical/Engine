using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This is a subclass of behavior, it does not automatically update
    /// every frame, but instead has to be scheduled by calling schedule.
    /// The beahvior will be queued at the end of its update phase and can only
    /// be scheduled if it is not already scheduled.
    /// </summary>
    public abstract class BehaviorScheduledUpdate : Behavior
    {
        private bool allowSchedule = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public BehaviorScheduledUpdate()
            : base()
        {
            hasUpdate = false;
        }

        /// <summary>
        /// This is the update function. It will be called on demad and cannot be overwritten since it handles bookeeping
        /// for if this instance can be scheduled or not.
        /// </summary>
        /// <param name="clock">The clock with info about the last update.</param>
        /// <param name="eventManager">The EventManager that is gathering input.</param>
        public override sealed void update(Clock clock, EventManager eventManager)
        {
            scheduledUpdate(clock, eventManager);
            allowSchedule = true;
        }

        /// <summary>
        /// Schedule this behavior for update. It will be scheduled in its update phase. If it is already scheduled, but has
        /// not executed it will not be scheduled again. Whether this executes on this frame or the next one depends on what phase
        /// this schedule function is called from.
        /// </summary>
        protected void schedule()
        {
            if(allowSchedule)
            {
                allowSchedule = false;
                BehaviorManager.scheduleUpdate(this);
            }
        }

        /// <summary>
        /// This is the update function. It will execute if scheduleUpdate was called previously.
        /// </summary>
        /// <param name="clock">The clock with info about the last update.</param>
        /// <param name="eventManager">The EventManager that is gathering input.</param>
        protected abstract void scheduledUpdate(Clock clock, EventManager eventManager);
    }
}
