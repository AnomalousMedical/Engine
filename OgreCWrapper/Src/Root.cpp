#include "Stdafx.h"

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

extern "C" _AnomalousExport Ogre::RenderWindow* Root_initialize(Ogre::Root* root, bool autoCreateWindow)
{
	return root->initialise(autoCreateWindow);
}

extern "C" _AnomalousExport Ogre::RenderWindow* Root_initializeTitle(Ogre::Root* root, bool autoCreateWindow, const char* windowTitle)
{
	return root->initialise(autoCreateWindow, windowTitle);
}

extern "C" _AnomalousExport Ogre::RenderWindow* Root_initializeTitleCustomCap(Ogre::Root* root, bool autoCreateWindow, const char* windowTitle, const char* customCapabilitiesConfig)
{
	return root->initialise(autoCreateWindow, windowTitle, customCapabilitiesConfig);
}

extern "C" _AnomalousExport bool Root_isInitialized(Ogre::Root* root)
{
	return root->isInitialised();
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_createSceneManagerTypeName(Ogre::Root* root, const char* typeName)
{
	return root->createSceneManager(typeName);
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_createSceneManagerTypeNameInstanceName(Ogre::Root* root, const char* typeName, const char* instanceName)
{
	return root->createSceneManager(typeName, instanceName);
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_createSceneManagerTypeMask(Ogre::Root* root, Ogre::SceneType typeMask)
{
	return root->createSceneManager(typeMask);
}

extern "C" _AnomalousExport Ogre::SceneManager* Root_createSceneManagerTypeMaskInstanceName(Ogre::Root* root, Ogre::SceneType typeMask, const char* instanceName)
{
	return root->createSceneManager(typeMask, instanceName);
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

extern "C" _AnomalousExport bool Root_renderOneFrame(Ogre::Root* root)
{
	return root->renderOneFrame();
}

extern "C" _AnomalousExport void Root_shutdown(Ogre::Root* root)
{
	root->shutdown();
}

extern "C" _AnomalousExport Ogre::RenderWindow* Root_getAutoCreatedWindow(Ogre::Root* root)
{
	return root->getAutoCreatedWindow();
}

extern "C" _AnomalousExport Ogre::RenderWindow* Root_createRenderWindow(Ogre::Root* root, const char* name, uint width, uint height, bool fullScreen)
{
	return root->createRenderWindow(name, width, height, fullScreen);
}

extern "C" _AnomalousExport Ogre::RenderWindow* Root_createRenderWindowParams(Ogre::Root* root, const char* name, uint width, uint height, bool fullScreen, const char* vsync, const char* aaMode, const char* fsaaHint, const char* externalWindowHandle, const char* monitorIndex, const char* nvPerfHud)
{
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
		params["externalWindowHandle"] = externalWindowHandle;
	}
	if(nvPerfHud != 0)
	{
		params["useNVPerfHUD"] = nvPerfHud;
	}
	params["monitorIndex"] = monitorIndex;
	#ifdef MAC_OSX
		params["macAPI"] = "cocoa";
		params["macAPICocoaUseNSView"] = "true";
	#endif
	return root->createRenderWindow(name, width, height, fullScreen, &params);
}

extern "C" _AnomalousExport void Root_detachRenderTarget(Ogre::Root* root, Ogre::RenderTarget* pWin)
{
	root->detachRenderTarget(pWin);
}

extern "C" _AnomalousExport Ogre::RenderTarget* Root_getRenderTarget(Ogre::Root* root, const char* name)
{
	return root->getRenderTarget(name);
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