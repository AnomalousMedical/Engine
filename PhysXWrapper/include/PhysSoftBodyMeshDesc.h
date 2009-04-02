#pragma once

#include "AutoPtr.h"
#include "Enums.h"

class NxSoftBodyMeshDesc;

namespace Physics
{

/// <summary>
/// Descriptor class for NxSoftBodyMesh. 
/// <para>
/// The mesh data is *copied* when an NxSoftBodyMesh object is created from this
/// descriptor. After the creation the user may discard the basic mesh data.
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysSoftBodyMeshDesc
{
private:
	System::String^ name;

internal:
	AutoPtr<NxSoftBodyMeshDesc> meshDesc;

public:
	/// <summary>
	/// Constructor.
	/// </summary>
	PhysSoftBodyMeshDesc(void);

	/// <summary>
	/// The name of the mesh.
	/// </summary>
	property System::String^ Name
	{ 
		System::String^ get() { return name; }
		void set(System::String^ value) { name = value; }
	}

	/// <summary>
	/// Number of vertices.
	/// </summary>
	property System::UInt32 NumVertices 
	{
		System::UInt32 get();
		void set(System::UInt32 value);
	}

	/// <summary>
	/// Number of tetrahedra.
	/// </summary>
	property System::UInt32 NumTetrahedra 
	{
		System::UInt32 get();
		void set(System::UInt32 value);
	}

	/// <summary>
	/// Offset between vertex points in bytes. 
	/// </summary>
	property System::UInt32 VertexStrideBytes 
	{
		System::UInt32 get();
		void set(System::UInt32 value);
	}

	/// <summary>
	/// Offset between tetrahedra in bytes. 
	/// </summary>
	property System::UInt32 TetrahedronStrideBytes 
	{
		System::UInt32 get();
		void set(System::UInt32 value);
	}

	/// <summary>
	/// Pointer to first vertex point. 
	/// Caller may add vertexStrideBytes bytes to the pointer to access the next point.
	/// </summary>
	property void* Vertices 
	{
		void* get();
		void set(void* value);
	}

	/// <summary>
	/// Pointer to first tetrahedron. Caller may add tetrahedronStrideBytes
	/// bytes to the pointer to access the next tetrahedron. These are
	/// quadruples of 0 based indices: vert0 vert1 vert2 vert3 vert0 vert1 vert2
	/// vert3 vert0 vert1 vert2 vert3 ... where vertex is either a 32 or 16 bit
    /// unsigned integer. There are numTetrahedra*4 indices. This is declared as
	/// a void pointer because it is actually either an NxU16 or a NxU32
    /// pointer. 
	/// </summary>
	property void* Tetrahedra 
	{
		void* get();
		void set(void* value);
	}

	/// <summary>
	/// Offset between vertex masses in bytes.
	/// </summary>
	property System::UInt32 VertexMassStrideBytes 
	{
		System::UInt32 get();
		void set(System::UInt32 value);
	}

	/// <summary>
	/// Offset between vertex flags in bytes.
	/// </summary>
	property System::UInt32 VertexFlagStrideBytes 
	{
		System::UInt32 get();
		void set(System::UInt32 value);
	}
	
	/// <summary>
	/// Pointer to first vertex mass. 
	/// Caller may add vertexMassStrideBytes bytes to the pointer to access the next vertex mass.
	/// </summary>
	property void* VertexMasses 
	{
		void* get();
		void set(void* value);
	}


	/// <summary>
	/// Pointer to first vertex flag. Flags are of type NxSoftBodyVertexFlags. 
	/// 
	/// Caller may add vertexFlagStrideBytes bytes to the pointer to access the next vertex flag.
	/// </summary>
	property void* VertexFlags 
	{
		void* get();
		void set(void* value);
	}

	/// <summary>
	/// Mesh flags. 
	/// </summary>
	property PhysSoftBodyMeshFlags Flags 
	{
		PhysSoftBodyMeshFlags get();
		void set(PhysSoftBodyMeshFlags value);
	}
};

}