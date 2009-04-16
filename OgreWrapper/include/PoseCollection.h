//Header
#pragma once

namespace Rendering{

ref class Pose;
ref class PoseCollection : public WrapperCollection<Pose^>
{
protected:
	virtual Pose^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~PoseCollection() {}

	Pose^ getObject(Ogre::Pose* nativeObject);

	void destroyObject(Ogre::Pose* nativeObject);
};

}