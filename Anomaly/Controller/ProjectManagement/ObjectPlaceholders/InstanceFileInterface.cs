using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Logging;
using Engine.ObjectManagement;
using Editor;
using Engine;
using System.Xml;
using Engine.Saving.XMLSaver;

namespace Anomaly
{
    public class InstanceFileInterface : ObjectPlaceholderInterface, IDisposable, SelectableObject
    {
        private static XmlSaver xmlSaver = new XmlSaver();

        private SimObjectController simObjectController;
        private bool showInstance = true;
        private Instance instance;
        private String filename;

        public InstanceFileInterface(String name, Object iconReferenceTag, String filename)
            :base(name, iconReferenceTag)
        {
            Deleted = false;
            this.filename = filename;
            using (XmlTextReader textReader = new XmlTextReader(filename))
            {
                instance = xmlSaver.restoreObject(textReader) as Instance;
            }
        }

        public void Dispose()
        {
            destroyInstance(simObjectController);
        }

        public override object getObject()
        {
            return instance;
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
            if (!Deleted)
            {
                Instance fileObj = editingObject as Instance;
                if (fileObj != null)
                {
                    using (XmlTextWriter textWriter = new XmlTextWriter(filename, Encoding.Default))
                    {
                        textWriter.Formatting = Formatting.Indented;
                        xmlSaver.saveObject(fileObj, textWriter);
                    }
                }
                else
                {
                    throw new Exception(String.Format("Cannot save object {0} because it is not of type {1}", editingObject.ToString(), typeof(Instance).ToString()));
                }
            }
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
            createSimObject(editingObject);
        }

        public void createInstance(SimObjectController simObjectController)
        {
            this.simObjectController = simObjectController;

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

        public bool Deleted { get; set; }

        public Instance Instance
        {
            get
            {
                return instance;
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

        #region SelectableObject Members

        public void editPosition(ref Vector3 translation, ref Quaternion rotation)
        {
            SimObjectBase simObj = simObjectController.getSimObject(Name) as SimObjectBase;
            if (simObj != null)
            {
                simObj.updatePosition(ref translation, ref rotation, null);
            }
            instance.Translation = translation;
            instance.Definition.Rotation = rotation;
        }

        public void editTranslation(ref Vector3 translation)
        {
            SimObjectBase simObj = simObjectController.getSimObject(Name) as SimObjectBase;
            if (simObj != null)
            {
                simObj.updateTranslation(ref translation, null);
            }
            instance.Translation = translation;
        }

        public void editRotation(ref Quaternion rotation)
        {
            SimObjectBase simObj = simObjectController.getSimObject(Name) as SimObjectBase;
            if (simObj != null)
            {
                simObj.updateRotation(ref rotation, null);
            }
            instance.Definition.Rotation = rotation;
        }

        public Quaternion getRotation()
        {
            return instance.Definition.Rotation;
        }

        public Vector3 getTranslation()
        {
            return instance.Translation;
        }

        #endregion
    }
}
