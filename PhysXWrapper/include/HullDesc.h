#pragma once

#include "AutoPtr.h"

class HullDesc;

namespace Physics
{

namespace StanHull
{

/// <summary>
/// Flags for hull computation.
/// </summary>
public enum class HullFlag : unsigned int
{
	/// <summary>
	/// Report results as triangles, not polygons.
	/// </summary>
	QF_TRIANGLES         = (1<<0),
	
	/// <summary>
	/// Reverse order of the triangle indices.
	/// </summary>
	QF_REVERSE_ORDER     = (1<<1), 
	
	/// <summary>
	/// Extrude hull based on this skin width.
	/// </summary>
	QF_SKIN_WIDTH        = (1<<2),
	
	/// <summary>
	/// No flags.
	/// </summary>
	QF_DEFAULT           = 0
};

/// <summary>
/// Description of hull to process.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class HullDesc
{
internal:
	AutoPtr<::HullDesc> desc;

public:
	/// <summary>
	/// Constructor.
	/// </summary>
	HullDesc(void);

	/// <summary>
	/// Check to see if a flag is set.
	/// </summary>
	/// <param name="flag">The flag to check.</param>
	/// <returns>True if the flag is set.</returns>
	bool hasHullFlag(HullFlag flag);

	/// <summary>
	/// Set the given flag.
	/// </summary>
	/// <param name="flag">The flag to set.</param>
	void setHullFlag(HullFlag flag);

	/// <summary>
	/// Clear the given flag.
	/// </summary>
	/// <param name="flag">The flag to clear.</param>
	void clearHullFlag(HullFlag flag);

	/// <summary>
	/// Flags to use when generating the convex hull.
	/// </summary>
	property HullFlag Flags 
	{
		HullFlag get();
		void set(HullFlag value);
	}

	/// <summary>
	/// Number of vertices in the input point cloud.
	/// </summary>
	property unsigned int Vcount 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// The array of vertices.
	/// </summary>
	property double* Vertices 
	{
		double* get();
		void set(double* value);
	}

	/// <summary>
	/// The stride of each vertex, in bytes.
	/// </summary>
	property unsigned int VertexStride 
	{
		unsigned int get();
		void set(unsigned int value);
	}

	/// <summary>
	/// The epsilon for removing duplicates.  This is a normalized value, if 
	/// normalized bit is on.
	/// </summary>
	property double NormalEpsilon 
	{
		double get();
		void set(double value);
	}

	/// <summary>
	/// The amount to infate the mesh.  Default is one centimeter.
	/// </summary>
	property double SkinWidth 
	{
		double get();
		void set(double value);
	}

	/// <summary>
	/// Maximum number of vertices to be considered for the hull.  The geometry
	/// will be simplified if it is greater than this number.
	/// </summary>
	property unsigned int MaxVertices 
	{
		unsigned int get();
		void set(unsigned int value);
	}
};

}

}