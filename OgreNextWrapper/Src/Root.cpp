#include "Stdafx.h"

#ifdef ANDROID
#include <jni.h>
#include <errno.h>
#include <android_native_app_glue.h>
#include <EGL/egl.h>
#include "Android/OgreAndroidEGLWindow.h"
Ogre::AndroidEGLWindow* androidWindow; //Keep a pointer to the android window, for now only supports 1 window on android.
#endif

extern "C" _AnomalousExport Ogre::Root* Root_Create(const char* pluginFileName, const char* configFileName, const char* logFileName)
{
	return new Ogre::Root(pluginFileName, configFileName, logFileName);
}

extern "C" _AnomalousExport void Root_Delete(Ogre::Root* ogreRoot)
{
	delete ogreRoot;
}

extern "C" _AnomalousExport void Root_saveConfig(Ogre::Root* root)
{
	root->saveConfig();
}

extern "C" _AnomalousExport bool Root_restoreConfig(Ogre::Root* root)
{
	return root->restoreConfig();
}

extern "C" _AnomalousExport bool Root_showConfigDialog(Ogre::Root* root)
{
	return root->showConfigDialog();
}

extern "C" _AnomalousExport void Root_addRenderSystem(Ogre::Root* root, Ogre::RenderSystem* newRend)
{
	root->addRenderSystem(newRend);
}

extern "C" _AnomalousExport Ogre::RenderSystem* Root_getRenderSystemByName(Ogre::Root* root, const char* name)
{
	return root->getRenderSystemByName(name);
}

extern "C" _AnomalousExport void Root_setRenderSystem(Ogre::Root* root, Ogre::RenderSystem* system)
{
	root->setRenderSystem(system);
}

extern "C" _AnomalousExport Ogre::RenderSystem* Root_getRenderSystem(Ogre::Root* root)
{
	return root->getRenderSystem();
}

extern "C" _AnomalousExport Ogre::Window* Root_initialize(Ogre::Root* root, bool autoCreateWindow)
{
	return root->initialise(autoCreateWindow);
}

extern "C" _AnomalousExport Ogre::Window* Root_initializeTitle(Ogre::Root* root, bool autoCreateWindow, const char* windowTitle)
{
	return root->initialise(autoCreateWindow, windowTitle);
}

extern "C" _AnomalousExport Ogre::Window* Root_initializeTitleCustomCap(Ogre::Root* root, bool autoCreateWindow, const char* windowTitle, const char* customCapabilitiesConfig)
{
	return root->initialise(autoCreateWindow, windowTitle, customCapabilitiesConfig);
}

extern "C" _AnomalousExport bool Root_isInitialized(Ogre::Root* root)
{
	return root->isInitialised();
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_createSceneManagerTypeName(Ogre::Root* root, const char* typeName, size_t numWorkerThreads)
{
	return root->createSceneManager(typeName, numWorkerThreads);
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_createSceneManagerTypeNameInstanceName(Ogre::Root* root, const char* typeName, size_t numWorkerThreads, const char* instanceName)
{
	return root->createSceneManager(typeName, numWorkerThreads, instanceName);
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_createSceneManagerTypeMask(Ogre::Root* root, Ogre::SceneType typeMask, size_t numWorkerThreads)
{
	return root->createSceneManager(typeMask, numWorkerThreads);
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_createSceneManagerTypeMaskInstanceName(Ogre::Root* root, Ogre::SceneType typeMask, size_t numWorkerThreads, const char* instanceName)
{
	return root->createSceneManager(typeMask, numWorkerThreads, instanceName);
}

extern "C" _AnomalousExport void Root_destroySceneManager(Ogre::Root* root, Ogre::SceneManager* sceneManager)
{
	root->destroySceneManager(sceneManager);
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_getSceneManager(Ogre::Root* root, const char* instanceName)
{
	return root->getSceneManager(instanceName);
}

extern "C" _AnomalousExport void Root_queueEndRendering(Ogre::Root* root)
{
	root->queueEndRendering();
}

extern "C" _AnomalousExport void Root_startRendering(Ogre::Root* root)
{
	root->startRendering();
}

extern "C" _AnomalousExport bool Root_renderOneFrame(Ogre::Root* root, float timeSinceLastFrame)
{
	return root->renderOneFrame(timeSinceLastFrame);
}

extern "C" _AnomalousExport void Root_shutdown(Ogre::Root* root)
{
	root->shutdown();
}

extern "C" _AnomalousExport Ogre::Window* Root_getAutoCreatedWindow(Ogre::Root* root)
{
	return root->getAutoCreatedWindow();
}

extern "C" _AnomalousExport Ogre::Window* Root_createRenderWindow(Ogre::Root* root, const char* name, uint width, uint height, bool fullScreen)
{
	return root->createRenderWindow(name, width, height, fullScreen);
}

extern "C" _AnomalousExport Ogre::Window* Root_createRenderWindowParams(Ogre::Root* root, const char* name, uint width, uint height, bool fullScreen, const char* vsync, const char* aaMode, const char* fsaaHint, const char* externalWindowHandle, const char* monitorIndex, const char* nvPerfHud, const char* contentScalingFactor)
{
#ifdef ANDROID
	AConfiguration* config = 0;
#endif

	Ogre::NameValuePairList params;
	params["vsync"] = vsync;
	if(fsaaHint != 0)
	{
		params["FSAAHint"] = fsaaHint;
	}
	if(aaMode != 0)
	{
		params["FSAA"] = aaMode;
	}
	if(externalWindowHandle != 0)
	{
#ifdef APPLE_IOS
        unsigned long* handleArray = (unsigned long *)Ogre::StringConverter::parseUnsignedLong(externalWindowHandle);
        params["externalWindowHandle"] = Ogre::StringConverter::toString(handleArray[0]);
        params["externalViewControllerHandle"] = Ogre::StringConverter::toString(handleArray[1]);
        params["externalViewHandle"] = Ogre::StringConverter::toString(handleArray[2]);
#elif ANDROID
		struct android_app* app = (struct android_app*)Ogre::StringConverter::parseUnsignedLong(externalWindowHandle);
		config = AConfiguration_new();
		AConfiguration_fromAssetManager(config, app->activity->assetManager);
		params["externalWindowHandle"] = Ogre::StringConverter::toString(reinterpret_cast<size_t>(app->window));
		params["androidConfig"] = Ogre::StringConverter::toString(reinterpret_cast<size_t>(config));
		params["preserveContext"] = "true";
		params["minDepthBufferSize"] = "24";
		params["maxDepthBufferSize"] = "24";
		params["minColourBufferSize"] = "32";
		params["maxColourBufferSize"] = "32";
		params["maxStencilBufferSize"] = "8";
#else
        params["externalWindowHandle"] = externalWindowHandle;
#endif
	}
	if(nvPerfHud != 0)
	{
		params["useNVPerfHUD"] = nvPerfHud;
	}
	params["monitorIndex"] = monitorIndex;
	params["noAltEnter"] = "true";
    
#ifdef MAC_OSX
		params["macAPI"] = "cocoa";
		params["macAPICocoaUseNSView"] = "true";
#endif
    
#if defined(MAC_OSX) || defined(APPLE_IOS)
		params["contentScalingFactor"] = contentScalingFactor;

        float scaleFactor = Ogre::StringConverter::parseReal(contentScalingFactor);
		width /= scaleFactor;
		height /= scaleFactor;
#endif
	Ogre::Window* rendWin = root->createRenderWindow(name, width, height, fullScreen, &params);

#ifdef ANDROID
	if(config != 0)
	{
		AConfiguration_delete(config);
	}
	androidWindow = static_cast<Ogre::AndroidEGLWindow*>(rendWin); //Assign our android window, note at this time it is only valid to create one window on android
#endif

	return rendWin;
}

extern "C" _AnomalousExport void Root_loadPlugin(Ogre::Root* root, const char* pluginName)
{
	try
	{
		root->loadPlugin(pluginName);
	}
	catch(Ogre::Exception& ex)
	{
		sendExceptionToManagedCode(ex);
	}
}

extern "C" _AnomalousExport void Root_unloadPlugin(Ogre::Root* root, const char* pluginName)
{
	root->unloadPlugin(pluginName);
}

extern "C" _AnomalousExport bool Root__fireFrameStarted(Ogre::Root* root, float timeSinceLastEvent, float timeSinceLastFrame)
{
#ifdef ANDROID
	androidWindow->ensureContextActive();
#endif

	static Ogre::FrameEvent fe;
	fe.timeSinceLastEvent = timeSinceLastEvent;
	fe.timeSinceLastFrame = timeSinceLastFrame;
	return root->_fireFrameStarted(fe);
}

extern "C" _AnomalousExport bool Root__fireFrameRenderingQueued(Ogre::Root* root, float timeSinceLastEvent, float timeSinceLastFrame)
{
	static Ogre::FrameEvent fe;
	fe.timeSinceLastEvent = timeSinceLastEvent;
	fe.timeSinceLastFrame = timeSinceLastFrame;
	return root->_fireFrameRenderingQueued(fe);
}

extern "C" _AnomalousExport bool Root__fireFrameEnded(Ogre::Root* root, float timeSinceLastEvent, float timeSinceLastFrame)
{
	static Ogre::FrameEvent fe;
	fe.timeSinceLastEvent = timeSinceLastEvent;
	fe.timeSinceLastFrame = timeSinceLastFrame;
	return root->_fireFrameEnded(fe);
}

extern "C" _AnomalousExport bool Root__fireFrameStartedNoArg(Ogre::Root* root)
{
	return root->_fireFrameStarted();
}

extern "C" _AnomalousExport bool Root__fireFrameRenderingQueuedNoArg(Ogre::Root* root)
{
	return root->_fireFrameRenderingQueued();
}

extern "C" _AnomalousExport bool Root__fireFrameEndedNoArg(Ogre::Root* root)
{
	return root->_fireFrameEnded();
}

extern "C" _AnomalousExport void Root_clearEventTimes(Ogre::Root* root)
{
	root->clearEventTimes();
}

extern "C" _AnomalousExport void Root_setFrameSmoothingPeriod(Ogre::Root* root, float period)
{
	root->setFrameSmoothingPeriod(period);
}

extern "C" _AnomalousExport float Root_getFrameSmoothingPeriod(Ogre::Root* root)
{
	return root->getFrameSmoothingPeriod();
}

extern "C" _AnomalousExport bool Root__updateAllRenderTargets(Ogre::Root* root)
{
	try
	{
		return root->_updateAllRenderTargets();
	}
	catch(Ogre::Exception& ex)
	{
		return false;
	}
}

extern "C" _AnomalousExport void Root_addFrameListener(Ogre::Root* root, Ogre::FrameListener* nativeFrameListener)
{
	root->addFrameListener(nativeFrameListener);
}

extern "C" _AnomalousExport void Root_removeFrameListener(Ogre::Root* root, Ogre::FrameListener* nativeFrameListener)
{
	root->removeFrameListener(nativeFrameListener);
}

extern "C" _AnomalousExport void ArchiveManager_addArchiveFactory(Ogre::ArchiveFactory* archiveFactory)
{
	Ogre::ArchiveManager::getSingleton().addArchiveFactory(archiveFactory);
}

extern "C" _AnomalousExport unsigned int Root_getDisplayMonitorCount(Ogre::Root* ogreRoot)
{
	return ogreRoot->getDisplayMonitorCount();
}

extern "C" _AnomalousExport Ogre::CompositorManager2 * Root_getCompositorManager2(Ogre::Root * root)
{
	return root->getCompositorManager2();
}