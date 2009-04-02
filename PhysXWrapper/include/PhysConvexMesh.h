#pragma once

class NxConvexMesh;

namespace Engine
{

namespace Physics
{

ref class PhysConvexMeshDesc;

/// <summary>
/// Wrapper class for NxConvexMesh.
/// A Convex Mesh. 
/// Internally represented as a list of convex polygons. The number of polygons 
/// is limited to 256.
/// To avoid duplicating data when you have several instances of a particular 
/// mesh positioned differently, you do not use this class to represent a convex 
/// object directly. Instead, you create an instance of this mesh via the 
/// NxConvexShape class.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysConvexMesh
{
internal:
	NxConvexMesh* convexMesh;

	/// <summary>
	/// Constructor
	/// </summary>
	PhysConvexMesh(NxConvexMesh* convexMesh);

public:
	/// <summary>
	/// Saves the mesh to a descriptor.
	/// </summary>
	/// <param name="desc">The descriptor to hold the result.</param>
	bool saveToDesc(PhysConvexMeshDesc^ desc);

	/// <summary>
	/// Returns the reference count for shared meshes.
	/// </summary>
	/// <returns>The current reference count, not used in any shapes if the count is 0.</returns>
	unsigned int getReferenceCount();
};

}

}