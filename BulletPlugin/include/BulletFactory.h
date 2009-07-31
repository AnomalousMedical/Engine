#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System::Collections::Generic;

namespace BulletPlugin
{

ref class RigidBodyDefinition;
ref class BulletFactoryEntry;
ref class BulletScene;

ref class BulletFactory : public SimElementFactory
{
private:
	BulletScene^ scene;
	List<BulletFactoryEntry^> rigidBodies;

internal:
	void addRigidBody(RigidBodyDefinition^ definition, SimObjectBase^ instance);

public:
	BulletFactory(BulletScene^ scene);

	virtual void createProducts();

	virtual void createStaticProducts();

	virtual void linkProducts();

	virtual void clearDefinitions();
};

}