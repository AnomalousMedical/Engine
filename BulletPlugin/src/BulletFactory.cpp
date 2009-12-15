#include "StdAfx.h"
#include "..\include\BulletFactory.h"
#include "BulletFactoryEntry.h"
#include "RigidBodyDefinition.h"
#include "TypedConstraintDefinition.h"
#include "SoftBodyDefinition.h"

namespace BulletPlugin
{

BulletFactory::BulletFactory(BulletScene^ scene)
:scene(scene)
{
}

void BulletFactory::createProducts()
{
	for each(BulletFactoryEntry^ entry in rigidBodies)
	{
		entry->createProduct(scene);
	}
	for each(BulletFactoryEntry^ entry in softBodies)
	{
		entry->createProduct(scene);
	}
	for each(BulletFactoryEntry^ entry in typedConstraints)
	{
		entry->createProduct(scene);
	}
}

void BulletFactory::createStaticProducts()
{
	for each(BulletFactoryEntry^ entry in rigidBodies)
	{
		entry->createStaticProduct(scene);
	}
	for each(BulletFactoryEntry^ entry in softBodies)
	{
		entry->createStaticProduct(scene);
	}
	for each(BulletFactoryEntry^ entry in typedConstraints)
	{
		entry->createStaticProduct(scene);
	}
}

void BulletFactory::linkProducts()
{

}

void BulletFactory::clearDefinitions()
{
	rigidBodies.Clear();
}

void BulletFactory::addRigidBody(RigidBodyDefinition^ definition, SimObjectBase^ instance)
{
	rigidBodies.Add(gcnew BulletFactoryEntry(instance, definition));
}

void BulletFactory::addSoftBody(SoftBodyDefinition^ definition, SimObjectBase^ instance)
{
	softBodies.Add(gcnew BulletFactoryEntry(instance, definition));
}

void BulletFactory::addTypedConstraint(TypedConstraintDefinition^ definition, SimObjectBase^ instance)
{
	typedConstraints.Add(gcnew BulletFactoryEntry(instance, definition));
}

}