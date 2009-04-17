//Header
#pragma once

namespace OgreWrapper
{

ref class ManualObject;
ref class ManualObjectCollection : public WrapperCollection<ManualObject^>
{
protected:
	virtual ManualObject^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~ManualObjectCollection() {}

	ManualObject^ getObject(Ogre::ManualObject* nativeObject);

	void destroyObject(Ogre::ManualObject* nativeObject);
};

}