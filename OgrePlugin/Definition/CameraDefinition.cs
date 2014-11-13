using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using OgreWrapper;
using Engine.Reflection;
using Engine.ObjectManagement;

namespace OgrePlugin
{
    class CameraDefinition : MovableObjectDefinition
    {
        #region Static

        private static FilteredMemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static CameraDefinition()
        {
            memberScanner = new FilteredMemberScanner();
            memberScanner.ProcessFields = false;
            EditableAttributeFilter filter = new EditableAttributeFilter();
            filter.TerminatingType = typeof(MovableObjectDefinition);
            memberScanner.Filter = filter;
        }

        #endregion Static

        public CameraDefinition(String name)
            : base(name)
        {
            AutoAspectRatio = true;
            LodBias = 1.0f;
            UseRenderingDistance = true;
            NearClipDistance = 1.0f;
            FarClipDistance = 1000.0f;
            PolygonMode = PolygonMode.PM_SOLID;
            RenderDistance = 1000.0f;
            AspectRatio = 1.333333333333f;
            ProjectionType = ProjectionType.PT_PERSPECTIVE;
            OrthoWindowWidth = -1.0f;
            OrthoWindowHeight = -1.0f;
            FOVy = 45.0f;
        }

        public CameraDefinition(String name, Camera camera)
            :base(name, camera)
        {
            AutoAspectRatio = camera.getAutoAspectRatio();
            LodBias = camera.getLodBias();
            UseRenderingDistance = camera.getUseRenderingDistance();
            NearClipDistance = camera.getNearClipDistance();
            FarClipDistance = camera.getFarClipDistance();
            PolygonMode = camera.getPolygonMode();
            RenderDistance = camera.getRenderingDistance();
            AspectRatio = camera.getAspectRatio();
            ProjectionType = camera.getProjectionType();
            OrthoWindowWidth = camera.getOrthoWindowWidth();
            OrthoWindowHeight = camera.getOrthoWindowHeight();
            FOVy = camera.getFOVy();
        }

        protected override void setupEditInterface(EditInterface editInterface)
        {
            ReflectedEditInterface.expandEditInterface(this, memberScanner, editInterface);
            editInterface.IconReferenceTag = EngineIcons.Camera;
        }

        internal override MovableObjectContainer createActualProduct(OgreSceneManager scene, String baseName)
        {
            Camera camera = scene.SceneManager.createCamera(baseName + Name);
            camera.setAutoAspectRatio(AutoAspectRatio);
            camera.setLodBias(LodBias);
            camera.setUseRenderingDistance(UseRenderingDistance);
            camera.setNearClipDistance(NearClipDistance);
            camera.setFarClipDistance(FarClipDistance);
            camera.setPolygonMode(PolygonMode);
            camera.setRenderingDistance(RenderDistance);
            camera.setAspectRatio(AspectRatio);
            camera.setProjectionType(ProjectionType);
            camera.setFOVy(FOVy);
            if (OrthoWindowWidth != -1.0f && OrthoWindowHeight != -1.0f)
            {
                camera.setOrthoWindow(OrthoWindowWidth, OrthoWindowHeight);
            }
            else if (OrthoWindowHeight != -1.0f)
            {
                camera.setOrthoWindowHeight(OrthoWindowHeight);
            }
            else if (OrthoWindowWidth != -1.0f)
            {
                camera.setOrthoWindowWidth(OrthoWindowWidth);
            }
            return new CameraContainer(Name, camera);
        }

        /// <summary>
        /// True to automaticly compute the aspect ratio.
        /// </summary>
        [Editable]
        public bool AutoAspectRatio { get; set; }

        /// <summary>
        /// The level of detail bias, are objects more or less detailed closer/further away.
        /// </summary>
        [Editable]
        public float LodBias { get; set; }

        /// <summary>
        /// Whether this camera should use the 'rendering distance' on objects to exclude 
        /// distant objects from the final image.
        /// </summary>
        [Editable]
        public bool UseRenderingDistance { get; set; }

        /// <summary>
        /// Sets the position of the near clipping plane.
        /// </summary>
        [Editable]
        public float NearClipDistance { get; set; }

        /// <summary>
        /// Sets the position of the far clipping plane.
        /// </summary>
        [Editable]
        public float FarClipDistance { get; set; }

        /// <summary>
        /// Set the polygon mode of the camera.
        /// </summary>
        [Editable]
        public PolygonMode PolygonMode { get; set; }

        /// <summary>
        /// The furthest rendering distance.
        /// </summary>
        [Editable]
        public float RenderDistance { get; set; }

        /// <summary>
        /// The aspect ratio of the camera.
        /// </summary>
        [Editable]
        public float AspectRatio { get; set; }

        /// <summary>
        /// The projection type, Orthographic or Projection
        /// </summary>
        [Editable]
        public ProjectionType ProjectionType { get; set; }

        /// <summary>
        /// The width of the ortho window.  Leave at -1 to calculate based on height
        /// and aspect ratio.
        /// </summary>
        [Editable]
        public float OrthoWindowWidth { get; set; }

        /// <summary>
        /// The height of the ortho window.  Leave at -1 to calculate based on width
        /// and aspect ratio.
        /// </summary>
        [Editable]
        public float OrthoWindowHeight { get; set; }

        /// <summary>
        /// Sets the Y-dimension Field Of View (FOV) of the frustum. 
        ///     Field Of View (FOV) is the angle made between the frustum's position, and the edges 
        ///     of the 'screen' onto which the scene is projected. High values (90+ degrees) result 
        ///     in a wide-angle, fish-eye kind of view, low values (30- degrees) in a stretched, 
        ///     telescopic kind of view. Typical values are between 45 and 60 degrees. 
        ///
        ///     This value represents the VERTICAL field-of-view. The horizontal field of view is 
        ///     calculated from this depending on the dimensions of the viewport (they will only be 
        ///     the same if the viewport is square). 
        ///     
        ///     NOTE: This is stored in radians.
        /// </summary>
        [Editable]
        public float FOVy { get; set; }

        protected override String InterfaceName
        {
            get
            {
                return "Camera";
            }
        }

        #region Saveable Members

        private const String AUTO_ASPECT_RATIO = "AutoAspectRatio";
        private const String LOD_BIAS = "LodBias";
        private const String USE_RENDERING_DISTANCE = "UseRenderingDistance";
        private const String NEAR_CLIP_DISTANCE = "NearClipDistance";
        private const String FAR_CLIP_DISTANCE = "FarClipDistance";
        private const String POLYGON_MODE = "PolygonMode";
        private const String RENDER_DISTANCE = "RenderDistance";
        private const String ASPECT_RATIO = "AspectRatio";
        private const String PROJECTION_TYPE = "ProjectionType";
        private const String ORTHO_WIDTH = "OrthoWidth";
        private const String ORTHO_HEIGHT = "OrthoHeight";
        private const String FOVY = "FOVy";

        /// <summary>
        /// Deserialize constructor.
        /// </summary>
        /// <param name="info"></param>
        private CameraDefinition(LoadInfo info)
            :base(info)
        {
            AutoAspectRatio = info.GetBoolean(AUTO_ASPECT_RATIO);
            LodBias = info.GetFloat(LOD_BIAS);
            RenderDistance = info.GetFloat(USE_RENDERING_DISTANCE);
            NearClipDistance = info.GetFloat(NEAR_CLIP_DISTANCE);
            FarClipDistance = info.GetFloat(FAR_CLIP_DISTANCE);
            PolygonMode = info.GetValue<PolygonMode>(POLYGON_MODE);
            RenderDistance = info.GetFloat(RENDER_DISTANCE);
            AspectRatio = info.GetFloat(ASPECT_RATIO);
            ProjectionType = info.GetValue<ProjectionType>(PROJECTION_TYPE);
            OrthoWindowWidth = info.GetFloat(ORTHO_WIDTH);
            OrthoWindowHeight = info.GetFloat(ORTHO_HEIGHT);
            FOVy = info.GetFloat(FOVY);
        }

        /// <summary>
        /// Get the info to save for the subclass.
        /// </summary>
        /// <param name="info">The info to fill out.</param>
        protected override void getSpecificInfo(SaveInfo info)
        {
            info.AddValue(AUTO_ASPECT_RATIO, AutoAspectRatio);
            info.AddValue(LOD_BIAS, LodBias);
            info.AddValue(USE_RENDERING_DISTANCE, UseRenderingDistance);
            info.AddValue(NEAR_CLIP_DISTANCE, NearClipDistance);
            info.AddValue(FAR_CLIP_DISTANCE, FarClipDistance);
            info.AddValue(POLYGON_MODE, PolygonMode);
            info.AddValue(RENDER_DISTANCE, RenderDistance);
            info.AddValue(ASPECT_RATIO, AspectRatio);
            info.AddValue(PROJECTION_TYPE, ProjectionType);
            info.AddValue(ORTHO_WIDTH, OrthoWindowWidth);
            info.AddValue(ORTHO_HEIGHT, OrthoWindowHeight);
            info.AddValue(FOVY, FOVy);
        }

        #endregion
    }
}
