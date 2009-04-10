using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using Engine.Editing;
using Engine.Reflection;

namespace PhysXPlugin
{
    /// <summary>
    /// This is a definition class for PhysXSceneManagers.
    /// </summary>
    public class PhysXSceneManagerDefinition : SimElementManagerDefinition
    {
        #region Static

        static MemberScanner memberScanner = new MemberScanner();

        static PhysXSceneManagerDefinition()
        {
            memberScanner.ProcessFields = false;
            memberScanner.ProcessNonPublicProperties = false;
        }

        #endregion Static

        #region Fields

        private PhysSceneDesc sceneDesc = new PhysSceneDesc();
        private String name;
        private ReflectedEditInterface editInterface = null;
        private PhysXInterface physInterface;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the definition.</param>
        internal PhysXSceneManagerDefinition(String name)
        {
            this.name = name;
            this.physInterface = PhysXInterface.Instance;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            sceneDesc.Dispose();
        }

        /// <summary>
        /// Get an EditInterface.
        /// </summary>
        /// <returns>An EditInterface for the definition or null if there is no interface.</returns>
        public EditInterface getEditInterface(DestroyEditInterfaceCommand destroyCommand)
        {
            if (editInterface == null)
            {
                editInterface = new ReflectedEditInterface(sceneDesc, memberScanner, Name);
                editInterface.setDestroyCommand(destroyCommand);
            }
            return editInterface;
        }

        /// <summary>
        /// Create the SimElementManager this definition defines and return it.
        /// This may not be safe to call more than once per definition.
        /// </summary>
        /// <returns>The SimElementManager this definition is designed to create.</returns>
        public SimElementManager createSimElementManager()
        {
            return physInterface.createScene(this);
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The PhysSceneDesc to create the scene with.
        /// </summary>
        public PhysSceneDesc SceneDesc
        {
            get
            {
                return sceneDesc;
            }
        }

        /// <summary>
        /// This will return the type the SimElementManager wishes to report
        /// itself as. Usually this will be the type of the class itself,
        /// however, it is possible to specify a superclass if desired. This
        /// will be the type reported to the SimSubScene. This should be the
        /// value returned by the SimElementManager this definition creates.
        /// </summary>
        /// <returns></returns>
        public Type getSimElementManagerType()
        {
            return typeof(PhysXSceneManager);
        }

        #endregion Properties
    }
}
