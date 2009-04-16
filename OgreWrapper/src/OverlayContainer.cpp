#include "StdAfx.h"
#include "..\include\OverlayContainer.h"

#include "OgreOverlayContainer.h"
#include "MarshalUtils.h"
#include "OverlayManager.h"

namespace OgreWrapper
{

OverlayContainer::OverlayContainer(Ogre::OverlayContainer* overlayContainer)
:OverlayElement(overlayContainer),
overlayContainer( overlayContainer )
{

}

OverlayContainer::~OverlayContainer()
{
	overlayContainer = 0;
}

Ogre::OverlayContainer* OverlayContainer::getOverlayContainer()
{
	return overlayContainer;
}

void OverlayContainer::addChild(OverlayElement^ elem)
{
	overlayContainer->addChild(elem->getOverlayElement());
}

void OverlayContainer::removeChild(System::String^ name)
{
	overlayContainer->removeChild(MarshalUtils::convertString(name));
}

OverlayElement^ OverlayContainer::getChild(System::String^ name)
{
	return OverlayManager::getInstance()->getObject(overlayContainer->getChild(MarshalUtils::convertString(name)));
}

}