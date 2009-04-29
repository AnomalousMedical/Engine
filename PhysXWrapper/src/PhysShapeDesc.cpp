#include "StdAfx.h"
#include "..\include\PhysShapeDesc.h"

#include "NxShapeDesc.h"

#include "MarshalUtils.h"
#include "MathUtil.h"
#include "PhysMaterial.h"

namespace PhysXWrapper
{

using namespace System;

PhysShapeDesc::PhysShapeDesc(NxShapeDesc* shapeDesc)
:shapeDesc(shapeDesc)
{
	
}

void PhysShapeDesc::setLocalPose(Engine::Vector3 trans, Engine::Quaternion rot)
{
	MathUtil::copyVector3(trans, shapeDesc->localPose.t);
	shapeDesc->localPose.M.fromQuat(MathUtil::convertNxQuaternion(rot));
}

ShapeFlag PhysShapeDesc::ShapeFlags::get() 
{ 
	return (ShapeFlag)shapeDesc->shapeFlags; 
}

void PhysShapeDesc::ShapeFlags::set(ShapeFlag shapeFlags) 
{ 
	shapeDesc->shapeFlags = (NxU32)shapeFlags; 
}

unsigned short PhysShapeDesc::Group::get() 
{ 
	return shapeDesc->group; 
}

void PhysShapeDesc::Group::set(unsigned short group) 
{ 
	shapeDesc->group = group; 
}

unsigned short PhysShapeDesc::MaterialIndex::get() 
{ 
	return shapeDesc->materialIndex; 
}

void PhysShapeDesc::MaterialIndex::set(unsigned short value) 
{ 
	shapeDesc->materialIndex = value; 
}

float PhysShapeDesc::Density::get() 
{ 
	return shapeDesc->density; 
}

void PhysShapeDesc::Density::set(float density) 
{ 
	shapeDesc->density = density; 
}

float PhysShapeDesc::Mass::get() 
{ 
	return shapeDesc->mass; 
}

void PhysShapeDesc::Mass::set(float mass) 
{ 
	shapeDesc->mass = mass; 
}

float PhysShapeDesc::SkinWidth::get() 
{ 
	return shapeDesc->skinWidth;
}

void PhysShapeDesc::SkinWidth::set(float skinWidth) 
{ 
	shapeDesc->skinWidth = skinWidth; 
}

unsigned int PhysShapeDesc::NonInteractingCompartmentTypes::get() 
{
	return shapeDesc->nonInteractingCompartmentTypes; 
}

void PhysShapeDesc::NonInteractingCompartmentTypes::set(unsigned int nonInteractingCompartmentTypes) 
{
	shapeDesc->nonInteractingCompartmentTypes = nonInteractingCompartmentTypes; 
}

}