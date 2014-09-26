using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine
{
    /// <summary>
    /// The interface is a subclass of behavior. The primary difference is that
    /// it provides a blank update function. Anything that derives from
    /// Interface will not be required to provide an update and this update
    /// function will not be called. This is designed to create external
    /// behaviors that still use all the mechanisms and rules of a behavior, but
    /// prevents any update function from being called.
    /// </summary>
    public abstract class BehaviorInterface : Behavior
    {
        public BehaviorInterface()
            : base()
        {
            hasUpdate = false;
        }

        /// <summary>
        /// This is the update function. It will never be called and cannot be overwritten.
        /// </summary>
        /// <param name="clock">The clock with info about the last update.</param>
        /// <param name="eventManager">The EventManager that is gathering input.</param>
        public override sealed void update(Clock clock, EventManager eventManager)
        {
            
        }
    }
}
