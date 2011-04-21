using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;
using Engine.Saving.XMLSaver;
using System.Xml;

namespace Anomaly
{
    /// <summary>
    /// This class represents a file that contains an object with an EditableInterface.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public abstract class EditableFileInterface<T> : ObjectPlaceholderInterface
        where T : class, Saveable
    {
        private static XmlSaver xmlSaver = new XmlSaver();

        private string filename;
        protected T fileObj;
        protected bool modified = false;

        public EditableFileInterface(String name, Object iconReferenceTag, String filename)
            :base(name, iconReferenceTag)
        {
            this.filename = filename;

            using (XmlTextReader textReader = new XmlTextReader(filename))
            {
                fileObj = xmlSaver.restoreObject(textReader) as T;
            }
        }

        public void save()
        {
            if (modified)
            {
                using (XmlTextWriter textWriter = new XmlTextWriter(filename, Encoding.Default))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(fileObj, textWriter);
                }
                modified = false;
            }
        }

        public override object getObject()
        {
            return fileObj;
        }

        public T getFileObject()
        {
            return fileObj;
        }

        public override void uiFieldUpdateCallback(EditInterface editInterface, object editingObject)
        {
            modified = true;
        }

        public override void uiEditingCompletedCallback(EditInterface editInterface, object editingObject)
        {
            
        }

        public String Filename
        {
            get
            {
                return filename;
            }
        }
    }
}
