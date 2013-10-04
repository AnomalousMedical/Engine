using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public interface ReshapeableRigidBody : RigidBody
    {
        /// <summary>
	    /// Create a new hull region by decomposing the mesh in desc. If the region
        /// does not exist it will be created. If it does exist it will be cleared
        /// and recreated.
	    /// </summary>
	    /// <param name="name">The name of the region.</param>
	    /// <param name="desc">The mesh description and algorithm configuration settings.</param>
        void createHullRegion(String name, ConvexDecompositionDesc desc);

	    /// <summary>
	    /// Create a new hull region by decomposing the mesh in desc. If the region
	    ///  does not exist it will be created. If it does exist it will be cleared
	    ///  and recreated.
	    /// </summary>
	    /// <param name="name">The name of the region.</param>
	    /// <param name="desc">The mesh description and algorithm configuration settings.</param>
	    /// <param name="origin">An origin for the hull region.</param>
	    /// <param name="orientation">An orientation for the hull region.</param>
        void createHullRegion(String name, ConvexDecompositionDesc desc, Vector3 origin, Quaternion orientation);

	    /// <summary>
	    /// Add a Sphere to the given region. If the region does not exist it will
        /// be created.
	    /// </summary>
	    /// <param name="sectionName">The name of the section to add the sphere to.</param>
	    /// <param name="radius">The radius of the sphere.</param>
	    /// <param name="origin">The origin of the sphere.</param>
        void addSphereShape(String regionName, float radius, Vector3 origin);

	    /// <summary>
	    /// Empty and destroy a region removing it from the collision shape.
	    /// </summary>
	    /// <param name="name">The name of the region to destroy.</param>
        void destroyRegion(String name);

	    /// <summary>
	    /// This function will recompute the mass props. It should be called when
        /// the collision shape is changed.
	    /// </summary>
        void recomputeMassProps();
    }
}
