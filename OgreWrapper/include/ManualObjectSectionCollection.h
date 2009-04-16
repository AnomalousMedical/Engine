//Header
#pragma once

namespace Engine
{

namespace Rendering{

ref class ManualObjectSection;
ref class ManualObjectSectionCollection : public WrapperCollection<ManualObjectSection^>
{
protected:
	virtual ManualObjectSection^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~ManualObjectSectionCollection() {}

	ManualObjectSection^ getObject(Ogre::ManualObject::ManualObjectSection* nativeObject);

	void destroyObject(Ogre::ManualObject::ManualObjectSection* nativeObject);
};

}

}