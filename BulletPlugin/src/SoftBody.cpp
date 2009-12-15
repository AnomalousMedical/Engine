#include "stdafx.h"
#include "SoftBody.h"

#include "SoftBodyDefinition.h"
#include "BulletScene.h"

namespace BulletPlugin
{

#pragma unmanaged

btSoftBody* createElipsoid(btSoftBodyWorldInfo* worldInfo, float* center, float* radius, int res)
{
	return btSoftBodyHelpers::CreateEllipsoid(*worldInfo, btVector3(center[0], center[1], center[2]), btVector3(radius[0], radius[1], radius[2]), res);
}

#pragma managed

SoftBody::SoftBody(SoftBodyDefinition^ description, BulletScene^ scene)
:SimElement(description->Name, description->Subscription),
scene(scene)
{
	float center[3] = {0, 0, 0};
	float radius[3] = {1, 1, 1};
	softBody = createElipsoid(scene->SoftBodyWorldInfo, center, radius, 512);
}

SoftBody::~SoftBody(void)
{
	if(softBody != 0)
	{
		if(Owner->Enabled)
		{
			scene->DynamicsWorld->removeSoftBody(softBody);
		}
		delete softBody;
		softBody = 0;
	}
}

SimElementDefinition^ SoftBody::saveToDefinition()
{
	return gcnew SoftBodyDefinition(this->Name);
}

void SoftBody::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{

}

void SoftBody::updateTranslationImpl(Vector3% translation)
{

}

void SoftBody::updateRotationImpl(Quaternion% rotation)
{

}

void SoftBody::updateScaleImpl(Vector3% scale)
{

}

void SoftBody::setEnabled(bool enabled)
{
	if(enabled)
	{
		scene->DynamicsWorld->addSoftBody(softBody);
	}
	else
	{
		scene->DynamicsWorld->removeSoftBody(softBody);
	}
}

}