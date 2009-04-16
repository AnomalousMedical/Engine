//Source
#include "stdafx.h"
#include "PassCollection.h"
#include "Pass.h"

namespace Rendering{

Pass^ PassCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew Pass(static_cast<Ogre::Pass*>(nativeObject), (Technique^)args[0]);
}

Pass^ PassCollection::getObject(Ogre::Pass* nativeObject, Technique^ parent)
{
	return getObjectVoid(nativeObject, parent);
}

void PassCollection::destroyObject(Ogre::Pass* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}