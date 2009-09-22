/// <file>MovableObject.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

namespace Ogre
{
	class MovableObject;
}

namespace OgreWrapper
{

ref class AxisAlignedBox;
ref class SceneNode;

/// <summary>
/// Enum of possible movement types.
/// </summary>
public enum class MovableTypes : unsigned int
{
	Entity,
	Light,
	Camera,
	ManualObject,
	BillboardChain,
	RibbonTrail,
	BillboardSet,
	Frustrum,
	BatchInstance,
	MovablePlane,
	ParticleSystem,
	SimpleRenderable,
	Other
};

/// <summary>
/// This abstract class allows SceneNodes to have subclasses attached to them.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class MovableObject abstract
{
private:
	Ogre::MovableObject* movableObject; //Managed by subclasses.

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="movableObject">The movable object to wrap.</param>
	MovableObject(Ogre::MovableObject* movableObject);

	/// <summary>
	/// Returns the underlying native moveable object.
	/// </summary>
	/// <returns>The underlying native moveable object.</returns>
	Ogre::MovableObject* getMovableObject();

public:

	/// <summary>
	/// Returns true if this object is attached to a SceneNode or TagPoint. 
	/// </summary>
	/// <returns>Returns true if this object is attached to a SceneNode or TagPoint.</returns>
	bool isAttached();

	/// <summary>
	/// Detaches an object from a parent SceneNode or TagPoint, if attached.
	/// </summary>
	void detatchFromParent();

	/// <summary>
	/// Returns true if this object is attached to a SceneNode or TagPoint, and
    /// this SceneNode / TagPoint is currently in an active part of the scene
    /// graph.
	/// </summary>
	/// <returns>True if the above conditions are met.</returns>
	bool isInScene();

	/// <summary>
	/// Get the instance name of the render node object.
	/// </summary>
	/// <returns>The name of the render node object.</returns>
	virtual System::String^ getName() = 0;

	/// <summary>
	/// Set the object visibility.
	/// </summary>
	/// <param name="visible">True for visible false for invisible.</param>
	void setVisible(bool visible);

	/// <summary>
	/// Determines if the object is visible.
	/// </summary>
	/// <returns>True for visible false for invisible.</returns>
	bool isVisible();

	/// <summary>
	/// Sets the visiblity flags for this object. As well as a simple true/false value for 
	/// visibility (as seen in setVisible), you can also set visiblity flags which when 'and'ed 
	/// with the SceneManager's visibility mask can also make an object invisible.
	/// </summary>
	/// <param name="flags">The visibilty flags.</param>
	void setVisibilityFlags(unsigned int flags);

	/// <summary>
	/// As setVisibilityFlags, except the flags passed as parameters are appended to the 
	/// existing flags on this object.
	/// </summary>
	/// <param name="flags">The visibilty flags.</param>
	void addVisiblityFlags(unsigned int flags);

	/// <summary>
	/// As setVisibilityFlags, except the flags passed as parameters are removed from the 
	/// existing flags on this object.
	/// </summary>
	/// <param name="flags">The visibilty flags.</param>
	void removeVisibilityFlags(unsigned int flags);

	/// <summary>
	/// Returns the visibility flags relevant for this object.
	/// </summary>
	/// <returns>The visibility flags.</returns>
	unsigned int getVisibilityFlags();

	/// <summary>
	/// Get the movable type of this object.
	/// </summary>
	/// <returns>The movable type of this object.</returns>
	MovableTypes getMovableType();

	/// <summary>
	/// Get a string with the movable type of this object.  This is useful if you get
	/// MovableTypes::Other as a result from getMovableType().  Ogre uses strings internally
	/// but the enum is more reliable in general.
	/// </summary>
	/// <returns>The movable type of this object as a string.</returns>
	System::String^ getOgreMovableType();

	/// <summary>
	/// Get an axis aligned bounding box for this movable object.
	/// </summary>
	/// <returns>The AABB for this object.</returns>
	AxisAlignedBox^ getBoundingBox();

	/// <summary>
	/// Sets whether or not the debug display of this object is enabled. 
	/// </summary>
	/// <param name="enabled">True to enable debug rendering.  False to disable.</param>
	void setDebugDisplayEnabled(bool enabled);

	/// <summary>
	/// Gets whether debug display of this object is enabled. 
	/// </summary>
	/// <returns>True if debug rendering is enabled.</returns>
	bool isDebugDisplayEnabled();

	/// <summary>
	/// Sets the render queue group this entity will be rendered through. 
	/// </summary>
	/// <param name="queueID">The queue id to add this object to.</param>
	void setRenderQueueGroup(unsigned char queueID);

	/// <summary>
	/// Gets the queue group for this entity.
	/// </summary>
	/// <returns>The render queue group of this object.</returns>
	unsigned char getRenderQueueGroup();

	SceneNode^ getParentSceneNode();
};

}