#include "StdAfx.h"
#include "..\include\SoftBodyProvider.h"
#include "SoftBodyProviderDefinition.h"

namespace BulletPlugin
{

SoftBodyProvider::SoftBodyProvider(SoftBodyProviderDefinition^ description)
:SimElement(description->Name, description->Subscription)
{
}

SoftBodyProvider::~SoftBodyProvider(void)
{
}

btSoftBody* SoftBodyProvider::createSoftBody(BulletScene^ scene)
{
	btSoftBody* softBody = createSoftBodyImpl(scene);

	return softBody;
}

void SoftBodyProvider::destroySoftBody(BulletScene^ scene)
{
	destroySoftBodyImpl(scene);
}

}