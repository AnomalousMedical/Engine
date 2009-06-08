using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Reflection;
using Engine.Saving;
using Engine.Attributes;

namespace Engine
{
    /// <summary>
    /// This abstract class allows subclasses to be used as part of behaviors.
    /// Classes deriving from this one must provide an empty constructor and a
    /// private constructor that takes a LoadInfo object. That must be passed to
    /// the BehaviorObject(LoadInfo info) protected constructor. Otherwise the
    /// same rules apply to these objects as far as editing and saving is
    /// concerned.
    /// </summary>
    public abstract class BehaviorObject : BehaviorObjectBase
    {
        [DoNotCopy]
        [DoNotSave]
        private EditInterface editInterface;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BehaviorObject()
        {

        }

        /// <summary>
        /// This function will provide the customized EditInterface for this
        /// class when it is scanned by the ReflectedEditInterface scanner. If
        /// it is not scanned with that scanner this function is not
        /// automatically called.
        /// </summary>
        /// <param name="memberName">The name of the member that contains this object.</param>
        /// <param name="scanner">The MemberScanner used to scan the parent object.</param>
        /// <returns>A new EditInterface for this class.</returns>
        public EditInterface getEditInterface(string memberName, MemberScanner scanner)
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, BehaviorEditMemberScanner.Scanner, memberName + " - " + this.GetType().Name, null);
            }
            return editInterface;
        }

        /// <summary>
        /// Create a new instance using the provided LoadInfo. This must be
        /// called from any deriving classes.
        /// </summary>
        /// <param name="info"></param>
        protected BehaviorObject(LoadInfo info)
        {
            ReflectedSaver.RestoreObject(this, info, BehaviorSaveMemberScanner.Scanner);
        }

        /// <summary>
        /// Get the info to save for the implementing class.
        /// </summary>
        /// <param name="info">The SaveInfo class to save into.</param>
        public void getInfo(SaveInfo info)
        {
            ReflectedSaver.SaveObject(this, info, BehaviorSaveMemberScanner.Scanner);
        }
    }
}
