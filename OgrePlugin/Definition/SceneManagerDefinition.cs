using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using OgreWrapper;
using Engine.Reflection;
using Engine.Saving;

namespace OgrePlugin
{
    public class SceneManagerDefinition : SimElementManagerDefinition
    {
        #region Static

        private static MemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SceneManagerDefinition()
        {
            memberScanner = new MemberScanner();
            memberScanner.ProcessFields = false;
            memberScanner.Filter = EditableAttributeFilter.Instance;
        }

        #endregion Static

        #region Fields

        private String name;
        private EditInterface editInterface;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the SceneManagerDefinition.</param>
        internal SceneManagerDefinition(String name)
        {
            this.name = name;
            SceneTypeMask = SceneType.ST_GENERIC;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Get an EditInterface.
        /// </summary>
        /// <returns>An EditInterface for the definition or null if there is not interface.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, name + " Ogre Scene", null);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the name of this scene.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get
            {
                return name;
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
            return typeof(SceneManagerDefinition);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// A series of values that describe the type of scene.
        /// </summary>
        [Editable("A series of values that describe the type of scene.")]
        public SceneType SceneTypeMask { get; set; }

        #endregion Properties

        #region Saveable Members

        private const String NAME = "Name";
        private const String SCENE_TYPE = "SceneTypeMask";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="info"></param>
        private SceneManagerDefinition(LoadInfo info)
        {
            name = info.GetString(NAME);
            SceneTypeMask = info.GetValue<SceneType>(SCENE_TYPE);
        }

        /// <summary>
        /// GetInfo function.
        /// </summary>
        /// <param name="info"></param>
        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, name);
            info.AddValue(SCENE_TYPE, SceneTypeMask);
        }

        #endregion
    }
}
