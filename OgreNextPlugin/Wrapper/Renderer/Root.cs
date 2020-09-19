﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;
using Engine;
using Anomalous.Interop;

namespace OgreNextPlugin
{
    [Flags]
    public enum SceneType : ushort
    {
        ST_GENERIC = 1,
        ST_EXTERIOR_CLOSE = 2,
        ST_EXTERIOR_FAR = 4,
        ST_EXTERIOR_REAL_FAR = 8,
        ST_INTERIOR = 16
    };

    public delegate void FrameEventHandler(FrameEvent frameEvent);

    public class Root : IDisposable
    {
        WrapperCollection<RenderSystem> renderSystems = new WrapperCollection<RenderSystem>(RenderSystem.createWrapper);
        WrapperCollection<SceneManager> scenes = new WrapperCollection<SceneManager>(SceneManager.createWrapper);
        WrapperCollection<RenderWindow> renderTargets = new WrapperCollection<RenderWindow>(RenderWindow.createWrapper);

        IntPtr ogreRoot;
        CallbackHandler callbackHandler;
        FrameEvent frameEvent = new FrameEvent();

        OgreLogConnection ogreLog;

        public event FrameEventHandler FrameStarted;
        public event FrameEventHandler FrameRenderingQueued;
        public event FrameEventHandler FrameEnded;
        public event Action Disposed;

        //Pointers to archive factories (engine archive and embedded)
        EmbeddedResourceArchiveFactory embeddedResources = new EmbeddedResourceArchiveFactory();
        OgreEngineArchiveFactory engineArchives = new OgreEngineArchiveFactory();
        MemoryArchiveFactory memoryArchives = new MemoryArchiveFactory();
        EmbeddedScalableResourceArchiveFactory embeddedScalableResources = new EmbeddedScalableResourceArchiveFactory();
        OgreScalableEngineArchiveFactory scalableEngineArchives = new OgreScalableEngineArchiveFactory();

        static Root instance;

        static Root()
        {
            OgreExceptionManager.initializeOgreExceptionManager();
        }

        public static Root getSingleton()
        {
            return instance;
        }

        public IntPtr NativePtr
        {
            get
            {
                return ogreRoot;
            }
        }

        public Root(String pluginFileName, String configFileName, String logFileName)
        {
            ogreRoot = Root_Create(pluginFileName, configFileName, logFileName);
            ogreLog = new OgreLogConnection();
            ogreLog.subscribe();

            callbackHandler = new CallbackHandler(this);

            ArchiveManager_addArchiveFactory(embeddedResources.NativeFactory);
            ArchiveManager_addArchiveFactory(engineArchives.NativeFactory);
            ArchiveManager_addArchiveFactory(memoryArchives.NativeFactory);
            ArchiveManager_addArchiveFactory(embeddedScalableResources.NativeFactory);
            ArchiveManager_addArchiveFactory(scalableEngineArchives.NativeFactory);
            instance = this;
        }

        private CompositorManager2 compositorManager2;
        public CompositorManager2 CompositorManager2
        {
            get
            {
                return compositorManager2 
                    ?? (compositorManager2 = new CompositorManager2(Root_getCompositorManager2(ogreRoot)));
            }
        }

        public void Dispose()
        {
            callbackHandler.Dispose();
            renderSystems.Dispose();
            scenes.Dispose();
            renderTargets.Dispose();
            Root_Delete(ogreRoot);
            embeddedResources.Dispose();
            engineArchives.Dispose();
            memoryArchives.Dispose();
            embeddedScalableResources.Dispose();
            scalableEngineArchives.Dispose();
            ogreLog.Dispose();
            if (Disposed != null)
            {
                Disposed.Invoke();
            }
        }

        public void saveConfig()
        {
            Root_saveConfig(ogreRoot);
        }

        public bool restoreConfig()
        {
            return Root_restoreConfig(ogreRoot);
        }

        public bool showConfigDialog()
        {
            return Root_showConfigDialog(ogreRoot);
        }

        public void addRenderSystem(RenderSystem newRend)
        {
            Root_addRenderSystem(ogreRoot, newRend.OgreRenderSystem);
        }

        public RenderSystem getRenderSystemByName(String name)
        {
            return renderSystems.getObject(Root_getRenderSystemByName(ogreRoot, name));
        }

        public void setRenderSystem(RenderSystem system)
        {
            Root_setRenderSystem(ogreRoot, system.OgreRenderSystem);
        }

        public RenderSystem getRenderSystem()
        {
            return renderSystems.getObject(Root_getRenderSystem(ogreRoot));
        }

        public RenderWindow initialize(bool autoCreateWindow)
        {
            return renderTargets.getObject(Root_initialize(ogreRoot, autoCreateWindow)) as RenderWindow;
        }

        public RenderWindow initialize(bool autoCreateWindow, String windowTitle)
        {
            return renderTargets.getObject(Root_initializeTitle(ogreRoot, autoCreateWindow, windowTitle)) as RenderWindow;
        }

        public RenderWindow initialize(bool autoCreateWindow, String windowTitle, String customCapabilitiesConfig)
        {
            return renderTargets.getObject(Root_initializeTitleCustomCap(ogreRoot, autoCreateWindow, windowTitle, customCapabilitiesConfig)) as RenderWindow;
        }

        public bool isInitialized()
        {
            return Root_isInitialized(ogreRoot);
        }

        public SceneManager createSceneManager(String typeName, uint numWorkerThreads)
        {
            return scenes.getObject(Root_createSceneManagerTypeName(ogreRoot, typeName, new UIntPtr(numWorkerThreads)));
        }

        public SceneManager createSceneManager(String typeName, String instanceName, uint numWorkerThreads)
        {
            return scenes.getObject(Root_createSceneManagerTypeNameInstanceName(ogreRoot, typeName, new UIntPtr(numWorkerThreads), instanceName));
        }

        public SceneManager createSceneManager(SceneType typeMask, uint numWorkerThreads)
        {
            return scenes.getObject(Root_createSceneManagerTypeMask(ogreRoot, typeMask, new UIntPtr(numWorkerThreads)));
        }

        public SceneManager createSceneManager(SceneType typeMask, String instanceName, uint numWorkerThreads)
        {
            return scenes.getObject(Root_createSceneManagerTypeMaskInstanceName(ogreRoot, typeMask, new UIntPtr(numWorkerThreads), instanceName));
        }

        public void destroySceneManager(SceneManager sceneManager)
        {
            IntPtr ogreSceneManager = sceneManager.OgreSceneManager;
            scenes.destroyObject(ogreSceneManager);
            Root_destroySceneManager(ogreRoot, ogreSceneManager);
        }

        public SceneManager getSceneManager(String instanceName)
        {
            return scenes.getObject(Root_getSceneManager(ogreRoot, instanceName));
        }

        public void queueEndRendering()
        {
            Root_queueEndRendering(ogreRoot);
        }

        public void startRendering()
        {
            Root_startRendering(ogreRoot);
        }

        public bool renderOneFrame(float timeSinceLastFrame)
        {
            return Root_renderOneFrame(ogreRoot, timeSinceLastFrame);
        }

        public void shutdown()
        {
            Root_shutdown(ogreRoot);
        }

        public RenderWindow getAutoCreatedWindow()
        {
            return renderTargets.getObject(Root_getAutoCreatedWindow(ogreRoot)) as RenderWindow;
        }

        public RenderWindow createRenderWindow(String name, uint width, uint height, bool fullScreen)
        {
            return renderTargets.getObject(Root_createRenderWindow(ogreRoot, name, width, height, fullScreen)) as RenderWindow;
        }

        public RenderWindow createRenderWindow(String name, uint width, uint height, bool fullScreen, Dictionary<String, String> miscParams)
        {
            //Retain miscParams for backward compatability, but split it up here to pass to ogre
            String vsync;
            String aaMode;
            String fsaaHint;
            String externalWindowHandle;
            String monitorIndex;
            String nvPerfHud;
            String contentScalingFactor;
            miscParams.TryGetValue("vsync", out vsync);
            miscParams.TryGetValue("FSAA", out aaMode);
            miscParams.TryGetValue("FSAAHint", out fsaaHint);
            miscParams.TryGetValue("externalWindowHandle", out externalWindowHandle);
            miscParams.TryGetValue("useNVPerfHUD", out nvPerfHud);
            miscParams.TryGetValue("contentScalingFactor", out contentScalingFactor);
            if (!miscParams.TryGetValue("monitorIndex", out monitorIndex))
            {
                monitorIndex = "0";
            }
            return renderTargets.getObject(Root_createRenderWindowParams(ogreRoot, name, width, height, fullScreen, vsync.ToString(), aaMode, fsaaHint, externalWindowHandle, monitorIndex, nvPerfHud, contentScalingFactor)) as RenderWindow;
        }

        public void loadPlugin(String pluginName)
        {
            Root_loadPlugin(ogreRoot, pluginName);
            OgreExceptionManager.fireAnyException();
        }

        public void unloadPlugin(String pluginName)
        {
            Root_unloadPlugin(ogreRoot, pluginName);
        }

        public bool _fireFrameStarted(float timeSinceLastEvent, float timeSinceLastFrame)
        {
            return Root__fireFrameStarted(ogreRoot, timeSinceLastEvent, timeSinceLastFrame);
        }

        public bool _fireFrameRenderingQueued(float timeSinceLastEvent, float timeSinceLastFrame)
        {
            return Root__fireFrameRenderingQueued(ogreRoot, timeSinceLastEvent, timeSinceLastFrame);
        }

        public bool _fireFrameEnded(float timeSinceLastEvent, float timeSinceLastFrame)
        {
            return Root__fireFrameEnded(ogreRoot, timeSinceLastEvent, timeSinceLastFrame);
        }

        public bool _fireFrameStarted()
        {
            return Root__fireFrameStartedNoArg(ogreRoot);
        }

        public bool _fireFrameRenderingQueued()
        {
            return Root__fireFrameRenderingQueuedNoArg(ogreRoot);
        }

        public bool _fireFrameEnded()
        {
            return Root__fireFrameEndedNoArg(ogreRoot);
        }

        public void clearEventTimes()
        {
            Root_clearEventTimes(ogreRoot);
        }

        public void setFrameSmoothingPeriod(float period)
        {
            Root_setFrameSmoothingPeriod(ogreRoot, period);
        }

        public float getFrameSmoothingPeriod()
        {
            return Root_getFrameSmoothingPeriod(ogreRoot);
        }

        public bool _updateAllRenderTargets()
        {
            return Root__updateAllRenderTargets(ogreRoot);
        }

        public uint getDisplayMonitorCount()
        {
            return Root_getDisplayMonitorCount(ogreRoot);
        }

        public void addArchiveFactory(OgreManagedArchiveFactory factory)
        {
            ArchiveManager_addArchiveFactory(factory.NativeFactory);
        }

        /// <summary>
        /// This function will wrap up a pointer with a RenderSystem object.
        /// It is intended only to be called from OgreInterface and is not a
        /// normal ogre function.
        /// </summary>
        /// <param name="renderSystem"></param>
        /// <returns></returns>
        internal RenderSystem _getRenderSystemWrapper(IntPtr renderSystem)
        {
            return renderSystems.getObject(renderSystem);
        }

        void frameStartedCallback(float timeSinceLastEvent, float timeSinceLastFrame)
        {
            if (FrameStarted != null)
            {
                frameEvent.timeSinceLastEvent = timeSinceLastEvent;
                frameEvent.timeSinceLastFrame = timeSinceLastFrame;
                FrameStarted.Invoke(frameEvent);
            }
        }

        void frameQueuedCallback(float timeSinceLastEvent, float timeSinceLastFrame)
        {
            if (FrameRenderingQueued != null)
            {
                frameEvent.timeSinceLastEvent = timeSinceLastEvent;
                frameEvent.timeSinceLastFrame = timeSinceLastFrame;
                FrameRenderingQueued.Invoke(frameEvent);
            }
        }

        void frameEndedCallback(float timeSinceLastEvent, float timeSinceLastFrame)
        {
            if (FrameEnded != null)
            {
                frameEvent.timeSinceLastEvent = timeSinceLastEvent;
                frameEvent.timeSinceLastFrame = timeSinceLastFrame;
                FrameEnded.Invoke(frameEvent);
            }
        }


        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_Create(String pluginFileName, String configFileName, String logFileName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_Delete(IntPtr ogreRoot);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_saveConfig(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root_restoreConfig(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root_showConfigDialog(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_addRenderSystem(IntPtr root, IntPtr newRend);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_getRenderSystemByName(IntPtr root, String name);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_setRenderSystem(IntPtr root, IntPtr system);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_getRenderSystem(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_initialize(IntPtr root, bool autoCreateWindow);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_initializeTitle(IntPtr root, bool autoCreateWindow, String windowTitle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_initializeTitleCustomCap(IntPtr root, bool autoCreateWindow, String windowTitle, String customCapabilitiesConfig);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root_isInitialized(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_createSceneManagerTypeName(IntPtr root, String typeName, UIntPtr numWorkerThreads);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_createSceneManagerTypeNameInstanceName(IntPtr root, String typeName, UIntPtr numWorkerThreads, String instanceName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_createSceneManagerTypeMask(IntPtr root, SceneType typeMask, UIntPtr numWorkerThreads);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_createSceneManagerTypeMaskInstanceName(IntPtr root, SceneType typeMask, UIntPtr numWorkerThreads, String instanceName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_destroySceneManager(IntPtr root, IntPtr sceneManager);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_getSceneManager(IntPtr root, String instanceName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_queueEndRendering(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_startRendering(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root_renderOneFrame(IntPtr root, float timeSinceLastFrame);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_shutdown(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_getAutoCreatedWindow(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_createRenderWindow(IntPtr root, String name, uint width, uint height, bool fullScreen);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_createRenderWindowParams(IntPtr root, String name, uint width, uint height, bool fullScreen, String vsync, String aaMode, String fsaaHint, String externalWindowHandle, String monitorIndex, String nvPerfHud, String contentScalingFactor);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_loadPlugin(IntPtr root, String pluginName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_unloadPlugin(IntPtr root, String pluginName);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameStarted(IntPtr root, float timeSinceLastEvent, float timeSinceLastFrame);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameRenderingQueued(IntPtr root, float timeSinceLastEvent, float timeSinceLastFrame);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameEnded(IntPtr root, float timeSinceLastEvent, float timeSinceLastFrame);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameStartedNoArg(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameRenderingQueuedNoArg(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameEndedNoArg(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_clearEventTimes(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_setFrameSmoothingPeriod(IntPtr root, float period);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern float Root_getFrameSmoothingPeriod(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__updateAllRenderTargets(IntPtr root);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_addFrameListener(IntPtr root, IntPtr nativeFrameListener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Root_removeFrameListener(IntPtr root, IntPtr nativeFrameListener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr NativeFrameListener_Create(FrameEventCallback frameStartedCallback, FrameEventCallback frameRenderingQueuedCallback, FrameEventCallback frameEndedCallback);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeFrameListener_Delete(IntPtr nativeFrameListener);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ArchiveManager_addArchiveFactory(IntPtr archiveFactory);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint Root_getDisplayMonitorCount(IntPtr ogreRoot);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Root_getCompositorManager2(IntPtr root);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FrameEventCallback(float arg0, float arg1); //No special version for aot, we do not need an instance for this one

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            static FrameEventCallback frameStart;
            static FrameEventCallback frameQueue;
            static FrameEventCallback frameEnd;

            static CallbackHandler()
            {
                frameStart = new FrameEventCallback(frameStartedStatic);
                frameQueue = new FrameEventCallback(frameQueuedStatic);
                frameEnd = new FrameEventCallback(frameEndedStatic);
            }

            //Note that we cheat on our FrameEvent callbacks since this is a singleton anyway.
            [Anomalous.Interop.MonoPInvokeCallback(typeof(FrameEventCallback))]
            static void frameStartedStatic(float timeSinceLastEvent, float timeSinceLastFrame)
            {
                instance.frameStartedCallback(timeSinceLastEvent, timeSinceLastFrame);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(FrameEventCallback))]
            static void frameQueuedStatic(float timeSinceLastEvent, float timeSinceLastFrame)
            {
                instance.frameQueuedCallback(timeSinceLastEvent, timeSinceLastFrame);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(FrameEventCallback))]
            static void frameEndedStatic(float timeSinceLastEvent, float timeSinceLastFrame)
            {
                instance.frameEndedCallback(timeSinceLastEvent, timeSinceLastFrame);
            }

            IntPtr nativeFrameListener;
            Root root;

            public CallbackHandler(Root root)
            {
                this.root = root;
                nativeFrameListener = NativeFrameListener_Create(frameStart, frameQueue, frameEnd);
                Root_addFrameListener(root.ogreRoot, nativeFrameListener);
            }

            public void Dispose()
            {
                Root_removeFrameListener(root.ogreRoot, nativeFrameListener);
                NativeFrameListener_Delete(nativeFrameListener);
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            IntPtr nativeFrameListener;
            FrameEventCallback frameStart;
            FrameEventCallback frameQueue;
            FrameEventCallback frameEnd;
            Root root;

            public CallbackHandler(Root root)
            {
                this.root = root;
                frameStart = new FrameEventCallback(root.frameStartedCallback);
                frameQueue = new FrameEventCallback(root.frameQueuedCallback);
                frameEnd = new FrameEventCallback(root.frameEndedCallback);
                nativeFrameListener = NativeFrameListener_Create(frameStart, frameQueue, frameEnd);
                Root_addFrameListener(root.ogreRoot, nativeFrameListener);
            }

            public void Dispose()
            {
                Root_removeFrameListener(root.ogreRoot, nativeFrameListener);
                NativeFrameListener_Delete(nativeFrameListener);
            }
        }
#endif

        #endregion 
    }
}
