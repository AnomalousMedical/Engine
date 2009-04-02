#include "StdAfx.h"
#include "..\include\PhysFixedJoint.h"
#include "NxPhysics.h"
#include "PhysFixedJointDesc.h"
#include "PhysActor.h"

namespace Engine
{

namespace Physics
{

PhysFixedJoint::PhysFixedJoint(Engine::Identifier^ name, NxFixedJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
:PhysJoint(name, joint, actor0, actor1, scene),
typedJoint(joint)
{

}

void PhysFixedJoint::saveToDesc(PhysFixedJointDesc^ desc)
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

void PhysFixedJoint::loadFromDesc(PhysFixedJointDesc^ desc)
{
	typedJoint->loadFromDesc(*desc->joint.Get());
}

NxJointDesc& PhysFixedJoint::getDesc()
{
	NxFixedJointDesc desc;
	typedJoint->saveToDesc(desc);
	return desc;
}

}

}