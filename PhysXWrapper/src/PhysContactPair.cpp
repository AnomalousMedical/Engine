#include "StdAfx.h"
#include "..\include\PhysContactPair.h"
#include "ContactIterator.h"
#include "PhysActor.h"
#include "MathUtil.h"

namespace PhysXWrapper
{

PhysContactPair::PhysContactPair(void)
:contactIterator(gcnew ContactIterator()),
csi(0)
{
}

void PhysContactPair::setPair(NxContactPair* contactPair)
{
	this->contactPair = contactPair;
}

void PhysContactPair::clearPair()
{
	if(csi != 0)
	{
		delete csi;
		csi = 0;
	}
}

bool PhysContactPair::isActorDeleted(int actor)
{
	return contactPair->isDeletedActor[actor];
}

PhysActor^ PhysContactPair::getActor(int actor)
{
	return *(PhysActorGCRoot*)(contactPair->actors[actor]->userData);
}

ContactIterator^ PhysContactPair::getContactIterator()
{
	if(csi == 0)
	{
		csi = new NxContactStreamIterator(contactPair->stream);
		contactIterator->setContactStreamIterator(csi);
	}
	return contactIterator;
}

Engine::Vector3 PhysContactPair::getSumNormalForce()
{
	return MathUtil::copyVector3(contactPair->sumNormalForce);
}

Engine::Vector3 PhysContactPair::getSumFrictionForce()
{
	return MathUtil::copyVector3(contactPair->sumFrictionForce);
}

}