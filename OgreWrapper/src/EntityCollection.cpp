//Source
#include "stdafx.h"
#include "EntityCollection.h"
#include "Entity.h"

namespace OgreWrapper{

Entity^ EntityCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew Entity(static_cast<Ogre::Entity*>(nativeObject), (System::String^)args[0], (System::String^)args[1]);
}

Entity^ EntityCollection::getObject(Ogre::Entity* nativeObject, System::String^ identifier, System::String^ meshName)
{
	return getObjectVoid(nativeObject, identifier, meshName);
}

void EntityCollection::destroyObject(Ogre::Entity* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}