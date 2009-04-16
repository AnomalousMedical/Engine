//Source
#include "stdafx.h"
#include "NodeAnimationTrackCollection.h"
#include "NodeAnimationTrack.h"

namespace Rendering{

NodeAnimationTrack^ NodeAnimationTrackCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	return gcnew NodeAnimationTrack(static_cast<Ogre::NodeAnimationTrack*>(nativeObject), (Animation^)args[0]);
}

NodeAnimationTrack^ NodeAnimationTrackCollection::getObject(Ogre::NodeAnimationTrack* nativeObject, Animation^ parent)
{
	return getObjectVoid(nativeObject, parent);
}

void NodeAnimationTrackCollection::destroyObject(Ogre::NodeAnimationTrack* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}