#include "Stdafx.h"

//This is not actually part of ogre, but is a way for the OgreInterface class to
//get its rendersystem info

enum RendersystemType
{
	DEFAULT = 0,
	D3D9 = 1,
	D3D11 = 2,
	OPEN_GL = 3
};

extern "C" _AnomalousExport Ogre::Plugin* OgreInterface_LoadRenderSystem(RendersystemType rendersystemType)
{
#ifdef WINDOWS
	String defaultRenderSystem = "RenderSystem_Direct3D9";
#endif

#ifdef MAC_OSX
	String defaultRenderSystem = "RenderSystem_GL";
#endif

	Ogre::String name;
	switch(rendersystemType)
	{
		case DEFAULT:
			name = defaultRenderSystem;
			break;
		case D3D9:
			name = "RenderSystem_Direct3D9";
			break;
		case D3D11:
			name = "RenderSystem_Direct3D11";
			break;
		case OPEN_GL:
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

extern "C" _AnomalousExport Ogre::RenderSystem* OgreInterface_GetRenderSystem(RendersystemType rendersystemType)
{
#ifdef WINDOWS
	String defaultRenderSystem = "Direct3D9 Rendering Subsystem";
	RendersystemType defaultRendersystemType = D3D9;
#endif

#ifdef MAC_OSX
	String defaultRenderSystem = "RenderSystem_GL";
	RendersystemType defaultRendersystemType = OPEN_GL;
#endif

	Ogre::Root* root = Ogre::Root::getSingletonPtr();
	Ogre::String name;
	switch(rendersystemType)
	{
		case DEFAULT:
			name = defaultRenderSystem;
			rendersystemType = defaultRendersystemType;
			break;
		case D3D9:
			name = "Direct3D9 Rendering Subsystem";
			break;
		case D3D11:
			name = "Direct3D11 Rendering Subsystem";
			break;
		case OPEN_GL:
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
	switch(rendersystemType)
	{
		case D3D9:
			rs->setConfigOption("Multi device memory hint", "Auto hardware buffers management");
			break;
	}

	return rs;
}