#include "StdAfx.h"
#include "..\include\PhysSoftBody.h"
#include "PhysSoftBodyMesh.h"
#include "NxPhysics.h"
#include "PhysSoftBodyDesc.h"
#include "MathUtil.h"
#include "PhysMeshData.h"
#include "PhysScene.h"
#include "MarshalUtils.h"
#include "PhysShape.h"

namespace Engine
{

namespace Physics
{

PhysSoftBody::PhysSoftBody(Engine::Identifier^ name, NxSoftBody* softBody)
:softBody(softBody), name(name), nameStringPtr(new std::string(MarshalUtils::convertString(name->FullName)))
{
	softBody->setName(nameStringPtr->c_str());
}

PhysSoftBody::~PhysSoftBody()
{
	softBody = 0;
}

bool PhysSoftBody::saveToDesc(PhysSoftBodyDesc^ desc)
{
	return softBody->saveToDesc(*desc->desc.Get());
}

PhysSoftBodyMesh^ PhysSoftBody::getSoftBodyMesh()
{
	return PhysSoftBodyMesh::getMeshObject(softBody->getSoftBodyMesh());
}

void PhysSoftBody::setVolumeStiffness(float stiffness)
{
	return softBody->setVolumeStiffness(stiffness);
}

float PhysSoftBody::getVolumeStiffness()
{
	return softBody->getVolumeStiffness();
}

void PhysSoftBody::setStretchingStiffness(float stiffness)
{
	return softBody->setStretchingStiffness(stiffness);
}

float PhysSoftBody::getStretchingStiffness()
{
	return softBody->getStretchingStiffness();
}

void PhysSoftBody::setDampingCoefficient(float dampingCoefficient)
{
	return softBody->setDampingCoefficient(dampingCoefficient);
}

float PhysSoftBody::getDampingCoefficient()
{
	return softBody->getDampingCoefficient();
}

void PhysSoftBody::setFriction(float friction)
{
	return softBody->setFriction(friction);
}

float PhysSoftBody::getFriction()
{
	return softBody->getFriction();
}

void PhysSoftBody::setTearFactor(float factor)
{
	return softBody->setTearFactor(factor);
}

float PhysSoftBody::getTearFactor()
{
	return softBody->getTearFactor();
}

void PhysSoftBody::setAttachmentTearFactor(float factor)
{
	return softBody->setAttachmentTearFactor(factor);
}

float PhysSoftBody::getAttachmentTearFactor()
{
	return softBody->getAttachmentTearFactor();
}

void PhysSoftBody::setParticleRadius(float particleRadius)
{
	return softBody->setParticleRadius(particleRadius);
}

float PhysSoftBody::getParticleRadius()
{
	return softBody->getParticleRadius();
}

float PhysSoftBody::getDensity()
{
	return softBody->getDensity();
}

float PhysSoftBody::getRelativeGridSpacing()
{
	return softBody->getRelativeGridSpacing();
}

System::UInt32 PhysSoftBody::getSolverIterations()
{
	return softBody->getSolverIterations();
}

void PhysSoftBody::setSolverIterations(System::UInt32 iterations)
{
	return softBody->setSolverIterations(iterations);
}

void PhysSoftBody::attachToShape(PhysShape^ shape, PhysSoftBodyAttachmentFlag flags)
{
	return softBody->attachToShape(shape->getNxShape(), static_cast<NxSoftBodyAttachmentFlag>(flags));
}

void PhysSoftBody::attachToCollidingShapes(PhysSoftBodyAttachmentFlag attachmentFlags)
{
	return softBody->attachToCollidingShapes(static_cast<NxSoftBodyAttachmentFlag>(attachmentFlags));
}

void PhysSoftBody::detachFromShape(PhysShape^ shape)
{
	return softBody->detachFromShape(shape->getNxShape());
}

void PhysSoftBody::attachVertexToShape(System::UInt32 vertexId, PhysShape^ shape, EngineMath::Vector3 localPos, PhysSoftBodyAttachmentFlag attachmentFlags)
{
	return softBody->attachVertexToShape(vertexId, shape->getNxShape(), MathUtil::copyVector3(localPos), static_cast<NxSoftBodyAttachmentFlag>(attachmentFlags));
}

void PhysSoftBody::attachVertexToGlobalPosition(System::UInt32 vertexId, EngineMath::Vector3% pos)
{
	return softBody->attachVertexToGlobalPosition(vertexId, MathUtil::copyVector3(pos));
}

void PhysSoftBody::freeVertex(System::UInt32 vertexId)
{
	return softBody->freeVertex(vertexId);
}

bool PhysSoftBody::tearVertex(System::UInt32 vertexId, EngineMath::Vector3% normal)
{
	return softBody->tearVertex(vertexId, MathUtil::copyVector3(normal));
}

bool PhysSoftBody::raycast(EngineMath::Ray3% worldRay, EngineMath::Vector3% hit, System::UInt32 vertexId)
{
	return softBody->raycast(MathUtil::copyRay(worldRay), MathUtil::copyVector3(hit), vertexId);
}

void PhysSoftBody::setGroup(System::UInt16 collisionGroup)
{
	return softBody->setGroup(collisionGroup);
}

System::UInt16 PhysSoftBody::getGroup()
{
	return softBody->getGroup();
}

void PhysSoftBody::setMeshData(PhysMeshData^ meshData)
{
	softBody->setMeshData(*meshData->meshData);
}

PhysMeshData^ PhysSoftBody::getMeshData()
{
	return gcnew PhysMeshData(softBody->getMeshData());
}

void PhysSoftBody::setPosition(EngineMath::Vector3% position, System::UInt32 vertexId)
{
	return softBody->setPosition(MathUtil::copyVector3(position), vertexId);
}

void PhysSoftBody::setPositions(void* buffer, System::UInt32 byteStride)
{
	return softBody->setPositions(buffer, byteStride);
}

EngineMath::Vector3 PhysSoftBody::getPosition(System::UInt32 vertexId)
{
	return MathUtil::copyVector3(softBody->getPosition(vertexId));
}

void PhysSoftBody::getPositions(void* buffer, System::UInt32 byteStride)
{
	return softBody->getPositions(buffer, byteStride);
}

void PhysSoftBody::setVelocity(EngineMath::Vector3% velocity, System::UInt32 vertexId)
{
	return softBody->setVelocity(MathUtil::copyVector3(velocity), vertexId);
}

void PhysSoftBody::setVelocities(void* buffer, System::UInt32 byteStride)
{
	return softBody->setVelocities(buffer, byteStride);
}

EngineMath::Vector3 PhysSoftBody::getVelocity(System::UInt32 vertexId)
{
	return MathUtil::copyVector3(softBody->getVelocity(vertexId));
}

void PhysSoftBody::getVelocities(void* buffer, System::UInt32 byteStride)
{
	return softBody->getVelocities(buffer, byteStride);
}

System::UInt32 PhysSoftBody::getNumberOfParticles()
{
	return softBody->getNumberOfParticles();
}

System::UInt32 PhysSoftBody::queryShapePointers()
{
	return softBody->queryShapePointers();
}

System::UInt32 PhysSoftBody::getStateByteSize()
{
	return softBody->getStateByteSize();
}

void PhysSoftBody::setCollisionResponseCoefficient(float coefficient)
{
	return softBody->setCollisionResponseCoefficient(coefficient);
}

float PhysSoftBody::getCollisionResponseCoefficient()
{
	return softBody->getCollisionResponseCoefficient();
}

void PhysSoftBody::setAttachmentResponseCoefficient(float coefficient)
{
	return softBody->setAttachmentResponseCoefficient(coefficient);
}

float PhysSoftBody::getAttachmentResponseCoefficient()
{
	return softBody->getAttachmentResponseCoefficient();
}

void PhysSoftBody::setFromFluidResponseCoefficient(float coefficient)
{
	return softBody->setFromFluidResponseCoefficient(coefficient);
}

float PhysSoftBody::getFromFluidResponseCoefficient()
{
	return softBody->getFromFluidResponseCoefficient();
}

void PhysSoftBody::setToFluidResponseCoefficient(float coefficient)
{
	return softBody->setToFluidResponseCoefficient(coefficient);
}

float PhysSoftBody::getToFluidResponseCoefficient()
{
	return softBody->getToFluidResponseCoefficient();
}

void PhysSoftBody::setExternalAcceleration(EngineMath::Vector3 acceleration)
{
	return softBody->setExternalAcceleration(MathUtil::copyVector3(acceleration));
}

EngineMath::Vector3 PhysSoftBody::getExternalAcceleration()
{
	return MathUtil::copyVector3(softBody->getExternalAcceleration());
}

void PhysSoftBody::setMinAdhereVelocity(float velocity)
{
	return softBody->setMinAdhereVelocity(velocity);
}

float PhysSoftBody::getMinAdhereVelocity()
{
	return softBody->getMinAdhereVelocity();
}

bool PhysSoftBody::isSleeping()
{
	return softBody->isSleeping();
}

float PhysSoftBody::getSleepLinearVelocity()
{
	return softBody->getSleepLinearVelocity();
}

void PhysSoftBody::setSleepLinearVelocity(float threshold)
{
	return softBody->setSleepLinearVelocity(threshold);
}

void PhysSoftBody::wakeUp(float wakeCounterValue)
{
	return softBody->wakeUp(wakeCounterValue);
}

void PhysSoftBody::putToSleep()
{
	return softBody->putToSleep();
}

void PhysSoftBody::setFlags(PhysSoftBodyFlag flags)
{
	return softBody->setFlags((NxSoftBodyFlag)flags);
}

PhysSoftBodyFlag PhysSoftBody::getFlags()
{
	return (PhysSoftBodyFlag)softBody->getFlags();
}

void PhysSoftBody::addForceAtVertex(EngineMath::Vector3% force, System::UInt32 vertexId, ForceMode mode)
{
	return softBody->addForceAtVertex(MathUtil::copyVector3(force), vertexId, (NxForceMode)mode);
}

void PhysSoftBody::addForceAtPos(EngineMath::Vector3% position, float magnitude, float radius, ForceMode mode)
{
	return softBody->addForceAtPos(MathUtil::copyVector3(position), magnitude, radius, (NxForceMode)mode);
}

System::UInt16 PhysSoftBody::getForceFieldMaterial()
{
	return softBody->getForceFieldMaterial();
}

void PhysSoftBody::setForceFieldMaterial(System::UInt16 material)
{
	return softBody->setForceFieldMaterial(material);
}

}

}