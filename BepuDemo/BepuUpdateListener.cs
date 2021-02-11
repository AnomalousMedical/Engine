using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using BepuPhysics;
using BepuUtilities;
using BepuUtilities.Memory;
using BepuPhysics.Collidables;

namespace BepuDemo
{
    class BepuUpdateListener : UpdateListener, IDisposable
    {

        //Camera Settings
        float YFov = (float)Math.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        float camRotSpeed = 0f;

        //Clear Color
        Engine.Color ClearColor = new Engine.Color(0.032f, 0.032f, 0.032f, 1.0f);

        //Light
        Vector3 lightDirection = Vector3.Forward;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3.0f;

        private readonly NativeOSWindow window;
        private readonly Cube shape;
        private readonly TextureLoader textureLoader;
        private readonly CC0TextureLoader cc0TextureLoader;
        private readonly EnvironmentMapBuilder envMapBuilder;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private ISwapChain swapChain;
        private IRenderDevice renderDevice;
        private IDeviceContext immediateContext;
        private PbrRenderAttribs pbrRenderAttribs = PbrRenderAttribs.CreateDefault();

        private PbrRenderer pbrRenderer;
        private AutoPtr<ITextureView> environmentMapSRV;
        private AutoPtr<IShaderResourceBinding> pboMatBinding;

        //BEPU
        //If you intend to reuse the BufferPool, disposing the simulation is a good idea- it returns all the buffers to the pool for reuse.
        //Here, we dispose it, but it's not really required; we immediately thereafter clear the BufferPool of all held memory.
        //Note that failing to dispose buffer pools can result in memory leaks.
        Simulation simulation;
        SimpleThreadDispatcher threadDispatcher;
        BufferPool bufferPool;

        BodyHandle sphereHandle;
        StaticHandle staticBoxHandle;

        BodyReference bodRef;
        //END

        public unsafe BepuUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            PbrRenderer m_GLTFRenderer,
            Cube shape,
            TextureLoader textureLoader,
            CC0TextureLoader cc0TextureLoader,
            EnvironmentMapBuilder envMapBuilder,
            IPbrCameraAndLight pbrCameraAndLight)
        {
            this.pbrRenderer = m_GLTFRenderer;
            this.swapChain = graphicsEngine.SwapChain;
            this.renderDevice = graphicsEngine.RenderDevice;
            this.immediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.shape = shape;
            this.textureLoader = textureLoader;
            this.cc0TextureLoader = cc0TextureLoader;
            this.envMapBuilder = envMapBuilder;
            this.pbrCameraAndLight = pbrCameraAndLight;
            Initialize();
        }

        public void Dispose()
        {
            //If you intend to reuse the BufferPool, disposing the simulation is a good idea- it returns all the buffers to the pool for reuse.
            //Here, we dispose it, but it's not really required; we immediately thereafter clear the BufferPool of all held memory.
            //Note that failing to dispose buffer pools can result in memory leaks.
            simulation.Dispose();
            threadDispatcher.Dispose();
            bufferPool.Clear();

            pboMatBinding.Dispose();
            environmentMapSRV.Dispose();
        }

        unsafe void Initialize()
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(renderDevice, immediateContext, "papermill/Fixed-", "png");

            

            pbrRenderer.PrecomputeCubemaps(renderDevice, immediateContext, environmentMapSRV.Obj);


            //Only one of these
            //Load a cc0 texture
            LoadCCoTexture();

            SetupBepu();
        }

        private void SetupBepu()
        {
            //The buffer pool is a source of raw memory blobs for the engine to use.
            bufferPool = new BufferPool();
            //Note that you can also control the order of internal stage execution using a different ITimestepper implementation.
            //The PositionFirstTimestepper is the simplest timestepping mode in a technical sense, but since it integrates velocity into position at the start of the frame, 
            //directly modified velocities outside of the timestep will be integrated before collision detection or the solver has a chance to intervene.
            //PositionLastTimestepper avoids that by running collision detection and the solver first at the cost of a tiny amount of overhead.
            //(You could avoid the issue with PositionFirstTimestepper by modifying velocities in the PositionFirstTimestepper's BeforeCollisionDetection callback 
            //instead of outside the timestep, too, but it's a little more complicated.)
            simulation = Simulation.Create(bufferPool, new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, -10, 0)), new PositionLastTimestepper());

            //Drop a ball on a big static box.
            var sphere = new Sphere(1);
            sphere.ComputeInertia(1, out var sphereInertia);
            this.sphereHandle = simulation.Bodies.Add(BodyDescription.CreateDynamic(new System.Numerics.Vector3(0, 5, 0), sphereInertia, new CollidableDescription(simulation.Shapes.Add(sphere), 0.1f), new BodyActivityDescription(0.01f)));

            this.staticBoxHandle = simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, 0, 0), new CollidableDescription(simulation.Shapes.Add(new Box(500, 1, 500)), 0.1f)));

            threadDispatcher = new SimpleThreadDispatcher(Environment.ProcessorCount);

            bodRef = simulation.Bodies.GetBodyReference(sphereHandle);
        }

        private unsafe void LoadCCoTexture()
        {
            using var ccoTextures = cc0TextureLoader.LoadTextureSet("cc0Textures/Chainmail004_1K");
            pboMatBinding = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: ccoTextures.BaseColorMap,
                normalMap: ccoTextures.NormalMap,
                physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                aoMap: ccoTextures.AmbientOcclusionMap
            );
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public unsafe void sendUpdate(Clock clock)
        {
            //Multithreading is pretty pointless for a simulation of one ball, but passing a IThreadDispatcher instance is all you have to do to enable multithreading.
            //If you don't want to use multithreading, don't pass a IThreadDispatcher.
            simulation.Timestep(clock.DeltaSeconds, threadDispatcher); //Careful of variable timestep here, not so good
            
            //Render
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var PreTransform = swapChain.GetDesc_PreTransform;
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            var CameraView = Matrix4x4.Translation(0.0f, -4.0f, 15.0f); //For some reason camera is backward on x, y

            // Apply pretransform matrix that rotates the scene according the surface orientation
            CameraView *= CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);

            var CameraWorld = CameraView.inverse();

            // Get projection matrix adjusted to the current screen orientation
            var CameraProj = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);
            var CameraViewProj = CameraView * CameraProj;
            var CameraWorldPos = CameraWorld.GetTranslation();

            pbrCameraAndLight.SetCamera(ref CameraProj, ref CameraViewProj, ref CameraWorldPos);
            pbrCameraAndLight.SetLight(ref lightDirection, ref lightColor, lightIntensity);

            var bodOrientation = bodRef.Pose.Orientation;
            var bodPos = bodRef.Pose.Position;
            var rot = new Quaternion(bodOrientation.X, bodOrientation.Y, bodOrientation.Z, bodOrientation.W);
            var pos = new Vector3(bodPos.X, bodPos.Y, bodPos.Z);

            var CubeModelTransform = rot.toRotationMatrix4x4(pos);

            pbrRenderer.Begin(immediateContext);
            pbrRenderer.Render(immediateContext, pboMatBinding.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, PbrAlphaMode.ALPHA_MODE_OPAQUE, ref CubeModelTransform, pbrRenderAttribs);

            this.swapChain.Present(1);
        }
    }
}
