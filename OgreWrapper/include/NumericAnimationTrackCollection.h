//Header
#pragma once

namespace Rendering{

ref class NumericAnimationTrack;
ref class NumericAnimationTrackCollection : public WrapperCollection<NumericAnimationTrack^>
{
protected:
	virtual NumericAnimationTrack^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~NumericAnimationTrackCollection() {}

	NumericAnimationTrack^ getObject(Ogre::NumericAnimationTrack* nativeObject);

	void destroyObject(Ogre::NumericAnimationTrack* nativeObject);
};

}