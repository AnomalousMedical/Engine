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
            : base(name)
        {

        }

        public ManualObjectDefinition(String name, ManualObject manualObject)
            : base(name, manualObject)
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

        protected override String InterfaceName
        {
            get
            {
                return "Manual Object";
            }
        }

        private ManualObjectDefinition(LoadInfo info)
            : base(info)
        {
            
        }

        protected override void getSpecificInfo(SaveInfo info)
        {
            
        }
    }
}
