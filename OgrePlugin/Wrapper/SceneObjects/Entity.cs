using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class Entity : MovableObject
    {
        private WrapperCollection<SubEntity> subEntities = new WrapperCollection<SubEntity>(SubEntity.createWrapper);
        private WrapperCollection<Entity> lodEntities = new WrapperCollection<Entity>(Entity.createWrapper);
        private SkeletonInstance skeleton;
        private AnimationStateSet animationStateSet = null;

        internal static Entity createWrapper(IntPtr entity, object[] args)
        {
            return new Entity(entity);
        }

        internal Entity(IntPtr entity)
            :base(entity)
        {
            if(hasSkeleton())
            {
                skeleton = new SkeletonInstance(Entity_getSkeleton(ogreObject));
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if(skeleton != null)
            {
                skeleton.Dispose();   
            }
            subEntities.Dispose();
            lodEntities.Dispose();
        }

        /// <summary>
	    /// Gets the Mesh that this Entity is based on.
	    /// </summary>
	    /// <returns>The mesh for this entity.</returns>
	    public MeshPtr getMesh()
        {
            MeshManager meshManager = MeshManager.getInstance();
            return meshManager.getObject(Entity_getMesh(ogreObject, meshManager.ProcessWrapperObjectCallback));
        }

	    /// <summary>
	    /// Gets a pointer to a SubEntity, ie a part of an Entity. 
	    /// </summary>
	    /// <param name="index">The index of the sub entity to find.</param>
	    /// <returns>The sub entity specified by index or null if it does not exist.</returns>
        public SubEntity getSubEntity(uint index)
        {
            return subEntities.getObject(Entity_getSubEntityIndex(ogreObject, index));
        }

	    /// <summary>
	    /// Gets a pointer to a SubEntity by name. 
	    /// </summary>
	    /// <param name="name">The name of the sub entity.</param>
	    /// <returns>The sub entity specified by name.</returns>
        public SubEntity getSubEntity(String name)
        {
            return subEntities.getObject(Entity_getSubEntity(ogreObject, name));
        }
    	
	    /// <summary>
	    /// Retrieves the number of SubEntity objects making up this entity. 
	    /// </summary>
	    /// <returns>The number of sub entities.</returns>
        public uint getNumSubEntities()
        {
            return Entity_getNumSubEntities(ogreObject);
        }

	    /// <summary>
	    /// Sets the material to use for the whole of this entity. 
	    /// </summary>
	    /// <param name="name">The name of the material.</param>
        public void setMaterialName(String name)
        {
            Entity_setMaterialName(ogreObject, name);
        }

	    /// <summary>
	    /// Get the specified AnimationState.
	    /// </summary>
	    /// <param name="name">The name of the AnimationState.</param>
	    /// <returns>The AnimationState specified or nullptr if it does not exist.</returns>
        public AnimationState getAnimationState(String name)
        {
            getAnimationStateSetFromOgre();
            if (animationStateSet != null)
            {
                return animationStateSet.getAnimationState(name);
            }
            return null;
        }

	    /// <summary>
	    /// Get all AnimationStates for this Entity.
	    /// </summary>
	    /// <returns>The AnimationStateSet for this Entity.</returns>
        public AnimationStateSet getAllAnimationStates()
        {
            getAnimationStateSetFromOgre();
            return animationStateSet;
        }

        private void getAnimationStateSetFromOgre()
        {
            if (animationStateSet == null)
            {
                IntPtr animationStatesFromOgre = Entity_getAllAnimationStates(ogreObject);
                if (animationStatesFromOgre != IntPtr.Zero)
                {
                    animationStateSet = new AnimationStateSet(animationStatesFromOgre);
                }
            }
        }

	    /// <summary>
	    /// Tells the Entity whether or not it should display it's skeleton, if it has one. 
	    /// </summary>
	    /// <param name="display">True to display skeleton.  False to hide it.</param>
        public void setDisplaySkeleton(bool display)
        {
            Entity_setDisplaySkeleton(ogreObject, display);
        }

	    /// <summary>
	    /// Returns whether or not the entity is currently displaying its skeleton. 
	    /// </summary>
	    /// <returns>True if the skeleton is displayed.  False if it is hidden.</returns>
        public bool getDisplaySkeleton()
        {
            return Entity_getDisplaySkeleton(ogreObject);
        }

	    /// <summary>
	    /// Gets a pointer to the entity representing the numbered manual level of detail.
	    /// The zero-based index never includes the original entity, unlike Mesh::getLodLevel.
	    /// </summary>
	    /// <param name="index">The level of detail index.</param>
	    /// <returns>The level of detail entity for index or null if it does not exist.</returns>
        public Entity getManualLodLevel(uint index)
        {
            return lodEntities.getObject(Entity_getManualLodLevel(ogreObject, index));
        }

	    /// <summary>
	    /// Returns the number of manual levels of detail that this entity supports. 
	    /// </summary>
	    /// <returns>The number of manual lod levels.</returns>
        public uint getNumManualLodLevels()
        {
            return Entity_getNumManualLodLevels(ogreObject);
        }

	    /// <summary>
	    /// Returns the current LOD used to render. 
	    /// </summary>
	    /// <returns>The current lod index.</returns>
        public ushort getCurrentLodIndex()
        {
            return Entity_getCurrentLodIndex(ogreObject);
        }

	    /// <summary>
	    /// Sets a level-of-detail bias for the mesh detail of this entity. 
	    /// Level of detail reduction is normally applied automatically based on the Mesh settings. 
	    /// However, it is possible to influence this behaviour for this entity by adjusting the LOD 
	    /// bias. This 'nudges' the mesh level of detail used for this entity up or down depending on 
	    /// your requirements. You might want to use this if there was a particularly important entity 
	    /// in your scene which you wanted to detail better than the others, such as a player model. 
	    /// 
        /// There are three parameters to this method; the first is a factor to apply; it defaults to 
	    /// 1.0 (no change), by increasing this to say 2.0, this model would take twice as long to 
	    /// reduce in detail, whilst at 0.5 this entity would use lower detail versions twice as 
	    /// quickly. The other 2 parameters are hard limits which let you set the maximum and minimum 
	    /// level-of-detail version to use, after all other calculations have been made. This lets you 
	    /// say that this entity should never be simplified, or that it can only use LODs below a 
	    /// certain level even when right next to the camera. 
	    /// </summary>
	    /// <param name="factor">Proportional factor to apply to the distance at which LOD is changed. Higher values increase the distance at which higher LODs are displayed (2.0 is twice the normal distance, 0.5 is half).</param>
	    /// <param name="maxDetailIndex">The index of the maximum LOD this entity is allowed to use (lower indexes are higher detail: index 0 is the original full detail model).</param>
	    /// <param name="minDetailIndex">The index of the minimum LOD this entity is allowed to use (higher indexes are lower detail). Use something like 99 if you want unlimited LODs (the actual LOD will be limited by the number in the Mesh).</param>
        public void setMeshLodBias(float factor, ushort maxDetailIndex, ushort minDetailIndex)
        {
            Entity_setMeshLodBias(ogreObject, factor, maxDetailIndex, minDetailIndex);
        }

	    /// <summary>
	    /// Sets a level-of-detail bias for the material detail of this entity.  See setMeshLodBias for
	    /// more info.
	    /// </summary>
	    /// <param name="factor">Proportional factor to apply to the distance at which LOD is changed. Higher values increase the distance at which higher LODs are displayed (2.0 is twice the normal distance, 0.5 is half).</param>
	    /// <param name="maxDetailIndex">The index of the maximum LOD this entity is allowed to use (lower indexes are higher detail: index 0 is the original full detail model).</param>
	    /// <param name="minDetailIndex">The index of the minimum LOD this entity is allowed to use (higher indexes are lower detail). Use something like 99 if you want unlimited LODs (the actual LOD will be limited by the number in the Mesh).</param>
        public void setMaterialLodBias(float factor, ushort maxDetailIndex, ushort minDetailIndex)
        {
            Entity_setMaterialLodBias(ogreObject, factor, maxDetailIndex, minDetailIndex);
        }

	    /// <summary>
	    /// Sets whether the polygon mode of this entire entity may be overridden by the camera detail settings.
	    /// </summary>
	    /// <param name="polygonModeOverrideable">True to allow overrides.  False to prevent them.</param>
        public void setPolygonModeOverrideable(bool polygonModeOverrideable)
        {
            Entity_setPolygonModeOverrideable(ogreObject, polygonModeOverrideable);
        }

	    //attach object to bone

	    //detach object from bone

	    //detach object from bone

	    /// <summary>
	    /// Detach all MovableObjects previously attached using attachObjectToBone. 
	    /// </summary>
        public void detachAllObjectsFromBone()
        {
            Entity_detachAllObjectsFromBone(ogreObject);
        }

	    //get attached object iterator

	    /// <summary>
	    /// Get the radius of the bounding sphere for this object.
	    /// </summary>
	    /// <returns>The radius of the bounding sphere.</returns>
        public float getBoundingRadius()
        {
            return Entity_getBoundingRadius(ogreObject);
        }

	    /// <summary>
	    /// Returns whether or not this entity is skeletally animated.
	    /// </summary>
	    /// <returns>True if the entity has a skeleton.  False if it does not.</returns>
        public bool hasSkeleton()
        {
            return Entity_hasSkeleton(ogreObject);
        }

	    /// <summary>
	    /// Get this Entity's personal skeleton instance.
	    /// </summary>
	    /// <returns>The skeleton if the entity has one or null if it does not.</returns>
        public SkeletonInstance getSkeleton()
        {
            return skeleton;
        }

	    /// <summary>
	    /// Returns whether or not hardware animation is enabled. 
	    /// </summary>
	    /// <returns>True if hardware animation is enabled.  False if it is disabled.</returns>
        public bool isHardwareAnimationEnabled()
        {
            return Entity_isHardwareAnimationEnabled(ogreObject);
        }

	    /// <summary>
	    /// Returns the number of requests that have been made for software animation.
	    /// 
	    /// If non-zero then software animation will be performed in updateAnimation regardless of the 
	    /// current setting of isHardwareAnimationEnabled or any internal optimise for eliminate 
	    /// software animation. Requests for software animation are made by calling the 
	    /// addSoftwareAnimationRequest() method.
	    /// </summary>
	    /// <returns>The number of software animation requests.</returns>
        public int getSoftwareAnimationRequests()
        {
            return Entity_getSoftwareAnimationRequests(ogreObject);
        }

	    /// <summary>
	    /// Returns the number of requests that have been made for software animation of normals.
	    /// 
	    /// If non-zero, and getSoftwareAnimationRequests() also returns non-zero, then software 
	    /// animation of normals will be performed in updateAnimation regardless of the current setting 
	    /// of isHardwareAnimationEnabled or any internal optimise for eliminate software animation. 
	    /// Currently it is not possible to force software animation of only normals. Consequently 
	    /// this value is always less than or equal to that returned by getSoftwareAnimationRequests(). 
	    /// Requests for software animation of normals are made by calling the 
	    /// addSoftwareAnimationRequest() method with 'true' as the parameter.
	    /// </summary>
	    /// <returns>The number of software normal animation requests.</returns>
        public int getSoftwareAnimationNormalsRequests()
        {
            return Entity_getSoftwareAnimationNormalsRequests(ogreObject);
        }

	    /// <summary>
	    /// Add a request for software animation. 
	    /// </summary>
	    /// <param name="normalsAlso">True to also request normals software animation.</param>
        public void addSoftwareAnimationRequest(bool normalsAlso)
        {
            Entity_addSoftwareAnimationRequest(ogreObject, normalsAlso);
        }

	    /// <summary>
	    /// Removes a request for software animation. 
	    /// </summary>
	    /// <param name="normalsAlso">True to also request normals software animation.</param>
        public void removeSoftwareAnimationRequest(bool normalsAlso)
        {
            Entity_removeSoftwareAnimationRequest(ogreObject, normalsAlso);
        }

	    /// <summary>
	    /// Shares the SkeletonInstance with the supplied entity. 
	    /// Note that in order for this to work, both entities must have the same Skeleton. 
	    /// </summary>
	    /// <param name="entity">The entity to share the skeleton with.</param>
        public void shareSkeletonInstanceWith(Entity entity)
        {
            Entity_shareSkeletonInstanceWith(ogreObject, entity.ogreObject);
        }

	    /// <summary>
	    /// Returns whether or not this entity is either morph or pose animated. 
	    /// </summary>
	    /// <returns>True if vertex animation is being used.  False if it is not.</returns>
        public bool hasVertexAnimation()
        {
            return Entity_hasVertexAnimation(ogreObject);
        }

	    /// <summary>
	    /// Stops sharing the SkeletonInstance with other entities. 
	    /// </summary>
        public void stopSharingSkeletonInstance()
        {
            Entity_stopSharingSkeletonInstance(ogreObject);
        }

	    /// <summary>
	    /// Returns whether this entity shares it's SkeltonInstance with other entity instances. 
	    /// </summary>
	    /// <returns>True if skeleton is being shared.  False if it is not.</returns>
        public bool sharesSkeletonInstance()
        {
            return Entity_sharesSkeletonInstance(ogreObject);
        }

	    /// <summary>
	    /// Updates the internal animation state set to include the latest available animations from 
	    /// the attached skeleton.
	    /// 
	    /// Use this method if you manually add animations to a skeleton, or have linked the skeleton 
	    /// to another for animation purposes since creating this entity.
	    /// 
	    /// If you have called getAnimationState prior to calling this method, the pointers will still 
	    /// remain valid.
	    /// </summary>
        public void refreshAvailableAnimationState()
        {
            Entity_refreshAvailableAnimationState(ogreObject);
        }

	    /// <summary>
	    /// Has this Entity been initialised yet? 
	    /// 
	    /// If this returns false, it means this Entity hasn't been completely constructed yet from 
	    /// the underlying resources (Mesh, Skeleton), which probably means they were delay-loaded and 
	    /// aren't available yet. This Entity won't render until it has been successfully initialised, 
	    /// nor will many of the manipulation methods function. 
	    /// </summary>
	    /// <returns>True if the entity is initalized.  False if it is not.</returns>
        public bool isInitialzed()
        {
            return Entity_isInitialzed(ogreObject);
        }

	    /// <summary>
	    /// Sets whether or not this object will cast shadows. 
	    /// </summary>
	    /// <param name="castShadows">True to cast shadows.  False to disable shadow casting.</param>
        public void setCastShadows(bool castShadows)
        {
            Entity_setCastShadows(ogreObject, castShadows);
        }

	    /// <summary>
	    /// Returns whether shadow casting is enabled for this object. 
	    /// </summary>
	    /// <returns>True if shadow casting is enabled.  False if shadow casting is disabled.</returns>
        public bool getCastShadows()
        {
            return Entity_getCastShadows(ogreObject);
        }

	    /// <summary>
	    /// Check the mesh to see if ray intersects any polygons within the mesh.  Returns true
	    /// if an intersection occured.  False if no intersection.  The position of the intersection
	    /// will be stored in result.  This function is adapted from the function found at
	    /// http://www.ogre3d.org/wiki/index.php/Raycasting_to_the_polygon_level written by gerds.
	    /// </summary>
	    /// <param name="ray">The ray to test in world coords.</param>
	    /// <param name="distanceOnRay">Output - The distance along the ray where the triangle was intersected.  Only valid if true is returned.</param>
	    /// <returns>True if an intersection occurred, false if it did not.</returns>
        public bool raycastPolygonLevel(Ray3 ray, ref float distanceOnRay)
        {
            return Entity_raycastPolygonLevel(ogreObject, ray, ref distanceOnRay);
        }

        /// <summary>
        /// Get the AABB for this movable object.
        /// </summary>
        /// <returns>The AABB for this object.</returns>
        public AxisAlignedBox getChildObjectsBoundingBox()
        {
            return Entity_getChildObjectsBoundingBox(ogreObject);
        }

        #region NativeWrapper

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Entity_getMesh(IntPtr entity, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Entity_getSubEntityIndex(IntPtr entity, uint index);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Entity_getSubEntity(IntPtr entity, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint Entity_getNumSubEntities(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_setMaterialName(IntPtr entity, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Entity_getAnimationState(IntPtr entity, String name);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Entity_getAllAnimationStates(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_setDisplaySkeleton(IntPtr entity, bool display);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Entity_getDisplaySkeleton(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Entity_getManualLodLevel(IntPtr entity, uint index);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint Entity_getNumManualLodLevels(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort Entity_getCurrentLodIndex(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_setMeshLodBias(IntPtr entity, float factor, ushort maxDetailIndex, ushort minDetailIndex);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_setMaterialLodBias(IntPtr entity, float factor, ushort maxDetailIndex, ushort minDetailIndex);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_setPolygonModeOverrideable(IntPtr entity, bool polygonModeOverrideable);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_detachAllObjectsFromBone(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float Entity_getBoundingRadius(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Entity_hasSkeleton(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Entity_getSkeleton(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Entity_isHardwareAnimationEnabled(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int Entity_getSoftwareAnimationRequests(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern int Entity_getSoftwareAnimationNormalsRequests(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_addSoftwareAnimationRequest(IntPtr entity, bool normalsAlso);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_removeSoftwareAnimationRequest(IntPtr entity, bool normalsAlso);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_shareSkeletonInstanceWith(IntPtr entity, IntPtr shareEntity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Entity_hasVertexAnimation(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_stopSharingSkeletonInstance(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Entity_sharesSkeletonInstance(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_refreshAvailableAnimationState(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Entity_isInitialzed(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Entity_setCastShadows(IntPtr entity, bool castShadows);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Entity_getCastShadows(IntPtr entity);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Entity_raycastPolygonLevel(IntPtr entity, Ray3 ray, ref float distanceOnRay);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern AxisAlignedBox Entity_getChildObjectsBoundingBox(IntPtr entity);

        #endregion
    }
}
