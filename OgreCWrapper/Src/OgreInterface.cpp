#include "Stdafx.h"

//This is not actually part of ogre, but is a way for the OgreInterface class to
//get its rendersystem info

enum RenderSystemType
{
	Default = 0,
	D3D11 = 1,
	OpenGL = 2
};

extern "C" _AnomalousExport Ogre::Plugin* OgreInterface_LoadRenderSystem(RenderSystemType rendersystemType)
{
#if defined(WINDOWS) || defined(WINRT)
	String defaultRenderSystem = "RenderSystem_Direct3D11";
#endif

#ifdef MAC_OSX
	String defaultRenderSystem = "/@macBundlePath/../../Frameworks/RenderSystem_GL.framework";
#endif

	Ogre::String name;
	switch(rendersystemType)
	{
		case Default:
			name = defaultRenderSystem;
			break;
		case D3D11:
			name = "RenderSystem_Direct3D11";
			break;
		case OpenGL:
			name = "RenderSystem_GL";
			break;
	}

	try
	{
		Ogre::Root::getSingleton().loadPlugin(name);
	}
	catch(Ogre::Exception& ex)
	{
		Ogre::Root::getSingleton().loadPlugin(defaultRenderSystem);
	}

	return NULL;
}

extern "C" _AnomalousExport void OgreInterface_UnloadRenderSystem(Ogre::Plugin* renderSystemPlugin)
{
	
}

extern "C" _AnomalousExport Ogre::RenderSystem* OgreInterface_GetRenderSystem(RenderSystemType rendersystemType)
{
#if defined(WINDOWS) || defined(WINRT)
	String defaultRenderSystem = "Direct3D11 Rendering Subsystem";
	RenderSystemType defaultRendersystemType = D3D11;
#endif

#ifdef MAC_OSX
	String defaultRenderSystem = "OpenGL Rendering Subsystem";
	RenderSystemType defaultRendersystemType = OpenGL;
#endif

	Ogre::Root* root = Ogre::Root::getSingletonPtr();
	Ogre::String name;
	switch(rendersystemType)
	{
		case Default:
			name = defaultRenderSystem;
			rendersystemType = defaultRendersystemType;
			break;
		case D3D11:
			name = "Direct3D11 Rendering Subsystem";
			break;
		case OpenGL:
			name = "OpenGL Rendering Subsystem";
			break;
	}

	Ogre::RenderSystem* rs = root->getRenderSystemByName(name);
	if(rs == NULL)
	{
		Ogre::LogManager::getSingletonPtr()->getDefaultLog()->stream() << "Could not find Render System '" << name << "' using default of '" << defaultRenderSystem << "' instead.";
		rs = root->getRenderSystemByName(defaultRenderSystem);
		rendersystemType = defaultRendersystemType;
	}

	//Determine any custom settings needed all the time, rendersystemType here
	//will be an actual rendersystem and not the default as we will have determined
	//what it is by this point.
	//Disabled for now, but this is where you do it.
	switch(rendersystemType)
	{
		case D3D11:
			rs->setConfigOption("Min Requested Feature Levels", "10.0");
			//rs->setConfigOption("Max Requested Feature Levels", "11.1");
			break;
	}

	return rs;
}