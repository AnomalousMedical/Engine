//Source
#include "stdafx.h"
#include "RenderSubEntityCollection.h"
#include "RenderSubEntity.h"

namespace Rendering{

RenderSubEntity^ RenderSubEntityCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew RenderSubEntity(static_cast<Ogre::SubEntity*>(nativeObject));
}

RenderSubEntity^ RenderSubEntityCollection::getObject(Ogre::SubEntity* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void RenderSubEntityCollection::destroyObject(Ogre::SubEntity* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}