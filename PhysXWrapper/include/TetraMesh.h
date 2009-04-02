#pragma once

#include "AutoPtr.h"
#include "NxTetra.h"

namespace Physics
{

/// <summary>
/// This will be filled out by the library to contain the generated tetra meshes
/// or descriptors.
/// </summary>
public ref class TetraMesh
{
private:
	AutoPtr<NxTetraMesh> mesh;

internal:
	NxTetraMesh* getNxTetraMesh();

public:
	/// <summary>
	/// Constructor
	/// </summary>
	TetraMesh(void);

	/// <summary>
	/// True if the mesh represents tetrahedra, otherwise it is triangles.
	/// </summary>
	property bool IsTetra 
	{
		bool get();
		void set(bool value);
	}

	/// <summary>
	/// Number of vertices.
	/// </summary>
	property unsigned int VertexCount 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// Array of vertices.
	/// </summary>
	property float* Vertices 
	{
		float* get();
		void set(float* value);
	}

	/// <summary>
	/// Number of triangles.
	/// </summary>
	property unsigned int TriangleCount 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// The triangle indices.
	/// </summary>
	property unsigned int* Indices 
	{
		unsigned int* get();
		void set(unsigned int* value);
	}
};

}