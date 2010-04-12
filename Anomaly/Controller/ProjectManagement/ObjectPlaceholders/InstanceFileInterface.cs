using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Logging;

namespace Anomaly
{
    public class InstanceFileInterface : EditableFileInterface<Instance>, IDisposable
    {
        private SimObjectController simObjectController;

        public InstanceFileInterface(String name, Object iconReferenceTag, String filename)
            :base(name, iconReferenceTag, filename)
        {

        }

        public void Dispose()
        {
            destroyInstance(simObjectController);
        }

        public override EditInterface getObjectEditInterface(object obj)
        {
            Instance instance = obj as Instance;
            if (instance != null)
            {
                return instance.getEditInterface();
            }
            else
            {
                throw new Exception(String.Format("Cannot get edit interface for object {0} because it is not of type {1}", obj.ToString(), typeof(Instance).ToString()));
            }
        }

        public override void uiEditingCompletedCallback(EditInterface editInterface, object editingObject)
        {
            base.uiEditingCompletedCallback(editInterface, editingObject);
            if (simObjectController != null)
            {
                Instance instance = editingObject as Instance;
                simObjectController.destroySimObject(instance.Name);
                simObjectController.createSimObject(instance.Definition);
            }
        }

        public override void uiFieldUpdateCallback(EditInterface editInterface, object editingObject)
        {
            base.uiFieldUpdateCallback(editInterface, editingObject);
            if (simObjectController != null)
            {
                Instance instance = editingObject as Instance;
                simObjectController.destroySimObject(instance.Name);
                simObjectController.createSimObject(instance.Definition);
            }
        }

        public void createInstance(SimObjectController simObjectController)
        {
            this.simObjectController = simObjectController;

            Instance instance = getFileObject();
            if (instance != null)
            {
                simObjectController.createSimObject(instance.Definition);
            }
            else
            {
                Log.Error("Could not create SimObject {0} because the instance definition could not be loaded.", instance.Name);
            }
        }

        public void destroyInstance(SimObjectController simObjectController)
        {
            if (this.simObjectController != null)
            {
                this.simObjectController = null;
                simObjectController.destroySimObject(Name);
            }
        }
    }
}
