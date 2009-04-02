#pragma once

class NxSoftBodyMesh;

namespace PhysXWrapper
{

ref class PhysSoftBodyMeshDesc;

ref class PhysSoftBodyMesh;

typedef System::Collections::Generic::Dictionary<System::IntPtr, PhysSoftBodyMesh^> MeshDictionary;

/// <summary>
/// A soft body mesh object.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysSoftBodyMesh
{
private:
	System::String^ name;

	static MeshDictionary^ meshDictionary = gcnew MeshDictionary();

internal:
	/// <summary>
	/// Constructor.  Internal use only.
	/// </summary>
	/// <param name="softMesh">The NxSoftBodyMesh to wrap.</param>
	PhysSoftBodyMesh(System::String^ name, NxSoftBodyMesh* softMesh);

	NxSoftBodyMesh* softMesh;

	/// <summary>
	/// Internal function to get the PhysSoftBodyMesh object for a given NxSoftBodyMesh.
	/// </summary>
	/// <param name="softMesh">The NxSoftBodyMesh* to find the mesh for.</param>
	/// <returns>The PhysSoftBodyMesh object wrapping the appropriate NxSoftBodyMesh.</returns>
	static PhysSoftBodyMesh^ getMeshObject(NxSoftBodyMesh* softMesh);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	~PhysSoftBodyMesh();

	/// <summary>
	/// The name of the mesh.
	/// </summary>
	property System::String^ Name
	{ 
		System::String^ get() { return name; }
	}

	/// <summary>
	/// Saves the soft body mesh descriptor. A soft body mesh is created via the
    /// cooker. The cooker potentially changes the order of the arrays
    /// references by the pointers vertices and triangles. Since saveToDesc
    /// returns the data of the cooked mesh, this data might differ from the
    /// originally provided data. Note that this is in contrast to the meshData
    /// member of NxSoftBodyDesc, which is guaranteed to provide data in the
    /// same order as that used to create the mesh. 
	/// </summary>
	/// <param name="desc">The mesh description to save to.</param>
	/// <returns></returns>
	bool saveToDesc(PhysSoftBodyMeshDesc^ desc);

	/// <summary>
	/// Gets the number of soft body instances referencing this soft body mesh. 
	/// </summary>
	/// <returns></returns>
	unsigned int getReferenceCount();
};

}