//Source
#include "stdafx.h"
#include "AnimationStateCollection.h"
#include "AnimationState.h"

namespace OgreWrapper{

AnimationState^ AnimationStateCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew AnimationState(static_cast<Ogre::AnimationState*>(nativeObject), (AnimationStateSet^)args[0]);
}

AnimationState^ AnimationStateCollection::getObject(Ogre::AnimationState* nativeObject, AnimationStateSet^ parent)
{
	return getObjectVoid(nativeObject, parent);
}

void AnimationStateCollection::destroyObject(Ogre::AnimationState* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}