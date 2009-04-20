using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// This is an entry in a save stream.
    /// </summary>
    public struct SaveEntry
    {
        private String name;
        private Type objectType;
        private Object value;
        private long objectID;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of an object.</param>
        /// <param name="value">The value of the object.</param>
        /// <param name="objectType">The type of the value.</param>
        public SaveEntry(String name, Object value, Type objectType)
            : this(name, value, objectType, - 1)
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of an object.</param>
        /// <param name="value">The value of the object.</param>
        /// <param name="objectType">The type of the value.</param>
        /// <param name="objectID">An objectID used to identify objects that cannot be direcly saved (Saveable subclasses).</param>
        public SaveEntry(String name, Object value, Type objectType, long objectID)
        {
            this.name = name;
            this.objectType = objectType;
            this.value = value;
            this.objectID = objectID;
        }

        /// <summary>
        /// The name of this entry.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The type of this entry.
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return objectType;
            }
        }

        /// <summary>
        /// The value of this entry.
        /// </summary>
        public Object Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// The ID if this object is a Saveable subclass.
        /// </summary>
        public long ObjectID
        {
            get
            {
                return objectID;
            }
        }
    }
}
