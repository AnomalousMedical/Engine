//Header
#pragma once

namespace Rendering{

ref class TransformKeyFrame;
ref class TransformKeyFrameCollection : public WrapperCollection<TransformKeyFrame^>
{
protected:
	virtual TransformKeyFrame^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~TransformKeyFrameCollection() {}

	TransformKeyFrame^ getObject(Ogre::TransformKeyFrame* nativeObject);

	void destroyObject(Ogre::TransformKeyFrame* nativeObject);
};

}