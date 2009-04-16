//Source
#include "stdafx.h"
#include "BoneCollection.h"
#include "Bone.h"

namespace Rendering{

Bone^ BoneCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew Bone(static_cast<Ogre::Bone*>(nativeObject));
}

Bone^ BoneCollection::getObject(Ogre::Bone* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void BoneCollection::destroyObject(Ogre::Bone* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}