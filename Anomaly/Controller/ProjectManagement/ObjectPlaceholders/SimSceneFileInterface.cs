using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;

namespace Anomaly
{
    class SimSceneFileInterface : EditableFileInterface<SimSceneDefinition>
    {
        public SimSceneFileInterface(String name, Object iconReferenceTag, String filename)
            :base(name, iconReferenceTag, filename)
        {

        }

        public override EditInterface getObjectEditInterface(object obj)
        {
            SimSceneDefinition simScene = obj as SimSceneDefinition;
            if (simScene != null)
            {
                return simScene.getEditInterface();
            }
            else
            {
                throw new Exception(String.Format("Cannot get edit interface for object {0} because it is not of type SimSceneDefinition", obj.ToString()));
            }
        }
    }
}
