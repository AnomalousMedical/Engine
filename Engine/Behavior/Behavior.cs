using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Engine
{
    /// <summary>
    /// This is the base class for all behaviors, which is where most of the
    /// extensibility outside of plugins takes place for the engine. By
    /// extending this class users can add custom logic to a SimObject by
    /// expanding its interface and implementing time based updates.
    /// </summary>
    public class Behavior : SimElement
    {
        /// <summary>
        /// Base constructor for the behavior.
        /// </summary>
        /// <param name="name">The name of the behavior.</param>
        /// <param name="subscription">The subscription updates the behavior listenes to.</param>
        public Behavior(String name, Subscription subscription)
            :base(name, subscription)
        {

        }

        /// <summary>
        /// Dispose function called on cleanup. This is hidden by being invoked
        /// by the internal member Destroy().
        /// </summary>
        protected override sealed void Dispose()
        {
            destroy();
        }

        protected virtual void destroy()
        {

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
    }
}
