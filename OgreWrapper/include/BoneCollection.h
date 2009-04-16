//Header
#pragma once

namespace OgreWrapper{

ref class Bone;
ref class BoneCollection : public WrapperCollection<Bone^>
{
protected:
	virtual Bone^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~BoneCollection() {}

	Bone^ getObject(Ogre::Bone* nativeObject);

	void destroyObject(Ogre::Bone* nativeObject);
};

}