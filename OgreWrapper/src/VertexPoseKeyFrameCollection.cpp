//Source
#include "stdafx.h"
#include "VertexPoseKeyFrameCollection.h"
#include "VertexPoseKeyFrame.h"

namespace Rendering{

VertexPoseKeyFrame^ VertexPoseKeyFrameCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew VertexPoseKeyFrame(static_cast<Ogre::VertexPoseKeyFrame*>(nativeObject));
}

VertexPoseKeyFrame^ VertexPoseKeyFrameCollection::getObject(Ogre::VertexPoseKeyFrame* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void VertexPoseKeyFrameCollection::destroyObject(Ogre::VertexPoseKeyFrame* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}