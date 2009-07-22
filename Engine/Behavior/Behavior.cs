using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Platform;
using Logging;
using Engine.Attributes;
using Engine.Renderer;
using Engine.Saving;
using Engine.Editing;

namespace Engine
{
    /// <summary>
    /// This is the base class for all behaviors, which is where most of the
    /// extensibility outside of plugins takes place for the engine. By
    /// extending this class users can add custom logic to a SimObject by
    /// expanding its interface and implementing time based updates. Behaviors
    /// should not implement constructors, but should instead override the
    /// constructed method.
    /// <para>
    /// By default a behavior will auto-generate an EditInterface using
    /// reflection. This interface will be generated for all fields and
    /// properties both public and nonpublic marked with the EditableAttribute.
    /// Note that any variables that are not the basic types or members of
    /// EngineMath must be subclasses of BehaviorObject or impelement the
    /// BehaviorObjectBase interface.
    /// </para>
    /// <para>
    /// By default a behavior will save all fields that are not marked with the
    /// DoNotSave attribute. Note that the serializer will not be able to save
    /// anything that does not extend the Saveable interface or is not a basic
    /// type. Any helper objects should extend BehaviorObject or
    /// BehaviorObjectBase in order to be safely delt with or they should be
    /// marked with the DoNotSaveAttribute.
    /// </para>
    /// </summary>
    [DoNotCopy]
    public abstract class Behavior : SimElement
    {
        #region Fields

        private bool valid = true;
        private BehaviorManager manager;
        private bool currentlyEnabled = false;
        internal bool hasUpdate = true;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Base constructor for the behavior.
        /// </summary>
        public Behavior()
            : base("NotInitialized", Subscription.None)
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// This function will set the attributes that would normally be set by a constructor.
        /// However, since behavior subclasses should not define constructors this function
        /// will be called to set the same values.
        /// </summary>
        /// <param name="name">The name of the Behavior.</param>
        /// <param name="subscription">The subscription of the behavior.</param>
        /// <param name="behaviorManager">The BehaviorManager that will update the behavior.</param>
        internal void setAttributes(String name, Subscription subscription, BehaviorManager behaviorManager)
        {
            this._behaviorSetAttributes(name, subscription);
            this.manager = behaviorManager;
        }

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
        /// Internal function to call the link function.
        /// </summary>
        internal void callLink()
        {
            link();
        }

        /// <summary>
        /// Override this function to perform custom operations after all
        /// behaviors have had their constructed functions called. Here it will
        /// be safe to do any operations that could be done in update.
        /// </summary>
        protected virtual void link()
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
        /// <param name="clock">The clock with info about the last update.</param>
        /// <param name="eventManager">The EventManager that is gathering input.</param>
        public abstract void update(Clock clock, EventManager eventManager);

        /// <summary>
        /// This function can be overwritten to draw debugging information when
        /// the BehaviorManager draws debug info and the behavior is active.
        /// </summary>
        /// <param name="debugDrawing">The DebugDrawingSurface to render to.</param>
        public virtual void drawDebugInfo(DebugDrawingSurface debugDrawing)
        {

        }

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
            if (valid && currentlyEnabled)
            {
                manager.deactivateBehavior(this);
            }
            valid = false;
            Log.Default.sendMessage("Behavior {0}, type={1} blacklisted.  Reason: {2}", LogLevel.Error, "Behavior", Name, this.GetType().Name, reason);
        }

        /// <summary>
        /// This function will blacklist the behavior for this load of the
        /// scene. This version can take a reason and optional params that will
        /// be used in a String.Format call.
        /// </summary>
        /// <param name="reason">The reason the behavior is being blacklisted. This is printed in the log.</param>
        protected void blacklist(String reason, params object[] args)
        {
            blacklist(String.Format(reason, args));
        }

        /// <summary>
        /// Internal function to call the custom load function.
        /// </summary>
        /// <param name="info">The LoadInfo.</param>
        internal void callCustomLoad(LoadInfo info)
        {
            customLoad(info);
        }

        /// <summary>
        /// Override this function to load custom information from the LoadInfo
        /// object.
        /// </summary>
        /// <param name="info">The LoadInfo to load data from.</param>
        protected virtual void customLoad(LoadInfo info)
        {

        }

        /// <summary>
        /// Internal function to call the custom save function.
        /// </summary>
        /// <param name="info">The SaveInfo.</param>
        internal void callCustomSave(SaveInfo info)
        {
            customSave(info);
        }

        /// <summary>
        /// Override this function to add custom data to be saved.
        /// </summary>
        /// <param name="info">The SaveInfo to add custom data to.</param>
        protected virtual void customSave(SaveInfo info)
        {

        }

        /// <summary>
        /// Internal function to call the customizeEditInterface function.
        /// </summary>
        /// <param name="editInterface">The EditInterface for this behavior.</param>
        internal void callCustomizeEditInterface(EditInterface editInterface)
        {
            customizeEditInterface(editInterface);
        }

        /// <summary>
        /// Override this funciton to add custom data to the behavior's EditInterface.
        /// </summary>
        /// <param name="editInterface">The EditInterface for this behavior.</param>
        protected virtual void customizeEditInterface(EditInterface editInterface)
        {

        }

        #region SimElement

        /// <summary>
        /// Dispose function called on cleanup. This is hidden by being invoked
        /// by the internal member Destroy().
        /// </summary>
        protected override sealed void Dispose()
        {
            destroy();
            if (currentlyEnabled)
            {
                manager.deactivateBehavior(this);
            }
        }

        /// <summary>
        /// This function will update the position of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        protected override sealed void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            
        }

        /// <summary>
        /// This function will update the translation of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        protected override sealed void updateTranslationImpl(ref Vector3 translation)
        {
            
        }

        /// <summary>
        /// This function will update the rotation of the SimElement.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        protected override sealed void updateRotationImpl(ref Quaternion rotation)
        {
            
        }

        /// <summary>
        /// This function will update the scale of the SimElement.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        protected override sealed void updateScaleImpl(ref Vector3 scale)
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
            if (hasUpdate && valid && enabled != currentlyEnabled)
            {
                if (enabled)
                {
                    manager.activateBehavior(this);
                }
                else
                {
                    manager.deactivateBehavior(this);
                }
                currentlyEnabled = enabled;
            }
        }

        /// <summary>
        /// Save this behavior to a definition. This function can be overwritten
        /// to customize the definition, but note that this will invalidate all
        /// the rules for saving objects. Also do not call base.saveToDefinition
        /// if this function is overwritten.
        /// </summary>
        /// <returns>A new BehaviorDefinition.</returns>
        public override sealed SimElementDefinition saveToDefinition()
        {
            BehaviorDefinition definition = new BehaviorDefinition(Name, MemberCopier.CreateCopy<Behavior>(this));
            return definition;
        }

        #endregion SimElement

        #endregion Functions

        #region Properties

        /// <summary>
        /// This will be true unless the behavior was blacklisted. This allows
        /// for easy checking to see if all components were sucessfully
        /// identified.
        /// </summary>
        protected bool Valid
        {
            get
            {
                return valid;
            }
        }

        #endregion Properties
    }
}
