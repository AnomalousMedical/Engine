//Source
#include "stdafx.h"
#include "NumericAnimationTrackCollection.h"
#include "NumericAnimationTrack.h"

namespace Rendering{

NumericAnimationTrack^ NumericAnimationTrackCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew NumericAnimationTrack(static_cast<Ogre::NumericAnimationTrack*>(nativeObject));
}

NumericAnimationTrack^ NumericAnimationTrackCollection::getObject(Ogre::NumericAnimationTrack* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void NumericAnimationTrackCollection::destroyObject(Ogre::NumericAnimationTrack* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}