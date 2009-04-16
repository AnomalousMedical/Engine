//Source
#include "stdafx.h"
#include "RenderEntityCollection.h"
#include "RenderEntity.h"

namespace OgreWrapper{

RenderEntity^ RenderEntityCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew RenderEntity(static_cast<Ogre::Entity*>(nativeObject), (System::String^)args[0], (System::String^)args[1]);
}

RenderEntity^ RenderEntityCollection::getObject(Ogre::Entity* nativeObject, System::String^ identifier, System::String^ meshName)
{
	return getObjectVoid(nativeObject, identifier, meshName);
}

void RenderEntityCollection::destroyObject(Ogre::Entity* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}