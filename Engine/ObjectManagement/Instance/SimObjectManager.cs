using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This class manages a set of SimObjects and the SimElementManagers for
    /// a scene.
    /// </summary>
    public class SimObjectManager
    {
        #region Fields

        LinkedList<SimElementManager> externalElementManagers = new LinkedList<SimElementManager>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimObjectManager()
        {

        }

        #endregion

        #region Functions

        /// <summary>
        /// Add a SimElementManager that is not owned by this
        /// SimObjectManager. This will not be auto disposed by this manager.
        /// </summary>
        /// <param name="elementManager">The SimElementManager to add.</param>
        public void addExternalSimElementManager(SimElementManager elementManager)
        {
            externalElementManagers.AddLast(elementManager);
        }

        /// <summary>
        /// Remove an external SimElementManager from this SimObjectManager.
        /// </summary>
        /// <param name="elementManager">The SimElementManager to remove.</param>
        public void removeExternalSimElementManager(SimElementManager elementManager)
        {
            externalElementManagers.AddLast(elementManager);
        }

        #endregion
    }
}
