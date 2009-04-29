using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper.StanHull;
using EngineMath;
using PhysXWrapper;
using Logging;

namespace Engine.Physics.ShapeLoading
{
    /// <summary>
    /// This class will build shapes for the PhysX Engine.
    /// </summary>
    public class PhysXShapeBuilder : ShapeBuilder
    {
        private ShapeCollection currentCompound = null;
        private PhysXShapeRepository repository;
        private HullLibrary hullLibrary = new HullLibrary();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="repository">The repository to put constructed shapes into.</param>
        public PhysXShapeBuilder(PhysXShapeRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Build a sphere shape.
        /// </summary>
        /// <param name="name">The name of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="translation">The location of the sphere.</param>
        public void buildSphere(string name, float radius, Vector3 translation, String material)
        {
            PhysSphereShapeDesc sphereDesc = new PhysSphereShapeDesc();
            sphereDesc.Radius = radius;
            sphereDesc.setLocalPose(translation, Quaternion.Identity);
            commitShape(sphereDesc, material, name);
        }

        /// <summary>
        /// Build a box.
        /// </summary>
        /// <param name="name">The name of the box.</param>
        /// <param name="extents">The half extents of the box.</param>
        /// <param name="translation">The translation of the box.</param>
        /// <param name="rotation">The rotation of the box.</param>
        public void buildBox(string name, Vector3 extents, Vector3 translation, Quaternion rotation, String material)
        {
            PhysBoxShapeDesc boxDesc = new PhysBoxShapeDesc();
            boxDesc.Dimensions = extents / 2.0f;
            boxDesc.setLocalPose(translation, rotation);
            commitShape(boxDesc, material, name);
        }

        /// <summary>
        /// Build a mesh shape.
        /// </summary>
        /// <param name="name">The name of the mesh.</param>
        /// <param name="vertices">The vertices in the mesh.</param>
        /// <param name="faces">The faces of the mesh.</param>
        /// <param name="translation">The translation of the mesh.</param>
        /// <param name="rotation">The rotation of the mesh.</param>
        public void buildMesh(string name, float[] vertices, int[] faces, Vector3 translation, Quaternion rotation, String material)
        {
            PhysTriangleMeshShapeDesc triMeshShapeDesc = new PhysTriangleMeshShapeDesc();
            triMeshShapeDesc.setLocalPose(translation, rotation);
            PhysTriangleMeshDesc triMeshDesc = new PhysTriangleMeshDesc();
            triMeshDesc.NumVertices = (uint)(vertices.Length / 3);
            triMeshDesc.NumTriangles = (uint)(faces.Length / 3);
            triMeshDesc.PointStrideBytes = sizeof(float) * 3;
            triMeshDesc.TriangleStrideBytes = sizeof(int) * 3;
            //cook the mesh
            bool meshBuilt = false;
            unsafe
            {
                fixed (float* vertsPtr = vertices)
                {
                    fixed (int* triPtr = faces)
                    {
                        triMeshDesc.Points = vertsPtr;
                        triMeshDesc.Triangles = triPtr;
                        PhysMemoryWriteBuffer buffer = new PhysMemoryWriteBuffer();
                        meshBuilt = PhysCooking.cookTriangleMesh(triMeshDesc, buffer);
                        if (meshBuilt)
                        {
                            PhysMemoryReadBuffer readBuffer = new PhysMemoryReadBuffer(buffer);
                            triMeshShapeDesc.MeshData = PhysSDK.Instance.createTriangleMesh(readBuffer);
                        }
                        buffer.Dispose();
                    }
                }
            }
            //Check to see if everything went ok
            if (meshBuilt)
            {
                if (commitShape(triMeshShapeDesc, material, name))
                {
                    repository.addTriangleMesh(name, triMeshShapeDesc.MeshData);
                }
                else
                {
                    PhysSDK.Instance.releaseTriangleMesh(triMeshShapeDesc.MeshData);
                }
            }
            else
            {
                Log.Default.sendMessage("Error creating triangle mesh " + name + " could not mesh from points given.  This shape will be ignored.", LogLevel.Error, "Physics");
            }
        }

        /// <summary>
        /// Build a plane shape.
        /// </summary>
        /// <param name="name">The name of the plane.</param>
        /// <param name="normal">The normal of the plane.</param>
        /// <param name="distance">The distance of the plane from the origin along its normal.</param>
        public void buildPlane(string name, Vector3 normal, float distance, String material)
        {
            PhysPlaneShapeDesc planeDesc = new PhysPlaneShapeDesc();
            planeDesc.Normal = normal;
            planeDesc.D = distance;
            commitShape(planeDesc, material, name);
        }

        /// <summary>
        /// Build a capsule shape.
        /// </summary>
        /// <param name="name">The name of the capsule.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="height">The height of the capsule.</param>
        /// <param name="translation">The translation of the capsule.</param>
        /// <param name="rotation">The rotation of the capsule.</param>
        public void buildCapsule(string name, float radius, float height, Vector3 translation, Quaternion rotation, String material)
        {
            PhysCapsuleShapeDesc capsuleDesc = new PhysCapsuleShapeDesc();
            capsuleDesc.setLocalPose(translation, rotation);
            capsuleDesc.Radius = radius;
            capsuleDesc.Height = height;
            commitShape(capsuleDesc, material, name);
        }

        /// <summary>
        /// Build a convex hull shape.
        /// </summary>
        /// <param name="name">The name of the hull.</param>
        /// <param name="vertices">The vertices of the hull.</param>
        /// <param name="faces">The faces of the hull.</param>
        /// <param name="translation">The translation of the hull.</param>
        /// <param name="rotation">The rotation of the hull.</param>
        public void buildConvexHull(string name, float[] vertices, int[] faces, Vector3 translation, Quaternion rotation, String material)
        {
            PhysConvexShapeDesc convexDesc = new PhysConvexShapeDesc();
            convexDesc.setLocalPose(translation, rotation);
            PhysConvexMeshDesc meshDesc = new PhysConvexMeshDesc();
            meshDesc.NumVertices = (uint)vertices.Length / 3;
            meshDesc.PointStrideBytes = sizeof(float) * 3;
            meshDesc.NumTriangles = (uint)faces.Length / 3;
            meshDesc.TriangleStrideBytes = sizeof(int) * 3;
            bool meshBuilt;
            //cook the mesh in the unsafe block
            unsafe
            {
                fixed (float* vertsPtr = vertices)
                {
                    fixed (int* triPtr = faces)
                    {
                        meshDesc.Points = vertsPtr;
                        meshDesc.Triangles = triPtr;
                        PhysMemoryWriteBuffer buffer = new PhysMemoryWriteBuffer();
                        meshBuilt = PhysCooking.cookConvexMesh(meshDesc, buffer);
                        if (meshBuilt)
                        {
                            PhysMemoryReadBuffer readBuffer = new PhysMemoryReadBuffer(buffer);
                            convexDesc.MeshData = PhysSDK.Instance.createConvexMesh(readBuffer);
                        }
                        buffer.Dispose();
                    }
                }
            }
            //Check to see if everything went ok
            if (meshBuilt)
            {
                if (!commitShape(convexDesc, material, name))
                {
                    PhysSDK.Instance.releaseConvexMesh(convexDesc.MeshData);
                    convexDesc.Dispose();
                }
                else
                {
                    repository.addConvexMesh(name, convexDesc.MeshData);
                }
            }
            else
            {
                Log.Default.sendMessage("Error creating convex hull " + name + " could not build hull from points given.  This shape will be ignored.", LogLevel.Error, "Physics");
            }
        }

        /// <summary>
        /// Build a convex hull from a point cloud.
        /// </summary>
        /// <param name="name">The name of the convex hull.</param>
        /// <param name="vertices">The vertices of the convex hull.</param>
        /// <param name="translation">The translation of the convex hull.</param>
        /// <param name="rotation">The rotation of the convex hull.</param>
        public void buildConvexHull(string name, float[] vertices, Vector3 translation, Quaternion rotation, String material)
        {
            PhysConvexShapeDesc convexDesc = new PhysConvexShapeDesc();
            convexDesc.setLocalPose(translation, rotation);
            PhysConvexMeshDesc meshDesc = new PhysConvexMeshDesc();
            meshDesc.NumVertices = (uint)vertices.Length / 3;
            meshDesc.PointStrideBytes = sizeof(float) * 3;
            meshDesc.Flags = ConvexFlags.NX_CF_COMPUTE_CONVEX;
            bool test;
            //cook the mesh in the unsafe block
            unsafe
            {
                fixed (float* vertsPtr = vertices)
                {
                    meshDesc.Points = vertsPtr;
                    PhysMemoryWriteBuffer buffer = new PhysMemoryWriteBuffer();
                    test = PhysCooking.cookConvexMesh(meshDesc, buffer);
                    if (test)
                    {
                        PhysMemoryReadBuffer readBuffer = new PhysMemoryReadBuffer(buffer);
                        convexDesc.MeshData = PhysSDK.Instance.createConvexMesh(readBuffer);
                    }
                }
            }
            //Check to see if everything went ok
            if (test)
            {
                if (!commitShape(convexDesc, material, name))
                {
                    PhysSDK.Instance.releaseConvexMesh(convexDesc.MeshData);
                    convexDesc.Dispose();
                }
                else
                {
                    repository.addConvexMesh(name, convexDesc.MeshData);
                }
            }
            else
            {
                Log.Default.sendMessage("Error creating convex hull " + name + " could not build hull from points given.  This shape will be ignored.", LogLevel.Error, "Physics");
            }
        }

        public void buildSoftBody(String name, float[] vertices, int[] tetrahedra, Vector3 translation, Quaternion rotation)
        {
            PhysSoftBodyMeshDesc softBodyMeshDesc = new PhysSoftBodyMeshDesc();
            softBodyMeshDesc.Name = name;
            softBodyMeshDesc.NumVertices = (uint)vertices.Length / 3;
            softBodyMeshDesc.NumTetrahedra = (uint)tetrahedra.Length / 4;
            softBodyMeshDesc.VertexStrideBytes = sizeof(float) * 3;
            softBodyMeshDesc.TetrahedronStrideBytes = 4 * sizeof(uint);
            softBodyMeshDesc.VertexMassStrideBytes = sizeof(float);
            softBodyMeshDesc.VertexFlagStrideBytes = sizeof(uint);
            softBodyMeshDesc.Flags = 0;

            PhysSoftBodyMesh softBodyMesh = null;
            unsafe
            {
                softBodyMeshDesc.VertexMasses = (void*)0;
                softBodyMeshDesc.VertexFlags = (void*)0;
                PhysMemoryWriteBuffer writeBuffer = new PhysMemoryWriteBuffer();
                fixed (float* vertPtr = vertices)
                {
                    fixed (int* tetraPtr = tetrahedra)
                    {
                        softBodyMeshDesc.Vertices = vertPtr;
                        softBodyMeshDesc.Tetrahedra = tetraPtr;
                        if (PhysCooking.cookSoftBodyMesh(softBodyMeshDesc, writeBuffer))
                        {
                            PhysMemoryReadBuffer readBuffer = new PhysMemoryReadBuffer(writeBuffer);
                            softBodyMesh = PhysSDK.Instance.createSoftBodyMesh(name, readBuffer);
                        }
                    }
                }
            }
            if (softBodyMesh != null)
            {
                repository.addSoftBodyMesh(softBodyMesh);
            }
            else
            {
                Log.Default.sendMessage("Error creating soft body {0}.", LogLevel.Error, "Physics", name);
            }
        }

        /// <summary>
        /// Start building a compound shape.  All shapes added before a call to stopCompound
        /// will be part of this new compound.
        /// </summary>
        /// <param name="name">The name of the compound shape.</param>
        public void startCompound(string name)
        {
            currentCompound = new ShapeCollection(name);
        }

        /// <summary>
        /// Stop building a compound shape and add it to the repository.
        /// </summary>
        /// <param name="name">The name of the compound shape.</param>
        public void stopCompound(string name)
        {
            repository.addCollection(currentCompound);
            currentCompound = null;
        }

        /// <summary>
        /// Helper function to add a shape to the repository or current compound.
        /// </summary>
        /// <param name="desc">The shape description to save.</param>
        private bool commitShape(PhysShapeDesc desc, String material, String name)
        {
            if (material != null)
            {
                PhysMaterial mat = repository.getMaterial(material);
                if (mat != null)
                {
                    throw new NotImplementedException();
                    //desc.setMaterial(mat);
                }
            }
            if (currentCompound != null)
            {
                currentCompound.addShape(desc);
                return true;
            }
            else
            {
                ShapeCollection shape = new ShapeCollection(name);
                shape.addShape(desc);
                return repository.addCollection(shape);
            }
        }

        /// <summary>
        /// Set the shape locatoin for this builder.
        /// </summary>
        /// <param name="location">The location to use.</param>
        public void setCurrentShapeLocation(ShapeLocation location)
        {
            repository.CurrentLoadingLocation = location;
        }

        public void createMaterial(PhysMaterialDesc material)
        {
            repository.addMaterial(material);
        }
    }
}
