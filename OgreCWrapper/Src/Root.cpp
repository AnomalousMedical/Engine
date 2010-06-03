#include "Stdafx.h"

extern "C" __declspec(dllexport) Ogre::Root* Root_Create(const char* pluginFileName, const char* configFileName, const char* logFileName)
{
	return new Ogre::Root(pluginFileName, configFileName, logFileName);
}

extern "C" __declspec(dllexport) Ogre::Plugin* RenderSystemPlugin_Create()
{
	Ogre::Plugin* plugin = new Ogre::D3D9Plugin();
	Ogre::Root::getSingleton().installPlugin(plugin);
	return plugin;
}

extern "C" __declspec(dllexport) Ogre::Plugin* CGPlugin_Create()
{
	Ogre::Plugin* plugin = new Ogre::CgPlugin();
	Ogre::Root::getSingleton().installPlugin(plugin);
	return plugin;
}

extern "C" __declspec(dllexport) void Root_Delete(Ogre::Root* ogreRoot)
{
	delete ogreRoot;
}

extern "C" __declspec(dllexport) void RenderSystemPlugin_Delete(Ogre::Plugin* renderSystemPlugin)
{
	delete renderSystemPlugin;
}

extern "C" __declspec(dllexport) void CGPlugin_Delete(Ogre::Plugin* cgPlugin)
{
	delete cgPlugin;
}

extern "C" __declspec(dllexport) void Root_saveConfig(Ogre::Root* root)
{
	root->saveConfig();
}

extern "C" __declspec(dllexport) bool Root_restoreConfig(Ogre::Root* root)
{
	return root->restoreConfig();
}

extern "C" __declspec(dllexport) bool Root_showConfigDialog(Ogre::Root* root)
{
	return root->showConfigDialog();
}

extern "C" __declspec(dllexport) void Root_addRenderSystem(Ogre::Root* root, Ogre::RenderSystem* newRend)
{
	root->addRenderSystem(newRend);
}

extern "C" __declspec(dllexport) Ogre::RenderSystem* Root_getRenderSystemByName(Ogre::Root* root, const char* name)
{
	return root->getRenderSystemByName(name);
}

extern "C" __declspec(dllexport) void Root_setRenderSystem(Ogre::Root* root, Ogre::RenderSystem* system)
{
	root->setRenderSystem(system);
}

extern "C" __declspec(dllexport) Ogre::RenderSystem* Root_getRenderSystem(Ogre::Root* root)
{
	return root->getRenderSystem();
}

extern "C" __declspec(dllexport) Ogre::RenderWindow* Root_initialize(Ogre::Root* root, bool autoCreateWindow)
{
	return root->initialise(autoCreateWindow);
}

extern "C" __declspec(dllexport) Ogre::RenderWindow* Root_initializeTitle(Ogre::Root* root, bool autoCreateWindow, const char* windowTitle)
{
	return root->initialise(autoCreateWindow, windowTitle);
}

extern "C" __declspec(dllexport) Ogre::RenderWindow* Root_initializeTitleCustomCap(Ogre::Root* root, bool autoCreateWindow, const char* windowTitle, const char* customCapabilitiesConfig)
{
	return root->initialise(autoCreateWindow, windowTitle, customCapabilitiesConfig);
}

extern "C" __declspec(dllexport) bool Root_isInitialized(Ogre::Root* root)
{
	return root->isInitialised();
}

extern "C" __declspec(dllexport) Ogre::SceneManager* Root_createSceneManagerTypeName(Ogre::Root* root, const char* typeName)
{
	return root->createSceneManager(typeName);
}

extern "C" __declspec(dllexport) Ogre::SceneManager* Root_createSceneManagerTypeNameInstanceName(Ogre::Root* root, const char* typeName, const char* instanceName)
{
	return root->createSceneManager(typeName, instanceName);
}

extern "C" __declspec(dllexport) Ogre::SceneManager* Root_createSceneManagerTypeMask(Ogre::Root* root, Ogre::SceneType typeMask)
{
	return root->createSceneManager(typeMask);
}

extern "C" __declspec(dllexport) Ogre::SceneManager* Root_createSceneManagerTypeMaskInstanceName(Ogre::Root* root, Ogre::SceneType typeMask, const char* instanceName)
{
	return root->createSceneManager(typeMask, instanceName);
}

extern "C" __declspec(dllexport) void Root_destroySceneManager(Ogre::Root* root, Ogre::SceneManager* sceneManager)
{
	root->destroySceneManager(sceneManager);
}

extern "C" __declspec(dllexport) Ogre::SceneManager* Root_getSceneManager(Ogre::Root* root, const char* instanceName)
{
	return root->getSceneManager(instanceName);
}

extern "C" __declspec(dllexport) void Root_queueEndRendering(Ogre::Root* root)
{
	root->queueEndRendering();
}

extern "C" __declspec(dllexport) void Root_startRendering(Ogre::Root* root)
{
	root->startRendering();
}

extern "C" __declspec(dllexport) bool Root_renderOneFrame(Ogre::Root* root)
{
	return root->renderOneFrame();
}

extern "C" __declspec(dllexport) void Root_shutdown(Ogre::Root* root)
{
	root->shutdown();
}

extern "C" __declspec(dllexport) Ogre::RenderWindow* Root_getAutoCreatedWindow(Ogre::Root* root)
{
	return root->getAutoCreatedWindow();
}

extern "C" __declspec(dllexport) Ogre::RenderWindow* Root_createRenderWindow(Ogre::Root* root, const char* name, uint width, uint height, bool fullScreen)
{
	return root->createRenderWindow(name, width, height, fullScreen);
}

extern "C" __declspec(dllexport) Ogre::RenderWindow* Root_createRenderWindowParams(Ogre::Root* root, const char* name, uint width, uint height, bool fullScreen, const char* vsync, const char* aaMode, const char* fsaaHint, const char* externalWindowHandle, const char* monitorIndex)
{
	Ogre::NameValuePairList params;
	params["vsync"] = vsync;
	if(fsaaHint != 0 && fsaaHint != Ogre::StringUtil::BLANK)
	{
		params["FSAAHint"] = fsaaHint;
	}
	if(aaMode != 0 && aaMode != Ogre::StringUtil::BLANK)
	{
		params["FSAA"] = aaMode;
	}
	if(externalWindowHandle != 0 && externalWindowHandle != Ogre::StringUtil::BLANK)
	{
		params["externalWindowHandle"] = externalWindowHandle;
	}
	params["monitorIndex"] = monitorIndex;
	return root->createRenderWindow(name, width, height, fullScreen, &params);
}

extern "C" __declspec(dllexport) void Root_detachRenderTarget(Ogre::Root* root, Ogre::RenderTarget* pWin)
{
	root->detachRenderTarget(pWin);
}

extern "C" __declspec(dllexport) Ogre::RenderTarget* Root_getRenderTarget(Ogre::Root* root, const char* name)
{
	return root->getRenderTarget(name);
}

extern "C" __declspec(dllexport) void Root_loadPlugin(Ogre::Root* root, const char* pluginName)
{
	root->loadPlugin(pluginName);
}

extern "C" __declspec(dllexport) void Root_unloadPlugin(Ogre::Root* root, const char* pluginName)
{
	root->unloadPlugin(pluginName);
}

extern "C" __declspec(dllexport) bool Root__fireFrameStarted(Ogre::Root* root, float timeSinceLastEvent, float timeSinceLastFrame)
{
	static Ogre::FrameEvent fe;
	fe.timeSinceLastEvent = timeSinceLastEvent;
	fe.timeSinceLastFrame = timeSinceLastFrame;
	return root->_fireFrameStarted(fe);
}

extern "C" __declspec(dllexport) bool Root__fireFrameRenderingQueued(Ogre::Root* root, float timeSinceLastEvent, float timeSinceLastFrame)
{
	static Ogre::FrameEvent fe;
	fe.timeSinceLastEvent = timeSinceLastEvent;
	fe.timeSinceLastFrame = timeSinceLastFrame;
	return root->_fireFrameRenderingQueued(fe);
}

extern "C" __declspec(dllexport) bool Root__fireFrameEnded(Ogre::Root* root, float timeSinceLastEvent, float timeSinceLastFrame)
{
	static Ogre::FrameEvent fe;
	fe.timeSinceLastEvent = timeSinceLastEvent;
	fe.timeSinceLastFrame = timeSinceLastFrame;
	return root->_fireFrameEnded(fe);
}

extern "C" __declspec(dllexport) bool Root__fireFrameStartedNoArg(Ogre::Root* root)
{
	return root->_fireFrameStarted();
}

extern "C" __declspec(dllexport) bool Root__fireFrameRenderingQueuedNoArg(Ogre::Root* root)
{
	return root->_fireFrameRenderingQueued();
}

extern "C" __declspec(dllexport) bool Root__fireFrameEndedNoArg(Ogre::Root* root)
{
	return root->_fireFrameEnded();
}

extern "C" __declspec(dllexport) void Root_clearEventTimes(Ogre::Root* root)
{
	root->clearEventTimes();
}

extern "C" __declspec(dllexport) void Root_setFrameSmoothingPeriod(Ogre::Root* root, float period)
{
	root->setFrameSmoothingPeriod(period);
}

extern "C" __declspec(dllexport) float Root_getFrameSmoothingPeriod(Ogre::Root* root)
{
	return root->getFrameSmoothingPeriod();
}

extern "C" __declspec(dllexport) bool Root__updateAllRenderTargets(Ogre::Root* root)
{
	return root->_updateAllRenderTargets();
}

extern "C" __declspec(dllexport) void Root_addFrameListener(Ogre::Root* root, Ogre::FrameListener* nativeFrameListener)
{
	root->addFrameListener(nativeFrameListener);
}

extern "C" __declspec(dllexport) void Root_removeFrameListener(Ogre::Root* root, Ogre::FrameListener* nativeFrameListener)
{
	root->removeFrameListener(nativeFrameListener);
}

extern "C" __declspec(dllexport) void ArchiveManager_addArchiveFactory(Ogre::ArchiveFactory* archiveFactory)
{
	Ogre::ArchiveManager::getSingleton().addArchiveFactory(archiveFactory);
}