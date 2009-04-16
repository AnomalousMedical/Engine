//Source
#include "stdafx.h"
#include "TransformKeyFrameCollection.h"
#include "TransformKeyFrame.h"

namespace OgreWrapper{

TransformKeyFrame^ TransformKeyFrameCollection::createWrapper(void* nativeObject, array<System::Object^>^ args)
{
	return gcnew TransformKeyFrame(static_cast<Ogre::TransformKeyFrame*>(nativeObject));
}

TransformKeyFrame^ TransformKeyFrameCollection::getObject(Ogre::TransformKeyFrame* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void TransformKeyFrameCollection::destroyObject(Ogre::TransformKeyFrame* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}