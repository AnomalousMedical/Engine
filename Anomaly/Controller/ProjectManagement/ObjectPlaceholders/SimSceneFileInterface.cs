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
            GenericClipboardEntry clipboardInterface = new GenericClipboardEntry(typeof(SimSceneDefinition));
            clipboardInterface.CopyFunction = copy;
            clipboardInterface.PasteFunction = paste;
            editInterface.ClipboardEntry = clipboardInterface;
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

        private object copy()
        {
            return EngineClipboard.copyObject(fileObj);
        }

        private void paste(object pasted)
        {
            fileObj = (SimSceneDefinition)pasted;
            modified = true;
        }
    }
}
