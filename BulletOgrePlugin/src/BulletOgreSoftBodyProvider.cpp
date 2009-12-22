#include "StdAfx.h"
#include "BulletOgreSoftBodyProvider.h"
#include "BulletOgreSoftBodyProviderDefinition.h"

#pragma unmanaged
#include "btBulletDynamicsCommon.h"
#include "BulletSoftBody\btSoftRigidDynamicsWorld.h"
#include "BulletSoftBody\btSoftBodyRigidBodyCollisionConfiguration.h"
#include "BulletSoftBody\btSoftBodyHelpers.h"
#pragma managed

namespace BulletOgrePlugin
{

#pragma unmanaged

btSoftBody* createElipsoid(btSoftBodyWorldInfo* worldInfo, float* center, float* radius, int res)
{
	return btSoftBodyHelpers::CreateEllipsoid(*worldInfo, btVector3(center[0], center[1], center[2]), btVector3(radius[0], radius[1], radius[2]), res);
}

#pragma managed

BulletOgreSoftBodyProvider::BulletOgreSoftBodyProvider(BulletOgreSoftBodyProviderDefinition^ def)
:SoftBodyProvider(def),
softBody(0)
{
}

BulletOgreSoftBodyProvider::~BulletOgreSoftBodyProvider(void)
{
}

void BulletOgreSoftBodyProvider::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{
}

void BulletOgreSoftBodyProvider::updateTranslationImpl(Vector3% translation)
{
}

void BulletOgreSoftBodyProvider::updateRotationImpl(Quaternion% rotation)
{
}

void BulletOgreSoftBodyProvider::updateScaleImpl(Vector3% scale)
{
}

void BulletOgreSoftBodyProvider::setEnabled(bool enabled)
{
	
}

void* BulletOgreSoftBodyProvider::createSoftBodyImpl(BulletScene^ scene)
{
	assert(softBody == 0);

	float center[3] = {0, 0, 0};
	float radius[3] = {1, 2, 1};
	softBody = createElipsoid(static_cast<btSoftBodyWorldInfo*>(scene->SoftBodyWorldInfoExternal), center, radius, 512);

	return softBody;
}

void BulletOgreSoftBodyProvider::destroySoftBodyImpl(BulletScene^ scene)
{
	assert(softBody != 0);

	delete softBody;
	softBody = 0;
}

SimElementDefinition^ BulletOgreSoftBodyProvider::saveToDefinition()
{
	return gcnew BulletOgreSoftBodyProviderDefinition(this->Name);
}

void BulletOgreSoftBodyProvider::updateOtherSubsystems()
{
	//If this had other subystem attachments they would be updated here. Also somewhere this has to be registered with the scene.
	//This example class does not do so because it has no updates.
}

}