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
            
        }

        protected override MovableObject createActualProduct(SceneNodeElement element, OgreSceneManager scene, SimObject simObject)
        {
            Identifier id = new Identifier(simObject.Name, Name);
            ManualObject obj = scene.createManualObject(id);
            element.attachObject(id, obj);
            return obj;
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
