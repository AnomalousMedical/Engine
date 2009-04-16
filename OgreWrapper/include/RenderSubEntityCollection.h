//Header
#pragma once

namespace Rendering{

ref class RenderSubEntity;
ref class RenderSubEntityCollection : public WrapperCollection<RenderSubEntity^>
{
protected:
	virtual RenderSubEntity^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~RenderSubEntityCollection() {}

	RenderSubEntity^ getObject(Ogre::SubEntity* nativeObject);

	void destroyObject(Ogre::SubEntity* nativeObject);
};

}