//Source
#include "stdafx.h"
#include "PoseCollection.h"
#include "Pose.h"

namespace Rendering{

Pose^ PoseCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew Pose(static_cast<Ogre::Pose*>(nativeObject));
}

Pose^ PoseCollection::getObject(Ogre::Pose* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void PoseCollection::destroyObject(Ogre::Pose* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}