//Source
#include "stdafx.h"
#include "ManualObjectCollection.h"
#include "ManualObject.h"

namespace OgreWrapper
{

ManualObject^ ManualObjectCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew ManualObject(static_cast<Ogre::ManualObject*>(nativeObject));
}

ManualObject^ ManualObjectCollection::getObject(Ogre::ManualObject* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void ManualObjectCollection::destroyObject(Ogre::ManualObject* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}