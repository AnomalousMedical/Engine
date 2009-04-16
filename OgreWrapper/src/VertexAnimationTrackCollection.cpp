//Source
#include "stdafx.h"
#include "VertexAnimationTrackCollection.h"
#include "VertexAnimationTrack.h"

namespace OgreWrapper{

VertexAnimationTrack^ VertexAnimationTrackCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew VertexAnimationTrack(static_cast<Ogre::VertexAnimationTrack*>(nativeObject), (Animation^)args[0]);
}

VertexAnimationTrack^ VertexAnimationTrackCollection::getObject(Ogre::VertexAnimationTrack* nativeObject, Animation^ parent)
{
	return getObjectVoid(nativeObject, parent);
}

void VertexAnimationTrackCollection::destroyObject(Ogre::VertexAnimationTrack* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}