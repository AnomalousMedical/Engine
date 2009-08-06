#include "StdAfx.h"
#include "..\include\BulletShapeBuilder.h"
#include "BulletShapeCollection.h"
#include "CompoundShapeCollection.h"
#include "BulletShapeRepository.h"

namespace BulletPlugin
{

#pragma unmanaged

void addChildShape(btCompoundShape* shape, btCollisionShape* child, float* trans, float* rot)
{
	shape->addChildShape(btTransform(btQuaternion(rot[0], rot[1], rot[2], rot[3]), btVector3(trans[0], trans[1], trans[2])), child);
}

btBoxShape* createBoxShape(float* extents)
{
	return new btBoxShape(btVector3(extents[0] / 2.0f, extents[1] / 2.0f, extents[2] / 2.0f));
}

//btPlaneShape* createPlaneShape(float* v, float f)
//{
//	return new btPlaneShape(btVector3(v[0], v[1], v[2]), f);
//}

#pragma managed

BulletShapeBuilder::BulletShapeBuilder()
:currentCompound(0)
{

}

void BulletShapeBuilder::setRepository(BulletShapeRepository^ repository)
{
	this->repository = repository;
}

void BulletShapeBuilder::buildSphere(String^ name, float radius, Vector3 translation, String^ material)
{
	commitShape(name, translation, Quaternion::Identity, new btSphereShape(radius));
}

void BulletShapeBuilder::buildBox(String^ name, Vector3 extents, Vector3 translation, Quaternion rotation, String^ material)
{
	commitShape(name, translation, rotation, createBoxShape(&extents.x));
}

void BulletShapeBuilder::buildMesh(String^ name, cli::array<float>^ vertices, cli::array<int>^ faces, Vector3 translation, Quaternion rotation, String^ material)
{

}

void BulletShapeBuilder::buildPlane(String^ name, Vector3 normal, float distance, String^ material)
{
	//commitShape(name, Vector3::Zero, Quaternion::Identity, createPlaneShape(&normal.x, distance));
}

void BulletShapeBuilder::buildCapsule(String^ name, float radius, float height, Vector3 translation, Quaternion rotation, String^ material)
{
	commitShape(name, translation, rotation, new btCapsuleShape(radius, height));
}

void BulletShapeBuilder::buildConvexHull(String^ name, cli::array<float>^ vertices, cli::array<int>^ faces, Vector3 translation, Quaternion rotation, String^ material)
{
	pin_ptr<float> verts = &vertices[0];
	commitShape(name, translation, rotation, new btConvexHullShape(verts, vertices->Length));
}

void BulletShapeBuilder::buildConvexHull(String^ name, cli::array<float>^ vertices, Vector3 translation, Quaternion rotation, String^ material)
{
	pin_ptr<float> verts = &vertices[0];
	commitShape(name, translation, rotation, new btConvexHullShape(verts, vertices->Length));
}

void BulletShapeBuilder::buildSoftBody(String^ name, cli::array<float>^ vertices, cli::array<int>^ tetrahedra, Vector3 translation, Quaternion rotation)
{

}

void BulletShapeBuilder::startCompound(String^ name)
{
	if(currentCompound == 0)
	{
		currentCompound = new btCompoundShape();
	}
	else
	{
		throw gcnew Exception(String::Format("Error loading compound collision shape {0}. The compound object already exists.", name));
	}
}

void BulletShapeBuilder::stopCompound(String^ name)
{
	repository->addCollection(gcnew CompoundShapeCollection(currentCompound, name));
	currentCompound = 0;
}

void BulletShapeBuilder::setCurrentShapeLocation(ShapeLocation^ location)
{
	repository->CurrentLoadingLocation = location;
}

void BulletShapeBuilder::createMaterial(String^ name, float restitution, float staticFriction, float dynamicFriction)
{

}

void BulletShapeBuilder::commitShape(String^ name, Vector3 translation, Quaternion rotation, btCollisionShape* collisionShape)
{
	if(currentCompound != 0)
	{
		addChildShape(currentCompound, collisionShape, &translation.x, &rotation.x);
	}
	else
	{
		repository->addCollection(gcnew BulletShapeCollection(collisionShape, name));
	}
}

}