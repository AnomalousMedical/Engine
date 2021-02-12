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
using Microsoft.Extensions.Logging;

namespace BepuDemo
{
    class BepuUpdateListener : UpdateListener, IDisposable
    {
        private const string CC0TexturePath = "cc0Textures/Pipe002_1K";

        //Camera Settings
        float YFov = (float)Math.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;
        Vector3 camPos = new Vector3(0, 2, -11);

        private const float HALF_PI = (float)Math.PI / 2.0f - 0.001f;
        Quaternion camRot = Quaternion.Identity;

        //Clear Color
        Engine.Color ClearColor = new Engine.Color(0.032f, 0.032f, 0.032f, 1.0f);

        //Light
        Vector3 lightDirection = Vector3.Forward;
        Vector4 lightColor = new Vector4(0, 0, 0, 0);
        float lightIntensity = 3f;

        private readonly NativeOSWindow window;
        private readonly Cube shape;
        private readonly TextureLoader textureLoader;
        private readonly CC0TextureLoader cc0TextureLoader;
        private readonly EnvironmentMapBuilder envMapBuilder;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly ILogger<BepuUpdateListener> logger;
        private readonly EventManager eventManager;
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

        Box boxShape;
        BodyInertia boxInertia;
        private List<BodyPositionSync> spherePositionSyncs = new List<BodyPositionSync>();
        //END

        ButtonEvent moveForward = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_W });
        ButtonEvent moveBackward = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_S });
        ButtonEvent moveLeft = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_A });
        ButtonEvent moveRight = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_D });
        ButtonEvent moveUp = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_E });
        ButtonEvent moveDown = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_Q });
        ButtonEvent pitchUp = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_UP });
        ButtonEvent pitchDown = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_DOWN });
        ButtonEvent yawLeft = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_LEFT });
        ButtonEvent yawRight = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_RIGHT });

        public unsafe BepuUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            PbrRenderer m_GLTFRenderer,
            Cube shape,
            TextureLoader textureLoader,
            CC0TextureLoader cc0TextureLoader,
            EnvironmentMapBuilder envMapBuilder,
            IPbrCameraAndLight pbrCameraAndLight,
            ILogger<BepuUpdateListener> logger,
            EventManager eventManager)
        {
            eventManager.addEvent(moveForward);
            eventManager.addEvent(moveBackward);
            eventManager.addEvent(moveLeft);
            eventManager.addEvent(moveRight);
            eventManager.addEvent(moveUp);
            eventManager.addEvent(moveDown);
            eventManager.addEvent(pitchUp);
            eventManager.addEvent(pitchDown);
            eventManager.addEvent(yawLeft);
            eventManager.addEvent(yawRight);

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
            this.logger = logger;
            this.eventManager = eventManager;
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
            //CreateShinyTexture();

            SetupBepu();
        }

        private unsafe void CreateShinyTexture()
        {
            const uint texDim = 10;
            var physDesc = new UInt32[texDim * texDim];
            var physDescSpan = new Span<UInt32>(physDesc);
            physDescSpan.Fill(0xff0000ff);

            var TexDesc = new TextureDesc();
            TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            TexDesc.Usage = USAGE.USAGE_IMMUTABLE;
            TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE;
            TexDesc.Depth = 1;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;
            TexDesc.MipLevels = 1;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;
            TexDesc.Width = 10;
            TexDesc.Height = 10;

            fixed (UInt32* pPhysDesc = physDesc)
            {
                var Level0Data = new TextureSubResData { pData = new IntPtr(pPhysDesc), Stride = texDim * 4 };
                var InitData = new TextureData { pSubResources = new List<TextureSubResData> { Level0Data } };

                using var physicalDescriptorMap = renderDevice.CreateTexture(TexDesc, InitData);

                pboMatBinding = pbrRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    physicalDescriptorMap: physicalDescriptorMap.Obj
                );
            }
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
            boxShape = new Box(1, 1, 1);
            boxShape.ComputeInertia(1, out boxInertia);

            simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, 0, 0), new CollidableDescription(simulation.Shapes.Add(new Box(1, 1, 1)), 0.1f)));

            simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, -6, 0), new CollidableDescription(simulation.Shapes.Add(new Box(100, 10, 100)), 0.1f)));

            //Taking off 1 thread could help stability https://github.com/bepu/bepuphysics2/blob/master/Documentation/PerformanceTips.md#general
            var numThreads = Math.Max(Environment.ProcessorCount - 1, 1);
            threadDispatcher = new SimpleThreadDispatcher(numThreads); 
        }

        private void CreateBox(Box box, BodyInertia boxInertia, System.Numerics.Vector3 position)
        {
            var sphereHandle = simulation.Bodies.Add(
                BodyDescription.CreateDynamic(
                    position,
                    boxInertia, new CollidableDescription(simulation.Shapes.Add(box), 0.1f), new BodyActivityDescription(0.01f)));

            var spherePositionSync = new BodyPositionSync(simulation.Bodies.GetBodyReference(sphereHandle));
            spherePositionSyncs.Add(spherePositionSync);
        }

        private unsafe void LoadCCoTexture()
        {
            using var ccoTextures = cc0TextureLoader.LoadTextureSet(CC0TexturePath);
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

        private long nextCubeSpawnTime = 0;
        private long nextCubeSpawnFrequency = 1 * Clock.SecondsToMicro;

        public unsafe void sendUpdate(Clock clock)
        {
            UpdateInput(clock);
            UpdatePhysics(clock);
            Render();
        }

        float moveSpeed = 10.0f;
        float viewSpeed = 1.0f;
        private void UpdateInput(Clock clock)
        {
            if (moveForward.Down)
            {
                camPos += Vector3.Forward * clock.DeltaSeconds * moveSpeed;
            }

            if (moveBackward.Down)
            {
                camPos += Vector3.Backward * clock.DeltaSeconds * moveSpeed;
            }

            if (moveLeft.Down)
            {
                camPos += Vector3.Left * clock.DeltaSeconds * moveSpeed;
            }

            if (moveRight.Down)
            {
                camPos += Vector3.Right * clock.DeltaSeconds * moveSpeed;
            }

            if (moveUp.Down)
            {
                camPos += Vector3.Up * clock.DeltaSeconds * moveSpeed;
            }

            if (moveDown.Down)
            {
                camPos += Vector3.Down * clock.DeltaSeconds * moveSpeed;
            }

            bool updateRotation = false;
            var yaw = 0.0f;
            var pitch = 0.0f;

            if (pitchUp.Down)
            {
                pitch = clock.DeltaSeconds * viewSpeed;
                updateRotation = true;
            }

            if (pitchDown.Down)
            {
                pitch = -clock.DeltaSeconds * viewSpeed;
                updateRotation = true;
            }

            if (yawLeft.Down)
            {
                yaw = -clock.DeltaSeconds * viewSpeed;
                updateRotation = true;
            }

            if (yawRight.Down)
            {
                yaw = clock.DeltaSeconds * viewSpeed;
                updateRotation = true;
            }

            if (updateRotation)
            {
                if (pitch > HALF_PI)
                {
                    pitch = HALF_PI;
                }
                if (pitch < -HALF_PI)
                {
                    pitch = -HALF_PI;
                }

                var yawRot = new Quaternion(Vector3.Up, yaw);
                var pitchRot = new Quaternion(Vector3.Left, pitch);
                camRot = camRot * yawRot * pitchRot;
            }
        }

        private unsafe void UpdatePhysics(Clock clock)
        {
            if (clock.CurrentTimeMicro > nextCubeSpawnTime)
            {
                CreateBox(boxShape, boxInertia, new System.Numerics.Vector3(-0.8f, 5, 0.8f));
                nextCubeSpawnTime += nextCubeSpawnFrequency;
                window.Title = $"BepuDemo - {spherePositionSyncs.Count} boxes.";
            }

            //Multithreading is pretty pointless for a simulation of one ball, but passing a IThreadDispatcher instance is all you have to do to enable multithreading.
            //If you don't want to use multithreading, don't pass a IThreadDispatcher.
            simulation.Timestep(clock.DeltaSeconds, threadDispatcher); //Careful of variable timestep here, not so good
        }

        private unsafe void Render()
        {
            //Render
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var PreTransform = swapChain.GetDesc_PreTransform;
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            // Set Camera, the rot can probably be cached between frames
            var preTransform = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);
            var cameraProj = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);
            pbrCameraAndLight.SetCameraPosition(ref camPos, ref camRot, ref preTransform, ref cameraProj);

            // Set Light
            pbrCameraAndLight.SetLight(ref lightDirection, ref lightColor, lightIntensity);

            //Draw cubes
            Vector3 cubePosition;
            Quaternion cubeOrientation;

            foreach (var spherePositionSync in spherePositionSyncs)
            {
                cubePosition = spherePositionSync.GetWorldPosition();
                cubeOrientation = spherePositionSync.GetWorldOrientation();

                pbrRenderer.Begin(immediateContext);
                pbrRenderer.Render(immediateContext, pboMatBinding.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, PbrAlphaMode.ALPHA_MODE_OPAQUE, ref cubePosition, ref cubeOrientation, pbrRenderAttribs);
            }

            cubePosition = Vector3.Zero;
            cubeOrientation = Quaternion.Identity;

            pbrRenderer.Begin(immediateContext);
            pbrRenderer.Render(immediateContext, pboMatBinding.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, PbrAlphaMode.ALPHA_MODE_OPAQUE, ref cubePosition, ref cubeOrientation, pbrRenderAttribs);

            this.swapChain.Present(1);
        }
    }
}
