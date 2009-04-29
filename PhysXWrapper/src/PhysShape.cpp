#include "StdAfx.h"
#include "..\include\PhysShape.h"
#include "PhysActor.h"
#include "PhysRaycastHit.h"
#include "MathUtil.h"

#include "NxShape.h"

namespace PhysXWrapper
{

PhysShape::PhysShape(NxShape* nxShape)
:nxShape( nxShape ), shapeRoot(new PhysShapeGCRoot())
{
	*shapeRoot.Get() = this;
	nxShape->userData = shapeRoot.Get();
}

PhysShape::~PhysShape()
{
	nxShape = 0;
}

NxShape* PhysShape::getNxShape()
{
	return nxShape;
}

PhysActor^ PhysShape::getActor()
{
	PhysActorGCRoot* physObject = (PhysActorGCRoot*)nxShape->getActor().userData;
	return *physObject;
}

void PhysShape::setGroup(unsigned short group)
{
	return nxShape->setGroup(group);
}

unsigned short PhysShape::getGroup()
{
	return nxShape->getGroup();
}

PhysShapeType PhysShape::getShapeType()
{
	return static_cast<PhysShapeType>(nxShape->getType());
}

void PhysShape::setFlag(ShapeFlag flag, bool value)
{
	return nxShape->setFlag(static_cast<NxShapeFlag>(flag), value);
}

bool PhysShape::getFlag(ShapeFlag flag)
{
	return nxShape->getFlag(static_cast<NxShapeFlag>(flag));
}

void PhysShape::setMaterial(unsigned short materialIndex)
{
	return nxShape->setMaterial(materialIndex);
}

unsigned short PhysShape::getMaterial()
{
	return nxShape->getMaterial();
}

void PhysShape::setSkinWidth(float skinWidth)
{
	return nxShape->setSkinWidth(skinWidth);
}

float PhysShape::getSkinWidth()
{
	return nxShape->getSkinWidth();
}

void PhysShape::setLocalPosition(Engine::Vector3 trans)
{
	return nxShape->setLocalPosition(MathUtil::copyVector3(trans));
}

void PhysShape::setLocalPosition(Engine::Vector3% trans)
{
	return nxShape->setLocalPosition(MathUtil::copyVector3(trans));
}

void PhysShape::setLocalOrientation(Engine::Quaternion rot)
{
	return nxShape->setLocalOrientation(MathUtil::quaternionToMat(rot));
}

void PhysShape::setLocalOrientation(Engine::Quaternion% rot)
{
	return nxShape->setLocalOrientation(MathUtil::quaternionToMat(rot));
}

Engine::Vector3 PhysShape::getLocalPosition()
{
	return MathUtil::copyVector3(nxShape->getLocalPosition());
}

Engine::Quaternion PhysShape::getLocalOrientation()
{
	return MathUtil::matToQuaternion(nxShape->getLocalOrientation());
}

void PhysShape::setGlobalPosition(Engine::Vector3 trans)
{
	return nxShape->setGlobalPosition(MathUtil::copyVector3(trans));
}

void PhysShape::setGlobalPosition(Engine::Vector3% trans)
{
	return nxShape->setGlobalPosition(MathUtil::copyVector3(trans));
}

void PhysShape::setGlobalOrientation(Engine::Quaternion rot)
{
	return nxShape->setGlobalOrientation(MathUtil::quaternionToMat(rot));
}

void PhysShape::setGlobalOrientation(Engine::Quaternion% rot)
{
	return nxShape->setGlobalOrientation(MathUtil::quaternionToMat(rot));
}

Engine::Vector3 PhysShape::getGlobalPosition()
{
	return MathUtil::copyVector3(nxShape->getGlobalPosition());
}

Engine::Quaternion PhysShape::getGlobalOrientation()
{
	return MathUtil::matToQuaternion(nxShape->getGlobalOrientation());
}

bool PhysShape::raycast(Engine::Ray3 worldRay, float maxDist, RaycastBit hint, PhysRaycastHit^ hit, bool firstHit)
{
	return nxShape->raycast(MathUtil::copyRay(worldRay), maxDist, (NxRaycastBit)hint, *hit->getNxRaycastHit(), firstHit);
}

bool PhysShape::raycast(Engine::Ray3% worldRay, float maxDist, RaycastBit hint, PhysRaycastHit^ hit, bool firstHit)
{
	return nxShape->raycast(MathUtil::copyRay(worldRay), maxDist, (NxRaycastBit)hint, *hit->getNxRaycastHit(), firstHit);
}

}