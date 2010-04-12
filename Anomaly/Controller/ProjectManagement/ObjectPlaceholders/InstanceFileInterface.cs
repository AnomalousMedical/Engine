using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Logging;
using Engine.ObjectManagement;

namespace Anomaly
{
    public class InstanceFileInterface : EditableFileInterface<Instance>, IDisposable
    {
        private SimObjectController simObjectController;
        private bool showInstance = true;

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
                EditInterface editInterface = instance.getEditInterface();
                editInterface.addCommand(new EditInterfaceCommand("Toggle Display", toggleHidden));
                return editInterface;
            }
            else
            {
                throw new Exception(String.Format("Cannot get edit interface for object {0} because it is not of type {1}", obj.ToString(), typeof(Instance).ToString()));
            }
        }

        public override void uiEditingCompletedCallback(EditInterface editInterface, object editingObject)
        {
            base.uiEditingCompletedCallback(editInterface, editingObject);
            createSimObject(editingObject);
        }

        private void createSimObject(object editingObject)
        {
            if (simObjectController != null)
            {
                Instance instance = editingObject as Instance;
                simObjectController.destroySimObject(instance.Name);
                SimObject simObj = simObjectController.createSimObject(instance.Definition);
                if (simObj != null && !showInstance)
                {
                    simObj.setEnabled(showInstance);
                }
            }
        }

        public override void uiFieldUpdateCallback(EditInterface editInterface, object editingObject)
        {
            base.uiFieldUpdateCallback(editInterface, editingObject);
            createSimObject(editingObject);
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

        public void setVisible(bool visible)
        {
            showInstance = visible;
            if (simObjectController != null)
            {
                SimObject obj = simObjectController.getSimObject(Name);
                obj.setEnabled(showInstance);
            }
        }

        private void toggleHidden(EditUICallback callback, EditInterfaceCommand command)
        {
            setVisible(!showInstance);
        }
    }
}
