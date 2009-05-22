#pragma once
/// <file>SceneNode.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#include "AutoPtr.h"
#include "Node.h"

namespace Ogre
{
	class SceneNode;
}

namespace OgreWrapper
{

ref class MovableObject;
ref class SceneManager;
typedef System::Collections::Generic::Dictionary<System::String^, MovableObject^> NodeObjectList;

/// <summary>
/// This is a wrapper for a node in the underlying renderer's scene graph.  Multiple
/// rendering objects can be attached to these nodes.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
public ref class SceneNode : public Node
{
private:
	Ogre::SceneNode* sceneNode;
	NodeObjectList^ nodeObjects;
	AutoPtr<Ogre::SceneNode> autoOgreNode;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="sceneNode">The underlying node to wrap.</param>
	/// <param name="name">The name of the scene node.</param>
	SceneNode(Ogre::SceneNode* sceneNode);

	/// <summary>
	/// Get the underlying native scene node.
	/// </summary>
	/// <returns>The underlying native scene node.</returns>
	Ogre::SceneNode* getSceneNode();
public:
	SceneNode(System::String^ name, SceneManager^ ownerScene);

	/// <summary>
	/// Destructor
	/// </summary>
	~SceneNode();

	/// <summary>
	/// Get the name of this scene node.
	/// </summary>
	/// <returns>The name of the scene node.</returns>
	System::String^ getName();

	/// <summary>
	/// Add another scene node as a child to this node.
	/// </summary>
	/// <param name="child">The child scene node to add.</param>
	void addChild(SceneNode^ child);

	/// <summary>
	/// Removes a child scene node.
	/// </summary>
	/// <param name="child">The child scene node to remove.</param>
	void removeChild(SceneNode^ child);

	/// <summary>
	/// Attach a MovableObject such as a Entity or a Light to this node.
	/// </summary>
	/// <param name="object">The object to attach to this node.</param>
	void attachObject(MovableObject^ object);

	/// <summary>
	/// Detach a MovableObject such as a Entity or a Light to this node.
	/// </summary>
	/// <param name="object">The object to detach from this node.</param>
	void detachObject(MovableObject^ object);

	/// <summary>
	/// Get an iterator over the SceneNodeObjects on this node.
	/// </summary>
	/// <returns>A list of SceneNodeObjects on this node.</returns>
	System::Collections::Generic::IEnumerable<MovableObject^>^ getNodeObjectIter();

	/// <summary>
	/// Get the render node object specified by name.
	/// </summary>
	/// <param name="name">The name of the requested object.</param>
	/// <returns>The requested MovableObject or null if it does not exist.</returns>
	MovableObject^ getNodeObject(System::String^ name);

	/// <summary>
	/// Enables / disables automatic tracking of a SceneNode.
	/// 
	/// Remarks:
    /// If you enable auto-tracking, this Camera will automatically rotate to look at the 
	/// target SceneNode every frame, no matter how it or SceneNode move. This is handy if 
	/// you want a Camera to be focused on a single object or group of objects. Note that by
	/// default the Camera looks at the origin of the SceneNode, if you want to tweak this, 
	/// e.g. if the object which is attached to this target node is quite big and you want 
	/// to point the camera at a specific point on it, provide a vector in the 'offset' 
	/// parameter and the camera's target point will be adjusted. 
	/// </summary>
	/// <param name="enabled">If true, the Camera will track the SceneNode supplied as the next parameter (cannot be null). If false the camera will cease tracking and will remain in it's current orientation.</param>
	/// <param name="target">Pointer to the SceneNode which this Camera will track. Make sure you don't delete this SceneNode before turning off tracking (e.g. SceneManager::clearScene will delete it so be careful of this). Can be null if and only if the enabled param is false.</param>
	/// <param name="offset">If supplied, the camera targets this point in local space of the target node instead of the origin of the target node. Good for fine tuning the look at point.</param>
	void setAutoTracking(bool enabled, SceneNode^ target, Engine::Vector3 offset);


	/// <summary>
	/// Makes all objects attached to this node become visible / invisible and
    /// all child nodes.
	/// </summary>
	/// <param name="visible">Whether the objects are to be made visible or invisible.</param>
	void setVisible(bool visible);

	/// <summary>
	/// Makes all objects attached to this node become visible / invisible. 
	/// </summary>
	/// <param name="visible">Whether the objects are to be made visible or invisible.</param>
	/// <param name="cascade">If true, this setting cascades into child nodes too.</param>
	void setVisible(bool visible, bool cascade);
};

}