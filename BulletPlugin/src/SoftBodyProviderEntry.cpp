#include "StdAfx.h"
#include "..\include\SoftBodyProviderEntry.h"
#include "SoftBodyProviderDefinition.h"

namespace BulletPlugin
{

SoftBodyProviderEntry::SoftBodyProviderEntry(SimObjectBase^ instance, SoftBodyProviderDefinition^ element, SimSubScene^ subScene)
:instance(instance),
element(element),
subScene(subScene)
{
}

SoftBodyProviderEntry::~SoftBodyProviderEntry(void)
{
}

void SoftBodyProviderEntry::createProduct(BulletScene^ scene)
{
	element->createProduct(instance, scene, subScene);
}

void SoftBodyProviderEntry::createStaticProduct(BulletScene^ scene)
{
	element->createStaticProduct(instance, scene, subScene);
}

}