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

	Entity^ getObject(Ogre::Entity* nativeObject);

	void destroyObject(Ogre::Entity* nativeObject);
};

}