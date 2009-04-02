#pragma once

class NxMat34;

namespace PhysXWrapper
{

/// <summary>
/// This interface allows clients to listen for position updates from the
/// Active Transforms in the update function.
/// </summary>
public interface class ActiveTransformCallback
{
public:
	/// <summary>
	/// Called to update the position.
	/// </summary>
	/// <param name="translation">The new translation of the object.</param>
	/// <param name="rotation">The new rotation of the object.</param>
	void firePositionUpdate(EngineMath::Vector3% translation, EngineMath::Quaternion% rotation);
};

}