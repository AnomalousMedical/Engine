#include "Stdafx.h"
#include "OgreOverlaySystem.h"

extern "C" _AnomalousExport Ogre::OverlaySystem* OverlaySystem_Create()
{
	return new Ogre::OverlaySystem();
}

extern "C" _AnomalousExport void OverlaySystem_Delete(Ogre::OverlaySystem* overlaySystem)
{
	delete overlaySystem;
}