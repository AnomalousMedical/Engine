using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [SingleEnum]
    public enum PolygonMode : uint
    {
	    PM_POINTS = 1,
	    PM_WIREFRAME = 2,
	    PM_SOLID = 3
    };

    [SingleEnum]
    public enum ProjectionType : uint
    {
        PT_ORTHOGRAPHIC,
        PT_PERSPECTIVE
    };

    public class Camera : MovableObject
    {
        internal static Camera createWrapper(IntPtr camera, object[] args)
        {
            return new Camera(camera);
        }

        private Camera(IntPtr camera)
            :base(camera)
        {

        }

        /// <summary>
	    /// Sets the position of the eye of the camera.
	    /// </summary>
	    /// <param name="position">The position to set the eye at.</param>
        public void setPosition(Vector3 position)
        {
            Camera_setPosition(ogreObject, position);
        }

	    /// <summary>
	    /// Returns the position of this camera.
	    /// </summary>
	    /// <returns>The camera position.</returns>
        public Vector3 getPosition()
        {
            return Camera_getPosition(ogreObject);
        }

	    /// <summary>
	    /// Gets the derived orientation of the camera, including any rotation inherited from a node 
	    /// attachment and reflection matrix.
	    /// </summary>
	    /// <returns>The camera orientation.</returns>
        public Quaternion getDerivedOrientation()
        {
            return Camera_getDerivedOrientation(ogreObject);
        }

	    /// <summary>
	    /// Gets the derived position of the camera, including any translation inherited from a node 
	    /// attachment and reflection matrix. 
	    /// </summary>
	    /// <returns>The camera position.</returns>
        public Vector3 getDerivedPosition()
        {
            return Camera_getDerivedPosition(ogreObject);
        }

	    /// <summary>
	    /// Gets the derived direction vector of the camera, including any rotation inherited from a node 
	    /// attachment and reflection matrix. 
	    /// </summary>
	    /// <returns>The camera direction.</returns>
        public Vector3 getDerivedDirection()
        {
            return Camera_getDerivedDirection(ogreObject);
        }

	    /// <summary>
	    /// Gets the derived up vector of the camera, including any rotation inherited from a node 
	    /// attachment and reflection matrix. 
	    /// </summary>
	    /// <returns>The camera up.</returns>
        public Vector3 getDerivedUp()
        {
            return Camera_getDerivedUp(ogreObject);
        }

	    /// <summary>
	    /// Gets the derived right vector of the camera, including any rotation inherited from a node 
	    /// attachment and reflection matrix. 
	    /// </summary>
	    /// <returns>The camera right.</returns>
        public Vector3 getDerivedRight()
        {
            return Camera_getDerivedRight(ogreObject);
        }

	    /// <summary>
	    /// Gets the real world orientation of the camera, including any rotation inherited from a node 
	    /// attachment. 
	    /// </summary>
	    /// <returns>The camera orientation.</returns>
        public Quaternion getRealOrientation()
        {
            return Camera_getRealOrientation(ogreObject);
        }

	    /// <summary>
	    /// Gets the real world position of the camera, including any translation inherited from a node 
	    /// attachment. 
	    /// </summary>
	    /// <returns>The camera position.</returns>
        public Vector3 getRealPosition()
        {
            return Camera_getRealPosition(ogreObject);
        }

	    /// <summary>
	    /// Gets the real world direction vector of the camera, including any rotation inherited from a node 
	    /// attachment. 
	    /// </summary>
	    /// <returns>The camera direction.</returns>
        public Vector3 getRealDirection()
        {
            return Camera_getRealDirection(ogreObject);
        }

	    /// <summary>
	    /// Gets the real world up vector of the camera, including any rotation inherited from a node attachment. 
	    /// </summary>
	    /// <returns>The camera up.</returns>
        public Vector3 getRealUp()
        {
            return Camera_getRealUp(ogreObject);
        }

	    /// <summary>
	    /// Gets the real world right vector of the camera, including any rotation inherited from a node attachment. 
	    /// </summary>
	    /// <returns>The camera right.</returns>
        public Vector3 getRealRight()
        {
            return Camera_getRealRight(ogreObject);
        }

	    /// <summary>
	    /// Sets the look at position of the camera.
	    /// </summary>
	    /// <param name="lookAt">The position of the look at point.</param>
        public void lookAt(Vector3 lookAt)
        {
            Camera_lookAt(ogreObject, lookAt);
        }

	    /// <summary>
	    /// Sets the level of rendering detail required from this camera.
	    /// </summary>
	    /// <param name="mode"></param>
        public void setPolygonMode(PolygonMode mode)
        {
            Camera_setPolygonMode(ogreObject, mode);
        }

	    /// <summary>
	    /// Retrieves the level of detail that the camera will render.
	    /// </summary>
	    /// <returns>Retrieves the level of detail that the camera will render.</returns>
        public PolygonMode getPolygonMode()
        {
            return Camera_getPolygonMode(ogreObject);
        }

	    /// <summary>
	    /// Sets the camera's direction vector. 
	    /// </summary>
	    /// <param name="x">X component</param>
	    /// <param name="y">Y component</param>
	    /// <param name="z">Z component</param>
        public void setDirection(float x, float y, float z)
        {
            Camera_setDirectionRaw(ogreObject, x, y, z);
        }

	    /// <summary>
	    /// Sets the camera's direction vector. 
	    /// </summary>
	    /// <param name="direction">Vector of the direction.</param>
        public void setDirection(Vector3 direction)
        {
            Camera_setDirection(ogreObject, direction);
        }

	    /// <summary>
	    /// Gets the camera's direction vector. 
	    /// </summary>
	    /// <param name="direction">Vector to fill out with the direction vector.</param>
        public Vector3 getDirection()
        {
            return Camera_getDirection(ogreObject);
        }

	    /// <summary>
	    /// Gets the camera's up vector.
	    /// </summary>
	    /// <param name="up">The vector to fill out.</param>
        public Vector3 getUp()
        {
            return Camera_getUp(ogreObject);
        }

	    /// <summary>
	    /// Gets the camera's right vector.
	    /// </summary>
	    /// <param name="right">The vector to fill out.</param>
        public Vector3 getRight()
        {
            return Camera_getRight(ogreObject);
        }
    	
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
        public void setLodBias(float factor)
        {
            Camera_setLodBias(ogreObject, factor);
        }

	    /// <summary>
	    /// Get the lod bias.
	    /// </summary>
	    /// <returns>The lod bias.</returns>
        public float getLodBias()
        {
            return Camera_getLodBias(ogreObject);
        }

	    /// <summary>
	    /// Gets a world space ray as cast from the camera through a viewport position.
	    /// </summary>
	    /// <param name="screenx">The x position at which the ray should intersect the viewport, in normalised screen coordinates [0,1]</param>
	    /// <param name="screeny">The y position at which the ray should intersect the viewport, in normalised screen coordinates [0,1]</param>
	    /// <returns></returns>
        public Ray3 getCameraToViewportRay(float screenx, float screeny)
        {
            return Camera_getCameraToViewportRay(ogreObject, screenx, screeny);
        }

	    /// <summary>
	    /// This method can be used to set a subset of the viewport as the rendering target.
	    /// </summary>
	    /// <param name="left">0 corresponds to left edge, 1 - to right edge (default - 0).</param>
	    /// <param name="top">0 corresponds to left edge, 1 - to right edge (default - 0).</param>
	    /// <param name="right">0 corresponds to left edge, 1 - to right edge (default - 1).</param>
	    /// <param name="bottom">0 corresponds to top edge, 1 - to bottom edge (default - 1).</param>
        public void setWindow(float left, float top, float right, float bottom)
        {
            Camera_setWindow(ogreObject, left, top, right, bottom);
        }

	    /// <summary>
	    /// Cancel view window.
	    /// </summary>
        public void resetWindow()
        {
            Camera_resetWindow(ogreObject);
        }

	    /// <summary>
	    /// Returns true if a viewport window is being used. 
	    /// </summary>
	    /// <returns>True if the viewport window is being used.</returns>
        public bool isWindowSet()
        {
            return Camera_isWindowSet(ogreObject);
        }

	    /// <summary>
	    /// If set to true a viewport that owns this frustum will be able to recalculate the aspect ratio whenever the frustum is resized. 
	    /// </summary>
	    /// <param name="autoRatio">True to enable auto resizing.</param>
        public void setAutoAspectRatio(bool autoRatio)
        {
            Camera_setAutoAspectRatio(ogreObject, autoRatio);
        }

	    /// <summary>
	    /// Returns true if auto aspect ratio is enabled.
	    /// </summary>
	    /// <returns>True if auto ratio is enabled.</returns>
        public bool getAutoAspectRatio()
        {
            return Camera_getAutoAspectRatio(ogreObject);
        }

	    /// <summary>
	    /// Returns the near clip distance.
	    /// </summary>
	    /// <returns>The near clip distance.</returns>
        public float getNearClipDistance()
        {
            return Camera_getNearClipDistance(ogreObject);
        }

	    /// <summary>
	    /// Returns the far clip distance.
	    /// </summary>
	    /// <returns>The far clip distance.</returns>
        public float getFarClipDistance()
        {
            return Camera_getFarClipDistance(ogreObject);
        }

	    /// <summary>
	    /// Set whether this camera should use the 'rendering distance' on objects to exclude 
	    /// distant objects from the final image.
	    ///
	    /// The default behavior is to use it. 
	    /// </summary>
	    /// <param name="use"></param>
        public void setUseRenderingDistance(bool use)
        {
            Camera_setUseRenderingDistance(ogreObject, use);
        }

	    /// <summary>
	    /// Returns true if rendering distance is being used to exclude objects.
	    /// </summary>
	    /// <returns>True if in use.</returns>
        public bool getUseRenderingDistance()
        {
            return Camera_getUseRenderingDistance(ogreObject);
        }

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
        public void setFOVy(float fovy)
        {
            Camera_setFOVy(ogreObject, fovy);
        }

	    /// <summary>
	    /// Get the vertical fov.
	    /// </summary>
	    /// <returns>The vertical fov in radians.</returns>
        public float getFOVy()
        {
            return Camera_getFOVy(ogreObject);
        }

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
        public void setNearClipDistance(float nearDistance)
        {
            Camera_setNearClipDistance(ogreObject, nearDistance);
        }

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
        public void setFarClipDistance(float farDistance)
        {
            Camera_setFarClipDistance(ogreObject, farDistance);
        }

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
        public void setAspectRatio(float ratio)
        {
            Camera_setAspectRatio(ogreObject, ratio);
        }

	    /// <summary>
	    /// Returns the aspect ratio.
	    /// </summary>
	    /// <returns>The aspect ratio.</returns>
        public float getAspectRatio()
        {
            return Camera_getAspectRatio(ogreObject);
        }

	    /// <summary>
	    /// Sets the distance at which the object is no longer rendered.
	    /// </summary>
	    /// <param name="dist">Distance beyond which the object will not be rendered (the 
	    /// default is 0, which means objects are always rendered).</param>
        public void setRenderingDistance(float dist)
        {
            Camera_setRenderingDistance(ogreObject, dist);
        }

	    /// <summary>
	    /// Get the rendering distance.
	    /// </summary>
	    /// <returns>The rendering distance.</returns>
        public float getRenderingDistance()
        {
            return Camera_getRenderingDistance(ogreObject);
        }

	    /// <summary>
	    /// Retrieves info on the type of projection used (orthographic or perspective). 
	    /// </summary>
	    /// <returns>ProjectionType enum.</returns>
        public ProjectionType getProjectionType()
        {
            return Camera_getProjectionType(ogreObject);
        }

	    /// <summary>
	    /// Sets the type of projection to use (orthographic or perspective). 
	    /// </summary>
	    /// <param name="type">One of the ProjectionType enum values.</param>
        public void setProjectionType(ProjectionType type)
        {
            Camera_setProjectionType(ogreObject, type);
        }

	    /// <summary>
	    /// Sets the orthographic window settings, for use with orthographic rendering only.
	    /// Calling this method will recalculate the aspect ratio, use setOrthoWindowHeight or 
	    /// setOrthoWindowWidth alone if you wish to preserve the aspect ratio but just fit one 
	    /// or other dimension to a particular size.
	    /// </summary>
	    /// <param name="w">Width of the window.</param>
	    /// <param name="h">Height of the window.</param>
        public void setOrthoWindow(float w, float h)
        {
            Camera_setOrthoWindow(ogreObject, w, h);
        }

	    /// <summary>
	    /// Sets the orthographic window width, for use with orthographic rendering only.
	    /// The height of the window will be calculated from the aspect ratio. 
	    /// </summary>
	    /// <param name="w">The width of the window.</param>
        public void setOrthoWindowWidth(float w)
        {
            Camera_setOrthoWindowWidth(ogreObject, w);
        }

	    /// <summary>
	    /// Sets the orthographic window height, for use with orthographic rendering only.
	    /// The width of the window will be calculated from the aspect ratio. 
	    /// </summary>
	    /// <param name="h">The height of the window.</param>
        public void setOrthoWindowHeight(float h)
        {
            Camera_setOrthoWindowHeight(ogreObject, h);
        }

	    /// <summary>
	    /// Gets the orthographic window width, for use with orthographic rendering only. 
	    /// </summary>
	    /// <returns>The window width.</returns>
        public float getOrthoWindowWidth()
        {
            return Camera_getOrthoWindowWidth(ogreObject);
        }

	    /// <summary>
	    /// Gets the orthographic window height, for use with orthographic rendering only. 
	    /// </summary>
	    /// <returns>The window height.</returns>
        public float getOrthoWindowHeight()
        {
            return Camera_getOrthoWindowHeight(ogreObject);
        }

        public Matrix4x4 getViewMatrix()
        {
            return Camera_getViewMatrix(ogreObject);
        }

        public Matrix4x4 getProjectionMatrix()
        {
            return Camera_getProjectionMatrix(ogreObject);
        }

        public void getFrustumExtents(out float outLeft, out float outRight, out float outTop, out float outBottom)
        {
            Camera_getFrustumExtents(ogreObject, out outLeft, out outRight, out outTop, out outBottom);
        }

        public void setFrustumExtents(float left, float right, float top, float bottom)
        {
            Camera_setFrustumExtents(ogreObject, left, right, top, bottom);
        }

        #region NativeWrapper

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setPosition(IntPtr camera, Vector3 position);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getPosition(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Quaternion Camera_getDerivedOrientation(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getDerivedPosition(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getDerivedDirection(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getDerivedUp(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getDerivedRight(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Quaternion Camera_getRealOrientation(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getRealPosition(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getRealDirection(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getRealUp(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getRealRight(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_lookAt(IntPtr camera, Vector3 lookAt);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setPolygonMode(IntPtr camera, PolygonMode mode);

        [DllImport("OgreCWrapper")]
        private static extern PolygonMode Camera_getPolygonMode(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setDirectionRaw(IntPtr camera, float x, float y, float z);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setDirection(IntPtr camera, Vector3 direction);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getDirection(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getUp(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Camera_getRight(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setLodBias(IntPtr camera, float factor);

        [DllImport("OgreCWrapper")]
        private static extern float Camera_getLodBias(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Ray3 Camera_getCameraToViewportRay(IntPtr camera, float screenx, float screeny);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setWindow(IntPtr camera, float left, float top, float right, float bottom);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_resetWindow(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern bool Camera_isWindowSet(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setAutoAspectRatio(IntPtr camera, bool autoRatio);

        [DllImport("OgreCWrapper")]
        private static extern bool Camera_getAutoAspectRatio(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern float Camera_getNearClipDistance(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern float Camera_getFarClipDistance(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setUseRenderingDistance(IntPtr camera, bool use);

        [DllImport("OgreCWrapper")]
        private static extern bool Camera_getUseRenderingDistance(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setFOVy(IntPtr camera, float fovy);

        [DllImport("OgreCWrapper")]
        private static extern float Camera_getFOVy(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setNearClipDistance(IntPtr camera, float nearDistance);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setFarClipDistance(IntPtr camera, float farDistance);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setAspectRatio(IntPtr camera, float ratio);

        [DllImport("OgreCWrapper")]
        private static extern float Camera_getAspectRatio(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setRenderingDistance(IntPtr camera, float dist);

        [DllImport("OgreCWrapper")]
        private static extern float Camera_getRenderingDistance(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern ProjectionType Camera_getProjectionType(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setProjectionType(IntPtr camera, ProjectionType type);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setOrthoWindow(IntPtr camera, float w, float h);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setOrthoWindowWidth(IntPtr camera, float w);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setOrthoWindowHeight(IntPtr camera, float h);

        [DllImport("OgreCWrapper")]
        private static extern float Camera_getOrthoWindowWidth(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern float Camera_getOrthoWindowHeight(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Matrix4x4 Camera_getViewMatrix(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern Matrix4x4 Camera_getProjectionMatrix(IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_getFrustumExtents(IntPtr camera, out float outLeft, out float outRight, out float outTop, out float outBottom);

        [DllImport("OgreCWrapper")]
        private static extern void Camera_setFrustumExtents(IntPtr camera, float left, float right, float top, float bottom);

        #endregion
    }
}
