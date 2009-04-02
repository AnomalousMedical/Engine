//Source
#include "stdafx.h"
#include "PhysJointCollection.h"
#include "PhysJoint.h"
#include "PhysPrismaticJoint.h"
#include "PhysRevoluteJoint.h"
#include "PhysCylindricalJoint.h"
#include "PhysSphericalJoint.h"
#include "PhysPointOnLineJoint.h"
#include "PhysPointInPlaneJoint.h"
#include "PhysDistanceJoint.h"
#include "PhysPulleyJoint.h"
#include "PhysFixedJoint.h"
#include "PhysD6Joint.h"
#include "PhysActor.h"
#include "PhysScene.h"

namespace Engine{

namespace Physics{

PhysJoint^ PhysJointCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	NxJoint* nxJoint = static_cast<NxJoint*>(nativeObject);
	PhysJoint^ physJoint = nullptr;
	PhysActor^ actor0 = static_cast<PhysActor^>(args[0]);
	PhysActor^ actor1 = static_cast<PhysActor^>(args[1]);
	PhysScene^ scene = static_cast<PhysScene^>(args[2]);
	switch(nxJoint->getType())
	{
		case NX_JOINT_PRISMATIC:
			physJoint = gcnew PhysPrismaticJoint(static_cast<NxPrismaticJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_REVOLUTE:
			physJoint = gcnew PhysRevoluteJoint(static_cast<NxRevoluteJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_CYLINDRICAL:
			physJoint = gcnew PhysCylindricalJoint(static_cast<NxCylindricalJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_SPHERICAL:
			physJoint = gcnew PhysSphericalJoint(static_cast<NxSphericalJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_POINT_ON_LINE:
			physJoint = gcnew PhysPointOnLineJoint(static_cast<NxPointOnLineJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_POINT_IN_PLANE:
			physJoint = gcnew PhysPointInPlaneJoint(static_cast<NxPointInPlaneJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_DISTANCE:
			physJoint = gcnew PhysDistanceJoint(static_cast<NxDistanceJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_PULLEY:
			physJoint = gcnew PhysPulleyJoint(static_cast<NxPulleyJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_FIXED:
			physJoint = gcnew PhysFixedJoint(static_cast<NxFixedJoint*>(nxJoint), actor0, actor1, scene);
			break;
		case NX_JOINT_D6:
			physJoint = gcnew PhysD6Joint(static_cast<NxD6Joint*>(nxJoint), actor0, actor1, scene);
			break;
	}
	return physJoint;
}

PhysJoint^ PhysJointCollection::getObject(NxJoint* nativeObject, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene)
{
	return getObjectVoid(nativeObject, actor0, actor1, scene);
}

void PhysJointCollection::destroyObject(NxJoint* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}

}