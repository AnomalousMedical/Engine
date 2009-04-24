using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Platform;
using Logging;

namespace Engine
{
    /// <summary>
    /// This is the base class for all behaviors, which is where most of the
    /// extensibility outside of plugins takes place for the engine. By
    /// extending this class users can add custom logic to a SimObject by
    /// expanding its interface and implementing time based updates. Behaviors
    /// should not implement constructors, but should instead override the
    /// constructed method.
    /// </summary>
    public abstract class Behavior : SimElement
    {
        private bool valid = true;

        #region Constructors

        /// <summary>
        /// Base constructor for the behavior.
        /// </summary>
        /// <param name="name">The name of the behavior.</param>
        /// <param name="subscription">The subscription updates the behavior listenes to.</param>
        internal Behavior(String name, Subscription subscription, BehaviorManager behaviorManager)
            :base(name, subscription)
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Internal function to call the constructed method. This hides the
        /// constructed function from other users of this class but still allows
        /// it to be called by the engine.
        /// </summary>
        internal void callConstructed()
        {
            constructed();
        }

        /// <summary>
        /// Override this function to do any work that would normally go in a
        /// constructor.
        /// </summary>
        protected virtual void constructed()
        {

        }

        /// <summary>
        /// Override this function to provide custom destroy behavior.
        /// </summary>
        protected virtual void destroy()
        {

        }

        /// <summary>
        /// This is the update function. It will be called every time the BehaviorManager is updated.
        /// </summary>
        /// <param name="time">The amount of time in seconds since the last update.</param>
        /// <param name="eventManager">The EventManager that is gathering input.</param>
        public abstract void update(double time, EventManager eventManager);

        /// <summary>
        /// This function will blacklist the behavior for this load of the
        /// scene. This will not destroy the behavior, but will cause it to
        /// never be made active. This allows for an easy way to disable a
        /// behavior if it is not valid when it is initialized. The destroy
        /// function will still be called, however, so make provisions when
        /// destroying resources if any errors occur.
        /// </summary>
        /// <param name="reason">The reason the behavior is being blacklisted. This is printed in the log.</param>
        protected void blacklist(String reason)
        {
            valid = false;
            Log.Default.sendMessage("Behavior {0}, type={1} blacklisted.  Reason: {2}", LogLevel.Error, "Behavior", Name, this.GetType().Name, reason);
        }

        #region SimElement

        /// <summary>
        /// Dispose function called on cleanup. This is hidden by being invoked
        /// by the internal member Destroy().
        /// </summary>
        protected override sealed void Dispose()
        {
            destroy();
        }

        /// <summary>
        /// This function will update the position of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        protected override sealed void updatePosition(ref EngineMath.Vector3 translation, ref EngineMath.Quaternion rotation)
        {
            
        }

        /// <summary>
        /// This function will update the translation of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        protected override sealed void updateTranslation(ref EngineMath.Vector3 translation)
        {
            
        }

        /// <summary>
        /// This function will update the rotation of the SimElement.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        protected override sealed void updateRotation(ref EngineMath.Quaternion rotation)
        {
            
        }

        /// <summary>
        /// This function will update the scale of the SimElement.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        protected override sealed void updateScale(ref EngineMath.Vector3 scale)
        {
            
        }

        /// <summary>
        /// This function will enable or disable the SimElement. What this
        /// means is subsystem dependent and may not reduce the processing of
        /// the object very much.
        /// </summary>
        /// <param name="enabled">True to enable the object. False to disable the object.</param>
        protected override sealed void setEnabled(bool enabled)
        {
            
        }

        /// <summary>
        /// Save this behavior to a definition.
        /// </summary>
        /// <returns>A new BehaviorDefinition.</returns>
        public override sealed SimElementDefinition saveToDefinition()
        {
            throw new NotImplementedException();
        }

        #endregion SimElement

        #endregion Functions
    }
}
