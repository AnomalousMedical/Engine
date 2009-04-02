#include "StdAfx.h"
#include "..\include\PhysCooking.h"
#include "PhysXLogger.h"
#pragma warning(push)
#pragma warning(disable : 4635)
#include "NxCooking.h"
#pragma warning(pop)
#include "PhysConvexMeshDesc.h"
#include "PhysMemoryWriteBuffer.h"
#include "PhysSDK.h"
#include "PhysTriangleMeshDesc.h"
#include "PhysSoftBodyMeshDesc.h"

namespace Engine
{

namespace Physics
{

bool PhysCooking::initCooking()
{
	return cook->NxInitCooking(NULL, PhysSDK::Instance->logger.Get());
}

void PhysCooking::closeCooking()
{
	cook->NxCloseCooking();
}

bool PhysCooking::cookConvexMesh(PhysConvexMeshDesc^ desc, PhysMemoryWriteBuffer^ stream)
{
	return cook->NxCookConvexMesh(*desc->meshDesc.Get(), *stream->writeBuffer.Get());
}

bool PhysCooking::cookTriangleMesh(PhysTriangleMeshDesc^ desc, PhysMemoryWriteBuffer^ stream)
{
	return cook->NxCookTriangleMesh(*desc->meshDesc.Get(), *stream->writeBuffer.Get());
}

bool PhysCooking::cookSoftBodyMesh(PhysSoftBodyMeshDesc^ desc, PhysMemoryWriteBuffer^ stream)
{
	return cook->NxCookSoftBodyMesh(*desc->meshDesc.Get(), *stream->writeBuffer.Get());
}

void PhysCooking::setCookingParams(PhysCookingParams params)
{
	NxCookingParams nxParams;
	nxParams.hintCollisionSpeed = params.hintCollisionSpeed;
	nxParams.skinWidth = params.skinWidth;
	nxParams.targetPlatform = (NxPlatform)params.targetPlatform;
	cook->NxSetCookingParams(nxParams);
}

PhysCookingParams PhysCooking::getCookingParams()
{
	NxCookingParams nxParams = cook->NxGetCookingParams();
	PhysCookingParams params;
	params.hintCollisionSpeed = nxParams.hintCollisionSpeed;
	params.skinWidth = nxParams.skinWidth;
	params.targetPlatform = (PhysPlatform)nxParams.targetPlatform;
	return params;
}

}

}