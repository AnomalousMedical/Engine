//Header
#pragma once

namespace Engine{

namespace Rendering{

ref class VertexAnimationTrack;
ref class Animation;
ref class VertexAnimationTrackCollection : public WrapperCollection<VertexAnimationTrack^>
{
protected:
	virtual VertexAnimationTrack^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~VertexAnimationTrackCollection() {}

	VertexAnimationTrack^ getObject(Ogre::VertexAnimationTrack* nativeObject, Animation^ parent);

	void destroyObject(Ogre::VertexAnimationTrack* nativeObject);
};

}

}