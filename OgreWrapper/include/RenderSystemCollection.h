//Header
#pragma once

namespace Rendering{

ref class RenderSystem;
ref class RenderSystemCollection : public WrapperCollection<RenderSystem^>
{
protected:
	virtual RenderSystem^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~RenderSystemCollection() {}

	RenderSystem^ getObject(Ogre::RenderSystem* nativeObject);

	void destroyObject(Ogre::RenderSystem* nativeObject);
};

}