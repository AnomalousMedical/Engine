#include "StdAfx.h"
#include "..\include\PhysCylindricalJointDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysCylindricalJointDesc::PhysCylindricalJointDesc()
:joint(new NxCylindricalJointDesc()), PhysJointDesc(joint.Get())
{

}

}

}