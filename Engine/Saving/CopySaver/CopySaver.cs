using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving.CopySaver
{
    /// <summary>
    /// The CopySaver saves an object by creating a copy of it using the
    /// save/load mechanism. This allows for nice copies to be made of objects
    /// using the same interface for serializing them, which reduces the amount
    /// of user code that has to be written.
    /// </summary>
    class CopySaver : HeaderWriter, FooterWriter, ValueWriter
    {
        private SaveControl saveControl;
        private LoadControl loadControl = new LoadControl();
        private Object lastLoadedObject = null;

        public CopySaver()
        {
            saveControl = new SaveControl(this, this, this);
        }

        public Object copyObject(Saveable source)
        {
            lastLoadedObject = null;
            saveControl.saveObject(source);
            return lastLoadedObject;
        }

        public void writeHeader(ObjectIdentifier objectId)
        {
            loadControl.startDefiningObject(objectId);
        }

        public void writeFooter(ObjectIdentifier objectId)
        {
            lastLoadedObject = loadControl.createCurrentObject();
        }

        public void writeValue(SaveEntry entry)
        {
            if (entry.ObjectID != SaveEntry.NULL_ID)
            {
                loadControl.addObjectValue(entry.Name, entry.ObjectID);
            }
            else
            {
                loadControl.addValue(entry.Name, entry.Value, entry.ObjectType);
            }
        }
    }
}
