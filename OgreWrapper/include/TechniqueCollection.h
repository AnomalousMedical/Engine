//Header
#pragma once

namespace OgreWrapper{

ref class Technique;
ref class Material;
ref class TechniqueCollection : public WrapperCollection<Technique^>
{
protected:
	virtual Technique^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~TechniqueCollection() {}

	Technique^ getObject(Ogre::Technique* nativeObject, Material^ parent);

	void destroyObject(Ogre::Technique* nativeObject);
};

}