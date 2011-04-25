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
using System.IO;

namespace Anomaly
{
    public class InstanceFileInterface : ObjectPlaceholderInterface, IDisposable, SelectableObject
    {
        private static XmlSaver xmlSaver = new XmlSaver();

        private SimObjectController simObjectController;
        private bool showInstance = true;
        private Instance instance;
        private String filename;
        private bool modified = false;
        private InstanceGroup parentGroup;

        public InstanceFileInterface(String name, Object iconReferenceTag, String filename, InstanceGroup parentGroup)
            :base(name, iconReferenceTag)
        {
            this.parentGroup = parentGroup;
            GenericClipboardEntry clipboardEntry = new GenericClipboardEntry(typeof(SimObjectDefinition));
            clipboardEntry.CopyFunction = copy;
            clipboardEntry.PasteFunction = paste;
            clipboardEntry.CutFunction = cut;
            editInterface.ClipboardEntry = clipboardEntry;
            editInterface.addCommand(new EditInterfaceCommand("Toggle Display", toggleHidden));
            Deleted = false;
            this.filename = filename;
            if (File.Exists(filename))
            {
                try
                {
                    using (XmlTextReader textReader = new XmlTextReader(filename))
                    {
                        instance = xmlSaver.restoreObject(textReader) as Instance;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Could not load Instance {0} because {1}", filename, ex.Message);
                    instance = new Instance(name, new GenericSimObjectDefinition(name));
                }
            }
        }

        public InstanceFileInterface(String name, Object iconReferenceTag, String filename, InstanceGroup parentGroup, Instance instance)
            :this(name, iconReferenceTag, filename, parentGroup)
        {
            this.instance = instance;
        }

        public void Dispose()
        {
            destroyInstance(simObjectController);
        }

        public void save()
        {
            if (Deleted)
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
            else if (Modified)
            {
                using (XmlTextWriter textWriter = new XmlTextWriter(filename, Encoding.Default))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(instance, textWriter);
                }
                Modified = false;
            }
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
                return editInterface;
            }
            else
            {
                throw new Exception(String.Format("Cannot get edit interface for object {0} because it is not of type {1}", obj.ToString(), typeof(Instance).ToString()));
            }
        }

        public override void uiEditingCompletedCallback(EditInterface editInterface, object editingObject)
        {
            
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
            Modified = true;
        }

        public void createInstance(SimObjectController simObjectController)
        {
            if (instance != null)
            {
                if (!simObjectController.hasSimObject(instance.Name))
                {
                    simObjectController.createSimObject(instance.Definition);
                    this.simObjectController = simObjectController;
                }
                else
                {
                    Log.Warning("A SimObject named {0} already exists in the scene. This instance has not been created.", Name);
                }
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
                setVisible(true);
            }
        }

        internal void buildInstance(SimObjectManagerDefinition simObjectManagerDefinition)
        {
            if (instance != null)
            {
                if (!simObjectManagerDefinition.hasSimObject(instance.Definition.Name))
                {
                    simObjectManagerDefinition.addSimObject(instance.Definition);
                }
                else
                {
                    Log.Error("Could not create SimObject {0} because an instance with that name already exists.", instance.Name);
                }
            }
            else
            {
                Log.Error("Could not create SimObject {0} because the instance definition could not be loaded.", instance.Name);
            }
        }

        internal void updatePosition(PositionCollection positions)
        {
            Position pos = positions.getPosition(instance.PositionKey);
            if (pos != null)
            {
                Vector3 trans = pos.Translation;
                Quaternion rot = pos.Rotation;
                editPosition(ref trans, ref rot);
            }
        }

        public bool Deleted { get; set; }

        public bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                if (modified != value)
                {
                    modified = value;
                    determineIcon();
                }
            }
        }

        public Instance Instance
        {
            get
            {
                return instance;
            }
        }

        public void setVisible(bool visible)
        {
            if (showInstance != visible)
            {
                showInstance = visible;
                if (simObjectController != null)
                {
                    SimObject obj = simObjectController.getSimObject(Name);
                    if (obj.Enabled != showInstance)
                    {
                        obj.setEnabled(showInstance);
                    }
                }
                determineIcon();
            }
        }

        private void toggleHidden(EditUICallback callback, EditInterfaceCommand command)
        {
            setVisible(!showInstance);
        }

        private object copy()
        {
            return EngineClipboard.copyObject(instance.Definition);
        }

        private object cut()
        {
            object copiedObject = copy();
            parentGroup.removeInstanceFile(this.Name);
            return copiedObject;
        }

        private void paste(object pasted)
        {
            parentGroup.pasteCallback(pasted);
        }

        private void determineIcon()
        {
            if (Modified)
            {
                if (showInstance)
                {
                    editInterface.IconReferenceTag = AnomalyIcons.InstanceModified;
                }
                else
                {
                    editInterface.IconReferenceTag = AnomalyIcons.InstanceModifiedHidden;
                }
            }
            else
            {
                if (showInstance)
                {
                    editInterface.IconReferenceTag = AnomalyIcons.Instance;
                }
                else
                {
                    editInterface.IconReferenceTag = AnomalyIcons.InstanceHidden;
                }
            }
        }

        #region SelectableObject Members

        public void editPosition(ref Vector3 translation, ref Quaternion rotation)
        {
            if (simObjectController != null)
            {
                SimObjectBase simObj = simObjectController.getSimObject(Name) as SimObjectBase;
                if (simObj != null)
                {
                    simObj.updatePosition(ref translation, ref rotation, null);
                }
            }
            instance.Translation = translation;
            instance.Definition.Rotation = rotation;
            Modified = true;
        }

        public void editTranslation(ref Vector3 translation)
        {
            if (simObjectController != null)
            {
                SimObjectBase simObj = simObjectController.getSimObject(Name) as SimObjectBase;
                if (simObj != null)
                {
                    simObj.updateTranslation(ref translation, null);
                }
            }
            instance.Translation = translation;
            Modified = true;
        }

        public void editRotation(ref Quaternion rotation)
        {
            if (simObjectController != null)
            {
                SimObjectBase simObj = simObjectController.getSimObject(Name) as SimObjectBase;
                if (simObj != null)
                {
                    simObj.updateRotation(ref rotation, null);
                }
            }
            instance.Definition.Rotation = rotation;
            Modified = true;
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
