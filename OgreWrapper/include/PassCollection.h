//Header
#pragma once

namespace OgreWrapper{

ref class Pass;
ref class Technique;
ref class PassCollection : public WrapperCollection<Pass^>
{
protected:
	virtual Pass^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~PassCollection() {}

	Pass^ getObject(Ogre::Pass* nativeObject, Technique^ parent);

	void destroyObject(Ogre::Pass* nativeObject);
};

}