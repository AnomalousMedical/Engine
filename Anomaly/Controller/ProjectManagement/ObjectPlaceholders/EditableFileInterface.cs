using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;
using Engine.Saving.XMLSaver;
using System.Xml;
using System.IO;

namespace Anomaly
{
    /// <summary>
    /// This class represents a file that contains an object with an EditableInterface.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    public abstract class EditableFileInterface<T> : ObjectPlaceholderInterface
        where T : class, Saveable
    {
        private static Saver saver = new Saver();

        private string filename;
        protected T fileObj;
        protected bool modified = false;

        public EditableFileInterface(String name, Object iconReferenceTag, String filename)
            :base(name, iconReferenceTag)
        {
            this.filename = filename;

            using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileObj = saver.restoreObject<T>(stream);
            }
        }

        public void save()
        {
            if (modified)
            {
                using (var stream = File.Open(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    saver.saveObject(fileObj, stream);
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

        public String Filename
        {
            get
            {
                return filename;
            }
        }
    }
}
