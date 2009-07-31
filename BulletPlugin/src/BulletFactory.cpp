#include "StdAfx.h"
#include "..\include\BulletFactory.h"
#include "BulletFactoryEntry.h"
#include "RigidBodyDefinition.h"

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
}

void BulletFactory::createStaticProducts()
{
	for each(BulletFactoryEntry^ entry in rigidBodies)
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

}