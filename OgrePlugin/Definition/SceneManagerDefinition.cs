using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using OgreWrapper;
using Engine.Reflection;

namespace OgrePlugin.Definition
{
    class SceneManagerDefinition : SimElementManagerDefinition
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

        internal SceneManagerDefinition(String name)
        {
            this.name = name;
            SceneTypeMask = SceneType.ST_GENERIC;
        }

        #endregion Constructors

        #region Functions

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, name + "Ogre Scene Manager", null);
            }
            return editInterface;
        }

        public SimElementManager createSimElementManager()
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public Type getSimElementManagerType()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion Functions

        #region Properties

        [Editable("A series of values that describe the type of scene.")]
        public SceneType SceneTypeMask { get; set; }

        #endregion Properties

        #region Saveable Members

        public void getInfo(Engine.Saving.SaveInfo info)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
