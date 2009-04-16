/// <file>RenderEntity.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "MovableObject.h"
#include "gcroot.h"
#include "VoidUserDefinedObject.h"
#include "AutoPtr.h"
#include "RenderEntityCollection.h"
#include "RenderSubEntityCollection.h"

namespace Ogre
{
	class Entity;
	class SubEntity;
}

namespace Rendering{

ref class AnimationState;
ref class AnimationStateSet;
ref class RenderSubEntity;
ref class SkeletonInstance;
ref class RenderEntity;
ref class MeshPtr;

ref class RenderEntity;

typedef gcroot<RenderEntity^> RenderEntityGCRoot;

/// <summary>
/// This class wraps a native rendering entity, such as a 3d mesh from a file
/// or a user manipulated 3d object.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
[Engine::Attributes::DoNotSaveAttribute]
public ref class RenderEntity : MovableObject
{
private:
	Ogre::Entity* entity;
	System::String^ name;
	System::String^ meshName;
	RenderSubEntityCollection subEntities;
	RenderEntityCollection lodEntities;
	SkeletonInstance^ skeleton;
	AnimationStateSet^ animationStateSet;
	AutoPtr<RenderEntityGCRoot> root;
	AutoPtr<VoidUserDefinedObject> userDefinedObj;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="entity">The entity to wrap.</param>
	/// <param name="name">The name of the RenderEntity.</param>
	RenderEntity(Ogre::Entity* entity, System::String^ name, System::String^ meshName);

	/// <summary>
	/// Get the mesh information for the mesh owned by this entity.
	/// </summary>
	/// <param name="vertex_count">Returns the number of vertices in the mesh.</param>
	/// <param name="vertices">Returns the vertices in the mesh.</param>
	/// <param name="index_count">Returns the number of indicies in the mesh.</param>
	/// <param name="indices">Returns the indicies of the mesh.</param>
	/// <param name="position">The world position of the mesh.</param>
	/// <param name="orient">The world orientation of the mesh.</param>
	/// <param name="scale">The world scale of the mesh.</param>
	void getMeshInformation(size_t &vertex_count, Ogre::Vector3* &vertices, size_t &index_count,
                            unsigned long* &indices, const Ogre::Vector3 &position,
                            const Ogre::Quaternion &orient, const Ogre::Vector3 &scale);

	/// <summary>
	/// Gets the underlying native entity.
	/// </summary>
	/// <returns>The underlying native entity.</returns>
	Ogre::Entity* getEntity();

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~RenderEntity();

	/// <summary>
	/// Gets the name of the entity.
	/// </summary>
	/// <returns>The name of the entity.</returns>
	virtual System::String^ getName() override;

	/// <summary>
	/// Gets the name of the mesh on this entity.
	/// </summary>
	/// <returns>The name of the entity's mesh.</returns>
	System::String^ getMeshName();

	/// <summary>
	/// Gets the Mesh that this Entity is based on.
	/// </summary>
	/// <returns>The mesh for this entity.</returns>
	MeshPtr^ getMesh();

	/// <summary>
	/// Gets a pointer to a SubEntity, ie a part of an Entity. 
	/// </summary>
	/// <param name="index">The index of the sub entity to find.</param>
	/// <returns>The sub entity specified by index or null if it does not exist.</returns>
	RenderSubEntity^ getSubEntity(unsigned int index);

	/// <summary>
	/// Gets a pointer to a SubEntity by name. 
	/// </summary>
	/// <param name="name">The name of the sub entity.</param>
	/// <returns>The sub entity specified by name.</returns>
	RenderSubEntity^ getSubEntity(System::String^ name);
	
	/// <summary>
	/// Retrieves the number of SubEntity objects making up this entity. 
	/// </summary>
	/// <returns>The number of sub entities.</returns>
	unsigned int getNumSubEntities();

	/// <summary>
	/// Sets the material to use for the whole of this entity. 
	/// </summary>
	/// <param name="name">The name of the material.</param>
	void setMaterialName(System::String^ name);

	/// <summary>
	/// Get the specified AnimationState.
	/// </summary>
	/// <param name="name">The name of the AnimationState.</param>
	/// <returns>The AnimationState specified or nullptr if it does not exist.</returns>
	AnimationState^ getAnimationState(System::String^ name);

	/// <summary>
	/// Get all AnimationStates for this RenderEntity.
	/// </summary>
	/// <returns>The AnimationStateSet for this RenderEntity.</returns>
	AnimationStateSet^ getAllAnimationStates();

	/// <summary>
	/// Tells the Entity whether or not it should display it's skeleton, if it has one. 
	/// </summary>
	/// <param name="display">True to display skeleton.  False to hide it.</param>
	void setDisplaySkeleton(bool display);

	/// <summary>
	/// Returns whether or not the entity is currently displaying its skeleton. 
	/// </summary>
	/// <returns>True if the skeleton is displayed.  False if it is hidden.</returns>
	bool getDisplaySkeleton();

	/// <summary>
	/// Gets a pointer to the entity representing the numbered manual level of detail.
	/// The zero-based index never includes the original entity, unlike Mesh::getLodLevel.
	/// </summary>
	/// <param name="index">The level of detail index.</param>
	/// <returns>The level of detail entity for index or null if it does not exist.</returns>
	RenderEntity^ getManualLodLevel(unsigned int index);

	/// <summary>
	/// Returns the number of manual levels of detail that this entity supports. 
	/// </summary>
	/// <returns>The number of manual lod levels.</returns>
	unsigned int getNumManualLodLevels();

	/// <summary>
	/// Returns the current LOD used to render. 
	/// </summary>
	/// <returns>The current lod index.</returns>
	unsigned short getCurrentLodIndex();

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
	void setMeshLodBias(float factor, unsigned short maxDetailIndex, unsigned short minDetailIndex);

	/// <summary>
	/// Sets a level-of-detail bias for the material detail of this entity.  See setMeshLodBias for
	/// more info.
	/// </summary>
	/// <param name="factor">Proportional factor to apply to the distance at which LOD is changed. Higher values increase the distance at which higher LODs are displayed (2.0 is twice the normal distance, 0.5 is half).</param>
	/// <param name="maxDetailIndex">The index of the maximum LOD this entity is allowed to use (lower indexes are higher detail: index 0 is the original full detail model).</param>
	/// <param name="minDetailIndex">The index of the minimum LOD this entity is allowed to use (higher indexes are lower detail). Use something like 99 if you want unlimited LODs (the actual LOD will be limited by the number in the Mesh).</param>
	void setMaterialLodBias(float factor, unsigned short maxDetailIndex, unsigned short minDetailIndex);

	/// <summary>
	/// Sets whether the polygon mode of this entire entity may be overridden by the camera detail settings.
	/// </summary>
	/// <param name="polygonModeOverrideable">True to allow overrides.  False to prevent them.</param>
	void setPolygonModeOverrideable(bool polygonModeOverrideable);

	//attach object to bone

	//detach object from bone

	//detach object from bone

	/// <summary>
	/// Detach all MovableObjects previously attached using attachObjectToBone. 
	/// </summary>
	void detachAllObjectsFromBone();

	//get attached object iterator

	/// <summary>
	/// Get the radius of the bounding sphere for this object.
	/// </summary>
	/// <returns>The radius of the bounding sphere.</returns>
	float getBoundingRadius();

	/// <summary>
	/// Returns whether or not this entity is skeletally animated.
	/// </summary>
	/// <returns>True if the entity has a skeleton.  False if it does not.</returns>
	bool hasSkeleton();

	/// <summary>
	/// Get this Entity's personal skeleton instance.
	/// </summary>
	/// <returns>The skeleton if the entity has one or null if it does not.</returns>
	SkeletonInstance^ getSkeleton();

	/// <summary>
	/// Returns whether or not hardware animation is enabled. 
	/// </summary>
	/// <returns>True if hardware animation is enabled.  False if it is disabled.</returns>
	bool isHardwareAnimationEnabled();

	/// <summary>
	/// Returns the number of requests that have been made for software animation.
	/// 
	/// If non-zero then software animation will be performed in updateAnimation regardless of the 
	/// current setting of isHardwareAnimationEnabled or any internal optimise for eliminate 
	/// software animation. Requests for software animation are made by calling the 
	/// addSoftwareAnimationRequest() method.
	/// </summary>
	/// <returns>The number of software animation requests.</returns>
	int getSoftwareAnimationRequests();

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
	int getSoftwareAnimationNormalsRequests();

	/// <summary>
	/// Add a request for software animation. 
	/// </summary>
	/// <param name="normalsAlso">True to also request normals software animation.</param>
	void addSoftwareAnimationRequest(bool normalsAlso);

	/// <summary>
	/// Removes a request for software animation. 
	/// </summary>
	/// <param name="normalsAlso">True to also request normals software animation.</param>
	void removeSoftwareAnimationRequest(bool normalsAlso);

	/// <summary>
	/// Shares the SkeletonInstance with the supplied entity. 
	/// Note that in order for this to work, both entities must have the same Skeleton. 
	/// </summary>
	/// <param name="entity">The entity to share the skeleton with.</param>
	void shareSkeletonInstanceWith(RenderEntity^ entity);

	/// <summary>
	/// Returns whether or not this entity is either morph or pose animated. 
	/// </summary>
	/// <returns>True if vertex animation is being used.  False if it is not.</returns>
	bool hasVertexAnimation();

	/// <summary>
	/// Stops sharing the SkeletonInstance with other entities. 
	/// </summary>
	void stopSharingSkeletonInstance();

	/// <summary>
	/// Returns whether this entity shares it's SkeltonInstance with other entity instances. 
	/// </summary>
	/// <returns>True if skeleton is being shared.  False if it is not.</returns>
	bool sharesSkeletonInstance();

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
	void refreshAvailableAnimationState();

	/// <summary>
	/// Has this Entity been initialised yet? 
	/// 
	/// If this returns false, it means this Entity hasn't been completely constructed yet from 
	/// the underlying resources (Mesh, Skeleton), which probably means they were delay-loaded and 
	/// aren't available yet. This Entity won't render until it has been successfully initialised, 
	/// nor will many of the manipulation methods function. 
	/// </summary>
	/// <returns>True if the entity is initalized.  False if it is not.</returns>
	bool isInitialzed();

	/// <summary>
	/// Sets whether or not this object will cast shadows. 
	/// </summary>
	/// <param name="castShadows">True to cast shadows.  False to disable shadow casting.</param>
	void setCastShadows(bool castShadows);

	/// <summary>
	/// Returns whether shadow casting is enabled for this object. 
	/// </summary>
	/// <returns>True if shadow casting is enabled.  False if shadow casting is disabled.</returns>
	bool getCastShadows();

	/// <summary>
	/// Check the mesh to see if ray intersects any polygons within the mesh.  Returns true
	/// if an intersection occured.  False if no intersection.  The position of the intersection
	/// will be stored in result.  This function is adapted from the function found at
	/// http://www.ogre3d.org/wiki/index.php/Raycasting_to_the_polygon_level written by gerds.
	/// </summary>
	/// <param name="ray">The ray to test in world coords.</param>
	/// <param name="distanceOnRay">Output - The distance along the ray where the triangle was intersected.  Only valid if true is returned.</param>
	/// <returns>True if an intersection occured, false if it did not.</returns>
	bool raycastPolygonLevel(EngineMath::Ray3 ray, float% distanceOnRay);
};

}