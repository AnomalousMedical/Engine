using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Engine.Saving
{
    public class LoadControl
    {
        /// <summary>
        /// The ObjectID given to all objects that are null.
        /// </summary>
        public const long NullObject = -1;
        private static readonly Type[] constructorArgs = { typeof(LoadInfo) };

        private Stack<LoadInfo> pooledInfos = new Stack<LoadInfo>();
        private Dictionary<long, ObjectIdentifier> identiferMap = new Dictionary<long, ObjectIdentifier>();
        private ObjectIdentifier currentIdentifier;
        private LoadInfo loadInfo;
        private TypeFinder typeFinder;

        public LoadControl(TypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
            loadInfo = new LoadInfo(typeFinder);
        }

        /// <summary>
        /// Reset so this object can be used again.
        /// </summary>
        public void reset()
        {
            identiferMap.Clear();
        }

        /// <summary>
        /// Start defining an object that is being loaded. Call this when a new
        /// object is identified in the stream.
        /// </summary>
        /// <param name="identifier">The ObjectIdentifier of the newly found object.</param>
        public void startDefiningObject(ObjectIdentifier identifier, int version)
        {
            loadInfo.reset();
            loadInfo.Version = version;
            currentIdentifier = identifier;
        }

        /// <summary>
        /// Create the object that has been defined. Call this when the end of
        /// an object definition is found in the stream.
        /// </summary>
        /// <returns></returns>
        public Object createCurrentObject()
        {
            ConstructorInfo constructor = currentIdentifier.ObjectType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, constructorArgs, null);
            if (constructor != null)
            {
                currentIdentifier.Value = constructor.Invoke(new Object[] { loadInfo });
                identiferMap.Add(currentIdentifier.ObjectID, currentIdentifier);
                return currentIdentifier.Value;
            }
            else
            {
                throw new SaveException(String.Format("The private constructor {0}(LoadInfo loadInfo) was not found for {1}. Please implement this method.", currentIdentifier.ObjectType.Name, currentIdentifier.ObjectType.FullName));
            }
        }

        /// <summary>
        /// Add a value to the currently defining object.
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <param name="value">The value of the value.</param>
        /// <param name="objectType">The type of the value.</param>
        public void addValue(string name, object value, Type objectType)
        {
            loadInfo.addValue(name, value, objectType);
        }

        /// <summary>
        /// Add an object that has been previoiusly discovered by objectID.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <param name="objectID">The objectID of a previously loaded object to use as the value.</param>
        public void addObjectValue(string name, long objectID)
        {
            if (objectID != -1)
            {
                ObjectIdentifier id = identiferMap[objectID];
                loadInfo.addValue(name, id.Value, id.ObjectType);
            }
            else
            {
                loadInfo.addValue(name, null, typeof(Object));
            }
        }

        public TypeFinder TypeFinder
        {
            get
            {
                return typeFinder;
            }
        }
    }
}
