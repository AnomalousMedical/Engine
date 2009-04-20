using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// This struct contains data for an object that is being written to a
    /// stream.
    /// </summary>
    public struct ObjectIdentifier
    {
        long objectID;
        Object value;
        Type objectType;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="objectID">The objectID of the object.</param>
        /// <param name="value">The value.</param>
        /// <param name="objectType">The type of the value.</param>
        public ObjectIdentifier(long objectID, Object value, Type objectType)
        {
            this.objectID = objectID;
            this.value = value;
            this.objectType = objectType;
        }

        /// <summary>
        /// A unique ID that identifies this particular object.
        /// </summary>
        public long ObjectID
        {
            get
            {
                return objectID;
            }
        }

        /// <summary>
        /// The type of this object.
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return objectType;
            }
        }

        /// <summary>
        /// The value of this object.
        /// </summary>
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
