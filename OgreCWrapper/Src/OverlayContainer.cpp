#include "Stdafx.h"

extern "C" __declspec(dllexport) void OverlayContainer_addChild(Ogre::OverlayContainer* overlayContainer, Ogre::OverlayElement* elem)
{
	overlayContainer->addChild(elem);
}

extern "C" __declspec(dllexport) void OverlayContainer_removeChild(Ogre::OverlayContainer* overlayContainer, String name)
{
	overlayContainer->removeChild(name);
}

extern "C" __declspec(dllexport) Ogre::OverlayElement* OverlayContainer_getChild(Ogre::OverlayContainer* overlayContainer, String name)
{
	return overlayContainer->getChild(name);
}