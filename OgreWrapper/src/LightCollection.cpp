//Source
#include "stdafx.h"
#include "LightCollection.h"
#include "Light.h"

namespace OgreWrapper
{

Light^ LightCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew Light(static_cast<Ogre::Light*>(nativeObject));
}

Light^ LightCollection::getObject(Ogre::Light* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void LightCollection::destroyObject(Ogre::Light* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}