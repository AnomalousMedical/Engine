#include "StdAfx.h"
#include "..\include\PhysActorDesc.h"

#include "NxPhysics.h"

#include "PhysShapeDesc.h"
#include "PhysBodyDesc.h"
#include "MathUtil.h"

namespace Engine
{

namespace Physics
{

using namespace System;

PhysActorDesc::PhysActorDesc()
:actorDesc(new NxActorDesc()),  
body(nullptr), 
shapeList(gcnew ShapeList())
{
	
}

void PhysActorDesc::setGlobalPose(EngineMath::Vector3 translation, EngineMath::Quaternion rotation)
{
	actorDesc->globalPose.M.fromQuat( MathUtil::convertNxQuaternion(rotation) );
	MathUtil::copyVector3(translation, actorDesc->globalPose.t);
}

void PhysActorDesc::setToDefault()
{
	actorDesc->setToDefault();
	body = nullptr;
}

bool PhysActorDesc::isValid()
{
	return actorDesc->isValid();
}

void PhysActorDesc::addShape(PhysShapeDesc^ shape)
{
	shapeList->AddLast(shape);
	//This is stupid to deref and then ref again, but const wont let the pointer through
	actorDesc->shapes.pushBack( &(*(shape->shapeDesc)) );
}

void PhysActorDesc::removeShape(PhysShapeDesc^ shape)
{
	shapeList->Remove(shape);
	//This is stupid to deref and then ref again, but const wont let the pointer through
	actorDesc->shapes.deleteEntry( &(*(shape->shapeDesc)) );
}

void PhysActorDesc::clearShapes()
{
	shapeList->Clear();
	actorDesc->shapes.clear();
}

PhysBodyDesc^ PhysActorDesc::Body::get() 
{ 
	return body; 
}

void PhysActorDesc::Body::set(PhysBodyDesc^ desc)
{
	body = desc;
	if( body != nullptr )
	{
		actorDesc->body = body->bodyDesc.Get();
	}
	else
	{
		actorDesc->body = NULL;
	}
}

float PhysActorDesc::Density::get() 
{ 
	return actorDesc->density; 
}

void PhysActorDesc::Density::set(float density) 
{ 
	actorDesc->density = density; 
}

unsigned int PhysActorDesc::Flags::get() 
{ 
	return actorDesc->flags; 
}

void PhysActorDesc::Flags::set(unsigned int flags) 
{
	actorDesc->flags = flags; 
}

unsigned short PhysActorDesc::Group::get() 
{ 
	return actorDesc->group; 
}

void PhysActorDesc::Group::set(unsigned short group) 
{ 
	actorDesc->group = group; 
}

unsigned short PhysActorDesc::DominanceGroup::get() 
{ 
	return actorDesc->dominanceGroup; 
}

void PhysActorDesc::DominanceGroup::set(unsigned short group) 
{
	actorDesc->dominanceGroup = group; 
}

ContactPairFlag PhysActorDesc::ContactReportFlags::get() 
{ 
	return (ContactPairFlag)actorDesc->contactReportFlags; 
}

void PhysActorDesc::ContactReportFlags::set(ContactPairFlag flags) 
{ 
	actorDesc->contactReportFlags = (NxU32)flags; 
}

unsigned short PhysActorDesc::ForceFieldMaterial::get() 
{ 
	return actorDesc->forceFieldMaterial; 
}

void PhysActorDesc::ForceFieldMaterial::set(unsigned short mat) 
{ 
	actorDesc->forceFieldMaterial = mat; 
}

}

}