#include "StdAfx.h"
#include "..\include\SoftBodyProvider.h"
#include "SoftBodyProviderDefinition.h"

namespace BulletPlugin
{

SoftBodyProvider::SoftBodyProvider(SoftBodyProviderDefinition^ description)
:SimElement(description->Name, description->Subscription),
staticRepresentationCreated(false)
{
}

SoftBodyProvider::~SoftBodyProvider(void)
{
	if(staticRepresentationCreated)
	{
		destroyStaticRepresentationImpl();
	}
}

btSoftBody* SoftBodyProvider::createSoftBody(BulletScene^ scene)
{
	btSoftBody* softBody = static_cast<btSoftBody*>(createSoftBodyImpl(scene));

	return softBody;
}

void SoftBodyProvider::destroySoftBody(BulletScene^ scene)
{
	destroySoftBodyImpl(scene);
}

void SoftBodyProvider::createStaticRepresentation()
{
	staticRepresentationCreated = true;
	createStaticRepresentationImpl();
}

void SoftBodyProvider::setInitialPosition(Vector3% translation, Quaternion% rotation)
{
	updatePositionImpl(translation, rotation);
}

}