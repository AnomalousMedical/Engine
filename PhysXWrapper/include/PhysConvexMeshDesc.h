#pragma once

#include "AutoPtr.h"
#include "Enums.h"
#include "NxConvexMeshDesc.h"

namespace Engine
{

namespace Physics
{

/// <summary>
/// Wrapper for NxConvexMeshDesc.
/// Descriptor class for NxConvexMesh.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysConvexMeshDesc
{
internal:
	AutoPtr<NxConvexMeshDesc> meshDesc;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysConvexMeshDesc(void);

	/// <summary>
	/// Destructor
	/// </summary>
	~PhysConvexMeshDesc();

	/// <summary>
	/// Number of vertices.
	/// </summary>
	property unsigned int NumVertices 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// Number of triangles.
	/// Hardware rigid body scenes have a limit of 32 faces per convex. 
	/// Fluid scenes have a limit of 64 faces per cooked convex for dynamic actors.
	/// </summary>
	property unsigned int NumTriangles 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// Offset between vertex points in bytes.
	/// </summary>
	property unsigned int PointStrideBytes 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// Offset between triangles in bytes.
	/// </summary>
	property unsigned int TriangleStrideBytes 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// Flags bits, combined from values of the enum NxConvexFlags.
	/// </summary>
	property ConvexFlags Flags 
	{
		ConvexFlags get();
		void set(ConvexFlags value);
	}

	/// <summary>
	/// Pointer to array of vertex positions. Pointer to first vertex point. 
	/// Caller may add pointStrideBytes bytes to the pointer to access the next point.
	/// </summary>
	property void* Points 
	{
		void* get();
		void set(void* value);
	}

	/// <summary>
	/// Pointer to array of triangle indices. 
	/// Pointer to first triangle. Caller may add triangleStrideBytes bytes to 
	/// the pointer to access the next triangle.
	/// </summary>
	property void* Triangles 
	{
		void* get();
		void set(void* value);
	}
};

}

}