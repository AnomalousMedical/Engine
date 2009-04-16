///// <file>Renderer.cpp</file>
///// <author>Andrew Piper</author>
///// <company>Joint Based Engineering</company>
///// <copyright>
///// Copyright (c) Joint Based Engineering 2008, All rights reserved
///// </copyright>
//
#include "StdAfx.h"
//#include "..\include\Renderer.h"
//
//#include "Ogre.h"
//#include "MarshalUtils.h"
//
//#include "RenderScene.h"
//#include "RenderTarget.h"
//#include "NRendererUpdate.h"
//#include "RenderWindow.h"
//#include "OgreLogListener.h"
//#include "RenderMaterialManager.h"
//#include "MeshManager.h"
//#include "HardwareBufferManager.h"
//#include "SkeletonManager.h"
//
//namespace Engine{
//
//namespace Rendering{
//
//Renderer::Renderer(void)
//:ogreLogListener( new OgreLogListener() ),
//root( new Ogre::Root( "", "", "" ) ), 
//rendererUpdate( new NRendererUpdate( root ) )
//{
//	Ogre::Log* log = Ogre::LogManager::getSingleton().getDefaultLog();
//	log->addListener(ogreLogListener);
//	log->setDebugOutputEnabled( false );
//}
//
//Renderer::~Renderer()
//{
//	RenderMaterialManager::getInstance()->cleanupReferences();
//	MeshManager::getInstance()->cleanupReferences();
//	SkeletonManager::getInstance()->cleanupReferences();
//	HardwareBufferManager::getInstance()->clearAllBufferWrappers();
//
//	if( rendererUpdate )
//	{
//		delete rendererUpdate;
//		rendererUpdate = 0;
//	}
//	if( root )
//	{
//		root->shutdown();
//		delete root;
//		root = 0;
//	}
//	if( ogreLogListener )
//	{
//		delete ogreLogListener;
//		ogreLogListener = 0;
//	}
//}
//
//bool Renderer::initialize()
//{
//	try{
//		root->loadPlugin("RenderSystem_Direct3D9");
//
//		Ogre::RenderSystem* rs = root->getRenderSystemByName("Direct3D9 Rendering Subsystem");
//		Ogre::String valid = rs->validateConfigOptions();
//		if( valid.length() != 0 ){
//			Logging::Log::Default->sendMessage("Invalid Ogre configuration {0}", Logging::LogLevel::Error, "Renderer", MarshalUtils::convertString(valid));
//		}
//		root->setRenderSystem(rs);
//		root->initialise( false, "" );
//
//		root->loadPlugin("Plugin_CgProgramManager");
//		return true;
//	}
//	catch( Ogre::Exception& e )
//	{
//		Logging::Log::Default->sendMessage("Ogre exception initializing Renderer.  Message: {0}", Logging::LogLevel::Error, "Renderer", MarshalUtils::convertString(e.getFullDescription()));
//	}
//	return false;
//}
//
//void Renderer::buildParamList(ParamList^ params, Ogre::NameValuePairList& pairList)
//{
//	if( params != nullptr )
//	{
//		for each(System::String^ key in params->Keys)
//		{
//			pairList[MarshalUtils::convertString(key)] = MarshalUtils::convertString(params[key]);
//		}
//	}
//}
//
//RenderWindow^ Renderer::createWindow( int width, int height, bool fullscreen, System::String^ windowName, ParamList^ miscParams )
//{
//	Ogre::NameValuePairList wndProp;
//	buildParamList(miscParams, wndProp);
//	
//	Ogre::RenderWindow* ogreRenderTarget = root->createRenderWindow(MarshalUtils::convertString( windowName ), width, height, fullscreen, &wndProp);
//
//	return renderTargets.getObject(ogreRenderTarget);
//}
//
//RenderWindow^ Renderer::embedWindow( int width, int height, System::String^ windowName, Windowing::OSWindow^ windowHandle, ParamList^ miscParams )
//{
//	Ogre::NameValuePairList wndProp;
//	wndProp["externalWindowHandle"] = Ogre::StringConverter::toString(windowHandle->Handle.ToInt32());
//	buildParamList(miscParams, wndProp);
//
//	Ogre::RenderWindow* ogreRenderTarget = root->createRenderWindow(MarshalUtils::convertString( windowName ), width, height, false, &wndProp );
//
//	return renderTargets.getObject(ogreRenderTarget);
//}
//
//RenderTarget^ Renderer::getRenderTarget(System::String^ name)
//{
//	return renderTargets.getExistingObject(root->getRenderTarget(MarshalUtils::convertString(name)));
//}
//
//void Renderer::destroyRenderTarget( RenderTarget^ target )
//{
//	Ogre::RenderTarget* ogreTarget = target->getRenderTarget();
//	Logging::Log::Default->sendMessage("Render Target " + target->getName() + " Statistics", Logging::LogLevel::Info, "Rendering");
//	Logging::Log::Default->sendMessage("--Last FPS: " + ogreTarget->getLastFPS(), Logging::LogLevel::Info, "Rendering");
//	Logging::Log::Default->sendMessage("--Average FPS: " + ogreTarget->getAverageFPS(), Logging::LogLevel::Info, "Rendering");
//	Logging::Log::Default->sendMessage("--Best FPS: " + ogreTarget->getBestFPS(), Logging::LogLevel::Info, "Rendering");
//	Logging::Log::Default->sendMessage("--Worst FPS: " + ogreTarget->getWorstFPS(), Logging::LogLevel::Info, "Rendering");
//	Logging::Log::Default->sendMessage("--Best Frame Time: " + ogreTarget->getBestFrameTime(), Logging::LogLevel::Info, "Rendering");
//	Logging::Log::Default->sendMessage("--Worst Frame Time: " + ogreTarget->getWorstFrameTime(), Logging::LogLevel::Info, "Rendering");
//
//	renderTargets.destroyObject(ogreTarget);
//	root->detachRenderTarget( ogreTarget );
//}
//
//RenderScene^ Renderer::createScene( String^ name )
//{
//	return scenes.getObject(root->createSceneManager(Ogre::ST_GENERIC, MarshalUtils::convertString( name )));
//}
//
//void Renderer::destroyScene( RenderScene^ scene )
//{
//	Ogre::SceneManager* sceneManager = scene->getSceneManager();
//	scenes.destroyObject(sceneManager);
//	root->destroySceneManager(sceneManager);
//}
//
//void Renderer::attachToTimer(Timing::Timer^ timer)
//{
//	timer->addFullSpeedUpdateListener( rendererUpdate );
//}
//
//}
//
//}