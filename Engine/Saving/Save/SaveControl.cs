using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine.ObjectManagement;

namespace Engine.Saving
{
    public class SaveControl
    {
        /// <summary>
        /// The ObjectID given to all objects that are null.
        /// </summary>
        public const long NullObject = -1;

        private Stack<SaveInfo> pooledInfos = new Stack<SaveInfo>();
        private long currentObjectID = 0;
        private Dictionary<Saveable, ObjectIdentifier> identiferMap = new Dictionary<Saveable, ObjectIdentifier>();
        private HeaderWriter headerWriter;
        private ValueWriterCollection valueWriters;
        private FooterWriter footerWriter;

        public SaveControl(HeaderWriter headerWriter, ValueWriterCollection valueWriters, FooterWriter footerWriter)
        {
            this.headerWriter = headerWriter;
            this.valueWriters = valueWriters;
            this.footerWriter = footerWriter;
        }

        /// <summary>
        /// Reset the object mappings.
        /// </summary>
        protected void reset()
        {
            currentObjectID = 0;
            identiferMap.Clear();
        }

        /// <summary>
        /// Save an object. This will return the object id for the Saveable
        /// passed in. If this saveable was already found it will return the
        /// previously assigned id, otherwise it will make a new one. If save is
        /// null then this function will return NullObject;
        /// </summary>
        /// <param name="save">The object to identify and save.</param>
        /// <returns>See description.</returns>
        internal long saveObject(Saveable save)
        {
            if (save == null)
            {
                return NullObject;
            }
            if (identiferMap.ContainsKey(save))
            {
                return identiferMap[save].ObjectID;
            }
            else
            {
                SaveInfo info = checkOutSaveInfo();
                ObjectIdentifier identifier = new ObjectIdentifier(currentObjectID++, save, save.GetType());
                identiferMap.Add(save, identifier);
                save.getInfo(info);
                writeObject(identifier, info);
                checkInSaveInfo(info);
                return identifier.ObjectID;
            }
        }

        protected void writeObject(ObjectIdentifier identifier, SaveInfo info)
        {
            headerWriter.writeHeader(identifier);
            foreach (SaveEntry entry in info.saveEntryIterator())
            {
                valueWriters.writeValue(entry);
            }
            footerWriter.writeFooter(identifier);
        }

        private SaveInfo checkOutSaveInfo()
        {
            if (pooledInfos.Count == 0)
            {
                return new SaveInfo(this);
            }
            else
            {
                return pooledInfos.Pop();
            }
        }

        private void checkInSaveInfo(SaveInfo info)
        {
            info.clear();
            pooledInfos.Push(info);
        }
    }
}
