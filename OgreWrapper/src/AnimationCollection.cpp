//Source
#include "stdafx.h"
#include "AnimationCollection.h"
#include "Animation.h"

namespace Engine{

namespace Rendering{

Animation^ AnimationCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew Animation(static_cast<Ogre::Animation*>(nativeObject));
}

Animation^ AnimationCollection::getObject(Ogre::Animation* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void AnimationCollection::destroyObject(Ogre::Animation* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}

}