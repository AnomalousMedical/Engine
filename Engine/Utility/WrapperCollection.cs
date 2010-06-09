using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class WrapperCollection<T> : IDisposable
        where T : IDisposable
    {
        public delegate T CreateWrapper(IntPtr nativeObject, object[] args);

        private Dictionary<IntPtr, T> ptrDictionary = new Dictionary<IntPtr, T>();
        private CreateWrapper createCallback;

        public WrapperCollection(CreateWrapper createCallback)
        {
            this.createCallback = createCallback;
        }

        public virtual void Dispose()
        {
            clearObjects();
        }

        public void clearObjects()
        {
            foreach (T obj in ptrDictionary.Values)
            {
                ((IDisposable)obj).Dispose();
            }
            ptrDictionary.Clear();
        }

        public T getObject(IntPtr nativeObject, params object[] args)
        {
            if (!ptrDictionary.ContainsKey(nativeObject))
            {
                ptrDictionary.Add(nativeObject, createCallback(nativeObject, args));
            }
            return ptrDictionary[nativeObject];
        }

        public bool getObjectNoCreate(IntPtr nativeObject, out T obj)
        {
            return ptrDictionary.TryGetValue(nativeObject, out obj);
        }

        /// <summary>
        /// Destroy an object. Returns the pointer that was passed in.
        /// </summary>
        /// <param name="nativeObject">The native object pointer, will be returned.</param>
        /// <returns>The IntPtr for the native object that was passed in.</returns>
        public IntPtr destroyObject(IntPtr nativeObject)
        {
            if (ptrDictionary.ContainsKey(nativeObject))
            {
                ptrDictionary[nativeObject].Dispose();
                ptrDictionary.Remove(nativeObject);
            }
            return nativeObject;
        }
    }
}
