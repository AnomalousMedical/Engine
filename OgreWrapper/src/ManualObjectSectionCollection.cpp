//Source
#include "stdafx.h"
#include "ManualObjectSectionCollection.h"
#include "ManualObjectSection.h"

namespace Engine
{

namespace Rendering{

ManualObjectSection^ ManualObjectSectionCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew ManualObjectSection(static_cast<Ogre::ManualObject::ManualObjectSection*>(nativeObject));
}

ManualObjectSection^ ManualObjectSectionCollection::getObject(Ogre::ManualObject::ManualObjectSection* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void ManualObjectSectionCollection::destroyObject(Ogre::ManualObject::ManualObjectSection* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}

}