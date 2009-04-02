#pragma once

struct NxRaycastHit;

namespace PhysXWrapper
{

ref class PhysActor;

/// <summary>
/// This class wraps the NxRaycastHit.  It cannot be constructed directly use PhysRaycastHit for that.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class RaycastHit
{
private:
	const NxRaycastHit* raycastHit;

internal:
	/// <summary>
	/// Set the NxRaycastHit object being wrapped by this class
	/// </summary>
	void setCurrentHit( const NxRaycastHit* raycastHit );

	/// <summary>
	/// Constructor
	/// </summary>
	RaycastHit(void);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~RaycastHit(void);

	/// <summary>
	/// Get the Actor the ray collided with
	/// </summary>
	PhysActor^ getCollidedActor();

	/// <summary>
	/// Get the impact point in world coordinates.
	/// </summary>
	void getWorldImpact( EngineMath::Vector3% impact );

	/// <summary>
	/// Get the impact normal in world coordinates.
	/// </summary>
	void getWorldNormal( EngineMath::Vector3% normal );

	/// <summary>
	/// Get the distance to the impact point
	/// </summary>
	float getDistance();

	/// <summary>
	/// Get the impact barycentric coordinates u value
	/// </summary>
	float getBarycentricU();

	/// <summary>
	/// Get the impact barycentric coordinates v value
	/// </summary>
	float getBarycentricV();
};

}