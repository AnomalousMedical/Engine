//Header
#pragma once

namespace OgreWrapper
{

ref class Light;
ref class LightCollection : public WrapperCollection<Light^>
{
protected:
	virtual Light^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~LightCollection() {}

	Light^ getObject(Ogre::Light* nativeObject);

	void destroyObject(Ogre::Light* nativeObject);
};

}