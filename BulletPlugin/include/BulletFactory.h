#pragma once

using namespace Engine;
using namespace Engine::ObjectManagement;
using namespace System::Collections::Generic;

namespace BulletPlugin
{

ref class RigidBodyDefinition;
ref class BulletFactoryEntry;
ref class BulletScene;
ref class TypedConstraintDefinition;
ref class SoftBodyDefinition;
ref class SoftBodyProviderEntry;
ref class SoftBodyProviderDefinition;
ref class BulletElementDefinition;

ref class BulletFactory : public SimElementFactory
{
private:
	BulletScene^ scene;
	List<BulletFactoryEntry^> rigidBodies;
	List<BulletFactoryEntry^> softBodies;
	List<BulletFactoryEntry^> typedConstraints;
	List<SoftBodyProviderEntry^> softBodyProviders;
	List<BulletFactoryEntry^> softBodyAnchors;

internal:
	void addRigidBody(RigidBodyDefinition^ definition, SimObjectBase^ instance);

	void addTypedConstraint(TypedConstraintDefinition^ definition, SimObjectBase^ instance);

	void addSoftBody(SoftBodyDefinition^ definition, SimObjectBase^ instance);

	void addSoftBodyProviderDefinition(SoftBodyProviderDefinition^ definition, SimObjectBase^ instance, SimSubScene^ subScene);

	void addSoftBodyAnchorOrJointDefinition(BulletElementDefinition^ definition, SimObjectBase^ instance);

public:
	BulletFactory(BulletScene^ scene);

	virtual void createProducts();

	virtual void createStaticProducts();

	virtual void linkProducts();

	virtual void clearDefinitions();
};

}