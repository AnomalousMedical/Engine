//Source
#include "stdafx.h"
#include "RenderSystemCollection.h"
#include "RenderSystem.h"

namespace OgreWrapper{

RenderSystem^ RenderSystemCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew RenderSystem(static_cast<Ogre::RenderSystem*>(nativeObject));
}

RenderSystem^ RenderSystemCollection::getObject(Ogre::RenderSystem* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void RenderSystemCollection::destroyObject(Ogre::RenderSystem* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}