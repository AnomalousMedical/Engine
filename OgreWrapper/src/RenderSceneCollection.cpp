//Source
#include "stdafx.h"
#include "RenderSceneCollection.h"
#include "RenderScene.h"

namespace Rendering{

RenderScene^ RenderSceneCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew RenderScene(static_cast<Ogre::SceneManager*>(nativeObject));
}

RenderScene^ RenderSceneCollection::getObject(Ogre::SceneManager* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void RenderSceneCollection::destroyObject(Ogre::SceneManager* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}