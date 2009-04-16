//Source
#include "stdafx.h"
#include "OverlayCollection.h"
#include "Overlay.h"

namespace OgreWrapper{

Overlay^ OverlayCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew Overlay(static_cast<Ogre::Overlay*>(nativeObject));
}

Overlay^ OverlayCollection::getObject(Ogre::Overlay* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void OverlayCollection::destroyObject(Ogre::Overlay* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}