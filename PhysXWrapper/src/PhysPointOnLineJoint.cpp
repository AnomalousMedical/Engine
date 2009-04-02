#include "StdAfx.h"
#include "..\include\PhysPointOnLineJoint.h"
#include "NxPhysics.h"
#include "PhysPointOnLineJointDesc.h"
#include "PhysActor.h"

namespace Engine
{

namespace Physics
{

PhysPointOnLineJoint::PhysPointOnLineJoint(Engine::Identifier^ name, NxPointOnLineJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(name, joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysPointOnLineJoint::saveToDesc(PhysPointOnLineJointDesc^ desc)
{
	typedJoint->saveToDesc(*desc->joint.Get());
	NxActor* actor0, *actor1;
	typedJoint->getActors(&actor0, &actor1);
	if(actor0)
	{
		desc->Actor[0] = *((PhysActorGCRoot*)actor0->userData);
	}
	if(actor1)
	{
		desc->Actor[1] = *((PhysActorGCRoot*)actor1->userData);
	}
}

void PhysPointOnLineJoint::loadFromDesc(PhysPointOnLineJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

NxJointDesc& PhysPointOnLineJoint::getDesc()
{
	NxPointOnLineJointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}

}