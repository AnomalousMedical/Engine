using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace OgreNextPlugin
{
    public class RenderSystem : IDisposable
    {
        internal static RenderSystem createWrapper(IntPtr nativePtr, object[] args)
        {
            return new RenderSystem(nativePtr);
        }

        IntPtr renderSystem;
        CallbackHandler callbackHandler;

        internal IntPtr OgreRenderSystem
        {
            get
            {
                return renderSystem;
            }
        }

        public RenderSystem(IntPtr renderSystem)
        {
            this.renderSystem = renderSystem;
            callbackHandler = new CallbackHandler();
        }

        public void Dispose()
        {
            renderSystem = IntPtr.Zero;
        }

        /// <summary>
	    /// Validates the options set for the rendering system, returning a message
        /// if there are problems. 
	    /// </summary>
	    /// <returns>An error message or an empty string if there are no problems.</returns>
        public String validateConfigOptions()
        {
            return RenderSystem_validateConfigOptions(renderSystem);
        }

        public bool hasConfigOption(String name)
        {
            return RenderSystem_hasConfigOption(renderSystem, name);
        }

        public ConfigOption getConfigOption(String name)
        {
            return callbackHandler.getConfigOption(this, name);
        }

        public void setConfigOption(String name, String value)
        {
            RenderSystem_setConfigOption(renderSystem, name, value);
        }

        public void _setViewMatrix(Matrix4x4 view)
        {
            RenderSystem__setViewMatrix(renderSystem, view);
        }

        public void _setProjectionMatrix(Matrix4x4 projection)
        {
            RenderSystem__setProjectionMatrix(renderSystem, projection);
        }

        //public void addListener(RenderSystemListener listener)
        //{
        //    RenderSystem_addListener(renderSystem, listener.Ptr);
        //}

        //public void removeListener(RenderSystemListener listener)
        //{
        //    RenderSystem_removeListener(renderSystem, listener.Ptr);
        //}

        public String Name
        {
            get
            {
                return Marshal.PtrToStringAnsi(RenderSystem_getName(renderSystem));
            }
        }

        #region RenderSystemCapabilities Cheats
        //We only need a couple of these functions, so we will cheat right now and wrap them through this class.

        public bool isShaderProfileSupported(String profile)
        {
            return RenderSystem_isShaderProfileSupported(renderSystem, profile);
        }

        #endregion

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderSystem_isShaderProfileSupported(IntPtr renderSystem, String profile);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern String RenderSystem_validateConfigOptions(IntPtr renderSystem);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem_setConfigOption(IntPtr renderSystem, String name, String value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem__setViewMatrix(IntPtr renderSystem, Matrix4x4 view);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem__setProjectionMatrix(IntPtr renderSystem, Matrix4x4 projection);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderSystem_hasConfigOption(IntPtr renderSystem, String option);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem_getConfigOptionInfo(IntPtr renderSystem, String option, SetConfigInfo setInfo, AddPossibleValue addValues
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem_addListener(IntPtr renderSystem, IntPtr listener);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderSystem_removeListener(IntPtr renderSystem, IntPtr listener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr RenderSystem_getName(IntPtr renderSystem);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void SetConfigInfo(IntPtr name, IntPtr currentValue, bool immutable
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void AddPossibleValue(IntPtr possibleValue
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        /// <summary>
        /// This does not need to dispose since the GCHandle is only active while recovering an option.
        /// </summary>
        class CallbackHandler
        {
            private static SetConfigInfo setInfo;
            private static AddPossibleValue addValue;

            static CallbackHandler()
            {
                setInfo = new SetConfigInfo(setDetails);
                addValue = new AddPossibleValue(addPossibleValue);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(SetConfigInfo))]
            private static void setDetails(IntPtr name, IntPtr currentValue, bool immutable, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ConfigOption)._setDetails(name, currentValue, immutable);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(AddPossibleValue))]
            private static void addPossibleValue(IntPtr possibleValue, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ConfigOption)._addPossibleValue(possibleValue);
            }

            public ConfigOption getConfigOption(RenderSystem renderSystem, String name)
            {
                ConfigOption configOption = new ConfigOption();
                GCHandle handle = GCHandle.Alloc(configOption);
                RenderSystem_getConfigOptionInfo(renderSystem.renderSystem, name, setInfo, addValue, GCHandle.ToIntPtr(handle));
                handle.Free();
                return configOption;
            }
        }
#else
        /// <summary>
        /// This does not need to dispose since the GCHandle is only active while recovering an option.
        /// </summary>
        class CallbackHandler
        {
            private SetConfigInfo setInfo;
            private AddPossibleValue addValue;

            public ConfigOption getConfigOption(RenderSystem renderSystem, String name)
            {
                ConfigOption configOption = new ConfigOption();
                setInfo = new SetConfigInfo(configOption._setDetails);
                addValue = new AddPossibleValue(configOption._addPossibleValue);
                RenderSystem_getConfigOptionInfo(renderSystem.renderSystem, name, setInfo, addValue);
                //Clear delegates to keep ConfigOption from hanging around until the next one is recovered.
                setInfo = null;
                addValue = null;
                return configOption;
            }
        }
#endif

        #endregion
    }
}
