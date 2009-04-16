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
        private LoadInfo loadInfo = new LoadInfo();

        public LoadControl()
        {

        }

        public void startDefiningObject(ObjectIdentifier identifier)
        {
            loadInfo.reset();
            currentIdentifier = identifier;
        }

        public Object createCurrentObject()
        {
            //currentIdentifier.Value = System.Activator.CreateInstance(currentIdentifier.ObjectType, loadInfo);
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

        public void addValue(string name, object value, Type objectType)
        {
            loadInfo.addValue(name, value, objectType);
        }

        public void addObjectValue(string name, long objectID)
        {
            ObjectIdentifier id = identiferMap[objectID];
            loadInfo.addValue(name, id.Value, id.ObjectType);
        }
    }
}
