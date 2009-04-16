//Source
#include "stdafx.h"
#include "SubEntityCollection.h"
#include "SubEntity.h"

namespace OgreWrapper{

SubEntity^ SubEntityCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew SubEntity(static_cast<Ogre::SubEntity*>(nativeObject));
}

SubEntity^ SubEntityCollection::getObject(Ogre::SubEntity* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void SubEntityCollection::destroyObject(Ogre::SubEntity* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}