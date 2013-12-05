#include "Stdafx.h"

extern "C" _AnomalousExport Ogre::CompositorInstance* CompositorManager_addCompositor(Ogre::Viewport* vp, String compositor, int addPosition)
{
	return Ogre::CompositorManager::getSingleton().addCompositor(vp, compositor, addPosition);
}

extern "C" _AnomalousExport void CompositorManager_removeCompositor(Ogre::Viewport* vp, String compositor)
{
	Ogre::CompositorManager::getSingleton().removeCompositor(vp, compositor);
}

extern "C" _AnomalousExport void CompositorManager_setCompositorEnabled(Ogre::Viewport* vp, String compositor, bool value)
{
	Ogre::CompositorManager::getSingleton().setCompositorEnabled(vp, compositor, value);
}