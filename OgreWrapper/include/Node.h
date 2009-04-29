#pragma once

#include "Enums.h"

namespace Ogre
{
	class Node;
}

namespace OgreWrapper
{

/// <summary>
/// Class representing a general-purpose node an articulated scene graph.
/// <para>
/// A node in the scene graph is a node in a structured tree. A node contains
/// information about the transformation which will apply to it and all of it's
/// children. Child nodes can have transforms of their own, which are combined
/// with their parent's transformations. 
/// </para>
/// <para>
/// This is an abstract class - concrete classes are based on this for specific
/// purposes, e.g. SceneNode, Bone 
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class Node abstract
{
public:
[Engine::Attributes::SingleEnum]
enum class TransformSpace : unsigned int
{
	TS_LOCAL,
	TS_PARENT,
	TS_WORLD
};

private:
	Ogre::Node* ogreNode;

internal:
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ogreNode">The Ogre::Node to wrap.</param>
	Node(Ogre::Node* ogreNode);


	/// <summary>
	/// Get the wrapped Ogre::Node.
	/// </summary>
	/// <returns>The wrapped Ogre::Node.</returns>
	Ogre::Node* getOgreNode();

public:
/// <summary>
	/// Returns a quaternion representing the nodes orientation. 
	/// </summary>
	/// <returns>A quaternion.</returns>
	Engine::Quaternion getOrientation();

	/// <summary>
	/// Sets the orientation of this node via a quaternion. 
	/// 
	/// Orientations, unlike other transforms, are not always inherited by child
	/// nodes. Whether or not orientations affect the orientation of the child
    /// nodes depends on the setInheritOrientation option of the child. In some
	/// cases you want a orientating of a parent node to apply to a child node
	/// (e.g. where the child node is a part of the same object, so you want it
	/// to be the same relative orientation based on the parent's orientation),
	/// but not in other cases (e.g. where the child node is just for
    /// positioning another object, you want it to maintain it's own
	/// orientation). The default is to inherit as with other transforms. 
	/// 
    /// Note that rotations are oriented around the node's origin. 
	/// </summary>
	/// <param name="q">The orientation to set.</param>
	void setOrientation(Engine::Quaternion q);

	/// <summary>
	/// Sets the orientation of this node via quaternion parameters. 
	/// </summary>
	/// <param name="x">x</param>
	/// <param name="y">y</param>
	/// <param name="z">z</param>
	/// <param name="w">w</param>
	void setOrientation(float x, float y, float z, float w);

	/// <summary>
	/// Resets the nodes orientation (local axes as world axes, no rotation). 
	/// </summary>
	void resetOrientation();

	/// <summary>
	/// Sets the position of the node relative to it's parent. 
	/// </summary>
	/// <param name="pos">The position to set.</param>
	void setPosition(Engine::Vector3 pos);

	/// <summary>
	/// Gets the position of the node relative to it's parent.
	/// </summary>
	/// <returns>A Vector3.</returns>
	Engine::Vector3 getPosition();

	/// <summary>
	/// Sets the scaling factor applied to this node.
	///
	/// Scaling factors, unlike other transforms, are not always inherited by
	/// child nodes. Whether or not scalings affect the size of the child nodes
    /// depends on the setInheritScale option of the child. In some cases you
	/// want a scaling factor of a parent node to apply to a child node (e.g.
	/// where the child node is a part of the same object, so you want it to be
	/// the same relative size based on the parent's size), but not in other
    /// cases (e.g. where the child node is just for positioning another object,
	/// you want it to maintain it's own size). The default is to inherit as
	/// with other transforms. 
	/// 
    /// Note that like rotations, scalings are oriented around the node's
    /// origin. 
	/// </summary>
	/// <param name="scale">The scale to set.</param>
	void setScale(Engine::Vector3 scale);

	/// <summary>
	/// Gets the scaling factor of this node. 
	/// </summary>
	/// <returns>The scaling factor.</returns>
	Engine::Vector3 getScale();

	/// <summary>
	/// Tells the node whether it should inherit orientation from it's parent
    /// node.
	/// 
	/// Orientations, unlike other transforms, are not always inherited by child
	/// nodes. Whether or not orientations affect the orientation of the child
    /// nodes depends on the setInheritOrientation option of the child. In some
	/// cases you want a orientating of a parent node to apply to a child node
	/// (e.g. where the child node is a part of the same object, so you want it
	/// to be the same relative orientation based on the parent's orientation),
	/// but not in other cases (e.g. where the child node is just for
    /// positioning another object, you want it to maintain it's own
	/// orientation). The default is to inherit as with other transforms.
	/// </summary>
	/// <param name="inherit">If true, this node's orientation will be affected by its parent's orientation. If false, it will not be affected.</param>
	void setInheritOrientation(bool inherit);

	/// <summary>
	/// Determine if this node inherits its orientation.
	/// </summary>
	/// <returns>Returns true if this node is affected by orientation applied to the parent node.</returns>
	bool getInheritOrientation();

	/// <summary>
	/// Tells the node whether it should inherit scaling factors from it's
    /// parent node.
	/// 
	/// Scaling factors, unlike other transforms, are not always inherited by
	/// child nodes. Whether or not scalings affect the size of the child nodes
    /// depends on the setInheritScale option of the child. In some cases you
	/// want a scaling factor of a parent node to apply to a child node (e.g.
	/// where the child node is a part of the same object, so you want it to be
	/// the same relative size based on the parent's size), but not in other
    /// cases (e.g. where the child node is just for positioning another object,
	/// you want it to maintain it's own size). The default is to inherit as
	/// with other transforms.
	/// </summary>
	/// <param name="inherit">If true, this node's scale will be affected by its parent's scale. If false, it will not be affected.</param>
	void setInheritScale(bool inherit);

	/// <summary>
	/// Returns true if this node is affected by scaling factors applied to the parent node.
	/// </summary>
	/// <returns></returns>
	bool getInheritScale();

	/// <summary>
	/// Scales the node, combining it's current scale with the passed in scaling
    /// factor.
	/// 
	/// This method applies an extra scaling factor to the node's existing
	/// scale, (unlike setScale which overwrites it) combining it's current
    /// scale with the new one. E.g. calling this method twice with
	/// Vector3(2,2,2) would have the same effect as setScale(Vector3(4,4,4)) if
	/// the existing scale was 1. 
	/// 
    /// Note that like rotations, scalings are oriented around the node's
    /// origin. 
	/// </summary>
	/// <param name="scale">The scale to set.</param>
	void scale(Engine::Vector3 scale);

	/// <summary>
	/// Scales the node, combining it's current scale with the passed in scaling
    /// factor.
	///
	/// See the Vector3 version of this function for more info.
	/// </summary>
	/// <param name="x">x</param>
	/// <param name="y">y</param>
	/// <param name="z">z</param>
	void scale(float x, float y, float z);

	/// <summary>
	/// Moves the node along the Cartesian axes.
	/// </summary>
	/// <param name="d">The distance to translate.</param>
	/// <param name="relativeTo">The transform space to use.</param>
	void translate(Engine::Vector3 d, TransformSpace relativeTo);

	/// <summary>
	/// Moves the node along the Cartesian axes. 
	/// </summary>
	/// <param name="x">x</param>
	/// <param name="y">y</param>
	/// <param name="z">z</param>
	/// <param name="relativeTo">The transform space to use.</param>
	void translate(float x, float y, float z, TransformSpace relativeTo);

	/// <summary>
	/// Rotate the node around the Z-axis. 
	/// </summary>
	/// <param name="angle">The amount to rotate.</param>
	/// <param name="relativeTo">The transform space to use.</param>
	void roll(float angle, TransformSpace relativeTo);

	/// <summary>
	/// Rotate the node around the X-axis.
	/// </summary>
	/// <param name="angle">The amount to rotate.</param>
	/// <param name="relativeTo">The transform space to use.</param>
	void pitch(float angle, TransformSpace relativeTo);

	/// <summary>
	/// Rotate the node around the Y-axis. 
	/// </summary>
	/// <param name="angle">The amount to rotate.</param>
	/// <param name="relativeTo">The transform space to use.</param>
	void yaw(float angle, TransformSpace relativeTo);

	/// <summary>
	/// Rotate the node around an arbitrary axis. 
	/// </summary>
	/// <param name="axis">The axis to rotate around.</param>
	/// <param name="angle">The amount to rotate.</param>
	/// <param name="relativeTo">The transform space to use.</param>
	void rotate(Engine::Vector3 axis, float angle, TransformSpace relativeTo);

	/// <summary>
	/// Rotate the node around an arbitrary axis. 
	/// </summary>
	/// <param name="q">The quaternion to rotate by.</param>
	/// <param name="relativeTo">The transform space to use.</param>
	void rotate(Engine::Quaternion q, TransformSpace relativeTo);

	/// <summary>
	/// Gets the position of the node as derived from all parents. 
	/// </summary>
	/// <returns>The derived position.</returns>
	Engine::Vector3 getDerivedPosition();

	/// <summary>
	/// Gets the scaling factor of the node as derived from all parents. 
	/// </summary>
	/// <returns>The derived scale.</returns>
	Engine::Vector3 getDerivedScale();

	/// <summary>
	/// Gets the orientation of the node as derived from all parents. 
	/// </summary>
	/// <returns>The derived orientation.</returns>
	Engine::Quaternion getDerivedOrientation();
};

}