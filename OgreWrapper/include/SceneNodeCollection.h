//Header
#pragma once

namespace OgreWrapper
{

ref class SceneNode;
ref class SceneNodeCollection : public WrapperCollection<SceneNode^>
{
protected:
	virtual SceneNode^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~SceneNodeCollection() {}

	SceneNode^ getObject(Ogre::SceneNode* nativeObject);

	void destroyObject(Ogre::SceneNode* nativeObject);
};

}