//Header
#pragma once

namespace OgreWrapper{

enum class RenderTargetType
{
	RenderWindow,
	RenderTexture,
	MultiRenderTarget
};

ref class RenderWindow;
ref class RenderTarget;
ref class RenderTargetCollection : public WrapperCollection<RenderTarget^>
{
protected:
	virtual RenderTarget^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~RenderTargetCollection() {}

	RenderWindow^ getObject(Ogre::RenderWindow* nativeObject);

	RenderTarget^ getExistingObject(Ogre::RenderTarget* nativeObject);

	void destroyObject(Ogre::RenderTarget* nativeObject);
};

}