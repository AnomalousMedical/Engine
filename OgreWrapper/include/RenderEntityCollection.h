//Header
#pragma once

namespace OgreWrapper{

ref class RenderEntity;
ref class RenderEntityCollection : public WrapperCollection<RenderEntity^>
{
protected:
	virtual RenderEntity^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~RenderEntityCollection() {}

	RenderEntity^ getObject(Ogre::Entity* nativeObject, System::String^ identifier, System::String^ meshName);

	void destroyObject(Ogre::Entity* nativeObject);
};

}