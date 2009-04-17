//Header
#pragma once

namespace OgreWrapper
{

ref class Camera;
ref class CameraCollection : public WrapperCollection<Camera^>
{
protected:
	virtual Camera^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~CameraCollection() {}

	Camera^ getObject(Ogre::Camera* nativeObject);

	void destroyObject(Ogre::Camera* nativeObject);
};

}