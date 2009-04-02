//Source
#include "stdafx.h"
#include "PhysJointCollection.h"
#include "PhysJoint.h"

namespace Engine{

namespace Physics{

PhysJoint^ PhysJointCollection::createWrapper(void* nativeObject, ...array<System::Object^>^ args)
{
	throw gcnew System::NotImplementedException();
	//return gcnew PhysJoint(static_cast<NxJoint*>(nativeObject));
	/*
	PhysJoint^ physJoint;
			switch(nxJoint->getType())
			{
				case NX_JOINT_PRISMATIC:
					physJoint = gcnew PhysPrismaticJoint(jointDesc->Name, (NxPrismaticJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_REVOLUTE:
					physJoint = gcnew PhysRevoluteJoint(jointDesc->Name, (NxRevoluteJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_CYLINDRICAL:
					physJoint = gcnew PhysCylindricalJoint(jointDesc->Name, (NxCylindricalJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_SPHERICAL:
					physJoint = gcnew PhysSphericalJoint(jointDesc->Name, (NxSphericalJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_POINT_ON_LINE:
					physJoint = gcnew PhysPointOnLineJoint(jointDesc->Name, (NxPointOnLineJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_POINT_IN_PLANE:
					physJoint = gcnew PhysPointInPlaneJoint(jointDesc->Name, (NxPointInPlaneJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_DISTANCE:
					physJoint = gcnew PhysDistanceJoint(jointDesc->Name, (NxDistanceJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_PULLEY:
					physJoint = gcnew PhysPulleyJoint(jointDesc->Name, (NxPulleyJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_FIXED:
					physJoint = gcnew PhysFixedJoint(jointDesc->Name, (NxFixedJoint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
				case NX_JOINT_D6:
					physJoint = gcnew PhysD6Joint(jointDesc->Name, (NxD6Joint*)nxJoint, jointDesc->Actor[0], jointDesc->Actor[1], this);
					break;
			}
	*/
}

PhysJoint^ PhysJointCollection::getObject(NxJoint* nativeObject)
{
	return getObjectVoid(nativeObject);
}

void PhysJointCollection::destroyObject(NxJoint* nativeObject)
{
	destroyObjectVoid(nativeObject);
}

}

}