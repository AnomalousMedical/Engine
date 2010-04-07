using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.ObjectManagement;
using Engine.Editing;
using OgreWrapper;

namespace OgrePlugin
{
    public class ManualObjectDefinition : MovableObjectDefinition
    {
        public ManualObjectDefinition(String name)
            : base(name, "Manual Object", null)
        {

        }

        public ManualObjectDefinition(String name, ManualObject manualObject)
            : base(name, manualObject, "Manual Object", null)
        {

        }

        protected override void setupEditInterface(EditInterface editInterface)
        {
            editInterface.IconReferenceTag = EngineIcons.ManualObject;
        }

        /// <summary>
        /// This method should not be called outside of ManualObjectDefinition.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="baseName"></param>
        /// <returns></returns>
        internal override MovableObjectContainer createActualProduct(OgreSceneManager scene, String baseName)
        {
            return new ManualObjectContainer(Name, scene.SceneManager.createManualObject(baseName + Name));
        }

        private ManualObjectDefinition(LoadInfo info)
            : base(info, "Manual Object", null)
        {
            
        }

        protected override void getSpecificInfo(SaveInfo info)
        {
            
        }
    }
}
