//Header
#pragma once

namespace Engine{

namespace Rendering{

ref class OverlayElement;
ref class OverlayElementCollection : public WrapperCollection<OverlayElement^>
{
protected:
	virtual OverlayElement^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~OverlayElementCollection() {}

	OverlayElement^ getObject(Ogre::OverlayElement* nativeObject);

	void destroyObject(Ogre::OverlayElement* nativeObject);
};

}

}