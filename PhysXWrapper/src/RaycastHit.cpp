#include "StdAfx.h"
#include "RaycastHit.h"
#include "NxPhysics.h"
#include <vcclr.h>
#include "PhysActor.h"
#include "MathUtil.h"

namespace Engine
{

namespace Physics
{

RaycastHit::RaycastHit(void)
:raycastHit( new NxRaycastHit() )
{

}

RaycastHit::~RaycastHit(void)
{
	
}

void RaycastHit::setCurrentHit( const NxRaycastHit* raycastHit )
{
	this->raycastHit = raycastHit;
}

PhysActor^ RaycastHit::getCollidedActor()
{
	if( raycastHit->shape )
	{
		PhysActorGCRoot* physObject = (PhysActorGCRoot*)raycastHit->shape->getActor().userData;
		return *physObject;
	}
	return nullptr;
}

void RaycastHit::getWorldImpact( EngineMath::Vector3% impact )
{
	MathUtil::copyVector3( raycastHit->worldImpact, impact );
}

void RaycastHit::getWorldNormal( EngineMath::Vector3% normal )
{
	MathUtil::copyVector3( raycastHit->worldNormal, normal );
}

float RaycastHit::getDistance()
{
	return raycastHit->distance;
}

float RaycastHit::getBarycentricU()
{
	return raycastHit->u;
}

float RaycastHit::getBarycentricV()
{
	return raycastHit->v;
}

}

}