/// <file>SceneManager.h</file>
/// <author>Andrew Piper</author>
/// <company>Joint Based Engineering</company>
/// <copyright>
/// Copyright (c) Joint Based Engineering 2008, All rights reserved
/// </copyright>

#pragma once

#include "NativeSceneListener.h"
#include "AutoPtr.h"
#include "PlaneBoundedVolumeListSceneQuery.h"
#include "EntityCollection.h"
#include "CameraCollection.h"
#include "LightCollection.h"
#include "SceneNodeCollection.h"
#include "ManualObjectCollection.h"

using namespace System;

namespace Ogre
{
	class SceneManager;
}

namespace OgreWrapper
{

class NativeSceneListener;

ref class Camera;
ref class Light;
ref class SceneNode;
ref class Entity;
ref class Renderer;
interface class SceneListener;
ref class ManualObject;
ref class RaySceneQuery;
ref class PlaneBoundedVolumeListSceneQuery;

/// <summary>
/// This is a scene in the renderer.  You can have multiple scenes at any time.
/// This is where Cameras, RenderEntities, Lights etc are created and added to
/// a scene.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class SceneManager
{
public:
	[Engine::Attributes::SingleEnum]
	enum class IlluminationRenderStage : unsigned int
	{
		IRS_NONE,
		IRS_RENDER_TO_TEXTURE,
		IRS_RENDER_RECEIVER_PASS
	};

private:
	Ogre::SceneManager* sceneManager;

	CameraCollection cameras;
	LightCollection lights;
	SceneNodeCollection sceneNodes;
	EntityCollection entities;
	ManualObjectCollection manualObjects;

	SceneNode^ rootNode;

	AutoPtr<NativeSceneListener> nativeSceneListener;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="sceneManager">The SceneManager to wrap.</param>
	SceneManager(Ogre::SceneManager* sceneManager);

	/// <summary>
	/// Gets the scene manager this class is wrapping.
	/// </summary>
	/// <returns>Returns the native SceneManager being wrapped.</returns>
	Ogre::SceneManager* getSceneManager();

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~SceneManager();

	/// <summary>
	/// Gets the name of the scene manager.
	/// </summary>
	/// <returns>The name of the scene manager.</returns>
	System::String^ getName();

	/// <summary>
	/// Creates a camera managed by this SceneManager.
	/// </summary>
	/// <param name="name">The name of the camera.</param>
	/// <returns>A new Camera wrapping the native camera.</returns>
    Camera^ createCamera(System::String^ name);

	/// <summary>
	/// Gets the named Camera.
	/// </summary>
	/// <param name="name">The name of the camera.</param>
	/// <returns>The Camera if the scene contains the camera, or null if it does not.</returns>
    Camera^ getCamera(System::String^ name);

	/// <summary>
	/// Returns whether a camera with the given name exists.
	/// </summary>
	/// <param name="name">The name of the camera.</param>
	/// <returns>True if the camera exists.  False if it does not.</returns>
	bool hasCamera(System::String^ name);

	/// <summary>
	/// Destroys the passed camera
	/// </summary>
	/// <param name="camera">The camera to destroy.</param>
	void destroyCamera( Camera^ camera );

	/// <summary>
	/// Creates a new light managed by this SceneManager.
	/// </summary>
	/// <param name="name">The name of the light.</param>
	/// <returns>A new Light wrapping a native light.</returns>
    Light^ createLight(System::String^ name);

	/// <summary>
	/// Gets the named Light.
	/// </summary>
	/// <param name="name">The name of the light.</param>
	/// <returns>The Light if the scene contains the light, or null if it does not.</returns>
    Light^ getLight(System::String^ name);

	/// <summary>
	/// Returns whether a light with the given name exists.
	/// </summary>
	/// <param name="name">The name of the light.</param>
	/// <returns>True if the light exists.  False if it does not.</returns>
	bool hasLight(System::String^ name);

	/// <summary>
	/// Destroys the passed light
	/// </summary>
	/// <param name="light">The light to destroy.</param>
	void destroyLight( Light^ light );

	/// <summary>
	/// Creates a new SceneNode with the given name.
	/// </summary>
	/// <param name="name">The name of the render node.</param>
	/// <returns>A new render node with the given name.</returns>
    SceneNode^ createSceneNode(System::String^ name);

	/// <summary>
	/// Gets the SceneNode at the root of the scene graph.
	/// </summary>
	/// <returns>The root SceneNode.</returns>
    SceneNode^ getRootSceneNode(void);

	/// <summary>
	/// Gets the named render node.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The Render node if it exists or else null.</returns>
    SceneNode^ getSceneNode(System::String^ name);

	/// <summary>
	/// Returns whether a SceneNode with the given name exists.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>True if the node exists.  False if it does not.</returns>
	bool hasSceneNode(System::String^ name);

	/// <summary>
	/// Destroys the render node passed.  You cannot destroy the root node using this
	/// method it will be destroyed automaticly.
	/// </summary>
	/// <param name="node">The SceneNode to destroy.</param>
	void destroySceneNode( SceneNode^ node );

	/// <summary>
	/// Create a Entity with the given name using the given mesh.
	/// </summary>
	/// <param name="entityName">The name of the entity.</param>
	/// <param name="meshName">The name of the mesh to use on this entity.</param>
	/// <returns>The new Entity.</returns>
    Entity^ createEntity(System::String^ entityName, System::String^ meshName);

	/// <summary>
	/// Get the named Entity.
	/// </summary>
	/// <param name="name">The name of the render entity.</param>
	/// <returns>The Entity if it exists or null if it does not.</returns>
	Entity^ getEntity(System::String^ name);
	
	/// <summary>
	/// Returns whether a Entity with the given name exists.
	/// </summary>
	/// <param name="name">The name of the render entity.</param>
	/// <returns>True if the entity exists.  False if it does not.</returns>
	bool hasEntity(System::String^ name);

	/// <summary>
	/// Destroys the Entity passed.
	/// </summary>
	/// <param name="entity">The Entity to destroy.</param>
	void destroyEntity( Entity^ entity );

	/// <summary>
	/// Create a ManualObject, an object which you populate with geometry manually through a 
	/// GL immediate-mode style interface. 
	/// </summary>
	/// <param name="name">The name to be given to the object (must be unique). </param>
	/// <returns>The newly created manual object.</returns>
	ManualObject^ createManualObject(System::String^ name);

	/// <summary>
	/// Retrieves a pointer to the named ManualObject. 
	/// </summary>
	/// <param name="name">The name of the object to look for.</param>
	/// <returns>The manual object identified by name or null if it is not found.</returns>
	ManualObject^ getManualObject(System::String^ name);

	/// <summary>
	/// Returns whether a manual object with the given name exists. 
	/// </summary>
	/// <param name="name">The name to search for.</param>
	/// <returns>True if the object is in the scene.  False if it is not.</returns>
	bool hasManualObject(System::String^ name);

	/// <summary>
	/// Removes and destroys a ManualObject from the SceneManager.
	/// </summary>
	/// <param name="obj">The object to destroy.</param>
	void destroyManualObject(ManualObject^ obj);

	/// <summary>
	/// Sets a mask which is bitwise 'and'ed with objects own visibility masks to determine if 
	/// the object is visible. Note that this is combined with any per-viewport visibility mask 
	/// through an 'and' operation.
	/// </summary>
	/// <param name="mask">The mask to set.</param>
	void setVisibilityMask(unsigned int mask);

	/// <summary>
	/// Gets a mask which is bitwise 'and'ed with objects own visibility masks
	/// to determine if the object is visible.
	/// </summary>
	/// <returns>The visibility mask.</returns>
	unsigned int getVisibilityMask();

	/// <summary>
	/// Add a listener which will get called back on scene manager events. 
	/// </summary>
	/// <param name="listener">The listener to add.</param>
	void addSceneListener(SceneListener^ listener);

	/// <summary>
	/// Remove a listener.
	/// </summary>
	/// <param name="listener">The listener to remove.</param>
	void removeSceneListener(SceneListener^ listener);

	/// <summary>
	/// Creates a RaySceneQuery for this scene manager. This method creates a
    /// new instance of a query object for this scene manager, looking for
    /// objects which fall along a ray. See SceneQuery and RaySceneQuery for
    /// full details. 
	/// 
    /// The instance returned from this method must be destroyed by calling
    /// SceneManager::destroyQuery when it is no longer required. 
	/// </summary>
	/// <param name="ray"></param>
	/// <param name="mask"></param>
	/// <returns></returns>
	RaySceneQuery^ createRaySceneQuery(EngineMath::Ray3 ray, unsigned long mask);

	/// <summary>
	/// Destroys a ray scene query. 
	/// </summary>
	/// <param name="query">The query to destroy.</param>
	void destroyQuery(RaySceneQuery^ query);

	/// <summary>
	/// Creates a PlaneBoundedVolumeListSceneQuery for this scene manager.
	/// 
	///  This method creates a new instance of a query object for this scene
	///  manager, for a region enclosed by a set of planes (normals pointing
	///  inwards). See SceneQuery and PlaneBoundedVolumeListSceneQuery for full
	///  details.
	/// 
	///  The instance returned from this method must be destroyed by calling
	///  SceneManager::destroyQuery when it is no longer required.
	/// </summary>
	/// <param name="volumes">Details of the volumes which describe the region for this query. </param>
	/// <returns>A new PlaneBoundedVolumeListSceneQuery.</returns>
	PlaneBoundedVolumeListSceneQuery^ createPlaneBoundedVolumeQuery(PlaneBoundedVolumeList^ volumes);

	/// <summary>
	/// Creates a PlaneBoundedVolumeListSceneQuery for this scene manager.
	/// 
	///  This method creates a new instance of a query object for this scene
	///  manager, for a region enclosed by a set of planes (normals pointing
	///  inwards). See SceneQuery and PlaneBoundedVolumeListSceneQuery for full
	///  details.
	/// 
	///  The instance returned from this method must be destroyed by calling
	///  SceneManager::destroyQuery when it is no longer required.
	/// </summary>
	/// <param name="volumes">Details of the volumes which describe the region for this query. </param>
	/// <param name="mask">The query mask to apply to this query; can be used to filter out certain objects; see SceneQuery for details. </param>
	/// <returns>A new PlaneBoundedVolumeListSceneQuery.</returns>
	PlaneBoundedVolumeListSceneQuery^ createPlaneBoundedVolumeQuery(PlaneBoundedVolumeList^ volumes, unsigned long mask);

	/// <summary>
	/// Destroys a Plane Bounded Volume scene query. 
	/// </summary>
	/// <param name="query">The query to destroy.</param>
	void destroyQuery(PlaneBoundedVolumeListSceneQuery^ query);

	/// <summary>
	/// Tells the SceneManager whether it should render the SceneNodes which
    /// make up the scene as well as the objects in the scene.
    /// 
	/// This method is mainly for debugging purposes. If you set this to 'true',
    /// each node will be rendered as a set of 3 axes to allow you to easily see
    /// the orientation of the nodes. 
	/// </summary>
	/// <param name="display">True to display scene nodes.  False to disable.</param>
	void setDisplaySceneNodes(bool display);

	/// <summary>
	/// Determine if all scene nodes axis are to be displayed. 
	/// </summary>
	/// <returns>True if all scene nodes axis are to be displayed.</returns>
	bool getDisplaySceneNodes();

	/// <summary>
	/// Allows all bounding boxes of scene nodes to be displayed. 
	/// </summary>
	/// <param name="bShow">True to enable bounding box rendering.</param>
	void showBoundingBoxes(bool bShow);

	/// <summary>
	/// Determine if all bounding boxes of scene nodes are to be displayed. 
	/// </summary>
	/// <returns>Returns true if all bounding boxes of scene nodes are to be displayed. </returns>
	bool getShowBoundingBoxes();

	/// <summary>
	/// Enables / disables the rendering of debug information for shadows.
	/// </summary>
	/// <param name="debug">True to enable debug shadows.  False to disable.</param>
	void setShowDebugShadows(bool debug);

	/// <summary>
	/// Determine if debug shadows are being rendererd.
	/// </summary>
	/// <returns>True if shadows are being rendered.</returns>
	bool getShowDebugShadows();
};

}