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

        public EditableFileInterface(String name, Object iconReferenceTag, String filename)
            :base(name, iconReferenceTag)
        {
            this.filename = filename;
            Deleted = false;
        }

        public override object getObject()
        {
            return getFileObject();
        }

        public T getFileObject()
        {
            using (XmlTextReader textReader = new XmlTextReader(filename))
            {
                return xmlSaver.restoreObject(textReader) as T;
            }
        }

        public override void saveObject(Object obj)
        {
            if (!Deleted)
            {
                T fileObj = obj as T;
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
                    throw new Exception(String.Format("Cannot save object {0} because it is not of type {1}", obj.ToString(), typeof(T).ToString()));
                }
            }
        }

        public bool Deleted { get; set; }
    }
}
