using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    class BulletShapeBuilder : ShapeBuilder
    {
        btCompoundShape currentCompound;
        BulletShapeRepository repository;

        public float ShapeMargin { get; set; }

        public BulletShapeBuilder()
        {
            currentCompound = null;
            ShapeMargin = 0.04f;
        }

        public void buildSphere(string name, float radius, Vector3 translation, string material)
        {
            commitShape(name, translation, Quaternion.Identity, new btSphereShape(radius, ShapeMargin));
        }

        public void buildBox(string name, Vector3 extents, Vector3 translation, Quaternion rotation, string material)
        {
            commitShape(name, translation, rotation, new btBoxShape(ref extents, ShapeMargin));
        }

        public void buildMesh(string name, float[] vertices, int[] faces, Vector3 translation, Quaternion rotation, string material)
        {
            throw new NotImplementedException();
        }

        public void buildPlane(string name, Vector3 normal, float distance, string material)
        {
            throw new NotImplementedException();
        }

        public void buildCapsule(string name, float radius, float height, Vector3 translation, Quaternion rotation, string material)
        {
            commitShape(name, translation, rotation, new btCapsuleShape(radius, height, ShapeMargin));
        }

        public unsafe void buildConvexHull(string name, float[] vertices, int[] faces, Vector3 translation, Quaternion rotation, string material)
        {
            fixed(float* verts = &vertices[0])
            {
                commitShape(name, translation, rotation, new btHullShape(verts, vertices.Length / 3, sizeof(Vector3), ShapeMargin));
            }
        }

        public unsafe void buildConvexHull(string name, float[] vertices, Vector3 translation, Quaternion rotation, string material)
        {
            fixed (float* verts = &vertices[0])
            {
                commitShape(name, translation, rotation, new btHullShape(verts, vertices.Length / 3, sizeof(Vector3), ShapeMargin));
            }
        }

        public void buildSoftBody(string name, float[] vertices, int[] tetrahedra, Vector3 translation, Quaternion rotation)
        {
            throw new NotImplementedException();
        }

        public void startCompound(string name)
        {
            if(currentCompound == null)
	        {
                currentCompound = new btCompoundShape(ShapeMargin);
	        }
	        else
	        {
		        throw new Exception(String.Format("Error loading compound collision shape {0}. The compound object already exists.", name));
	        }
        }

        public void stopCompound(string name)
        {
            repository.addCollection(new CompoundShapeCollection(currentCompound, name));
	        currentCompound = null;
        }

        public void setCurrentShapeLocation(ShapeLocation location)
        {
            repository.CurrentLoadingLocation = location;
        }

        public void createMaterial(string name, float restitution, float staticFriction, float dynamicFriction)
        {
            throw new NotImplementedException();
        }

        internal void setRepository(BulletShapeRepository repository)
        {
            this.repository = repository;
        }

        private void commitShape(String name, Vector3 translation, Quaternion rotation, btCollisionShape collisionShape)
        {
	        if(currentCompound != null)
	        {
                currentCompound.addChildShape(collisionShape, translation, rotation);
	        }
	        else
	        {
		        repository.addCollection(new BulletShapeCollection(collisionShape, name));
	        }
        }
    }
}
