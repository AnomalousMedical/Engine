#include "StdAfx.h"
#include "..\include\PhysCylindricalJointDesc.h"
#include "NxPhysics.h"

namespace PhysXWrapper
{

PhysCylindricalJointDesc::PhysCylindricalJointDesc()
:joint(new NxCylindricalJointDesc()), PhysJointDesc(joint.Get())
{

}

}