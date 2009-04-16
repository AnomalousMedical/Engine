//Header
#pragma once

namespace Rendering{

ref class NodeAnimationTrack;
ref class Animation;
ref class NodeAnimationTrackCollection : public WrapperCollection<NodeAnimationTrack^>
{
protected:
	virtual NodeAnimationTrack^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~NodeAnimationTrackCollection() {}

	NodeAnimationTrack^ getObject(Ogre::NodeAnimationTrack* nativeObject, Animation^ parent);

	void destroyObject(Ogre::NodeAnimationTrack* nativeObject);
};

}