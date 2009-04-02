#pragma once

class NxTetraInterface;

namespace Physics
{

ref class TetraMesh;

public ref class TetraInterface
{
private:
	NxTetraInterface* tetraInterface;

	static TetraInterface^ instance = nullptr;

	TetraInterface(NxTetraInterface* tetraInterface);

public:
	static TetraInterface^ getTetraInterface();

	/// <summary>
	/// Create a tetramesh and place it into mesh. Be sure to call
    /// releaseTetraMesh on this mesh to clean it up with you are finished.
	/// </summary>
	/// <param name="mesh">Mesh to populate.</param>
	/// <param name="vcount">The number of vertices.</param>
	/// <param name="vertices">The vertex positions.</param>
	/// <param name="tcount">The number of triangles or tetrahdra.</param>
	/// <param name="indices">The indices 3 per tri or 4 per tetrahdron.</param>
	/// <param name="isTetra">Flag to indicate triangles or tetrahedron.</param>
	/// <returns>True if the mesh was built correctly.</returns>
	bool createTetraMesh(TetraMesh^ mesh, unsigned int vcount, const float *vertices, unsigned int tcount, const unsigned int *indices, bool isTetra);

	/// <summary>
	/// Set the subdivision level used to calculate the tetra mesh. Default is 30.
	/// </summary>
	/// <param name="subdivisionLevel">The subdivision level to set.</param>
	void setSubdivisionLevel(unsigned int subdivisionLevel);

	/// <summary>
	/// Create an iso surface from a mesh.
	/// </summary>
	/// <param name="input">The input mesh to generate a tetra mesh from.</param>
	/// <param name="output">The output iso surface mesh.</param>
	/// <param name="isoSingle"></param>
	/// <returns>The number of indices in the iso surface.</returns>
	unsigned int createIsoSurface(TetraMesh^ input, TetraMesh^ output, bool isoSingle);

	/// <summary>
	/// Simplify the surface of a tetra mesh.
	/// </summary>
	/// <param name="factor">The simplification factor.</param>
	/// <param name="input">The input mesh to simplify.</param>
	/// <param name="output">The output simplified mesh.</param>
	/// <returns>The number of indices in the simplified surface.</returns>
	unsigned int simplifySurface(float factor, TetraMesh^ input, TetraMesh^ output);

	/// <summary>
	/// Create a tetra mesh from another mesh.
	/// </summary>
	/// <param name="inputMesh">The input mesh to generate the tetra mesh from.</param>
	/// <param name="outputMesh">The output tetra mesh.</param>
	/// <returns>The number of indices in the tetra mesh.</returns>
	unsigned int createTetraMesh(TetraMesh^ inputMesh, TetraMesh^ outputMesh);

	/// <summary>
	/// Release the resources from a tetra mesh. This should be run on any
    /// output meshes from the api.
	/// </summary>
	/// <param name="mesh">The mesh to release.</param>
	/// <returns></returns>
	bool releaseTetraMesh(TetraMesh^ mesh);
};

}