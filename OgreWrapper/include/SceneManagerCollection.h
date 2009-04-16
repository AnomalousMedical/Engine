//Header
#pragma once

namespace OgreWrapper{

ref class SceneManager;
ref class Renderer;
ref class RenderSceneCollection : public WrapperCollection<SceneManager^>
{
protected:
	virtual SceneManager^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~RenderSceneCollection() {}

	SceneManager^ getObject(Ogre::SceneManager* nativeObject);

	void destroyObject(Ogre::SceneManager* nativeObject);
};

}