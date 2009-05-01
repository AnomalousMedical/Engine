using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private ValueWriter valueWriters;
        private FooterWriter footerWriter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="headerWriter">The HeaderWriter to use to write headers.</param>
        /// <param name="valueWriters">The writers to use to restore values.</param>
        /// <param name="footerWriter">The FooterWriter to write footers.</param>
        public SaveControl(HeaderWriter headerWriter, ValueWriter valueWriters, FooterWriter footerWriter)
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

        /// <summary>
        /// Write an object to the stream.
        /// </summary>
        /// <param name="identifier">The ObjectIdentifier of the object to write.</param>
        /// <param name="info">The info for the object to write.</param>
        protected void writeObject(ObjectIdentifier identifier, SaveInfo info)
        {
            headerWriter.writeHeader(identifier);
            foreach (SaveEntry entry in info.saveEntryIterator())
            {
                valueWriters.writeValue(entry);
            }
            footerWriter.writeFooter(identifier);
        }

        /// <summary>
        /// Helper function to manage checking out of pooled SaveInfos.
        /// </summary>
        /// <returns>A pooled save info.</returns>
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

        /// <summary>
        /// Check in a SaveInfo.
        /// </summary>
        /// <param name="info">The info to check in.</param>
        private void checkInSaveInfo(SaveInfo info)
        {
            info.clear();
            pooledInfos.Push(info);
        }
    }
}
