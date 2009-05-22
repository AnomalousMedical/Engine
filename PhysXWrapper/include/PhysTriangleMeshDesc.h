#pragma once

#include "AutoPtr.h"
#include "Enums.h"

class NxTriangleMeshDesc;

namespace PhysXWrapper
{

/// <summary>
/// A structure describing a triangle mesh.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class PhysTriangleMeshDesc
{
internal:
	AutoPtr<NxTriangleMeshDesc> meshDesc;

public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysTriangleMeshDesc(void);

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
	/// Pointer to first vertex point.
	/// </summary>
	property void* Points 
	{
		void* get();
		void set(void* value);
	}

	/// <summary>
	/// Pointer to first triangle.
	/// </summary>
	property void* Triangles 
	{
		void* get();
		void set(void* value);
	}

	/// <summary>
	/// Flags bits, combined from values of the enum MeshFlags.
	/// </summary>
	property MeshFlags Flags 
	{
		MeshFlags get();
		void set(MeshFlags value);
	}

	/// <summary>
	/// If materialIndices is NULL (not used) then this should be zero. Otherwise 
	/// this is the offset between material indices in bytes. This is at least 
	/// sizeof(NxMaterialIndex).
	/// </summary>
	property unsigned int MaterialIndexStride 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// Optional pointer to first material index, or NULL. There are 
	/// NxSimpleTriangleMesh::numTriangles indices in total. Caller may add 
	/// materialIndexStride bytes to the pointer to access the next triangle.
	/// 
	/// When a triangle mesh collides with another object, a material is required at the 
	/// collision point. If materialIndices is NULL, then the material of the 
	/// NxTriangleMeshShape instance (specified via NxShapeDesc::materialIndex) is used. 
	/// Otherwise, if the point of contact is on a triangle with index i, then the material 
	/// index is determined as: 
	/// NxMaterialIndex index = *(NxMaterialIndex *)(((NxU8*)materialIndices) + materialIndexStride * i);
	/// 
	/// If the contact point falls on a vertex or an edge, a triangle adjacent to the vertex 
	/// or edge is selected, and its index used to look up a material. The selection is 
	/// arbitrary but consistent over time
	/// </summary>
	property void* MaterialIndices 
	{
		void* get();
		void set(void* value);
	}

	/// <summary>
	/// The SDK computes convex edges of a mesh and use them for collision detection. 
	/// This parameter allows you to setup a tolerance for the convex edge detector.
	/// </summary>
	property float ConvexEdgeThreshold 
	{
		float get();
		void set(float value);
	}

};

}