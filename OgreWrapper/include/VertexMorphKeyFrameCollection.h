//Header
#pragma once

namespace OgreWrapper{

ref class VertexMorphKeyFrame;
ref class VertexMorphKeyFrameCollection : public WrapperCollection<VertexMorphKeyFrame^>
{
protected:
	virtual VertexMorphKeyFrame^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~VertexMorphKeyFrameCollection() {}

	VertexMorphKeyFrame^ getObject(Ogre::VertexMorphKeyFrame* nativeObject);

	void destroyObject(Ogre::VertexMorphKeyFrame* nativeObject);
};

}