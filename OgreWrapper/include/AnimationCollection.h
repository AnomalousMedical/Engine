//Header
#pragma once

namespace OgreWrapper{

ref class Animation;
ref class AnimationCollection : public WrapperCollection<Animation^>
{
protected:
	virtual Animation^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~AnimationCollection() {}

	Animation^ getObject(Ogre::Animation* nativeObject);

	void destroyObject(Ogre::Animation* nativeObject);
};

}