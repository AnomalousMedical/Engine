//Header
#pragma once

namespace OgreWrapper{

ref class SubEntity;
ref class SubEntityCollection : public WrapperCollection<SubEntity^>
{
protected:
	virtual SubEntity^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~SubEntityCollection() {}

	SubEntity^ getObject(Ogre::SubEntity* nativeObject);

	void destroyObject(Ogre::SubEntity* nativeObject);
};

}