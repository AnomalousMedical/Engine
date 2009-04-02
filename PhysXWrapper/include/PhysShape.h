#pragma once

#include "Enums.h"
#include "vcclr.h"
#include "AutoPtr.h"

class NxShape;

namespace PhysXWrapper{

ref class PhysActor;
ref class PhysRaycastHit;
ref class PhysShape;

typedef gcroot<PhysShape^> PhysShapeGCRoot;

/// <summary>
/// Abstract base class for the various collision shapes.
/// <para>
/// An instance of a subclass can be created by calling the createShape() method
/// of the PhysActor class, or by adding the shape descriptors into the
/// PhysActorDesc class before creating the actor.
/// </para>
/// </summary>
public ref class PhysShape abstract
{
private:
	NxShape* nxShape;
	AutoPtr<PhysShapeGCRoot> shapeRoot;

internal:
	/// <summary>
	/// Returns the native NxShape
	/// </summary>
	NxShape* getNxShape();
public:
	/// <summary>
	/// Constructor
	/// </summary>
	PhysShape(NxShape* nxShape);

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysShape();

	/// <summary>
	/// Retrieves the actor which this shape is associated with.
	/// </summary>
	/// <returns>The actor this shape is associated with.</returns>
	PhysActor^ getActor();

	/// <summary>
	/// Sets which collision group this shape is part of. 
	/// <para>
	/// Default group is 0. Maximum possible group is 31. Collision groups are
    /// sets of shapes which may or may not be set to collision detect with each
    /// other; this can be set using NxScene::setGroupCollisionFlag()
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// </summary>
	/// <param name="group">The group to set the collision shape as a part of.</param>
	void setGroup(unsigned short group);

	/// <summary>
	/// Retrieves the value set with setGroup(). NxCollisionGroup is an integer
    /// between 0 and 31.
	/// </summary>
	/// <returns>The collision group this shape belongs to.</returns>
	unsigned short getGroup();

	/// <summary>
	/// Sets shape flags. 
	/// <para>
	/// The shape may be turned into a trigger by setting one or more of the
    /// above TriggerFlag-s to true. A trigger shape will not collide with other
    /// shapes. Instead, if a shape enters the trigger's volume, a trigger event
    /// will be sent to the user via the NxUserTriggerReport::onTrigger method.
    /// You can set a NxUserTriggerReport object with
    /// NxScene::setUserTriggerReport().
	/// </para>
	/// <para>
	/// Since version 2.1.1 this is also used to setup generic (non-trigger)
    /// flags.
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// </summary>
	/// <param name="flag">The new shape flags to set for this shape. See NxShapeFlag.</param>
	/// <param name="value">True to set the flags. False to clear the flags specified in flag.</param>
	void setFlag(ShapeFlag flag, bool value);

	/// <summary>
	/// Retrieves shape flags.
	/// </summary>
	/// <param name="flag">The flag to retrieve.</param>
	/// <returns>The value of the flag specified.</returns>
	bool getFlag(ShapeFlag flag);

	/// <summary>
	/// Assigns a material index to the shape. 
	/// <para>
	/// The material index can be retrieved by calling
    /// NxMaterial::getMaterialIndex(). If the material index is invalid, it
    /// will still be recorded, but the default material (at index 0) will
    /// effectively be used for simulation.
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// </summary>
	/// <param name="materialIndex">The material index to assign to the shape. See NxMaterial.</param>
	void setMaterial(unsigned short materialIndex);

	/// <summary>
	/// Retrieves the material index currently assigned to the shape.
	/// </summary>
	/// <returns>The material index of the material associated with the shape.</returns>
	unsigned short getMaterial();

	/// <summary>
	/// Sets the skin width. See NxShapeDesc::skinWidth.
	/// </summary>
	/// <param name="skinWidth">The new skin width. Range: (0,inf)</param>
	void setSkinWidth(float skinWidth);

	/// <summary>
	/// Retrieves the skin width. See NxShapeDesc::skinWidth.
	/// </summary>
	/// <returns>The skin width of the shape.</returns>
	float getSkinWidth();

	/// <summary>
	/// The setLocal*() methods set the pose of the shape in actor space, i.e.
    /// relative to the actor they are owned by. 
	/// <para>
	/// This transformation is identity by default.
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// <para>
	/// Note: Does not automatically update the inertia properties of the owning
    /// actor (if applicable); use NxActor::updateMassFromShapes() to do this. 
	/// </para>
	/// </summary>
	/// <param name="trans">The new position of the shape relative to the actor frame. Range: position vector</param>
	void setLocalPosition(EngineMath::Vector3 trans);

	/// <summary>
	/// The setLocal*() methods set the pose of the shape in actor space, i.e.
    /// relative to the actor they are owned by. 
	/// <para>
	/// This transformation is identity by default.
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// <para>
	/// Note: Does not automatically update the inertia properties of the owning
    /// actor (if applicable); use NxActor::updateMassFromShapes() to do this. 
	/// </para>
	/// </summary>
	/// <param name="trans">The new position of the shape relative to the actor frame. Range: position vector</param>
	void setLocalPosition(EngineMath::Vector3% trans);

	/// <summary>
	/// The setLocal*() methods set the pose of the shape in actor space, i.e.
    /// relative to the actor they are owned by. 
	/// <para>
	/// This transformation is identity by default.
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// <para>
	/// Note: Does not automatically update the inertia properties of the owning
    /// actor (if applicable); use NxActor::updateMassFromShapes() to do this. 
	/// </para>
	/// </summary>
	/// <param name="rot">The new orientation relative to the actor frame.</param>
	void setLocalOrientation(EngineMath::Quaternion rot);

	/// <summary>
	/// The setLocal*() methods set the pose of the shape in actor space, i.e.
    /// relative to the actor they are owned by. 
	/// <para>
	/// This transformation is identity by default.
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// <para>
	/// Note: Does not automatically update the inertia properties of the owning
    /// actor (if applicable); use NxActor::updateMassFromShapes() to do this. 
	/// </para>
	/// </summary>
	/// <param name="rot">The new orientation relative to the actor frame.</param>
	void setLocalOrientation(EngineMath::Quaternion% rot);

	/// <summary>
	/// The getLocal*() methods retrieve the pose of the shape in actor space,
    /// i.e. relative to the actor they are owned by. This transformation is
    /// identity by default.
	/// </summary>
	/// <returns>Position of shape relative to actors frame.</returns>
	EngineMath::Vector3 getLocalPosition();

	/// <summary>
	/// The getLocal*() methods retrieve the pose of the shape in actor space,
    /// i.e. relative to the actor they are owned by. This transformation is
    /// identity by default.
	/// </summary>
	/// <returns>Orientation of shape relative to actors frame.</returns>
	EngineMath::Quaternion getLocalOrientation();

	/// <summary>
	/// The setGlobal() calls are convenience methods which transform the passed
    /// parameter into the current local space of the actor and then call
    /// setLocalPose(). 
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// <para>
	/// Note: Does not automatically update the inertia properties of the owning
    /// actor (if applicable); use NxActor::updateMassFromShapes() to do this. 
	/// </para>
	/// </summary>
	/// <param name="trans">The new shape position, relative to the global frame. Range: position vector</param>
	void setGlobalPosition(EngineMath::Vector3 trans);

	/// <summary>
	/// The setGlobal() calls are convenience methods which transform the passed
    /// parameter into the current local space of the actor and then call
    /// setLocalPose(). 
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// <para>
	/// Note: Does not automatically update the inertia properties of the owning
    /// actor (if applicable); use NxActor::updateMassFromShapes() to do this. 
	/// </para>
	/// </summary>
	/// <param name="trans">The new shape position, relative to the global frame. Range: position vector</param>
	void setGlobalPosition(EngineMath::Vector3% trans);

	/// <summary>
	/// The setGlobal() calls are convenience methods which transform the passed
    /// parameter into the current local space of the actor and then call
    /// setLocalPose(). 
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// <para>
	/// Note: Does not automatically update the inertia properties of the owning
    /// actor (if applicable); use NxActor::updateMassFromShapes() to do this. 
	/// </para>
	/// </summary>
	/// <param name="rot">The new shape orientation relative to the global frame.</param>
	void setGlobalOrientation(EngineMath::Quaternion rot);

	/// <summary>
	/// The setGlobal() calls are convenience methods which transform the passed
    /// parameter into the current local space of the actor and then call
    /// setLocalPose(). 
	/// <para>
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </para>
	/// <para>
	/// Note: Does not automatically update the inertia properties of the owning
    /// actor (if applicable); use NxActor::updateMassFromShapes() to do this. 
	/// </para>
	/// </summary>
	/// <param name="rot">The new shape orientation relative to the global frame.</param>
	void setGlobalOrientation(EngineMath::Quaternion% rot);

	/// <summary>
	/// The getGlobal*() methods retrieve the shape's current world space pose.
    /// This is the local pose multiplied by the actor's current global pose. 
	/// </summary>
	/// <returns>Pose of shape relative to the global frame.</returns>
	EngineMath::Vector3 getGlobalPosition();

	/// <summary>
	/// The getGlobal*() methods retrieve the shape's current world space pose.
    /// This is the local pose multiplied by the actor's current global pose. 
	/// </summary>
	/// <returns>Orientation of the shape realative to the global frame.</returns>
	EngineMath::Quaternion getGlobalOrientation();

	/// <summary>
	/// casts a world-space ray against the shape. 
	/// <para>
	/// maxDist is the maximum allowed distance for the ray. You can use this
    /// for segment queries. hintFlags is a combination of NxRaycastBit flags.
    /// firstHit is a hint saying you're only interested in a boolean answer.
	/// </para>
	/// <para>
	/// Note: Make certain that the direction vector of NxRay is normalized.
	/// </para>
	/// </summary>
	/// <param name="worldRay">The ray to intersect against the shape in the global frame. Range See NxRay </param>
	/// <param name="maxDist">The maximum distance to check along the ray. Range: (0,inf) </param>
	/// <param name="hint">a combination of NxRaycastBit flags. Specifies which members of NxRaycastHit the user is interested in(eg normal, material etc) </param>
	/// <param name="hit">Retrieves the information computed from a ray intersection.</param>
	/// <param name="firstHit">is a hint saying you're only interested in a boolean answer.</param>
	/// <returns>True if the ray intersects the shape.</returns>
	bool raycast(EngineMath::Ray3 worldRay, float maxDist, RaycastBit hint, PhysRaycastHit^ hit, bool firstHit);

	/// <summary>
	/// casts a world-space ray against the shape. 
	/// <para>
	/// maxDist is the maximum allowed distance for the ray. You can use this
    /// for segment queries. hintFlags is a combination of NxRaycastBit flags.
    /// firstHit is a hint saying you're only interested in a boolean answer.
	/// </para>
	/// <para>
	/// Note: Make certain that the direction vector of NxRay is normalized.
	/// </para>
	/// </summary>
	/// <param name="worldRay">The ray to intersect against the shape in the global frame. Range See NxRay </param>
	/// <param name="maxDist">The maximum distance to check along the ray. Range: (0,inf) </param>
	/// <param name="hint">a combination of NxRaycastBit flags. Specifies which members of NxRaycastHit the user is interested in(eg normal, material etc) </param>
	/// <param name="hit">Retrieves the information computed from a ray intersection.</param>
	/// <param name="firstHit">is a hint saying you're only interested in a boolean answer.</param>
	/// <returns>True if the ray intersects the shape.</returns>
	bool raycast(EngineMath::Ray3% worldRay, float maxDist, RaycastBit hint, PhysRaycastHit^ hit, bool firstHit);
};

}

/*******
* Not implemented
getType()
setCCDSkeleton()
getCCDSkeleton()
setName()
getName()
setGroupsMask()
getGroupsMask()
getNonInteractingCompartmentTypes()
setNonInteractingCompartmentTypes()
*/
