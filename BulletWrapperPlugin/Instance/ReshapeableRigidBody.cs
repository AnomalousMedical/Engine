using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace BulletPlugin
{
    public class ReshapeableRigidBody : RigidBody
    {
        ReshapeableRigidBody(/*Reshapeable*/RigidBodyDefinition description, BulletScene scene, IntPtr collisionShape, Vector3 initialTrans, Quaternion initialRot)
            :base(description, scene, collisionShape, initialTrans, initialRot)
        {

        }

        protected override void Dispose()
        {

        }

        /// <summary>
	    /// Create a new hull region by decomposing the mesh in desc. If the region
        /// does not exist it will be created. If it does exist it will be cleared
        /// and recreated.
	    /// </summary>
	    /// <param name="name">The name of the region.</param>
	    /// <param name="desc">The mesh description and algorithm configuration settings.</param>
        public void createHullRegion(String name, ConvexDecompositionDesc desc)
        {
            throw new NotImplementedException();
        }

	    /// <summary>
	    /// Create a new hull region by decomposing the mesh in desc. If the region
	    ///  does not exist it will be created. If it does exist it will be cleared
	    ///  and recreated.
	    /// </summary>
	    /// <param name="name">The name of the region.</param>
	    /// <param name="desc">The mesh description and algorithm configuration settings.</param>
	    /// <param name="origin">An origin for the hull region.</param>
	    /// <param name="orientation">An orientation for the hull region.</param>
        public void createHullRegion(String name, ConvexDecompositionDesc desc, Vector3 origin, Quaternion orientation)
        {
            throw new NotImplementedException();
        }

	    /// <summary>
	    /// Add a Sphere to the given region. If the region does not exist it will
        /// be created.
	    /// </summary>
	    /// <param name="sectionName">The name of the section to add the sphere to.</param>
	    /// <param name="radius">The radius of the sphere.</param>
	    /// <param name="origin">The origin of the sphere.</param>
        public void addSphereShape(String regionName, float radius, Vector3 origin)
        {
            throw new NotImplementedException();
        }

	    /// <summary>
	    /// Empty and destroy a region removing it from the collision shape.
	    /// </summary>
	    /// <param name="name">The name of the region to destroy.</param>
        public void destroyRegion(String name)
        {
            throw new NotImplementedException();
        }

	    /// <summary>
	    /// This function will recompute the mass props. It should be called when
        /// the collision shape is changed.
	    /// </summary>
        public void recomputeMassProps()
        {
            throw new NotImplementedException();
        }
    }
}
