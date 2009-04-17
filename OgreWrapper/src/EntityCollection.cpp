//Source
#include "stdafx.h"
#include "EntityCollection.h"
#include "Entity.h"

namespace OgreWrapper{

Entity^ EntityCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew Entity(static_cast<Ogre::Entity*>(nativeObject));
}

Entity^ EntityCollection::getObject(Ogre::Entity* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void EntityCollection::destroyObject(Ogre::Entity* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}