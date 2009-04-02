#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxPointInPlaneJointDesc.h"

namespace Engine
{

namespace Physics
{

/// <summary>
/// Wrapper for NxPointInPlaneJointDesc.
/// Describes PointInPlaneJoint.
/// </summary>
public ref class PhysPointInPlaneJointDesc : public PhysJointDesc
{
internal:
	AutoPtr<NxPointInPlaneJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="name">Debug name for the joint.</param>
	PhysPointInPlaneJointDesc(Engine::Identifier^ name);
};

}

}