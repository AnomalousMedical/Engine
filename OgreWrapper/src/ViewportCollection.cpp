//Source
#include "stdafx.h"
#include "ViewportCollection.h"
#include "Viewport.h"

namespace OgreWrapper
{

Viewport^ ViewportCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew Viewport(static_cast<Ogre::Viewport*>(nativeObject));
}

Viewport^ ViewportCollection::getObject(Ogre::Viewport* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void ViewportCollection::destroyObject(Ogre::Viewport* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}