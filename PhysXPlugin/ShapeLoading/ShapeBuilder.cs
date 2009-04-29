using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;

namespace Engine.Physics.ShapeLoading
{
    /// <summary>
    /// This interface will allow clients to get information from a data source to 
    /// build collision shapes.  All values passed to a function are only considered
    /// valid during the execution of that function.
    /// </summary>
    public interface ShapeBuilder
    {
        /// <summary>
        /// Construct a sphere.
        /// </summary>
        /// <param name="name">The name of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="translation">The position of the sphere in a compound shape's local coords.</param>
        void buildSphere(String name, float radius, Vector3 translation, String material);

        /// <summary>
        /// Construct a box.
        /// </summary>
        /// <param name="name">The name of the box.</param>
        /// <param name="extents">The half extents of the box in each dimension.</param>
        /// <param name="translation">The location of the box in the shape's local coords.</param>
        /// <param name="rotation">The rotation of the box in the shape's local coords.</param>
        void buildBox(String name, Vector3 extents, Vector3 translation, Quaternion rotation, String material);

        /// <summary>
        /// Construct a triangle mesh.
        /// </summary>
        /// <param name="name">The name of the triangle mesh.</param>
        /// <param name="vertices">The vertices of the mesh.</param>
        /// <param name="faces">The triangles of the mesh.</param>
        /// <param name="translation">The translation of the mesh in the shape's local coords.</param>
        /// <param name="rotation">The rotation of the mesh in the shape's local coords.</param>
        void buildMesh(String name, float[] vertices, int[] faces, Vector3 translation, Quaternion rotation, String material);

        /// <summary>
        /// Construct a plane shape.
        /// </summary>
        /// <param name="name">The name of the plane.</param>
        /// <param name="normal">A vector describing a normal to the plane.</param>
        /// <param name="distance">The distance from the origin the plane is located along its normal.</param>
        void buildPlane(String name, Vector3 normal, float distance, String material);

        /// <summary>
        /// Construct a capsule shape.
        /// </summary>
        /// <param name="name">The name of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="height">The height of the capsule.</param>
        /// <param name="translation">The location of the capsule in the shape's local coords.</param>
        /// <param name="rotation">The rotation of the capsule in the shape's local coords.</param>
        void buildCapsule(String name, float radius, float height, Vector3 translation, Quaternion rotation, String material);

        /// <summary>
        /// Construct a convex hull shape with defined triangles.
        /// </summary>
        /// <param name="name">The name of the convex hull.</param>
        /// <param name="vertices">The vertices of the convex hull.</param>
        /// <param name="faces">The triangles of the convex hull.</param>
        /// <param name="translation">The translation of the convex hull in the shape's local coords.</param>
        /// <param name="rotation">The rotation of the convex hull in the shape's local coords.</param>
        void buildConvexHull(String name, float[] vertices, int[] faces, Vector3 translation, Quaternion rotation, String material);

        /// <summary>
        /// Construct a convex hull that has no triangle data.
        /// </summary>
        /// <param name="name">The name of the convex hull.</param>
        /// <param name="vertices">The vertices of the convex hull.</param>
        /// <param name="translation">The translation of the convex hull in the shape's local coords.</param>
        /// <param name="rotation">The rotation of the convex hull in the shapes's local coords.</param>
        void buildConvexHull(String name, float[] vertices, Vector3 translation, Quaternion rotation, String material);

        /// <summary>
        /// Build a soft body mesh.
        /// </summary>
        /// <param name="name">The name of the soft body.</param>
        /// <param name="vertices">The vertices in the soft body.</param>
        /// <param name="tetrahedra">The tetrahedra of the soft body.</param>
        void buildSoftBody(String name, float[] vertices, int[] tetrahedra, Vector3 translation, Quaternion rotation);

        /// <summary>
        /// Start building a compound shape.  Any shapes that are added between this call
        /// and a call to stopCompound should be added to a compound shape.  Only one
        /// compound can be built at a time, they cannot be nested.
        /// </summary>
        /// <param name="name">The name of the compound shape.</param>
        void startCompound(String name);

        /// <summary>
        /// Finish building a compound shape.  All new shapes added after this call should
        /// be considered independent shapes, and it is also safe to call startCompound again.
        /// </summary>
        /// <param name="name">The name of the compound shape.</param>
        void stopCompound(String name);

        /// <summary>
        /// Set the current location that is being loaded so the shapes loaded from that location
        /// can be tracked.
        /// </summary>
        /// <param name="location">The location currently being loaded.</param>
        void setCurrentShapeLocation(ShapeLocation location);

        /// <summary>
        /// This function will build a material with the given parameters.
        /// </summary>
        /// <param name="material"></param>
        void createMaterial(PhysMaterialDesc material);
    }
}
