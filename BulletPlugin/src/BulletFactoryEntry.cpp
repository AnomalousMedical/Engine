#include "StdAfx.h"
#include "..\include\BulletFactoryEntry.h"
#include "BulletElementDefinition.h"

namespace BulletPlugin
{

BulletFactoryEntry::BulletFactoryEntry(SimObjectBase^ instance, BulletElementDefinition^ element)
:instance(instance), 
element(element)
{
}

void BulletFactoryEntry::createProduct(BulletScene^ scene)
{
    element->createProduct(instance, scene);
}

void BulletFactoryEntry::createStaticProduct(BulletScene^ scene)
{
    element->createStaticProduct(instance, scene);
}

}