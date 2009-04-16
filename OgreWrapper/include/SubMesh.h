#pragma once

namespace Ogre
{
	class SubMesh;
}

namespace Rendering{

ref class VertexData;
ref class IndexData;

/// <summary>
/// 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class SubMesh
{
private:
	Ogre::SubMesh* subMesh;
	VertexData^ vertex;
	IndexData^ index;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="subMesh">The Ogre::SubMesh to wrap.</param>
	SubMesh(Ogre::SubMesh* subMesh);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~SubMesh(void);

	/// <summary>
	/// True if this SubMesh uses the vertices defined in the parent mesh.
	/// </summary>
	property bool UseSharedVertices 
	{
		bool get();
	}

	/// <summary>
	/// Dedicated vertex data (only valid if useSharedVertices = false). This
    /// data is completely owned by this submesh. The use of shared or
    /// non-shared buffers is determined when model data is converted to the
    /// OGRE .mesh format. 
	/// </summary>
	property VertexData^ vertexData
	{
		VertexData^ get();
	}

	/// <summary>
	/// Face index data. 
	/// </summary>
	property IndexData^ indexData
	{
		IndexData^ get();
	}
};

}