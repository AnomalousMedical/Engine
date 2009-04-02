#include "StdAfx.h"
#include "..\include\PhysSoftBodyDesc.h"
#include "NxPhysics.h"
#include "MathUtil.h"
#include "PhysMeshData.h"
#include "PhysSoftBodyMesh.h"

namespace Physics
{

PhysSoftBodyDesc::PhysSoftBodyDesc()
:desc(new NxSoftBodyDesc()),
softBodyMesh(nullptr)
{
	meshData = gcnew PhysMeshData(&desc->meshData);
}

void PhysSoftBodyDesc::setGlobalPose(EngineMath::Vector3 translation, EngineMath::Quaternion rotation)
{
	desc->globalPose.M.fromQuat( MathUtil::convertNxQuaternion(rotation) );
	MathUtil::copyVector3(translation, desc->globalPose.t);
}

PhysSoftBodyMesh^ PhysSoftBodyDesc::SoftBodyMesh::get() 
{
	if(softBodyMesh == nullptr && desc->softBodyMesh != NULL)
	{
		softBodyMesh = PhysSoftBodyMesh::getMeshObject(desc->softBodyMesh);
	}
	return softBodyMesh;
}

void PhysSoftBodyDesc::SoftBodyMesh::set(PhysSoftBodyMesh^ value) 
{
	softBodyMesh = value;
	if(value != nullptr)
	{
		desc->softBodyMesh = value->softMesh;
	}
	else
	{
		desc->softBodyMesh = NULL;
	}
}

float PhysSoftBodyDesc::ParticleRadius::get() 
{
	return desc->particleRadius;
}

void PhysSoftBodyDesc::ParticleRadius::set(float value) 
{
	desc->particleRadius = value;
}

float PhysSoftBodyDesc::Density::get() 
{
	return desc->density;
}

void PhysSoftBodyDesc::Density::set(float value) 
{
	desc->density = value;
}

float PhysSoftBodyDesc::VolumeStiffness::get() 
{
	return desc->volumeStiffness;
}

void PhysSoftBodyDesc::VolumeStiffness::set(float value) 
{
	desc->volumeStiffness = value;
}

float PhysSoftBodyDesc::StretchingStiffness::get() 
{
	return desc->stretchingStiffness;
}

void PhysSoftBodyDesc::StretchingStiffness::set(float value) 
{
	desc->stretchingStiffness = value;
}

float PhysSoftBodyDesc::DampingCoefficient::get() 
{
	return desc->dampingCoefficient;
}

void PhysSoftBodyDesc::DampingCoefficient::set(float value) 
{
	desc->dampingCoefficient = value;
}

float PhysSoftBodyDesc::Friction::get() 
{
	return desc->friction;
}

void PhysSoftBodyDesc::Friction::set(float value) 
{
	desc->friction = value;
}

float PhysSoftBodyDesc::TearFactor::get() 
{
	return desc->tearFactor;
}

void PhysSoftBodyDesc::TearFactor::set(float value) 
{
	desc->tearFactor = value;
}

float PhysSoftBodyDesc::CollisionResponseCoefficient::get() 
{
	return desc->collisionResponseCoefficient;
}

void PhysSoftBodyDesc::CollisionResponseCoefficient::set(float value) 
{
	desc->collisionResponseCoefficient = value;
}

float PhysSoftBodyDesc::AttachmentResponseCoefficient::get() 
{
	return desc->attachmentResponseCoefficient;
}

void PhysSoftBodyDesc::AttachmentResponseCoefficient::set(float value) 
{
	desc->attachmentResponseCoefficient = value;
}

float PhysSoftBodyDesc::AttachmentTearFactor::get() 
{
	return desc->attachmentTearFactor;
}

void PhysSoftBodyDesc::AttachmentTearFactor::set(float value) 
{
	desc->attachmentTearFactor = value;
}

float PhysSoftBodyDesc::ToFluidResponseCoefficient::get() 
{
	return desc->toFluidResponseCoefficient;
}

void PhysSoftBodyDesc::ToFluidResponseCoefficient::set(float value) 
{
	desc->toFluidResponseCoefficient = value;
}

float PhysSoftBodyDesc::FromFluidResponseCoefficient::get() 
{
	return desc->fromFluidResponseCoefficient;
}

void PhysSoftBodyDesc::FromFluidResponseCoefficient::set(float value) 
{
	desc->fromFluidResponseCoefficient = value;
}

float PhysSoftBodyDesc::MinAdhereVelocity::get() 
{
	return desc->minAdhereVelocity;
}

void PhysSoftBodyDesc::MinAdhereVelocity::set(float value) 
{
	desc->minAdhereVelocity = value;
}

System::UInt32 PhysSoftBodyDesc::SolverIterations::get() 
{
	return desc->solverIterations;
}

void PhysSoftBodyDesc::SolverIterations::set(System::UInt32 value) 
{
	desc->solverIterations = value;
}

EngineMath::Vector3 PhysSoftBodyDesc::ExternalAcceleration::get() 
{
	return MathUtil::copyVector3(desc->externalAcceleration);
}

void PhysSoftBodyDesc::ExternalAcceleration::set(EngineMath::Vector3 value) 
{
	desc->externalAcceleration = MathUtil::copyVector3(value);
}

float PhysSoftBodyDesc::WakeUpCounter::get() 
{
	return desc->wakeUpCounter;
}

void PhysSoftBodyDesc::WakeUpCounter::set(float value) 
{
	desc->wakeUpCounter = value;
}

float PhysSoftBodyDesc::SleepLinearVelocity::get() 
{
	return desc->sleepLinearVelocity;
}

void PhysSoftBodyDesc::SleepLinearVelocity::set(float value) 
{
	desc->sleepLinearVelocity = value;
}

PhysMeshData^ PhysSoftBodyDesc::MeshData::get() 
{
	return meshData;
}

System::UInt16 PhysSoftBodyDesc::CollisionGroup::get() 
{
	return desc->collisionGroup;
}

void PhysSoftBodyDesc::CollisionGroup::set(System::UInt16 value) 
{
	desc->collisionGroup = value;
}

System::UInt16 PhysSoftBodyDesc::ForceFieldMaterial::get() 
{
	return desc->forceFieldMaterial;
}

void PhysSoftBodyDesc::ForceFieldMaterial::set(System::UInt16 value) 
{
	desc->forceFieldMaterial = value;
}

float PhysSoftBodyDesc::RelativeGridSpacing::get() 
{
	return desc->relativeGridSpacing;
}

void PhysSoftBodyDesc::RelativeGridSpacing::set(float value) 
{
	desc->relativeGridSpacing = value;
}

PhysSoftBodyFlag PhysSoftBodyDesc::Flags::get() 
{
	return (PhysSoftBodyFlag)desc->flags;
}

void PhysSoftBodyDesc::Flags::set(PhysSoftBodyFlag value) 
{
	desc->flags = static_cast<NxU32>(value);
}

}