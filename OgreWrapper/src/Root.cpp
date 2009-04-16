#include "StdAfx.h"
#include "..\include\Root.h"

#include "OgreRoot.h"
#include "MarshalUtils.h"
#include "RenderSystem.h"
#include "RenderWindow.h"
#include "RenderScene.h"
#include "RenderTarget.h"

namespace Rendering
{

Root::Root()
:ogreRoot(new Ogre::Root())
{
	instance = this;
}

Root::Root(System::String^ pluginFileName)
:ogreRoot(new Ogre::Root(MarshalUtils::convertString(pluginFileName)))
{
	instance = this;
}

Root::Root(System::String^ pluginFileName, System::String^ configFileName)
:ogreRoot(new Ogre::Root(MarshalUtils::convertString(pluginFileName), MarshalUtils::convertString(configFileName)))
{
	instance = this;
}

Root::Root(System::String^ pluginFileName, System::String^ configFileName, System::String^ logFileName)
:ogreRoot(new Ogre::Root(MarshalUtils::convertString(pluginFileName), MarshalUtils::convertString(configFileName), MarshalUtils::convertString(logFileName)))
{
	instance = this;
}

Root::~Root()
{
	
}

Ogre::Root* Root::getRoot()
{
	return ogreRoot.Get();
}

void Root::saveConfig()
{
	return ogreRoot->saveConfig();
}

bool Root::restoreConfig()
{
	return ogreRoot->restoreConfig();
}

bool Root::showConfigDialog()
{
	return ogreRoot->showConfigDialog();
}

void Root::addRenderSystem(RenderSystem^ newRend)
{
	return ogreRoot->addRenderSystem(newRend->getRenderSystem());
}

RenderSystemList^ Root::getAvailableRenderers()
{
	Ogre::RenderSystemList* ogreRenderSystems = ogreRoot->getAvailableRenderers();
	RenderSystemList^ renderSystemList = gcnew RenderSystemList(ogreRenderSystems->size());
	for(Ogre::RenderSystemList::iterator iter = ogreRenderSystems->begin(); iter != ogreRenderSystems->end(); iter++)
	{
		renderSystemList->Add(renderSystems.getObject(*iter));
	}
	return renderSystemList;
}

RenderSystem^ Root::getRenderSystemByName(System::String^ name)
{
	return renderSystems.getObject(ogreRoot->getRenderSystemByName(MarshalUtils::convertString(name)));
}

void Root::setRenderSystem(RenderSystem^ system)
{
	return ogreRoot->setRenderSystem(system->getRenderSystem());
}

RenderSystem^ Root::getRenderSystem()
{
	return renderSystems.getObject(ogreRoot->getRenderSystem());
}

RenderWindow^ Root::initialize(bool autoCreateWindow)
{
	return renderTargets.getObject(ogreRoot->initialise(autoCreateWindow));
}

RenderWindow^ Root::initialize(bool autoCreateWindow, System::String^ windowTitle)
{
	return renderTargets.getObject(ogreRoot->initialise(autoCreateWindow, MarshalUtils::convertString(windowTitle)));
}

RenderWindow^ Root::initialize(bool autoCreateWindow, System::String^ windowTitle, System::String^ customCapabilitiesConfig)
{
	return renderTargets.getObject(ogreRoot->initialise(autoCreateWindow, MarshalUtils::convertString(windowTitle), MarshalUtils::convertString(customCapabilitiesConfig)));
}

bool Root::isInitialized()
{
	return ogreRoot->isInitialised();
}

RenderScene^ Root::createSceneManager(System::String^ typeName)
{
	return scenes.getObject(ogreRoot->createSceneManager(MarshalUtils::convertString(typeName)));
}

RenderScene^ Root::createSceneManager(System::String^ typeName, System::String^ instanceName)
{
	return scenes.getObject(ogreRoot->createSceneManager(MarshalUtils::convertString(typeName), MarshalUtils::convertString(instanceName)));
}

RenderScene^ Root::createSceneManager(SceneType typeMask)
{
	return scenes.getObject(ogreRoot->createSceneManager(static_cast<Ogre::SceneTypeMask>(typeMask)));
}

RenderScene^ Root::createSceneManager(SceneType typeMask, System::String^ instanceName)
{
	return scenes.getObject(ogreRoot->createSceneManager(static_cast<Ogre::SceneTypeMask>(typeMask), MarshalUtils::convertString(instanceName)));
}

void Root::destroySceneManager(RenderScene^ sceneManager)
{
	Ogre::SceneManager* ogreSceneManager = sceneManager->getSceneManager();
	scenes.destroyObject(ogreSceneManager);
	ogreRoot->destroySceneManager(ogreSceneManager);
}

RenderScene^ Root::getSceneManager(System::String^ instanceName)
{
	return scenes.getObject(ogreRoot->getSceneManager(MarshalUtils::convertString(instanceName)));
}

SceneManagerList^ Root::getSceneManagerIterator()
{
	throw gcnew System::NotImplementedException();
	//return ogreRoot->();
}

System::String^ Root::getErrorDescription(long errorNumber)
{
	return MarshalUtils::convertString(ogreRoot->getErrorDescription(errorNumber));
}

void Root::queueEndRendering()
{
	return ogreRoot->queueEndRendering();
}

void Root::startRendering()
{
	return ogreRoot->startRendering();
}

bool Root::renderOneFrame()
{
	return ogreRoot->renderOneFrame();
}

void Root::shutdown()
{
	return ogreRoot->shutdown();
}

RenderWindow^ Root::getAutoCreatedWindow()
{
	return renderTargets.getObject(ogreRoot->getAutoCreatedWindow());
}

RenderWindow^ Root::createRenderWindow(System::String^ name, unsigned int width, unsigned int height, bool fullScreen)
{
	return renderTargets.getObject(ogreRoot->createRenderWindow(MarshalUtils::convertString(name), width, height, fullScreen));
}

RenderWindow^ Root::createRenderWindow(System::String^ name, unsigned int width, unsigned int height, bool fullScreen, ParamList^ paramList)
{
	Ogre::NameValuePairList pairList;
	for each(System::String^ key in paramList->Keys)
	{
		pairList[MarshalUtils::convertString(key)] = MarshalUtils::convertString(paramList[key]);
	}
	return renderTargets.getObject(ogreRoot->createRenderWindow(MarshalUtils::convertString(name), width, height, fullScreen, &pairList));
}

void Root::detachRenderTarget(RenderTarget^ pWin)
{
	Ogre::RenderTarget* renderTarget = pWin->getRenderTarget();
	renderTargets.destroyObject(renderTarget);
	return ogreRoot->detachRenderTarget(renderTarget);
}

void Root::detachRenderTarget(System::String^ name)
{
	Ogre::RenderTarget* renderTarget = ogreRoot->getRenderTarget(MarshalUtils::convertString(name));
	renderTargets.destroyObject(renderTarget);
	return ogreRoot->detachRenderTarget(renderTarget);
}

RenderTarget^ Root::getRenderTarget(System::String^ name)
{
	return renderTargets.getExistingObject(ogreRoot->getRenderTarget(MarshalUtils::convertString(name)));
}

void Root::loadPlugin(System::String^ pluginName)
{
	return ogreRoot->loadPlugin(MarshalUtils::convertString(pluginName));
}

void Root::unloadPlugin(System::String^ pluginName)
{
	return ogreRoot->unloadPlugin(MarshalUtils::convertString(pluginName));
}

bool Root::_fireFrameStarted(float timeSinceLastEvent, float timeSinceLastFrame)
{
	Ogre::FrameEvent evt;
	evt.timeSinceLastEvent = timeSinceLastEvent;
	evt.timeSinceLastFrame = timeSinceLastFrame;
	return ogreRoot->_fireFrameStarted(evt);
}

bool Root::_fireFrameRenderingQueued(float timeSinceLastEvent, float timeSinceLastFrame)
{
	Ogre::FrameEvent evt;
	evt.timeSinceLastEvent = timeSinceLastEvent;
	evt.timeSinceLastFrame = timeSinceLastFrame;
	return ogreRoot->_fireFrameRenderingQueued(evt);
}

bool Root::_fireFrameEnded(float timeSinceLastEvent, float timeSinceLastFrame)
{
	Ogre::FrameEvent evt;
	evt.timeSinceLastEvent = timeSinceLastEvent;
	evt.timeSinceLastFrame = timeSinceLastFrame;
	return ogreRoot->_fireFrameEnded(evt);
}

bool Root::_fireFrameStarted()
{
	return ogreRoot->_fireFrameStarted();
}

bool Root::_fireFrameRenderingQueued()
{
	return ogreRoot->_fireFrameRenderingQueued();
}

bool Root::_fireFrameEnded()
{
	return ogreRoot->_fireFrameEnded();
}

unsigned long Root::getNextFrameNumber()
{
	return ogreRoot->getNextFrameNumber();
}

void Root::clearEventTimes()
{
	return ogreRoot->clearEventTimes();
}

void Root::setFrameSmoothingPeriod(float period)
{
	return ogreRoot->setFrameSmoothingPeriod(period);
}

float Root::getFrameSmoothingPeriod()
{
	return ogreRoot->getFrameSmoothingPeriod();
}

}