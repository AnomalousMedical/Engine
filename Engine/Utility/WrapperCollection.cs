using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class WrapperCollection<T> : IDisposable
    {
        private static readonly String DISPOSABLE_INTERFACE_NAME = typeof(IDisposable).FullName;

        public delegate T CreateWrapper(IntPtr nativeObject, object[] args);
        public delegate void DestroyWrapper(T wrapper);

        private Dictionary<IntPtr, T> ptrDictionary = new Dictionary<IntPtr, T>();
        private CreateWrapper createCallback;
        private DestroyWrapper destroyCallback;

        /// <summary>
        /// This constructor can be used for objects that implement IDisposable.
        /// </summary>
        /// <param name="createCallback">The wrapper to call to create the object.</param>
        public WrapperCollection(CreateWrapper createCallback)
        {
            if (typeof(T).GetInterface(DISPOSABLE_INTERFACE_NAME) != null)
            {
                this.createCallback = createCallback;
                this.destroyCallback = disposableDelete;
            }
            else
            {
                throw new Exception("Tried to create a WrapperCollection for type {0} that is not IDisposable. Please explicitly defind a destroyCallback and use the other constructor for this type.");
            }
        }

        /// <summary>
        /// This constructor can be used for objects that need a custom destroy method.
        /// </summary>
        /// <param name="createCallback">The function to call to create the object.</param>
        /// <param name="destroyCallback">The function to call to destroy the object.</param>
        public WrapperCollection(CreateWrapper createCallback, DestroyWrapper destroyCallback)
        {
            this.createCallback = createCallback;
            this.destroyCallback = destroyCallback;
        }

        public virtual void Dispose()
        {
            clearObjects();
        }

        public void clearObjects()
        {
            foreach (T obj in ptrDictionary.Values)
            {
                destroyCallback(obj);
            }
            ptrDictionary.Clear();
        }

        public T getObject(IntPtr nativeObject, params object[] args)
        {
            T result;
            if (!ptrDictionary.TryGetValue(nativeObject, out result) && nativeObject != IntPtr.Zero)
            {
                result = createCallback(nativeObject, args);
                ptrDictionary.Add(nativeObject, result);
            }
            return result;
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
                destroyCallback(ptrDictionary[nativeObject]);
                ptrDictionary.Remove(nativeObject);
            }
            return nativeObject;
        }

        public IEnumerable<T> WrappedObjects
        {
            get
            {
                return ptrDictionary.Values;
            }
        }

        public int WrappedObjectCount
        {
            get
            {
                return ptrDictionary.Count;
            }
        }

        private static void disposableDelete(T wrapper)
        {
            ((IDisposable)wrapper).Dispose();
        }
    }
}
