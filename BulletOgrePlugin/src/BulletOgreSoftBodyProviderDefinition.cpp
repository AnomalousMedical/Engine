#include "StdAfx.h"
#include "BulletOgreSoftBodyProviderDefinition.h"
#include "BulletOgreSoftBodyProvider.h"

namespace BulletOgrePlugin
{

BulletOgreSoftBodyProviderDefinition::BulletOgreSoftBodyProviderDefinition(String^ name)
:SoftBodyProviderDefinition(name)
{

}

BulletOgreSoftBodyProviderDefinition::~BulletOgreSoftBodyProviderDefinition(void)
{

}

SoftBodyProvider^ BulletOgreSoftBodyProviderDefinition::createProductImpl(SimObjectBase^ instance, BulletScene^ bulletScene, SimSubScene^ subScene)
{
	return gcnew BulletOgreSoftBodyProvider(this);
}

BulletOgreSoftBodyProviderDefinition::BulletOgreSoftBodyProviderDefinition(LoadInfo^ info)
:SoftBodyProviderDefinition(info)
{

}

void BulletOgreSoftBodyProviderDefinition::getInfo(SaveInfo^ info)
{
	SoftBodyProviderDefinition::getInfo(info);
}

}