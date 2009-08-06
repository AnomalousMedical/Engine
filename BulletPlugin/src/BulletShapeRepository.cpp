#include "StdAfx.h"
#include "..\include\BulletShapeRepository.h"

namespace BulletPlugin
{

BulletShapeRepository::BulletShapeRepository(void)
{

}

BulletShapeRepository::~BulletShapeRepository()
{

}

void BulletShapeRepository::addConvexMesh(String^ name, ConvexMesh^ mesh)
{

}

void BulletShapeRepository::destroyConvexMesh(String^ name)
{

}

void BulletShapeRepository::addTriangleMesh(String^ name, TriangleMesh^ mesh)
{

}

void BulletShapeRepository::destroyTriangleMesh(String^ name)
{

}

void BulletShapeRepository::addMaterial(String^ name, ShapeMaterial^ materialDesc)
{

}

bool BulletShapeRepository::hasMaterial(String^ name)
{
	throw gcnew NotImplementedException();
}

ShapeMaterial^ BulletShapeRepository::getMaterial(String^ name)
{
	throw gcnew NotImplementedException();
}

void BulletShapeRepository::destroyMaterial(String^ name)
{

}

void BulletShapeRepository::addSoftBodyMesh(SoftBodyMesh^ softBodyMesh)
{

}

SoftBodyMesh^ BulletShapeRepository::getSoftBodyMesh(String^ name)
{
	throw gcnew NotImplementedException();
}

void BulletShapeRepository::destroySoftBodyMesh(String^ name)
{

}

}