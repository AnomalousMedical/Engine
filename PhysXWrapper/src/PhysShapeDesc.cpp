#include "StdAfx.h"
#include "..\include\PhysShapeDesc.h"

#include "NxShapeDesc.h"

#include "MarshalUtils.h"
#include "MathUtil.h"
#include "PhysMaterial.h"

namespace Physics
{

using namespace System;

PhysShapeDesc::PhysShapeDesc(NxShapeDesc* shapeDesc, String^ name)
:shapeDesc(shapeDesc), name(name), materialName(nullptr)
{
	
}

void PhysShapeDesc::setLocalPose(EngineMath::Vector3 trans, EngineMath::Quaternion rot)
{
	MathUtil::copyVector3(trans, shapeDesc->localPose.t);
	shapeDesc->localPose.M.fromQuat(MathUtil::convertNxQuaternion(rot));
}

void PhysShapeDesc::setMaterial(PhysMaterial^ material)
{
	if(material != nullptr)
	{
		materialName = material->getName();
		shapeDesc->materialIndex = material->getMaterialIndex();
	}
	else
	{
		shapeDesc->materialIndex = 0;
		materialName = nullptr;
	}
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

System::String^ PhysShapeDesc::MaterialName::get() 
{ 
	return materialName; 
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

String^ PhysShapeDesc::Name::get() 
{ 
	return name; 
}

void PhysShapeDesc::Name::set(String^ name) 
{ 
	shapeDesc->name = MarshalUtils::convertString(name).c_str();
	this->name = name;
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