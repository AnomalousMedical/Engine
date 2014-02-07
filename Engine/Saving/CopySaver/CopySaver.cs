using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// The CopySaver saves an object by creating a copy of it using the
    /// save/load mechanism. This allows for nice copies to be made of objects
    /// using the same interface for serializing them, which reduces the amount
    /// of user code that has to be written.
    /// </summary>
    /// <remarks>
    /// One note about this class in relation to the ObjectIdentifierFactory, here
    /// that class will not be used. This should not matter the intention of customizing
    /// the ObjectIdentifierFactory is to be able to load objects that may no longer exist,
    /// this saver deals only with objects that are currently in memory.
    /// </remarks>
    public class CopySaver : HeaderWriter, FooterWriter, ValueWriter
    {
        public static readonly CopySaver Default = new CopySaver();

        private SaveControl saveControl;
        private LoadControl loadControl;
        private Object lastLoadedObject = null;

        public CopySaver()
            :this(new DefaultTypeFinder())
        {
            
        }

        public CopySaver(TypeFinder typeFinder)
        {
            saveControl = new SaveControl(this, this, this);
            loadControl = new LoadControl(typeFinder);
        }

        public T copy<T>(T source)
            where T : Saveable
        {
            return (T)copyObject(source);
        }

        public Object copyObject(Saveable source)
        {
            lastLoadedObject = null;
            saveControl.saveObject(source);
            loadControl.reset();
            saveControl.reset();
            return lastLoadedObject;
        }

        public void writeHeader(ObjectIdentifier objectId, int version)
        {
            //Important to make a copy here, we do not want to modify the original
            //This is what would happen when the object was saved to disk and reloaded anyway.
            loadControl.startDefiningObject(new ObjectIdentifier(objectId), version);
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
