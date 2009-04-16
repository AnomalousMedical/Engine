//Source
#include "stdafx.h"
#include "RenderSceneCollection.h"
#include "RenderScene.h"

namespace OgreWrapper{

SceneManager^ RenderSceneCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew SceneManager(static_cast<Ogre::SceneManager*>(nativeObject));
}

SceneManager^ RenderSceneCollection::getObject(Ogre::SceneManager* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void RenderSceneCollection::destroyObject(Ogre::SceneManager* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}