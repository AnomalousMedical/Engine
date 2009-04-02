#pragma once

#include "AutoPtr.h"
#include "PhysJointDesc.h"
#include "NxPointOnLineJointDesc.h"

namespace PhysXWrapper
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