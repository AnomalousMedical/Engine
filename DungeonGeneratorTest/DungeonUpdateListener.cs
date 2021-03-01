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
using Engine.CameraMovement;
using RogueLikeMapBuilder;

namespace DungeonGeneratorTest
{
    class BepuUpdateListener : UpdateListener, IDisposable
    {
        private const string CC0TexturePath = "cc0Textures/Wood049_1K";

        //Camera Settings
        float YFov = MathFloat.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        //Clear Color
        Engine.Color ClearColor = new Engine.Color(0.032f, 0.032f, 0.032f, 1.0f);

        //Light
        Vector3 lightDirection = Vector3.Forward;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 1f;

        private readonly NativeOSWindow window;
        private readonly Cube shape;
        private readonly TextureLoader textureLoader;
        private readonly CC0TextureLoader cc0TextureLoader;
        private readonly EnvironmentMapBuilder envMapBuilder;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly ILogger<BepuUpdateListener> logger;
        private readonly EventManager eventManager;
        private readonly FirstPersonFlyCamera cameraControls;
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
        //END

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
            EventManager eventManager,
            FirstPersonFlyCamera cameraControls,
            ICoroutineRunner coroutineRunner)
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
            this.logger = logger;
            this.eventManager = eventManager;
            this.cameraControls = cameraControls;
            cameraControls.Position = new Vector3(0, 2, -11);
            Initialize();

            var random = new Random(1);
            IEnumerator<YieldAction> co()
            {
                while (true)
                {
                    //Quick test with the console
                    var mapBuilder = new csMapbuilder(random, 150, 150);
                    mapBuilder.Build_ConnectedStartRooms();
                    var map = mapBuilder.map;
                    for (int y = 0; y < 150; ++y)
                    {
                        for (int x = 0; x < 150; ++x)
                        {
                            if (map[x, y] == 0)
                            {
                                Console.Write('X');
                            }
                            else
                            {
                                Console.Write('0');
                            }
                        }
                        Console.WriteLine();
                    }
                    yield return coroutineRunner.WaitSeconds(0.3f);
                }
            }
            coroutineRunner.Run(co());
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

            //Taking off 1 thread could help stability https://github.com/bepu/bepuphysics2/blob/master/Documentation/PerformanceTips.md#general
            var numThreads = Math.Max(Environment.ProcessorCount - 1, 1);
            threadDispatcher = new SimpleThreadDispatcher(numThreads); 
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
            cameraControls.UpdateInput(clock);
            UpdatePhysics(clock);
            Render();
        }

        private unsafe void UpdatePhysics(Clock clock)
        {
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

            // Set Camera
            var preTransform = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);
            var cameraProj = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);
            pbrCameraAndLight.SetCameraPosition(cameraControls.Position, cameraControls.Orientation, ref preTransform, ref cameraProj);

            // Set Light
            pbrCameraAndLight.SetLight(ref lightDirection, ref lightColor, lightIntensity);

            //Draw cubes
            Vector3 cubePosition;
            Quaternion cubeOrientation;

            //foreach (var spherePositionSync in spherePositionSyncs)
            //{
            //    cubePosition = spherePositionSync.GetWorldPosition();
            //    cubeOrientation = spherePositionSync.GetWorldOrientation();

            //    pbrRenderer.Begin(immediateContext);
            //    pbrRenderer.Render(immediateContext, pboMatBinding.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, ref cubePosition, ref cubeOrientation, pbrRenderAttribs);
            //}

            cubePosition = Vector3.Zero;
            cubeOrientation = Quaternion.Identity;

            pbrRenderer.Begin(immediateContext);
            pbrRenderer.Render(immediateContext, pboMatBinding.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, ref cubePosition, ref cubeOrientation, pbrRenderAttribs);

            this.swapChain.Present(1);
        }
    }
}
