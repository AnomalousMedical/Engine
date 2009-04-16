//Header
#pragma once

namespace Engine{

namespace Rendering{

ref class AnimationState;
ref class AnimationStateSet;
ref class AnimationStateCollection : public WrapperCollection<AnimationState^>
{
protected:
	virtual AnimationState^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~AnimationStateCollection() {}

	AnimationState^ getObject(Ogre::AnimationState* nativeObject, AnimationStateSet^ parent);

	void destroyObject(Ogre::AnimationState* nativeObject);
};

}

}