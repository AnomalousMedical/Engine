#pragma once

using namespace System;
using namespace Engine;

namespace BulletPlugin
{

ref class BulletShapeRepository : ShapeRepository
{
public:
	BulletShapeRepository(void);

	virtual ~BulletShapeRepository();

	/// <summary>
    /// Add a convex mesh that will be managed by this reposotory.  This will
    /// be released when this object is disposed.
    /// </summary>
    /// <param name="mesh">The mesh to add.</param>
    virtual void addConvexMesh(String^ name, ConvexMesh^ mesh) override;

    /// <summary>
    /// Remove a convex mesh from the repository and destroy it.  The mesh is no longer
    /// usable after this method is called.
    /// </summary>
    /// <param name="name">The mesh to remove.</param>
    virtual void destroyConvexMesh(String^ name) override;

    /// <summary>
    /// Add control over a triangle mesh to this repository.  When the repository
    /// is disposed this mesh will be released and will no longer be avaliable.
    /// </summary>
    /// <param name="mesh">The triangle mesh to add.</param>
    virtual void addTriangleMesh(String^ name, TriangleMesh^ mesh) override;

    /// <summary>
    /// Remove a triangle mesh from this repository and destroy it.  The mesh is no longer
    /// usable after this method is called.
    /// </summary>
    /// <param name="name">The mesh to remove.</param>
    virtual void destroyTriangleMesh(String^ name) override;

    /// <summary>
    /// Add a material to the repository.
    /// </summary>
    /// <param name="name">The name of the material.</param>
    /// <param name="material">The material itself.</param>
    virtual void addMaterial(String^ name, ShapeMaterial^ materialDesc) override;

    /// <summary>
    /// Determine if the repository has a given material.
    /// </summary>
    /// <param name="name">The name of the material.</param>
    /// <returns>True if the repository has the given material.</returns>
    virtual bool hasMaterial(String^ name) override;

    /// <summary>
    /// Get the material named name.
    /// </summary>
    /// <param name="name">The name of the material.</param>
    virtual ShapeMaterial^ getMaterial(String^ name) override;

    /// <summary>
    /// Destroy a material and remove it from this repositiory.
    /// </summary>
    /// <param name="name">The name of the material to destroy.</param>
    virtual void destroyMaterial(String^ name) override;

    /// <summary>
    /// Add a soft body mesh to the repository.
    /// </summary>
    /// <param name="softBodyMesh">The soft body mesh to add.</param>
    virtual void addSoftBodyMesh(SoftBodyMesh^ softBodyMesh) override;

    /// <summary>
    /// Get a soft body mesh from the repository.
    /// </summary>
    /// <param name="name">The name of the soft body mesh to get.</param>
    /// <returns>The soft body mesh requested, or null if it does not exist.</returns>
    virtual SoftBodyMesh^ getSoftBodyMesh(String^ name) override;

    /// <summary>
    /// Destroy the soft body mesh specified by name.
    /// </summary>
    /// <param name="name">The name of the soft body mesh to destroy.</param>
    virtual void destroySoftBodyMesh(String^ name) override;
};

}