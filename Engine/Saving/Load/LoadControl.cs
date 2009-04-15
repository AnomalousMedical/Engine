using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    public class LoadControl
    {
        /// <summary>
        /// The ObjectID given to all objects that are null.
        /// </summary>
        public const long NullObject = -1;

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
            currentIdentifier.Value = System.Activator.CreateInstance(currentIdentifier.ObjectType, loadInfo);
            identiferMap.Add(currentIdentifier.ObjectID, currentIdentifier);
            return currentIdentifier.Value;
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
