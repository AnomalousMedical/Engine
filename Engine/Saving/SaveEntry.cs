using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    public struct SaveEntry
    {
        private String name;
        private Type objectType;
        private Object value;
        private long objectID;

        public SaveEntry(String name, Object value, Type objectType)
            : this(name, value, objectType, - 1)
        {
            
        }

        public SaveEntry(String name, Object value, Type objectType, long objectID)
        {
            this.name = name;
            this.objectType = objectType;
            this.value = value;
            this.objectID = objectID;
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public Type ObjectType
        {
            get
            {
                return objectType;
            }
        }

        public Object Value
        {
            get
            {
                return value;
            }
        }

        public long ObjectID
        {
            get
            {
                return objectID;
            }
        }
    }
}
