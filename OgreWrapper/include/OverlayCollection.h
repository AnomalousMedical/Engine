//Header
#pragma once

namespace OgreWrapper{

ref class Overlay;
ref class OverlayCollection : public WrapperCollection<Overlay^>
{
protected:
	virtual Overlay^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~OverlayCollection() {}

	Overlay^ getObject(Ogre::Overlay* nativeObject);

	void destroyObject(Ogre::Overlay* nativeObject);
};

}