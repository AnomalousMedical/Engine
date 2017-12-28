#include "stdafx.h"
#include "OgreFramework.hpp"

extern "C" _AnomalousExport OgreFramework* OgreFramework_Create()
{
	return new OgreFramework();
}

extern "C" _AnomalousExport void OgreFramework_Destroy(OgreFramework* framework)
{
	delete framework;
}

extern "C" _AnomalousExport bool OgreFramework_initOgre(OgreFramework* framework, Ogre::Root* root, Ogre::SceneManager* sceneManager)
{
	return framework->initOgre(root, sceneManager);
}

extern "C" _AnomalousExport void OgreFramework_updateOgre(OgreFramework* framework, double timeSinceLastFrame)
{
	framework->updateOgre(timeSinceLastFrame);
}