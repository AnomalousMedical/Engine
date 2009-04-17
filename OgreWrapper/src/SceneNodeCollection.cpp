//Source
#include "stdafx.h"
#include "SceneNodeCollection.h"
#include "SceneNode.h"

namespace OgreWrapper
{

SceneNode^ SceneNodeCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew SceneNode(static_cast<Ogre::SceneNode*>(nativeObject));
}

SceneNode^ SceneNodeCollection::getObject(Ogre::SceneNode* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void SceneNodeCollection::destroyObject(Ogre::SceneNode* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}