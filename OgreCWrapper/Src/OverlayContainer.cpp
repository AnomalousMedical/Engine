#include "Stdafx.h"

extern "C" _AnomalousExport void OverlayContainer_addChild(Ogre::OverlayContainer* overlayContainer, Ogre::OverlayElement* elem)
{
	overlayContainer->addChild(elem);
}

extern "C" _AnomalousExport void OverlayContainer_removeChild(Ogre::OverlayContainer* overlayContainer, String name)
{
	overlayContainer->removeChild(name);
}

extern "C" _AnomalousExport Ogre::OverlayElement* OverlayContainer_getChild(Ogre::OverlayContainer* overlayContainer, String name)
{
	return overlayContainer->getChild(name);
}