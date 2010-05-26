using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class SceneManager : IDisposable
    {
        [SingleEnum]
	    public enum IlluminationRenderStage : uint
	    {
		    IRS_NONE,
		    IRS_RENDER_TO_TEXTURE,
		    IRS_RENDER_RECEIVER_PASS
	    };

        internal static SceneManager createWrapper(IntPtr nativePointer, object[] args)
        {
            return new SceneManager(nativePointer);
        }

        IntPtr ogreSceneManager;
        WrapperCollection<Camera> cameras = new WrapperCollection<Camera>(Camera.createWrapper);
        WrapperCollection<Light> lights = new WrapperCollection<Light>(Light.createWrapper);
        WrapperCollection<SceneNode> sceneNodes = new WrapperCollection<SceneNode>(SceneNode.createWrapper);
        WrapperCollection<Entity> entities = new WrapperCollection<Entity>(Entity.createWrapper);
        WrapperCollection<ManualObject> manualObjects = new WrapperCollection<ManualObject>(ManualObject.createWrapper);
        RenderQueue renderQueue;
        ManagedSceneListener sceneListener;

        protected SceneManager(IntPtr ogreSceneManager)
        {
            this.ogreSceneManager = ogreSceneManager;
            renderQueue = new RenderQueue(SceneManager_getRenderQueue(ogreSceneManager));
            sceneListener = new ManagedSceneListener(this);
            SceneManager_addSceneListener(ogreSceneManager, sceneListener.NativeSceneListener);
        }

        public void Dispose()
        {
            SceneManager_addSceneListener(ogreSceneManager, sceneListener.NativeSceneListener);
            sceneListener.Dispose();
            ogreSceneManager = IntPtr.Zero;
            cameras.Dispose();
            lights.Dispose();
            sceneNodes.Dispose();
            entities.Dispose();
            manualObjects.Dispose();
            renderQueue.Dispose();
        }

        internal IntPtr OgreSceneManager
        {
            get
            {
                return ogreSceneManager;
            }
        }

        /// <summary>
	    /// Gets the name of the scene manager.
	    /// </summary>
	    /// <returns>The name of the scene manager.</returns>
        public String getName()
        {
            return Marshal.PtrToStringAnsi(SceneManager_getName(ogreSceneManager));
        }

	    /// <summary>
	    /// Creates a camera managed by this SceneManager.
	    /// </summary>
	    /// <param name="name">The name of the camera.</param>
	    /// <returns>A new Camera wrapping the native camera.</returns>
        public Camera createCamera(String name)
        {
            return cameras.getObject(SceneManager_createCamera(ogreSceneManager, name));
        }

	    /// <summary>
	    /// Gets the named Camera.
	    /// </summary>
	    /// <param name="name">The name of the camera.</param>
	    /// <returns>The Camera if the scene contains the camera, or null if it does not.</returns>
        public Camera getCamera(String name)
        {
            return cameras.getObject(SceneManager_getCamera(ogreSceneManager, name));
        }

	    /// <summary>
	    /// Returns whether a camera with the given name exists.
	    /// </summary>
	    /// <param name="name">The name of the camera.</param>
	    /// <returns>True if the camera exists.  False if it does not.</returns>
        public bool hasCamera(String name)
        {
            return SceneManager_hasCamera(ogreSceneManager, name);
        }

	    /// <summary>
	    /// Destroys the passed camera
	    /// </summary>
	    /// <param name="camera">The camera to destroy.</param>
        public void destroyCamera(Camera camera)
        {
            cameras.destroyObject(camera.OgreObject);
            SceneManager_destroyCamera(ogreSceneManager, camera.OgreObject);
        }

	    /// <summary>
	    /// Creates a new light managed by this SceneManager.
	    /// </summary>
	    /// <param name="name">The name of the light.</param>
	    /// <returns>A new Light wrapping a native light.</returns>
        public Light createLight(String name)
        {
            return lights.getObject(SceneManager_createLight(ogreSceneManager, name));
        }

	    /// <summary>
	    /// Gets the named Light.
	    /// </summary>
	    /// <param name="name">The name of the light.</param>
	    /// <returns>The Light if the scene contains the light, or null if it does not.</returns>
        public Light getLight(String name)
        {
            return lights.getObject(SceneManager_getLight(ogreSceneManager, name));
        }

	    /// <summary>
	    /// Returns whether a light with the given name exists.
	    /// </summary>
	    /// <param name="name">The name of the light.</param>
	    /// <returns>True if the light exists.  False if it does not.</returns>
        public bool hasLight(String name)
        {
            return SceneManager_hasLight(ogreSceneManager, name);
        }

	    /// <summary>
	    /// Destroys the passed light
	    /// </summary>
	    /// <param name="light">The light to destroy.</param>
        public void destroyLight(Light light)
        {
            lights.destroyObject(light.OgreObject);
            SceneManager_destroyLight(ogreSceneManager, light.OgreObject);
        }

	    /// <summary>
	    /// Creates a new SceneNode with the given name.
	    /// </summary>
	    /// <param name="name">The name of the render node.</param>
	    /// <returns>A new render node with the given name.</returns>
        public SceneNode createSceneNode(String name)
        {
            return sceneNodes.getObject(SceneManager_createSceneNode(ogreSceneManager, name));
        }

	    /// <summary>
	    /// Gets the SceneNode at the root of the scene graph.
	    /// </summary>
	    /// <returns>The root SceneNode.</returns>
        public SceneNode getRootSceneNode()
        {
            return sceneNodes.getObject(SceneManager_getRootSceneNode(ogreSceneManager));
        }

	    /// <summary>
	    /// Gets the named render node.
	    /// </summary>
	    /// <param name="name">The name of the node.</param>
	    /// <returns>The Render node if it exists or else null.</returns>
        public SceneNode getSceneNode(String name)
        {
            return sceneNodes.getObject(SceneManager_getSceneNode(ogreSceneManager, name));
        }

	    /// <summary>
	    /// Returns whether a SceneNode with the given name exists.
	    /// </summary>
	    /// <param name="name">The name of the node.</param>
	    /// <returns>True if the node exists.  False if it does not.</returns>
        public bool hasSceneNode(String name)
        {
            return SceneManager_hasSceneNode(ogreSceneManager, name);
        }

	    /// <summary>
	    /// Destroys the render node passed.  You cannot destroy the root node using this
	    /// method it will be destroyed automaticly.
	    /// </summary>
	    /// <param name="node">The SceneNode to destroy.</param>
        public void destroySceneNode(SceneNode node)
        {
            sceneNodes.destroyObject(node.OgreNode);
            SceneManager_destroySceneNode(ogreSceneManager, node.OgreNode);
        }

	    /// <summary>
	    /// Create a Entity with the given name using the given mesh.
	    /// </summary>
	    /// <param name="entityName">The name of the entity.</param>
	    /// <param name="meshName">The name of the mesh to use on this entity.</param>
	    /// <returns>The new Entity.</returns>
        public Entity createEntity(String entityName, String meshName)
        {
            return entities.getObject(SceneManager_createEntity(ogreSceneManager, entityName, meshName));
        }

	    /// <summary>
	    /// Get the named Entity.
	    /// </summary>
	    /// <param name="name">The name of the render entity.</param>
	    /// <returns>The Entity if it exists or null if it does not.</returns>
        public Entity getEntity(String name)
        {
            return entities.getObject(SceneManager_getEntity(ogreSceneManager, name));
        }
    	
	    /// <summary>
	    /// Returns whether a Entity with the given name exists.
	    /// </summary>
	    /// <param name="name">The name of the render entity.</param>
	    /// <returns>True if the entity exists.  False if it does not.</returns>
        public bool hasEntity(String name)
        {
            return SceneManager_hasEntity(ogreSceneManager, name);
        }

	    /// <summary>
	    /// Destroys the Entity passed.
	    /// </summary>
	    /// <param name="entity">The Entity to destroy.</param>
        public void destroyEntity(Entity entity)
        {
            entities.destroyObject(entity.OgreObject);
            SceneManager_destroyEntity(ogreSceneManager, entity.OgreObject);
        }

	    /// <summary>
	    /// Create a ManualObject, an object which you populate with geometry manually through a 
	    /// GL immediate-mode style interface. 
	    /// </summary>
	    /// <param name="name">The name to be given to the object (must be unique). </param>
	    /// <returns>The newly created manual object.</returns>
        public ManualObject createManualObject(String name)
        {
            return manualObjects.getObject(SceneManager_createManualObject(ogreSceneManager, name));
        }

	    /// <summary>
	    /// Retrieves a pointer to the named ManualObject. 
	    /// </summary>
	    /// <param name="name">The name of the object to look for.</param>
	    /// <returns>The manual object identified by name or null if it is not found.</returns>
        public ManualObject getManualObject(String name)
        {
            return manualObjects.getObject(SceneManager_getManualObject(ogreSceneManager, name));
        }

	    /// <summary>
	    /// Returns whether a manual object with the given name exists. 
	    /// </summary>
	    /// <param name="name">The name to search for.</param>
	    /// <returns>True if the object is in the scene.  False if it is not.</returns>
        public bool hasManualObject(String name)
        {
            return SceneManager_hasManualObject(ogreSceneManager, name);
        }

	    /// <summary>
	    /// Removes and destroys a ManualObject from the SceneManager.
	    /// </summary>
	    /// <param name="obj">The object to destroy.</param>
        public void destroyManualObject(ManualObject obj)
        {
            manualObjects.destroyObject(obj.OgreObject);
            SceneManager_destroyManualObject(ogreSceneManager, obj.OgreObject);
        }

	    /// <summary>
	    /// Sets a mask which is bitwise 'and'ed with objects own visibility masks to determine if 
	    /// the object is visible. Note that this is combined with any per-viewport visibility mask 
	    /// through an 'and' operation.
	    /// </summary>
	    /// <param name="mask">The mask to set.</param>
        public void setVisibilityMask(uint mask)
        {
            SceneManager_setVisibilityMask(ogreSceneManager, mask);
        }

	    /// <summary>
	    /// Gets a mask which is bitwise 'and'ed with objects own visibility masks
	    /// to determine if the object is visible.
	    /// </summary>
	    /// <returns>The visibility mask.</returns>
        public uint getVisibilityMask()
        {
            return SceneManager_getVisibilityMask(ogreSceneManager);
        }

	    /// <summary>
	    /// Add a listener which will get called back on scene manager events. 
	    /// </summary>
	    /// <param name="listener">The listener to add.</param>
        public void addSceneListener(SceneListener listener)
        {
            sceneListener.addSceneListener(listener);
        }

	    /// <summary>
	    /// Remove a listener.
	    /// </summary>
	    /// <param name="listener">The listener to remove.</param>
        public void removeSceneListener(SceneListener listener)
        {
            sceneListener.removeSceneListener(listener);
        }

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
        public RaySceneQuery createRaySceneQuery(Ray3 ray, uint mask)
        {
            throw new NotImplementedException();
            //return SceneManager_createRaySceneQuery(ogreSceneManager);
        }

	    /// <summary>
	    /// Destroys a ray scene query. 
	    /// </summary>
	    /// <param name="query">The query to destroy.</param>
        public void destroyQuery(RaySceneQuery query)
        {
            throw new NotImplementedException();
            //SceneManager_destroyQuery(ogreSceneManager);
        }

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
        public PlaneBoundedVolumeListSceneQuery createPlaneBoundedVolumeQuery(LinkedList<PlaneBoundedVolume> volumes)
        {
            throw new NotImplementedException();
            //return SceneManager_createPlaneBoundedVolumeQuery(ogreSceneManager);
        }

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
        public PlaneBoundedVolumeListSceneQuery createPlaneBoundedVolumeQuery(LinkedList<PlaneBoundedVolume> volumes, ulong mask)
        {
            throw new NotImplementedException();
            //return SceneManager_createPlaneBoundedVolumeQuery(ogreSceneManager);
        }

	    /// <summary>
	    /// Destroys a Plane Bounded Volume scene query. 
	    /// </summary>
	    /// <param name="query">The query to destroy.</param>
        public void destroyQuery(PlaneBoundedVolumeListSceneQuery query)
        {
            throw new NotImplementedException();
            //SceneManager_destroyQuery(ogreSceneManager, query.NavtiveQuery);
        }

	    /// <summary>
	    /// Tells the SceneManager whether it should render the SceneNodes which
        /// make up the scene as well as the objects in the scene.
        /// 
	    /// This method is mainly for debugging purposes. If you set this to 'true',
        /// each node will be rendered as a set of 3 axes to allow you to easily see
        /// the orientation of the nodes. 
	    /// </summary>
	    /// <param name="display">True to display scene nodes.  False to disable.</param>
        public void setDisplaySceneNodes(bool display)
        {
            SceneManager_setDisplaySceneNodes(ogreSceneManager, display);
        }

	    /// <summary>
	    /// Determine if all scene nodes axis are to be displayed. 
	    /// </summary>
	    /// <returns>True if all scene nodes axis are to be displayed.</returns>
        public bool getDisplaySceneNodes()
        {
            return SceneManager_getDisplaySceneNodes(ogreSceneManager);
        }

	    /// <summary>
	    /// Retrieves the internal render queue, for advanced users only. 
	    /// </summary>
	    /// <remarks>
	    /// The render queue is mainly used internally to manage the scene object
        /// rendering queue, it also exports some methods to allow advanced users to
        /// configure the behavior of rendering process. Most methods provided by
        /// RenderQueue are supposed to be used internally only, you should
        /// reference to the RenderQueue API for more information. Do not access
        /// this directly unless you know what you are doing. 
	    /// </remarks>
	    /// <returns>The RenderQueue for this SceneManager.</returns>
        public RenderQueue getRenderQueue()
        {
            return renderQueue;
        }

	    /// <summary>
	    /// Allows all bounding boxes of scene nodes to be displayed. 
	    /// </summary>
	    /// <param name="bShow">True to enable bounding box rendering.</param>
        public void showBoundingBoxes(bool bShow)
        {
            SceneManager_showBoundingBoxes(ogreSceneManager, bShow);
        }

	    /// <summary>
	    /// Determine if all bounding boxes of scene nodes are to be displayed. 
	    /// </summary>
	    /// <returns>Returns true if all bounding boxes of scene nodes are to be displayed. </returns>
        public bool getShowBoundingBoxes()
        {
            return SceneManager_getShowBoundingBoxes(ogreSceneManager);
        }

        public void setShadowTechnique(ShadowTechnique technique)
        {
            SceneManager_setShadowTechnique(ogreSceneManager, technique);
        }

        public ShadowTechnique getShadowTechnique()
        {
            return SceneManager_getShadowTechnique(ogreSceneManager);
        }

	    /// <summary>
	    /// Enables / disables the rendering of debug information for shadows.
	    /// </summary>
	    /// <param name="debug">True to enable debug shadows.  False to disable.</param>
        public void setShowDebugShadows(bool debug)
        {
            SceneManager_setShowDebugShadows(ogreSceneManager, debug);
        }

	    /// <summary>
	    /// Determine if debug shadows are being rendererd.
	    /// </summary>
	    /// <returns>True if shadows are being rendered.</returns>
        public bool getShowDebugShadows()
        {
            return SceneManager_getShowDebugShadows(ogreSceneManager);
        }

        public void setShadowColor(Color color)
        {
            SceneManager_setShadowColor(ogreSceneManager, color);
        }

        public Color getShadowColor()
        {
            return SceneManager_getShadowColor(ogreSceneManager);
        }

        public void setShadowDirectionalLightExtrusionDistance(float dist)
        {
            SceneManager_setShadowDirectionalLightExtrusionDistance(ogreSceneManager, dist);
        }

        public float getShadowDirectionalLightExtrusionDistance()
        {
            return SceneManager_getShadowDirectionalLightExtrusionDistance(ogreSceneManager);
        }

        public void setShadowFarDistance(float distance)
        {
            SceneManager_setShadowFarDistance(ogreSceneManager, distance);
        }

        public float getShadowFarDistance()
        {
            return SceneManager_getShadowFarDistance(ogreSceneManager);
        }

        public float getShadowFarDistanceSquared()
        {
            return SceneManager_getShadowFarDistanceSquared(ogreSceneManager);
        }

        public void setShadowIndexBufferSize(int size)
        {
            SceneManager_setShadowIndexBufferSize(ogreSceneManager, size);
        }

        public int getShadowIndexBufferSize()
        {
            return SceneManager_getShadowIndexBufferSize(ogreSceneManager);
        }

        public void setShadowTextureSize(ushort size)
        {
            SceneManager_setShadowTextureSize(ogreSceneManager, size);
        }

        public void setShadowTextureConfig(int shadowIndex, ushort width, ushort height, PixelFormat format)
        {
            SceneManager_setShadowTextureConfig(ogreSceneManager, shadowIndex, width, height, format);
        }

        public void setShadowTexturePixelFormat(PixelFormat fmt)
        {
            SceneManager_setShadowTexturePixelFormat(ogreSceneManager, fmt);
        }

        public void setShadowTextureCount(int count)
        {
            SceneManager_setShadowTextureCount(ogreSceneManager, count);
        }

        public int getShadowTextureCount()
        {
            return SceneManager_getShadowTextureCount(ogreSceneManager);
        }

        public void setShadowTextureCountPerLightType(Light.LightTypes type, int count)
        {
            SceneManager_setShadowTextureCountPerLightType(ogreSceneManager, type, count);
        }

        public int getShadowTextureCountPerLightType(Light.LightTypes type)
        {
            return SceneManager_getShadowTextureCountPerLightType(ogreSceneManager, type);
        }

        public void setShadowTextureSettings(ushort size, ushort count, PixelFormat fmt)
        {
            SceneManager_setShadowTextureSettings(ogreSceneManager, size, count, fmt);
        }

	    //getShadowTexture

        public void setShadowDirLightTextureOffset(float offset)
        {
            SceneManager_setShadowDirLightTextureOffset(ogreSceneManager, offset);
        }

        public float getShadowDirLightTextureOffset()
        {
            return SceneManager_getShadowDirLightTextureOffset(ogreSceneManager);
        }

        public void setShadowTextureFadeStart(float fadeStart)
        {
            SceneManager_setShadowTextureFadeStart(ogreSceneManager, fadeStart);
        }

        public void setShadowTextureFadeEnd(float fadeEnd)
        {
            SceneManager_setShadowTextureFadeEnd(ogreSceneManager, fadeEnd);
        }

        public void setShadowTextureSelfShadow(bool selfShadow)
        {
            SceneManager_setShadowTextureSelfShadow(ogreSceneManager, selfShadow);
        }

        public bool getShadowTextureSelfShadow()
        {
            return SceneManager_getShadowTextureSelfShadow(ogreSceneManager);
        }

        public void setShadowTextureCasterMaterial(String name)
        {
            SceneManager_setShadowTextureCasterMaterial(ogreSceneManager, name);
        }

        public void setShadowTextureReceiverMaterial(String name)
        {
            SceneManager_setShadowTextureReceiverMaterial(ogreSceneManager, name);
        }

        public void setShadowCasterRenderBackFaces(bool bf)
        {
            SceneManager_setShadowCasterRenderBackFaces(ogreSceneManager, bf);
        }

        public bool getShadowCasterRenderBackFaces()
        {
            return SceneManager_getShadowCasterRenderBackFaces(ogreSceneManager);
        }
    	
	    //void setShadowCameraSetup (const ShadowCameraSetupPtr &shadowSetup)
    	
	    //virtual constShadowCameraSetupPtr & 	getShadowCameraSetup () const

        public void setShadowUseInfiniteFarPlane(bool enable)
        {
            SceneManager_setShadowUseInfiniteFarPlane(ogreSceneManager, enable);
        }

        public bool isShadowTechniqueStencilBased()
        {
            return SceneManager_isShadowTechniqueStencilBased(ogreSceneManager);
        }

        public bool isShadowTechniqueTextureBased()
        {
            return SceneManager_isShadowTechniqueTextureBased(ogreSceneManager);
        }

        public bool isShadowTechniqueModulative()
        {
            return SceneManager_isShadowTechniqueModulative(ogreSceneManager);
        }

        public bool isShadowTechniqueAdditive()
        {
            return SceneManager_isShadowTechniqueAdditive(ogreSceneManager);
        }

        public bool isShadowTechniqueIntegrated()
        {
            return SceneManager_isShadowTechniqueIntegrated(ogreSceneManager);
        }

        public bool isShadowTechniqueInUse()
        {
            return SceneManager_isShadowTechniqueInUse(ogreSceneManager);
        }

        public void setShadowUseLightClipPlanes(bool enabled)
        {
            SceneManager_setShadowUseLightClipPlanes(ogreSceneManager, enabled);
        }

        public bool getShadowUseLightClipPlanes()
        {
            return SceneManager_getShadowUseLightClipPlanes(ogreSceneManager);
        }

        public void setSkyPlane(bool enabled, float d, Vector3 normal, String matName, float scale, float tiling, bool drawFirst, int bow)
        {
            SceneManager_setSkyPlane(ogreSceneManager, enabled, d, normal, matName, scale, tiling, drawFirst, bow);
        }

        public void setSkyBox(bool enabled, String matName, float distance, bool drawFirst)
        {
            SceneManager_setSkyBox(ogreSceneManager, enabled, matName, distance, drawFirst);
        }

        public void setSkyDome(bool enabled, String matName)
        {
            SceneManager_setSkyDome(ogreSceneManager, enabled, matName);
        }

        #region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_getRenderQueue(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_getName(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_createCamera(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_getCamera(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_hasCamera(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_destroyCamera(IntPtr ogreSceneManager, IntPtr camera);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_createLight(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_getLight(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_hasLight(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_destroyLight(IntPtr ogreSceneManager, IntPtr light);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_createSceneNode(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_getRootSceneNode(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_getSceneNode(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_hasSceneNode(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_destroySceneNode(IntPtr ogreSceneManager, IntPtr node);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_createEntity(IntPtr ogreSceneManager, String entityName, String meshName);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_getEntity(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_hasEntity(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_destroyEntity(IntPtr ogreSceneManager, IntPtr entity);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_createManualObject(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_getManualObject(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_hasManualObject(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_destroyManualObject(IntPtr ogreSceneManager, IntPtr obj);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setVisibilityMask(IntPtr ogreSceneManager, uint mask);

        [DllImport("OgreCWrapper")]
        private static extern uint SceneManager_getVisibilityMask(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_addSceneListener(IntPtr ogreSceneManager, IntPtr listener);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_removeSceneListener(IntPtr ogreSceneManager, IntPtr listener);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr SceneManager_createRaySceneQuery(IntPtr ogreSceneManager, Ray3 ray, uint mask);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_destroyRayQuery(IntPtr ogreSceneManager, IntPtr query);

        //[DllImport("OgreCWrapper")]
        //private static extern IntPtr SceneManager_createPlaneBoundedVolumeQuery(IntPtr ogreSceneManager, LinkedList<PlaneBoundedVolume> volumes);

        //[DllImport("OgreCWrapper")]
        //private static extern IntPtr SceneManager_createPlaneBoundedVolumeQuery(IntPtr ogreSceneManager, LinkedList<PlaneBoundedVolume> volumes, ulong mask);

        //[DllImport("OgreCWrapper")]
        //private static extern void SceneManager_destroyPlaneBoundedVolumeListQuery(IntPtr ogreSceneManager, IntPtr query);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setDisplaySceneNodes(IntPtr ogreSceneManager, bool display);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_getDisplaySceneNodes(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_showBoundingBoxes(IntPtr ogreSceneManager, bool bShow);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_getShowBoundingBoxes(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTechnique(IntPtr ogreSceneManager, ShadowTechnique technique);

        [DllImport("OgreCWrapper")]
        private static extern ShadowTechnique SceneManager_getShadowTechnique(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShowDebugShadows(IntPtr ogreSceneManager, bool debug);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_getShowDebugShadows(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowColor(IntPtr ogreSceneManager, Color color);

        [DllImport("OgreCWrapper")]
        private static extern Color SceneManager_getShadowColor(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowDirectionalLightExtrusionDistance(IntPtr ogreSceneManager, float dist);

        [DllImport("OgreCWrapper")]
        private static extern float SceneManager_getShadowDirectionalLightExtrusionDistance(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowFarDistance(IntPtr ogreSceneManager, float distance);

        [DllImport("OgreCWrapper")]
        private static extern float SceneManager_getShadowFarDistance(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern float SceneManager_getShadowFarDistanceSquared(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowIndexBufferSize(IntPtr ogreSceneManager, int size);

        [DllImport("OgreCWrapper")]
        private static extern int SceneManager_getShadowIndexBufferSize(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureSize(IntPtr ogreSceneManager, ushort size);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureConfig(IntPtr ogreSceneManager, int shadowIndex, ushort width, ushort height, PixelFormat format);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTexturePixelFormat(IntPtr ogreSceneManager, PixelFormat fmt);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureCount(IntPtr ogreSceneManager, int count);

        [DllImport("OgreCWrapper")]
        private static extern int SceneManager_getShadowTextureCount(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureCountPerLightType(IntPtr ogreSceneManager, Light.LightTypes type, int count);

        [DllImport("OgreCWrapper")]
        private static extern int SceneManager_getShadowTextureCountPerLightType(IntPtr ogreSceneManager, Light.LightTypes type);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureSettings(IntPtr ogreSceneManager, ushort size, ushort count, PixelFormat fmt);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowDirLightTextureOffset(IntPtr ogreSceneManager, float offset);

        [DllImport("OgreCWrapper")]
        private static extern float SceneManager_getShadowDirLightTextureOffset(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureFadeStart(IntPtr ogreSceneManager, float fadeStart);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureFadeEnd(IntPtr ogreSceneManager, float fadeEnd);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureSelfShadow(IntPtr ogreSceneManager, bool selfShadow);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_getShadowTextureSelfShadow(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureCasterMaterial(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowTextureReceiverMaterial(IntPtr ogreSceneManager, String name);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowCasterRenderBackFaces(IntPtr ogreSceneManager, bool bf);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_getShadowCasterRenderBackFaces(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowUseInfiniteFarPlane(IntPtr ogreSceneManager, bool enable);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_isShadowTechniqueStencilBased(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_isShadowTechniqueTextureBased(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_isShadowTechniqueModulative(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_isShadowTechniqueAdditive(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_isShadowTechniqueIntegrated(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_isShadowTechniqueInUse(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setShadowUseLightClipPlanes(IntPtr ogreSceneManager, bool enabled);

        [DllImport("OgreCWrapper")]
        private static extern bool SceneManager_getShadowUseLightClipPlanes(IntPtr ogreSceneManager);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setSkyPlane(IntPtr ogreSceneManager, bool enabled, float d, Vector3 normal, String matName, float scale, float tiling, bool drawFirst, float bow);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setSkyBox(IntPtr ogreSceneManager, bool enabled, String matName, float distance, bool drawFirst);

        [DllImport("OgreCWrapper")]
        private static extern void SceneManager_setSkyDome(IntPtr ogreSceneManager, bool enabled, String matName);

        #endregion
    }

    public enum ShadowTechnique
    {
        /** No shadows */
        SHADOWTYPE_NONE = 0x00,
	    /** Mask for additive shadows (not for direct use, use  SHADOWTYPE_ enum instead)
	    */
	    SHADOWDETAILTYPE_ADDITIVE = 0x01,
	    /** Mask for modulative shadows (not for direct use, use  SHADOWTYPE_ enum instead)
	    */
	    SHADOWDETAILTYPE_MODULATIVE = 0x02,
	    /** Mask for integrated shadows (not for direct use, use SHADOWTYPE_ enum instead)
	    */
	    SHADOWDETAILTYPE_INTEGRATED = 0x04,
	    /** Mask for stencil shadows (not for direct use, use  SHADOWTYPE_ enum instead)
	    */
	    SHADOWDETAILTYPE_STENCIL = 0x10,
	    /** Mask for texture shadows (not for direct use, use  SHADOWTYPE_ enum instead)
	    */
	    SHADOWDETAILTYPE_TEXTURE = 0x20,
    	
        /** Stencil shadow technique which renders all shadow volumes as
            a modulation after all the non-transparent areas have been 
            rendered. This technique is considerably less fillrate intensive 
            than the additive stencil shadow approach when there are multiple
            lights, but is not an accurate model. 
        */
        SHADOWTYPE_STENCIL_MODULATIVE = 0x12,
        /** Stencil shadow technique which renders each light as a separate
            additive pass to the scene. This technique can be very fillrate
            intensive because it requires at least 2 passes of the entire
            scene, more if there are multiple lights. However, it is a more
            accurate model than the modulative stencil approach and this is
            especially apparent when using coloured lights or bump mapping.
        */
        SHADOWTYPE_STENCIL_ADDITIVE = 0x11,
        /** Texture-based shadow technique which involves a monochrome render-to-texture
            of the shadow caster and a projection of that texture onto the 
            shadow receivers as a modulative pass. 
        */
        SHADOWTYPE_TEXTURE_MODULATIVE = 0x22,
    	
        /** Texture-based shadow technique which involves a render-to-texture
            of the shadow caster and a projection of that texture onto the 
            shadow receivers, built up per light as additive passes. 
		    This technique can be very fillrate intensive because it requires numLights + 2 
		    passes of the entire scene. However, it is a more accurate model than the 
		    modulative approach and this is especially apparent when using coloured lights 
		    or bump mapping.
        */
        SHADOWTYPE_TEXTURE_ADDITIVE = 0x21,

	    /** Texture-based shadow technique which involves a render-to-texture
	    of the shadow caster and a projection of that texture on to the shadow
	    receivers, with the usage of those shadow textures completely controlled
	    by the materials of the receivers.
	    This technique is easily the most flexible of all techniques because 
	    the material author is in complete control over how the shadows are
	    combined with regular rendering. It can perform shadows as accurately
	    as SHADOWTYPE_TEXTURE_ADDITIVE but more efficiently because it requires
	    less passes. However it also requires more expertise to use, and 
	    in almost all cases, shader capable hardware to really use to the full.
	    @note The 'additive' part of this mode means that the colour of
	    the rendered shadow texture is by default plain black. It does
	    not mean it does the adding on your receivers automatically though, how you
	    use that result is up to you.
	    */
	    SHADOWTYPE_TEXTURE_ADDITIVE_INTEGRATED = 0x25,
	    /** Texture-based shadow technique which involves a render-to-texture
		    of the shadow caster and a projection of that texture on to the shadow
		    receivers, with the usage of those shadow textures completely controlled
		    by the materials of the receivers.
		    This technique is easily the most flexible of all techniques because 
		    the material author is in complete control over how the shadows are
		    combined with regular rendering. It can perform shadows as accurately
		    as SHADOWTYPE_TEXTURE_ADDITIVE but more efficiently because it requires
		    less passes. However it also requires more expertise to use, and 
		    in almost all cases, shader capable hardware to really use to the full.
		    @note The 'modulative' part of this mode means that the colour of
		    the rendered shadow texture is by default the 'shadow colour'. It does
		    not mean it modulates on your receivers automatically though, how you
		    use that result is up to you.
	    */
	    SHADOWTYPE_TEXTURE_MODULATIVE_INTEGRATED = 0x26
    };
}
