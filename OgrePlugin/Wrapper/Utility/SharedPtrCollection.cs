using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Logging;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    /// <summary>
    /// This delegate can be passed to any native function that returns a Ogre::SharedPtr to make it get processsed by the collection.
    /// </summary>
    /// <param name="nativeObject"></param>
    /// <param name="stackSharedPtr"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ProcessWrapperObjectDelegate(IntPtr nativeObject, IntPtr stackSharedPtr); //AOT handled by client classes.

    /// <summary>
    /// <para>
    /// This class provides a mechanism for dealing with Ogre::SharedPtr
    /// instances. These are reference counted pointers owned by unmanaged ogre
    /// memory that are usually used to hold a memory intensive resource or
    /// something similar. They just contain a pointer to another class that
    /// contains what we are actually interested in. However, they cannot simply
    /// be moved over to the managed side or there could be memory problems if
    /// the pointer is freed on the unmanaged side while the managed side still
    /// thinks its alive.
    /// </para>
    /// <para>
    /// To get around this problem this collection provides a way to deal with
    /// the SharedPtrs by allocating one on the unmanaged heap the first time it
    /// is requested by managed code. This SharedPtr will now be kept around
    /// keeping the Ogre resource alive until all the managed pointers to it
    /// have gone away. The heap SharedPtr will then be deleted and Ogre will
    /// take over from there. Obviously the user on the managed side must
    /// dispose the pointers they are returned or this will not work. This is
    /// easily accomplished with using blocks.
    /// </para>
    /// <para>
    /// The process involved to accomplish this is somewhat tricky, but it is
    /// easily repeatable. The idea is to have the PInvoke function that returns
    /// the SharedPtr instead have its signature modified from the basic Ogre
    /// implementation. It should take the object and arguments to get the
    /// results as normal, but also include a ProcessWrapperObjectDelegate
    /// argument that calls back into this class.  This can be passed through 
    /// whatever appropriate method is needed. It should also return a pointer
    /// to the object the SharedPtr points to or the actual resource itself. See
    /// the MaterialManager for some examples.
    /// </para>
    /// <para>
    /// Now the actual process goes like this. 
    /// <list type="">
    /// <item>The managed wrapper function calls the native function with the correct arguments.</item>
    /// <item>The unmanaged function gets a stack allocated Ogre::SharedPtr using the appropriate method being wrapped.</item>
    /// <item>This pointer and the actual object pointer are passed to the ProcessWrapperObjectDelegate.</item>
    /// <item>This function processes the shared pointer and if needed will create the heap allocated unmanaged SharedPtr</item>
    /// <item>These methods return, now it is imperative that a managed SharedPtr be returned so the reference counting on this side can take effect or the heap pointer will leak and cause the ogre resource (which can be large) to leak.</item>
    /// <item>The user does something with the pointer and disposes of it when they are done.</item>
    /// <item>If all references are deleted when the object is disposed the heap allocated pointer is destroyed.</item>
    /// </list>
    /// </para>
    /// <para>
    /// The collections work off of three callback delegates. The first will
    /// create the managed wrapper class using the pointer to the actual ogre
    /// object to be wrapped. The second will create a heap allocated
    /// Ogre::SharedPtr from the given Ogre::SharedPtr and will likely just
    /// point to a PInvoke import. The third will delete this shared pointer off
    /// the heap when required.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

        public SharedPtrCollection(CreateWrapperDelegate createWrapper, CreateHeapSharedPtrDelegate createHeapSharedPtr, DeleteHeapSharedPtrDelegate deleteHeapSharedPtr, ProcessWrapperObjectDelegate overrideProcessWrapperDelegate = null)
        {
            this.createWrapper = createWrapper;
            this.createHeapSharedPtr = createHeapSharedPtr;
            this.deleteHeapSharedPtr = deleteHeapSharedPtr;
            if (overrideProcessWrapperDelegate == null)
            {
                ProcessWrapperCallback = new ProcessWrapperObjectDelegate(processWrapperObject);
            }
            else
            {
                ProcessWrapperCallback = overrideProcessWrapperDelegate;
            }
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
            if (nativeObject != IntPtr.Zero && !ptrDictionary.ContainsKey(nativeObject))
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
            if (nativeObject != IntPtr.Zero && ptrDictionary.TryGetValue(nativeObject, out entry))
            {
                SharedPtr<T> sp = new SharedPtr<T>(entry.ManagedWrapper, nativeObject, entry.HeapSharedPtr, this);
                entry.addPointer(sp);
                return sp;
            }
            return null;
        }

        /// <summary>
        /// Get a temporary pointer to a shared object, if the object exists in this collection that wrapper will be used
        /// or else a new one will be created but not tracked. These objects should be considered extremely volitale
        /// since they are not tracked by shared ptrs at all, they should be considered valid for the duration of a
        /// callback or other case where it is extremely unlikely the material will be destroyed.
        /// </summary>
        /// <param name="nativeObject"></param>
        /// <returns></returns>
        public T getTemporaryObject(IntPtr nativeObject, Func<IntPtr, T> createWrapperCallback)
        {
            if(nativeObject == IntPtr.Zero)
            {
                return default(T);
            }
            SharedPtrEntry<T> entry;
            if (ptrDictionary.TryGetValue(nativeObject, out entry))
            {
                return entry.ManagedWrapper;
            }
            else
            {
                return createWrapperCallback(nativeObject);
            }
        }

        /// <summary>
        /// Clear all objects in this collection. This will output all pointers
        /// that were still outstanding when the method was called, which could
        /// indicate memory leaks.
        /// </summary>
        public void clearObjects()
        {
            String filename = "Unknown";
            if (OgreInterface.TrackMemoryLeaks)
            {
                StackTrace st = new StackTrace(true);
                filename = st.GetFrame(0).GetFileName();

                if (ptrDictionary.Count > 0)
                {
                    Log.ImportantInfo("{0} memory leaks detected in the SharedPtrCollection {1}.", ptrDictionary.Count, this.GetType().Name);
                }
                else
                {
                    Log.ImportantInfo("No memory leaks detected in the SharedPtrCollection {0}.", this.GetType().Name);
                }
            }

            foreach (SharedPtrEntry<T> entry in ptrDictionary.Values)
            {
                Log.Error("Memory leak detected in {0}.  There were {1} instances of the pointer outstanding.  Double check to make sure all SharedPtrs of this type are being disposed.", filename, entry.NumReferences);
                if (OgreInterface.TrackMemoryLeaks)
                {
                    foreach (SharedPtr<T> ptr in entry.Pointers)
                    {
                        Log.Error("Leaked pointer stack trace:");
                        foreach (StackFrame f in ptr.ST.GetFrames())
                        {
                            if (f.GetFileName() != null)
                            {
                                Log.Error("-\t{0} in file {1}:{2}:{3}", f.GetMethod(), f.GetFileName(), f.GetFileLineNumber(), f.GetFileColumnNumber());
                            }
                        }
                    }
                }
                else
                {
                    Log.Error("No stack trace info available. Please enable OgreInterface.TrackMemoryLeaks to view this info.");
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
