using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Engine;

namespace Anomaly
{
    class SimSceneFileInterface : EditableFileInterface<SimSceneDefinition>
    {
        private delegate void CopyPasteDelegate();

        public SimSceneFileInterface(String name, Object iconReferenceTag, String filename)
            :base(name, iconReferenceTag, filename)
        {
            EditInterfaceCommand copyCommand = new EditInterfaceCommand("Copy", copy);
            editInterface.addCommand(copyCommand);
            EditInterfaceCommand pasteCommand = new EditInterfaceCommand("Paste", paste);
            editInterface.addCommand(pasteCommand);
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

        private void copy(EditUICallback callback, EditInterfaceCommand caller)
        {
            AnomalyClipboard.storeObject(fileObj);
        }

        private void paste(EditUICallback callback, EditInterfaceCommand caller)
        {
            SimSceneDefinition copiedDef = AnomalyClipboard.copyStoredObject<SimSceneDefinition>();
            if (copiedDef != null)
            {
                fileObj = copiedDef;
                modified = true;
            }
        }
    }
}
