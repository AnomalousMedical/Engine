using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;
using Engine.ObjectManagement;

namespace OgrePlugin
{
    public class OgreSceneManager : SimElementManager
    {
        private Root ogreRoot;
        private SceneManager scene;
        private String name;
        private OgreFactory factory;

        public OgreSceneManager(String name, SceneManager scene)
        {
            this.ogreRoot = Root.getSingleton();
            this.scene = scene;
            this.name = name;
            this.factory = new OgreFactory();
        }

        #region Functions

        public SimElementFactory getFactory()
        {
            return factory;
        }

        public Type getSimElementManagerType()
        {
            return this.GetType();
        }

        public string getName()
        {
            return name;
        }

        public SimElementManagerDefinition createDefinition()
        {
            OgreSceneManagerDefinition definition = new OgreSceneManagerDefinition(name);
            return definition;
        }

        public void Dispose()
        {
            Root.getSingleton().destroySceneManager(scene);
        }

        #endregion Functions
    }
}
