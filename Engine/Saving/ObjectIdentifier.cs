using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// This struct contains data for an object that is being written to a
    /// stream.
    /// </summary>
    public class ObjectIdentifier
    {
        protected static readonly Type[] constructorArgs = { typeof(LoadInfo) };

        private static Dictionary<Type, ConstructorInfo> ConstructorCache = new Dictionary<Type, ConstructorInfo>();

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

        public ObjectIdentifier(ObjectIdentifier toClone)
            : this(toClone.objectID, toClone.value, toClone.objectType)
        {

        }

        /// <summary>
        /// Create a new instance of the object specified by this identifier, you can override this in subclasses
        /// and there is no need to call the base method, it just returns a new instance in a generic default way.
        /// </summary>
        /// <param name="loadInfo"></param>
        /// <returns></returns>
        public virtual Object restoreObject(LoadInfo loadInfo)
        {
            ConstructorInfo constructor;
            if (!ConstructorCache.TryGetValue(ObjectType, out constructor))
            {
                constructor = ObjectType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(IsLoadConstructor);
                ConstructorCache.Add(ObjectType, constructor);
            }
            if (constructor != null)
            {
                Value = constructor.Invoke(new Object[] { loadInfo });
                return Value;
            }
            else
            {
                throw new SaveException(String.Format("The private constructor {0}(LoadInfo loadInfo) was not found for {1}. Please implement this method.", ObjectType.Name, ObjectType.FullName));
            }
        }

        private bool IsLoadConstructor(ConstructorInfo i)
        {
            bool valid = false;
            if (!i.IsPublic)
            {
                var par = i.GetParameters();
                valid = par.Length == 1 && par[0].ParameterType == typeof(LoadInfo);
            }
            return valid;
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
