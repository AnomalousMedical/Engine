#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxPointOnLineJointDesc.h"

namespace Engine
{

namespace Physics
{

/// <summary>
/// Wrapper for NxPointOnLineJointDesc.
/// Describes PointOnLineJoint.
/// </summary>
public ref class PhysPointOnLineJointDesc : public PhysJointDesc
{
internal:
	AutoPtr<NxPointOnLineJointDesc> joint;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysPointOnLineJointDesc();
};

}

}