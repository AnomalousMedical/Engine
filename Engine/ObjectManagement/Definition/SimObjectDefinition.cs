using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine.Editing;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This class defines an instance of a sim object.
    /// </summary>
    public interface SimObjectDefinition : Saveable
    {
        #region Functions

        /// <summary>
        /// Register with factories to build this definition into the given SimObject.
        /// </summary>
        /// <param name="instance">The SimObject that will get the built elements.</param>
        void register(SimSubScene subScene, SimObject instance);

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        EditInterface getEditInterface();

        #endregion Functions

        #region Properties

        /// <summary>
        /// The instance name of this SimObject.
        /// </summary>
        String Name
        {
            get;
            set;
        }

        #endregion Properties
    }
}
