using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Logging;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    /// <summary>
    /// This delegate can be passed to any native function that returns a Ogre::SharedPtr to make it get processsed by the collection.
    /// </summary>
    /// <param name="nativeObject"></param>
    /// <param name="stackSharedPtr"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ProcessWrapperObjectDelegate(IntPtr nativeObject, IntPtr stackSharedPtr);

    class SharedPtrCollection<T> : IDisposable
        where T : IDisposable
    {
        /// <summary>
        /// This delegate will create the wrapper classes for the native object in the SharedPtr.
        /// </summary>
        /// <param name="nativeObject"></param>
        /// <returns></returns>
        public delegate T CreateWrapperDelegate(IntPtr nativeObject);

        /// <summary>
        /// This delegate will allocate a new SharedPtr on the heap that will be managed by the entry created for that native object.
        /// </summary>
        /// <param name="stackSharedPtr"></param>
        /// <returns></returns>
        public delegate IntPtr CreateHeapSharedPtrDelegate(IntPtr stackSharedPtr);

        /// <summary>
        /// This delegate will delete the SharedPtr that is on the heap.
        /// </summary>
        /// <param name="heapSharedPtr"></param>
        public delegate void DeleteHeapSharedPtrDelegate(IntPtr heapSharedPtr);

        private CreateWrapperDelegate createWrapper;
        private CreateHeapSharedPtrDelegate createHeapSharedPtr;
        private DeleteHeapSharedPtrDelegate deleteHeapSharedPtr;
        private Dictionary<IntPtr, SharedPtrEntry<T>> ptrDictionary = new Dictionary<IntPtr, SharedPtrEntry<T>>();

        public SharedPtrCollection(CreateWrapperDelegate createWrapper, CreateHeapSharedPtrDelegate createHeapSharedPtr, DeleteHeapSharedPtrDelegate deleteHeapSharedPtr)
        {
            this.createWrapper = createWrapper;
            this.createHeapSharedPtr = createHeapSharedPtr;
            this.deleteHeapSharedPtr = deleteHeapSharedPtr;
            ProcessWrapperCallback = new ProcessWrapperObjectDelegate(processWrapperObject);
        }

        public void Dispose()
        {
            clearObjects();
            ProcessWrapperCallback = null;
        }

        public ProcessWrapperObjectDelegate ProcessWrapperCallback { get; private set; }

        /// <summary>
        /// This function will be called by a native function that is required to
        /// return a SharedPtr. It will process the native object and shared
        /// pointer and create a managed side sharedPtr and wrapper if needed.
        /// This sharedPtr will stay around until the collection detects that is
        /// has no more open handles. In order for this to happen at least one
        /// SharedPtr must be checked out, which will happen naturally since you
        /// want to return one anyway from the function that called the native
        /// function that called back to here.
        /// </summary>
        /// <param name="nativeObject">The object stackSharedPtr points to.</param>
        /// <param name="stackSharedPtr">A SharedPtr on the stack from ogre.</param>
        public void processWrapperObject(IntPtr nativeObject, IntPtr stackSharedPtr)
        {
            if(!ptrDictionary.ContainsKey(nativeObject))
            {
                SharedPtrEntry<T> entry = new SharedPtrEntry<T>(createWrapper(nativeObject), nativeObject, createHeapSharedPtr(stackSharedPtr));
                ptrDictionary.Add(nativeObject, entry);
            }
        }

        /// <summary>
        /// Get a managed SharedPtr for the given ogre object.
        /// </summary>
        /// <param name="nativeObject">The native object to get the pointer for.</param>
        /// <returns>A new SharedPtr for the object.</returns>
        public SharedPtr<T> getObject(IntPtr nativeObject)
        {
            SharedPtrEntry<T> entry;
            if(ptrDictionary.TryGetValue(nativeObject, out entry))
            {
                SharedPtr<T> sp = new SharedPtr<T>(entry.ManagedWrapper, nativeObject, this);
                entry.addPointer(sp);
                return sp;
            }
            return null;
        }

        /// <summary>
        /// Clear all objects in this collection. This will output all pointers
        /// that were still outstanding when the method was called, which could
        /// indicate memory leaks.
        /// </summary>
        public void clearObjects()
        {
            StackTrace st = new StackTrace(true);
            String filename = st.GetFrame(0).GetFileName();
	        foreach(SharedPtrEntry<T> entry in ptrDictionary.Values)
	        {
		        Log.Error("Memory leak detected in {0}.  There were {1} instances of the pointer outstanding.  Double check to make sure all SharedPtrs of this type are being disposed.", filename, entry.NumReferences);
		        foreach(SharedPtr<T> ptr in entry.Pointers)
		        {
			        Log.Error("Leaked pointer stack trace:");
			        foreach(StackFrame f in ptr.ST.GetFrames())
			        {
				        if(f.GetFileName() != null)
				        {
					        Log.Error("-\t{0} in file {1}:{2}:{3}", f.GetMethod(), f.GetFileName(), f.GetFileLineNumber(), f.GetFileColumnNumber());
				        }
			        }
		        }
		        entry.Dispose();
	        }
	        ptrDictionary.Clear();
        }

        /// <summary>
        /// Return a pointer to this collection. This is done in the SharedPtr
        /// dispose method. If all the references are returned the entry will be
        /// disposed and the heap allocated shared pointer will be released.
        /// </summary>
        /// <param name="sharedPtr"></param>
        public void returnPtr(SharedPtr<T> sharedPtr)
        {
            IntPtr key = sharedPtr.NativeObject;
            SharedPtrEntry<T> entry = ptrDictionary[key];
            entry.removePointer(sharedPtr);
            if (!entry.HasReferences)
            {
                ptrDictionary.Remove(key);
                deleteHeapSharedPtr(entry.HeapSharedPtr);
                entry.Dispose();
            }
        }
    }
}
