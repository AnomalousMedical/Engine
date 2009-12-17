#include "StdAfx.h"
#include "..\include\EllipsoidSoftBodyProvider.h"
#include "EllipsoidSoftBodyProviderDefinition.h"
#include "BulletScene.h"

namespace BulletPlugin
{

#pragma unmanaged

btSoftBody* createElipsoid(btSoftBodyWorldInfo* worldInfo, float* center, float* radius, int res)
{
	return btSoftBodyHelpers::CreateEllipsoid(*worldInfo, btVector3(center[0], center[1], center[2]), btVector3(radius[0], radius[1], radius[2]), res);
}

#pragma managed

EllipsoidSoftBodyProvider::EllipsoidSoftBodyProvider(EllipsoidSoftBodyProviderDefinition^ def)
:SoftBodyProvider(def),
softBody(0)
{
}

EllipsoidSoftBodyProvider::~EllipsoidSoftBodyProvider(void)
{
}

void EllipsoidSoftBodyProvider::updatePositionImpl(Vector3% translation, Quaternion% rotation)
{
}

void EllipsoidSoftBodyProvider::updateTranslationImpl(Vector3% translation)
{
}

void EllipsoidSoftBodyProvider::updateRotationImpl(Quaternion% rotation)
{
}

void EllipsoidSoftBodyProvider::updateScaleImpl(Vector3% scale)
{
}

void EllipsoidSoftBodyProvider::setEnabled(bool enabled)
{
}

btSoftBody* EllipsoidSoftBodyProvider::createSoftBodyImpl(BulletScene^ scene)
{
	assert(softBody == 0);

	float center[3] = {0, 0, 0};
	float radius[3] = {1, 1, 1};
	softBody = createElipsoid(scene->SoftBodyWorldInfo, center, radius, 512);

	return softBody;
}

void EllipsoidSoftBodyProvider::destroySoftBodyImpl(BulletScene^ scene)
{
	assert(softBody != 0);

	delete softBody;
	softBody = 0;
}

SimElementDefinition^ EllipsoidSoftBodyProvider::saveToDefinition()
{
	return gcnew EllipsoidSoftBodyProviderDefinition(this->Name);
}

}