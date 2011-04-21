using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;

namespace Anomaly
{
    public class AnomalyClipboard
    {
        private static CopySaver copySaver = new CopySaver();
        private static Saveable storedObject;

        private AnomalyClipboard() { }

        public static void storeObject(Saveable toStore)
        {
            storedObject = toStore;
        }

        public static object copyStoredObject()
        {
            if (storedObject != null)
            {
                return copySaver.copyObject(storedObject);
            }
            return null;
        }

        public static T copyStoredObject<T>()
            where T : Saveable
        {
            if (storedObject != null && storedObject is T)
            {
                return copySaver.copy<T>((T)storedObject);
            }
            return default(T);
        }

        public static Type StoredObjectType
        {
            get
            {
                return storedObject != null ? storedObject.GetType() : null;
            }
        }

        public static bool HasStoredObject
        {
            get
            {
                return storedObject != null;
            }
        }
    }
}
