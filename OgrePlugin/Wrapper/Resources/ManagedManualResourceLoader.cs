using Anomalous.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    /// <summary>
    /// Interface describing a manual resource loader.
    /// </summary>
    /// <remarks>
    /// Resources are usually loaded from files; however in some cases you
    /// want to be able to set the data up manually instead. This provides
    /// some problems, such as how to reload a Resource if it becomes
    /// unloaded for some reason, either because of memory constraints, or
    /// because a device fails and some or all of the data is lost.
    /// <para>
    /// This interface should be implemented by all classes which wish to
    /// provide manual data to a resource. They provide a pointer to themselves
    /// when defining the resource (via the appropriate ResourceManager), 
    /// and will be called when the Resource tries to load. 
    /// They should implement the loadResource method such that the Resource 
    /// is in the end set up exactly as if it had loaded from a file, 
    /// although the implementations will likely differ between subclasses 
    /// of Resource, which is why no generic algorithm can be stated here.
    /// </para>
    /// <para>
    /// The loader must remain valid for the entire life of the resource,
    /// so that if need be it can be called upon to re-load the resource
    /// at any time.
    /// </para>
    /// </remarks>
    public abstract class ManagedManualResourceLoader : IDisposable
    {
        private IntPtr ptr;
        private CallbackHandler callbackHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        public ManagedManualResourceLoader()
        {
            callbackHandler = new CallbackHandler();
            ptr = callbackHandler.create(this);
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            callbackHandler.Dispose();
            ManagedManualResourceLoader_Delete(ptr);
        }

        /// <summary>
        /// Called when a resource wishes to load.  Note that this could get
        /// called in a background thread even in just a semithreaded ogre
        /// (OGRE_THREAD_SUPPORT==2).  Thus, you must not access the rendersystem from
        /// this callback.  Do that stuff in loadResource.
        /// </summary>
        protected virtual void prepareResource()
        {

        }

        /// <summary>
        /// Called when a resource wishes to prepare.
        /// </summary>
        protected abstract void loadResource();

        /// <summary>
        /// The native pointer.
        /// </summary>
        internal IntPtr Ptr
        {
            get { return ptr; }
        }

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ManagedManualResourceLoader_Create(NativeAction prepareResource, NativeAction loadResource
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedManualResourceLoader_Delete(IntPtr nativeRenderQueueListener);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            static NativeAction prepareResourceCallback;
            static NativeAction loadResourceCallback;

            static CallbackHandler()
            {
                prepareResourceCallback = new NativeAction(prepareResource);
                loadResourceCallback = new NativeAction(loadResource);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void prepareResource(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedManualResourceLoader).prepareResource();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void loadResource(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedManualResourceLoader).loadResource();
            }

            public IntPtr create(ManagedManualResourceLoader obj)
            {
                handle = GCHandle.Alloc(obj);
                return ManagedManualResourceLoader_Create(prepareResourceCallback, loadResourceCallback, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            NativeAction prepareResourceCallback;
            NativeAction loadResourceCallback;

            public IntPtr create(ManagedManualResourceLoader obj)
            {
                prepareResourceCallback = new NativeAction(obj.prepareResource);
                loadResourceCallback = new NativeAction(obj.loadResource);
                return ManagedManualResourceLoader_Create(prepareResourceCallback, loadResourceCallback);
            }

            public void Dispose()
            {

            }
        }
#endif
    }
}
