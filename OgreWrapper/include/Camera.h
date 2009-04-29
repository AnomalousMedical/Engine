/// <file>Camera.h</file>
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
#include "Enums.h"

namespace Ogre
{
	class Camera;
}

namespace OgreWrapper
{

class VoidUserDefinedObject;

ref class Camera;

typedef gcroot<Camera^> CameraGCRoot;

/// <summary>
/// A wrapper for native camera classes.  It provides useful utilities for
/// manipulating a 3d camera.
/// </summary>
[Engine::Attributes::NativeSubsystemType]
[Engine::Attributes::DoNotSaveAttribute]
public ref class Camera : public MovableObject
{
private:
	Ogre::Camera* camera;
	AutoPtr<VoidUserDefinedObject> userDefinedObj;
	AutoPtr<CameraGCRoot> camRoot;

internal:
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="camera">The camera to wrap.</param>
	/// <param name="name">The name of this camera.</param>
	Camera(Ogre::Camera* camera);

	/// <summary>
	/// Destructor
	/// </summary>
	~Camera();

public:
	/// <summary>
	/// Returns the native camera.
	/// </summary>
	/// <returns>The native camera.</returns>
	Ogre::Camera* getCamera();

	/// <summary>
	/// Returns the name of this camera.
	/// </summary>
	/// <returns>The camera name.</returns>
	virtual System::String^ getName() override;

	/// <summary>
	/// Sets the position of the eye of the camera.
	/// </summary>
	/// <param name="position">The position to set the eye at.</param>
	void setPosition( Engine::Vector3 position );

	/// <summary>
	/// Returns the position of this camera.
	/// </summary>
	/// <returns>The camera position.</returns>
	Engine::Vector3 getPosition();

	/// <summary>
	/// Gets the derived orientation of the camera, including any rotation inherited from a node 
	/// attachment and reflection matrix.
	/// </summary>
	/// <returns>The camera orientation.</returns>
	Engine::Quaternion getDerivedOrientation();

	/// <summary>
	/// Gets the derived position of the camera, including any translation inherited from a node 
	/// attachment and reflection matrix. 
	/// </summary>
	/// <returns>The camera position.</returns>
	Engine::Vector3 getDerivedPosition();

	/// <summary>
	/// Gets the derived direction vector of the camera, including any rotation inherited from a node 
	/// attachment and reflection matrix. 
	/// </summary>
	/// <returns>The camera direction.</returns>
	Engine::Vector3 getDerivedDirection();

	/// <summary>
	/// Gets the derived up vector of the camera, including any rotation inherited from a node 
	/// attachment and reflection matrix. 
	/// </summary>
	/// <returns>The camera up.</returns>
	Engine::Vector3 getDerivedUp();

	/// <summary>
	/// Gets the derived right vector of the camera, including any rotation inherited from a node 
	/// attachment and reflection matrix. 
	/// </summary>
	/// <returns>The camera right.</returns>
	Engine::Vector3 getDerivedRight();

	/// <summary>
	/// Gets the real world orientation of the camera, including any rotation inherited from a node 
	/// attachment. 
	/// </summary>
	/// <returns>The camera orientation.</returns>
	Engine::Quaternion getRealOrientation();

	/// <summary>
	/// Gets the real world position of the camera, including any translation inherited from a node 
	/// attachment. 
	/// </summary>
	/// <returns>The camera position.</returns>
	Engine::Vector3 getRealPosition();

	/// <summary>
	/// Gets the real world direction vector of the camera, including any rotation inherited from a node 
	/// attachment. 
	/// </summary>
	/// <returns>The camera direction.</returns>
	Engine::Vector3 getRealDirection();

	/// <summary>
	/// Gets the real world up vector of the camera, including any rotation inherited from a node attachment. 
	/// </summary>
	/// <returns>The camera up.</returns>
	Engine::Vector3 getRealUp();

	/// <summary>
	/// Gets the real world right vector of the camera, including any rotation inherited from a node attachment. 
	/// </summary>
	/// <returns>The camera right.</returns>
	Engine::Vector3 getRealRight();

	/// <summary>
	/// Sets the look at position of the camera.
	/// </summary>
	/// <param name="lookAt">The position of the look at point.</param>
	void lookAt( Engine::Vector3 lookAt );

	/// <summary>
	/// Sets the level of rendering detail required from this camera.
	/// </summary>
	/// <param name="mode"></param>
	void setPolygonMode(PolygonMode mode);

	/// <summary>
	/// Retrieves the level of detail that the camera will render.
	/// </summary>
	/// <returns>Retrieves the level of detail that the camera will render.</returns>
	PolygonMode getPolygonMode();

	/// <summary>
	/// Sets the camera's direction vector. 
	/// </summary>
	/// <param name="x">X component</param>
	/// <param name="y">Y component</param>
	/// <param name="z">Z component</param>
	void setDirection(float x, float y, float z);

	/// <summary>
	/// Sets the camera's direction vector. 
	/// </summary>
	/// <param name="direction">Vector of the direction.</param>
	void setDirection(Engine::Vector3 direction);

	/// <summary>
	/// Gets the camera's direction vector. 
	/// </summary>
	/// <param name="direction">Vector to fill out with the direction vector.</param>
	void getDirection(Engine::Vector3% direction);

	/// <summary>
	/// Gets the camera's up vector.
	/// </summary>
	/// <param name="up">The vector to fill out.</param>
	void getUp(Engine::Vector3% up);

	/// <summary>
	/// Gets the camera's right vector.
	/// </summary>
	/// <param name="right">The vector to fill out.</param>
	void getRight(Engine::Vector3% right);
	
	/// <summary>
	/// This method can be used to influence the overall level of detail of the scenes 
	/// rendered using this camera. Various elements of the scene have level-of-detail 
	/// reductions to improve rendering speed at distance; this method allows you to hint to 
	/// those elements that you would like to adjust the level of detail that they would 
	/// normally use (up or down). 
	/// 
    /// The most common use for this method is to reduce the overall level of detail used 
	/// for a secondary camera used for sub viewports like rear-view mirrors etc. Note that 
	/// scene elements are at liberty to ignore this setting if they choose, this is merely 
	/// a hint.
	/// </summary>
	/// <param name="factor">The factor to apply to the usual level of detail calculation. Higher values increase the detail, so 2.0 doubles the normal detail and 0.5 halves it.</param>
	void setLodBias(float factor);

	/// <summary>
	/// Get the lod bias.
	/// </summary>
	/// <returns>The lod bias.</returns>
	float getLodBias();

	/// <summary>
	/// Gets a world space ray as cast from the camera through a viewport position.
	/// </summary>
	/// <param name="screenx">The x position at which the ray should intersect the viewport, in normalised screen coordinates [0,1]</param>
	/// <param name="screeny">The y position at which the ray should intersect the viewport, in normalised screen coordinates [0,1]</param>
	/// <returns></returns>
	Engine::Ray3 getCameraToViewportRay(float screenx, float screeny);

	/// <summary>
	/// This method can be used to set a subset of the viewport as the rendering target.
	/// </summary>
	/// <param name="left">0 corresponds to left edge, 1 - to right edge (default - 0).</param>
	/// <param name="top">0 corresponds to left edge, 1 - to right edge (default - 0).</param>
	/// <param name="right">0 corresponds to left edge, 1 - to right edge (default - 1).</param>
	/// <param name="bottom">0 corresponds to top edge, 1 - to bottom edge (default - 1).</param>
	void setWindow(float left, float top, float right, float bottom);

	/// <summary>
	/// Cancel view window.
	/// </summary>
	void resetWindow();

	/// <summary>
	/// Returns true if a viewport window is being used. 
	/// </summary>
	/// <returns>True if the viewport window is being used.</returns>
	bool isWindowSet();

	/// <summary>
	/// If set to true a vieport that owns this frustum will be able to recalculate the aspect ratio whenever the frustum is resized. 
	/// </summary>
	/// <param name="autoRatio">True to enable auto resizing.</param>
	void setAutoAspectRatio(bool autoRatio);

	/// <summary>
	/// Returns true if auto aspect ratio is enabled.
	/// </summary>
	/// <returns>True if auto ratio is enabled.</returns>
	bool getAutoAspectRatio();

	/// <summary>
	/// Returns the near clip distance.
	/// </summary>
	/// <returns>The near clip distance.</returns>
	float getNearClipDistance();

	/// <summary>
	/// Returns the far clip distance.
	/// </summary>
	/// <returns>The far clip distance.</returns>
	float getFarClipDistance();

	/// <summary>
	/// Set whether this camera should use the 'rendering distance' on objects to exclude 
	/// distant objects from the final image.
	///
	/// The default behaviour is to use it. 
	/// </summary>
	/// <param name="use"></param>
	void setUseRenderingDistance(bool use);

	/// <summary>
	/// Returns true if rendering distance is being used to exclude objects.
	/// </summary>
	/// <returns>True if in use.</returns>
	bool getUseRenderingDistance();

	/// <summary>
	/// Field Of View (FOV) is the angle made between the frustum's position, and the edges 
	/// of the 'screen' onto which the scene is projected. High values (90+ degrees) result 
	/// in a wide-angle, fish-eye kind of view, low values (30- degrees) in a stretched, 
	/// telescopic kind of view. Typical values are between 45 and 60 degrees. 
	///
	/// This value represents the VERTICAL field-of-view. The horizontal field of view is 
	/// calculated from this depending on the dimensions of the viewport (they will only be 
	/// the same if the viewport is square). 
	/// </summary>
	/// <param name="fovy">The fov to set, must be in degrees.</param>
	void setFOVy(float fovy);

	/// <summary>
	/// Get the vertical fov.
	/// </summary>
	/// <returns>The vertical fov in radians.</returns>
	float getFOVy();

	/// <summary>
	/// Sets the position of the near clipping plane.
	/// 
	/// Remarks:
    /// The position of the near clipping plane is the distance from the frustums position 
	/// to the screen on which the world is projected. The near plane distance, combined 
	/// with the field-of-view and the aspect ratio, determines the size of the viewport 
	/// through which the world is viewed (in world co-ordinates). Note that this world 
	/// viewport is different to a screen viewport, which has it's dimensions expressed in 
	/// pixels. The frustums viewport should have the same aspect ratio as the screen 
	/// viewport it renders into to avoid distortion. 
	/// </summary>
	/// <param name="nearDistance">The distance to the near clipping plane from the frustum in world coordinates.</param>
	void setNearClipDistance(float nearDistance);

	/// <summary>
	/// Sets the distance to the far clipping plane.
	/// 
	/// Remarks:
    /// The view frustrum is a pyramid created from the frustum position and the edges of 
	/// the viewport. This method sets the distance for the far end of that pyramid. 
	/// Different applications need different values: e.g. a flight sim needs a much further 
	/// far clipping plane than a first-person shooter. An important point here is that the 
	/// larger the ratio between near and far clipping planes, the lower the accuracy of the 
	/// Z-buffer used to depth-cue pixels. This is because the Z-range is limited to the size
	/// of the Z buffer (16 or 32-bit) and the max values must be spread over the gap 
	/// between near and far clip planes. As it happens, you can affect the accuracy far 
	/// more by altering the near distance rather than the far distance, but keep this in 
	/// mind. 
	/// </summary>
	/// <param name="farDistance">The far distance.</param>
	void setFarClipDistance(float farDistance);

	/// <summary>
	///   	
	/// Sets the aspect ratio for the frustum viewport.
	/// 
	/// Remarks:
    /// The ratio between the x and y dimensions of the rectangular area visible through the 
	/// frustum is known as aspect ratio: aspect = width / height . 
	/// 
    /// The default for most fullscreen windows is 1.3333 - this is also assumed by Ogre 
	/// unless you use this method to state otherwise. 
	/// </summary>
	/// <param name="ratio">The aspect ratio to set.</param>
	void setAspectRatio(float ratio);

	/// <summary>
	/// Returns the aspect ratio.
	/// </summary>
	/// <returns>The aspect ratio.</returns>
	float getAspectRatio();

	/// <summary>
	/// Sets the distance at which the object is no longer rendered.
	/// </summary>
	/// <param name="dist">Distance beyond which the object will not be rendered (the 
	/// default is 0, which means objects are always rendered).</param>
	void setRenderingDistance(float dist);

	/// <summary>
	/// Get the rendering distance.
	/// </summary>
	/// <returns>The rendering distance.</returns>
	float getRenderingDistance();

	/// <summary>
	/// Retrieves info on the type of projection used (orthographic or perspective). 
	/// </summary>
	/// <returns>ProjectionType enum.</returns>
	ProjectionType getProjectionType();

	/// <summary>
	/// Sets the type of projection to use (orthographic or perspective). 
	/// </summary>
	/// <param name="type">One of the ProjectionType enum values.</param>
	void setProjectionType(ProjectionType type);

	/// <summary>
	/// Sets the orthographic window settings, for use with orthographic rendering only.
	/// Calling this method will recalculate the aspect ratio, use setOrthoWindowHeight or 
	/// setOrthoWindowWidth alone if you wish to preserve the aspect ratio but just fit one 
	/// or other dimension to a particular size.
	/// </summary>
	/// <param name="w">Width of the window.</param>
	/// <param name="h">Height of the window.</param>
	void setOrthoWindow(float w, float h);

	/// <summary>
	/// Sets the orthographic window width, for use with orthographic rendering only.
	/// The height of the window will be calculated from the aspect ratio. 
	/// </summary>
	/// <param name="w">The width of the window.</param>
	void setOrthoWindowWidth(float w);

	/// <summary>
	/// Sets the orthographic window height, for use with orthographic rendering only.
	/// The width of the window will be calculated from the aspect ratio. 
	/// </summary>
	/// <param name="h">The height of the window.</param>
	void setOrthoWindowHeight(float h);

	/// <summary>
	/// Gets the orthographic window width, for use with orthographic rendering only. 
	/// </summary>
	/// <returns>The window width.</returns>
	float getOrthoWindowWidth();

	/// <summary>
	/// Gets the orthographic window height, for use with orthographic rendering only. 
	/// </summary>
	/// <returns>The window height.</returns>
	float getOrthoWindowHeight();
};

}