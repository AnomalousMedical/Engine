using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    /// <summary>
    /// The SharedPtrEntry class holds the managed wrapper instance for a native
    /// class. It also holds the pointer to the heap allocated SharedPtr, which
    /// will keep the resource alive until it is deleted. This will be deleted
    /// when all managed SharedPtrs have been returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class SharedPtrEntry<T> : IDisposable
        where T : IDisposable
    {
	    T managedWrapper;
        LinkedList<SharedPtr<T>> ptrList = new LinkedList<SharedPtr<T>>();
        IntPtr heapSharedPtr;
        IntPtr nativeObject;

	    public SharedPtrEntry(T managedWrapper, IntPtr nativeObject, IntPtr heapSharedPtr)
        {
            this.managedWrapper = managedWrapper;
            this.heapSharedPtr = heapSharedPtr;
            this.nativeObject = nativeObject;
        }
	    
        public void Dispose()
        {
            managedWrapper.Dispose();
        }

        public void addPointer(SharedPtr<T> ptr)
        {
            ptrList.AddLast(ptr);
        }

        public void removePointer(SharedPtr<T> ptr)
        {
            ptrList.Remove(ptr);
        }

        public bool HasReferences
        {
            get
            {
                return ptrList.Count != 0;
            }
        }

        public int NumReferences
        {
            get
            {
                return ptrList.Count;
            }
        }

        public T ManagedWrapper
        {
            get
            {
                return managedWrapper;
            }
        }

        public IEnumerable<SharedPtr<T>> Pointers
        {
            get
            {
                return ptrList;
            }
        }

        public IntPtr NativeObject
        {
            get
            {
                return nativeObject;
            }
        }

        public IntPtr HeapSharedPtr
        {
            get
            {
                return heapSharedPtr;
            }
        }
    }
}
