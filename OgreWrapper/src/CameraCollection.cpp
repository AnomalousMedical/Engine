//Source
#include "stdafx.h"
#include "CameraCollection.h"
#include "Camera.h"

namespace OgreWrapper
{

Camera^ CameraCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew Camera(static_cast<Ogre::Camera*>(nativeObject));
}

Camera^ CameraCollection::getObject(Ogre::Camera* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void CameraCollection::destroyObject(Ogre::Camera* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}