//Source
#include "stdafx.h"
#include "VertexMorphKeyFrameCollection.h"
#include "VertexMorphKeyFrame.h"

namespace Rendering{

VertexMorphKeyFrame^ VertexMorphKeyFrameCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew VertexMorphKeyFrame(static_cast<Ogre::VertexMorphKeyFrame*>(nativeObject));
}

VertexMorphKeyFrame^ VertexMorphKeyFrameCollection::getObject(Ogre::VertexMorphKeyFrame* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void VertexMorphKeyFrameCollection::destroyObject(Ogre::VertexMorphKeyFrame* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}