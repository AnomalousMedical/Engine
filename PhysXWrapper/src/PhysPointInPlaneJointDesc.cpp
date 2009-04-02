#include "StdAfx.h"
#include "..\include\PhysPointInPlaneJointDesc.h"
#include "NxPhysics.h"

namespace Engine
{

namespace Physics
{

PhysPointInPlaneJointDesc::PhysPointInPlaneJointDesc(Engine::Identifier^ name)
:joint(new NxPointInPlaneJointDesc()), PhysJointDesc(joint.Get(), name)
{

}

}

}