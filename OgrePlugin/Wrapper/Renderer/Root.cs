using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;
using Engine;

namespace OgreWrapper
{
    [MultiEnum]
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
        WrapperCollection<RenderTarget> renderTargets = new WrapperCollection<RenderTarget>(RenderTarget.createWrapper);

        IntPtr ogreRoot;
        IntPtr renderSystemPlugin;
        IntPtr cgPlugin;

        IntPtr nativeFrameListener;
        FrameEventCallback frameStart;
        FrameEventCallback frameQueue;
        FrameEventCallback frameEnd;
        FrameEvent frameEvent = new FrameEvent();

        public event FrameEventHandler FrameStarted;
        public event FrameEventHandler FrameRenderingQueued;
        public event FrameEventHandler FrameEnded;

        //Pointers to archive factories (engine archive and embedded)
        EmbeddedResourceArchiveFactory embeddedResources = new EmbeddedResourceArchiveFactory();
        OgreEngineArchiveFactory engineArchives = new OgreEngineArchiveFactory();

        static Root instance;

        public static Root getSingleton()
        {
            return instance;
        }

        public Root(String pluginFileName, String configFileName, String logFileName)
        {
            ogreRoot = Root_Create(pluginFileName, configFileName, logFileName);
            cgPlugin = CGPlugin_Create();
            renderSystemPlugin = RenderSystemPlugin_Create();
            frameStart = new FrameEventCallback(frameStartedCallback);
            frameQueue = new FrameEventCallback(frameQueuedCallback);
            frameEnd = new FrameEventCallback(frameEndedCallback);
            nativeFrameListener = NativeFrameListener_Create(frameStart, frameQueue, frameEnd);
            Root_addFrameListener(ogreRoot, nativeFrameListener);
            ArchiveManager_addArchiveFactory(embeddedResources.NativeFactory);
            ArchiveManager_addArchiveFactory(engineArchives.NativeFactory);
            instance = this;
        }

        public void Dispose()
        {
            Root_removeFrameListener(ogreRoot, nativeFrameListener);
            NativeFrameListener_Delete(nativeFrameListener);
            renderSystems.Dispose();
            scenes.Dispose();
            renderTargets.Dispose();
            Root_Delete(ogreRoot);
            embeddedResources.Dispose();
            engineArchives.Dispose();
            RenderSystemPlugin_Delete(renderSystemPlugin);
            CGPlugin_Delete(cgPlugin);
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

        public RenderSystem getPlatformDefaultRenderSystem()
        {
            return renderSystems.getObject(Root_getPlatformDefaultRenderSystem(ogreRoot));
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
            return renderTargets.getObject(Root_initialize(ogreRoot, autoCreateWindow), RenderTargetType.RenderWindow) as RenderWindow;
        }

        public RenderWindow initialize(bool autoCreateWindow, String windowTitle)
        {
            return renderTargets.getObject(Root_initializeTitle(ogreRoot, autoCreateWindow, windowTitle), RenderTargetType.RenderWindow) as RenderWindow;
        }

        public RenderWindow initialize(bool autoCreateWindow, String windowTitle, String customCapabilitiesConfig)
        {
            return renderTargets.getObject(Root_initializeTitleCustomCap(ogreRoot, autoCreateWindow, windowTitle, customCapabilitiesConfig), RenderTargetType.RenderWindow) as RenderWindow;
        }

        public bool isInitialized()
        {
            return Root_isInitialized(ogreRoot);
        }

        public SceneManager createSceneManager(String typeName)
        {
            return scenes.getObject(Root_createSceneManagerTypeName(ogreRoot, typeName));
        }

        public SceneManager createSceneManager(String typeName, String instanceName)
        {
            return scenes.getObject(Root_createSceneManagerTypeNameInstanceName(ogreRoot, typeName, instanceName));
        }

        public SceneManager createSceneManager(SceneType typeMask)
        {
            return scenes.getObject(Root_createSceneManagerTypeMask(ogreRoot, typeMask));
        }

        public SceneManager createSceneManager(SceneType typeMask, String instanceName)
        {
            return scenes.getObject(Root_createSceneManagerTypeMaskInstanceName(ogreRoot, typeMask, instanceName));
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

        public bool renderOneFrame()
        {
            return Root_renderOneFrame(ogreRoot);
        }

        public void shutdown()
        {
            Root_shutdown(ogreRoot);
        }

        public RenderWindow getAutoCreatedWindow()
        {
            return renderTargets.getObject(Root_getAutoCreatedWindow(ogreRoot), RenderTargetType.RenderWindow) as RenderWindow;
        }

        public RenderWindow createRenderWindow(String name, uint width, uint height, bool fullScreen)
        {
            return renderTargets.getObject(Root_createRenderWindow(ogreRoot, name, width, height, fullScreen), RenderTargetType.RenderWindow) as RenderWindow;
        }

        public RenderWindow createRenderWindow(String name, uint width, uint height, bool fullScreen, Dictionary<String, String> miscParams)
        {
            //Retain miscParams for backward compatability, but split it up here to pass to ogre
            String vsync;
            String aaMode;
            String fsaaHint;
            String externalWindowHandle;
            String monitorIndex;
            miscParams.TryGetValue("vsync", out vsync);
            miscParams.TryGetValue("FSAA", out aaMode);
            miscParams.TryGetValue("FSAAHint", out fsaaHint);
            miscParams.TryGetValue("externalWindowHandle", out externalWindowHandle);
            if(!miscParams.TryGetValue("monitorIndex", out monitorIndex))
            {
                monitorIndex = "0";
            }
            return renderTargets.getObject(Root_createRenderWindowParams(ogreRoot, name, width, height, fullScreen, vsync.ToString(), aaMode, fsaaHint, externalWindowHandle, monitorIndex), RenderTargetType.RenderWindow) as RenderWindow;
        }

        public void detachRenderTarget(RenderTarget pWin)
        {
            IntPtr ogreWindow = pWin.OgreRenderTarget;
            renderTargets.destroyObject(ogreWindow);
            Root_detachRenderTarget(ogreRoot, ogreWindow);
        }

        public void detachRenderTarget(String name)
        {
            detachRenderTarget(getRenderTarget(name));
        }

        public RenderTarget getRenderTarget(String name)
        {
            RenderTarget target;
            renderTargets.getObjectNoCreate(Root_getRenderTarget(ogreRoot, name), out target);
            return target;
        }

        public void loadPlugin(String pluginName)
        {
            Root_loadPlugin(ogreRoot, pluginName);
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

        void frameStartedCallback(float timeSinceLastEvent, float timeSinceLastFrame)
        {
            if(FrameStarted != null)
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

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_Create(String pluginFileName, String configFileName, String logFileName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr RenderSystemPlugin_Create();

        [DllImport("OgreCWrapper")]
        private static extern IntPtr CGPlugin_Create();

        [DllImport("OgreCWrapper")]
        private static extern void Root_Delete(IntPtr ogreRoot);

        [DllImport("OgreCWrapper")]
        private static extern void RenderSystemPlugin_Delete(IntPtr renderSystemPlugin);

        [DllImport("OgreCWrapper")]
        private static extern void CGPlugin_Delete(IntPtr cgPlugin);

        [DllImport("OgreCWrapper")]
        private static extern void Root_saveConfig(IntPtr root);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root_restoreConfig(IntPtr root);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root_showConfigDialog(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern void Root_addRenderSystem(IntPtr root, IntPtr newRend);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_getRenderSystemByName(IntPtr root, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_getPlatformDefaultRenderSystem(IntPtr ogreRoot);

        [DllImport("OgreCWrapper")]
        private static extern void Root_setRenderSystem(IntPtr root, IntPtr system);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_getRenderSystem(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_initialize(IntPtr root, bool autoCreateWindow);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_initializeTitle(IntPtr root, bool autoCreateWindow, String windowTitle);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_initializeTitleCustomCap(IntPtr root, bool autoCreateWindow, String windowTitle, String customCapabilitiesConfig);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root_isInitialized(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_createSceneManagerTypeName(IntPtr root, String typeName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_createSceneManagerTypeNameInstanceName(IntPtr root, String typeName, String instanceName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_createSceneManagerTypeMask(IntPtr root, SceneType typeMask);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_createSceneManagerTypeMaskInstanceName(IntPtr root, SceneType typeMask, String instanceName);

        [DllImport("OgreCWrapper")]
        private static extern void Root_destroySceneManager(IntPtr root, IntPtr sceneManager);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_getSceneManager(IntPtr root, String instanceName);

        [DllImport("OgreCWrapper")]
        private static extern void Root_queueEndRendering(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern void Root_startRendering(IntPtr root);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root_renderOneFrame(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern void Root_shutdown(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_getAutoCreatedWindow(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_createRenderWindow(IntPtr root, String name, uint width, uint height, bool fullScreen);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_createRenderWindowParams(IntPtr root, String name, uint width, uint height, bool fullScreen, String vsync, String aaMode, String fsaaHint, String externalWindowHandle, String monitorIndex);

        [DllImport("OgreCWrapper")]
        private static extern void Root_detachRenderTarget(IntPtr root, IntPtr pWin);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr Root_getRenderTarget(IntPtr root, String name);

        [DllImport("OgreCWrapper")]
        private static extern void Root_loadPlugin(IntPtr root, String pluginName);

        [DllImport("OgreCWrapper")]
        private static extern void Root_unloadPlugin(IntPtr root, String pluginName);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameStarted(IntPtr root, float timeSinceLastEvent, float timeSinceLastFrame);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameRenderingQueued(IntPtr root, float timeSinceLastEvent, float timeSinceLastFrame);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameEnded(IntPtr root, float timeSinceLastEvent, float timeSinceLastFrame);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameStartedNoArg(IntPtr root);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameRenderingQueuedNoArg(IntPtr root);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__fireFrameEndedNoArg(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern void Root_clearEventTimes(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern void Root_setFrameSmoothingPeriod(IntPtr root, float period);

        [DllImport("OgreCWrapper")]
        private static extern float Root_getFrameSmoothingPeriod(IntPtr root);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Root__updateAllRenderTargets(IntPtr root);

        [DllImport("OgreCWrapper")]
        private static extern void Root_addFrameListener(IntPtr root, IntPtr nativeFrameListener);

        [DllImport("OgreCWrapper")]
        private static extern void Root_removeFrameListener(IntPtr root, IntPtr nativeFrameListener);

        //NativeFrameListener
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FrameEventCallback(float timeSinceLastEvent, float timeSinceLastFrame);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr NativeFrameListener_Create(FrameEventCallback frameStartedCallback, FrameEventCallback frameRenderingQueuedCallback, FrameEventCallback frameEndedCallback);

        [DllImport("OgreCWrapper")]
        private static extern void NativeFrameListener_Delete(IntPtr nativeFrameListener);

        [DllImport("OgreCWrapper")]
        private static extern void ArchiveManager_addArchiveFactory(IntPtr archiveFactory);

        #endregion 
    }
}
