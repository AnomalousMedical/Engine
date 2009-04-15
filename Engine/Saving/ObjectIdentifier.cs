using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    public struct ObjectIdentifier
    {
        long objectID;
        Object value;
        Type objectType;

        public ObjectIdentifier(long objectID, Object value, Type objectType)
        {
            this.objectID = objectID;
            this.value = value;
            this.objectType = objectType;
        }

        public long ObjectID
        {
            get
            {
                return objectID;
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

            internal set
            {
                this.value = value;
            }
        }
    }
}
