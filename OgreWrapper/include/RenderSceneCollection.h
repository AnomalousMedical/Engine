//Header
#pragma once

namespace Engine{

namespace Rendering{

ref class RenderScene;
ref class Renderer;
ref class RenderSceneCollection : public WrapperCollection<RenderScene^>
{
protected:
	virtual RenderScene^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~RenderSceneCollection() {}

	RenderScene^ getObject(Ogre::SceneManager* nativeObject);

	void destroyObject(Ogre::SceneManager* nativeObject);
};

}

}