//Source
#include "stdafx.h"
#include "TechniqueCollection.h"
#include "Technique.h"

namespace OgreWrapper{

Technique^ TechniqueCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew Technique(static_cast<Ogre::Technique*>(nativeObject), (Material^)args[0]);
}

Technique^ TechniqueCollection::getObject(Ogre::Technique* nativeObject, Material^ parent)
{
	return getObjectVoid(nativeObject, parent);
}

void TechniqueCollection::destroyObject(Ogre::Technique* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}