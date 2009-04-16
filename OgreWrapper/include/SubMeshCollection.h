//Header
#pragma once

namespace Engine{

namespace Rendering{

ref class SubMesh;
ref class SubMeshCollection : public WrapperCollection<SubMesh^>
{
protected:
	virtual SubMesh^ createWrapper(void* nativeObject, array<System::Object^>^ args) override;

public:
	virtual ~SubMeshCollection() {}

	SubMesh^ getObject(Ogre::SubMesh* nativeObject);

	void destroyObject(Ogre::SubMesh* nativeObject);
};

}

}