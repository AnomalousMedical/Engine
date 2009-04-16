//Header
#pragma once

namespace OgreWrapper{

ref class Entity;
ref class EntityCollection : public WrapperCollection<Entity^>
{
protected:
	virtual Entity^ createWrapper(void* nativeObject, ...array<System::Object^>^ args) override;

public:
	virtual ~EntityCollection() {}

	Entity^ getObject(Ogre::Entity* nativeObject, System::String^ identifier, System::String^ meshName);

	void destroyObject(Ogre::Entity* nativeObject);
};

}