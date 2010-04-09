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
    public class EditableFileInterface<T> : EditableProperty
        where T : class, Saveable
    {
        private static XmlSaver xmlSaver = new XmlSaver();

        private EditInterface editInterface;
        private string filename;

        public EditableFileInterface(String name, Object iconReferenceTag, String filename)
        {
            this.filename = filename;
            editInterface = new EditInterface(name);
            editInterface.IconReferenceTag = iconReferenceTag;
            editInterface.addEditableProperty(this);
        }

        public T getFileObject()
        {
            using (XmlTextReader textReader = new XmlTextReader(filename))
            {
                return xmlSaver.restoreObject(textReader) as T;
            }
        }

        public void saveObject(T fileObj)
        {
            using (XmlTextWriter textWriter = new XmlTextWriter(filename, Encoding.Default))
            {
                textWriter.Formatting = Formatting.Indented;
                xmlSaver.saveObject(fileObj, textWriter);
            }
        }

        public EditInterface getEditInterface()
        {
            return editInterface;
        }

        public String Filename
        {
            get
            {
                return filename;
            }
        }

        public string getValue(int column)
        {
            return "";
        }

        public void setValueStr(int column, string value)
        {

        }

        public bool canParseString(int column, string value, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        public Type getPropertyType(int column)
        {
            return typeof(object);
        }
    }
}
