//Header
#pragma once

namespace Rendering{

ref class VertexPoseKeyFrame;
ref class VertexPoseKeyFrameCollection : public WrapperCollection<VertexPoseKeyFrame^>
{
protected:
	virtual VertexPoseKeyFrame^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~VertexPoseKeyFrameCollection() {}

	VertexPoseKeyFrame^ getObject(Ogre::VertexPoseKeyFrame* nativeObject);

	void destroyObject(Ogre::VertexPoseKeyFrame* nativeObject);
};

}