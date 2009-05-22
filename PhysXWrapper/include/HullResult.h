#pragma once

#include "AutoPtr.h"

class HullResult;

namespace PhysXWrapper
{

namespace StanHull
{

/// <summary>
/// The results of a hull computation.
/// </summary>
[Engine::Attributes::NativeSubsystemTypeAttribute]
public ref class HullResult
{
internal:
	AutoPtr<::HullResult> result;

public:
	HullResult(void);

	/// <summary>
	/// True if indices represents polygons, False indices are triangles.
	/// </summary>
	/// <remarks>
	/// If triangles, then indices are array indexes into the vertex list.
	/// If polygons, indices are in the form (number of points in face) (p1, p2, p3, ..) etc..
	/// </remarks>
	property bool IsPolygons
	{
		bool get();
	}

	/// <summary>
	/// Number of vertices in the output hull.
	/// </summary>
	property unsigned int NumOutputVertices 
	{
		unsigned int get();
	}

	/// <summary>
	/// Array of vertices, 3 doubles each x,y,z.
	/// </summary>
	property double* OutputVertices 
	{
		double* get();
	}

	/// <summary>
	/// The number of faces produced.
	/// </summary>
	property unsigned int NumFaces 
	{
		unsigned int get();
	}

	/// <summary>
	/// The total number of indices.
	/// </summary>
	property unsigned int NumIndices 
	{
		unsigned int get();
	}

	/// <summary>
	/// Pointer to indices.
	/// </summary>
	property unsigned int* Indices 
	{
		unsigned int* get();
	}
};

}

}