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
using DungeonGenerator;
using System.Diagnostics;
using System.Threading.Tasks;
using SharpGui;

namespace DungeonGeneratorTest
{
    class BepuUpdateListener : UpdateListener, IDisposable
    {
        private const string FloorTexturePath = "cc0Textures/Wood049_1K";
        private const string WallTexturePath = "cc0Textures/Ground042_1K";

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
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly ICoroutineRunner coroutineRunner;
        private ISwapChain swapChain;
        private IRenderDevice renderDevice;
        private IDeviceContext immediateContext;
        private PbrRenderAttribs pbrRenderAttribs = PbrRenderAttribs.CreateDefault();

        private PbrRenderer pbrRenderer;
        private AutoPtr<ITextureView> environmentMapSRV;
        private AutoPtr<IShaderResourceBinding> floorTexture;
        private AutoPtr<IShaderResourceBinding> wallTexture;

        private MapMesh mapMesh;

        private SharpButton nextScene = new SharpButton() { Text = "Next Scene" };
        private bool loadingLevel = false;
        private int currentSeed = 23;

        //BEPU
        //If you intend to reuse the BufferPool, disposing the simulation is a good idea- it returns all the buffers to the pool for reuse.
        //Here, we dispose it, but it's not really required; we immediately thereafter clear the BufferPool of all held memory.
        //Note that failing to dispose buffer pools can result in memory leaks.
        Simulation simulation;
        SimpleThreadDispatcher threadDispatcher;
        BufferPool bufferPool;
        //END

        public BepuUpdateListener(
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
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
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
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.coroutineRunner = coroutineRunner;
            cameraControls.Position = new Vector3(0, 2, -11);
            Initialize();
            LoadNextScene();
        }

        private void LoadNextScene()
        {
            coroutineRunner.RunTask(async () =>
            {
                MapMesh newMapMesh = null;
                await Task.Run(() =>
                {
                    loadingLevel = true;
                    var sw = new Stopwatch();
                    sw.Start();
                    //Quick test with the console
                    var seed = currentSeed++;
                    var random = new Random(seed);
                    var mapBuilder = new csMapbuilder(random, 50, 50);
                    mapBuilder.CorridorSpace = 10;
                    mapBuilder.RoomDistance = 3;
                    mapBuilder.Room_Min = new IntSize2(2, 2);
                    mapBuilder.Room_Max = new IntSize2(6, 6); //Between 3-6 is good here, 3 for more cityish with small rooms, 6 for more open with more big rooms, sometimes connected
                    mapBuilder.Corridor_Max = 4;
                    mapBuilder.Horizontal = false;
                    mapBuilder.Build_ConnectedStartRooms();
                    mapBuilder.AddNorthConnector();
                    mapBuilder.AddSouthConnector();
                    mapBuilder.AddWestConnector();
                    mapBuilder.AddEastConnector();
                    sw.Stop();
                    var map = mapBuilder.map;
                    var mapWidth = mapBuilder.Map_Size.Width;
                    var mapHeight = mapBuilder.Map_Size.Height;

                    for (int mapY = mapBuilder.Map_Size.Height - 1; mapY > -1; --mapY)
                    {
                        for (int mapX = 0; mapX < mapWidth; ++mapX)
                        {
                            switch (map[mapX, mapY])
                            {
                                case csMapbuilder.EmptyCell:
                                    Console.Write(' ');
                                    break;
                                case csMapbuilder.MainCorridorCell:
                                    Console.Write('M');
                                    break;
                                case csMapbuilder.RoomCell:
                                    Console.Write('S');
                                    break;
                                case csMapbuilder.RoomCell + 1:
                                    Console.Write('E');
                                    break;
                                default:
                                    Console.Write('X');
                                    break;
                            }
                        }
                        Console.WriteLine();
                    }

                    for (int mapY = mapBuilder.Map_Size.Height - 1; mapY > -1; --mapY)
                    {
                        for (int mapX = 0; mapX < mapWidth; ++mapX)
                        {
                            if (map[mapX, mapY] == csMapbuilder.EmptyCell)
                            {
                                Console.Write(' ');
                            }
                            else
                            {
                                Console.Write(map[mapX, mapY]);
                            }
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine($"Level seed {seed}");
                    Console.WriteLine($"Created in {sw.ElapsedMilliseconds}");
                    Console.WriteLine(mapBuilder.StartRoom);
                    Console.WriteLine(mapBuilder.EndRoom);
                    Console.WriteLine("--------------------------------------------------");

                    newMapMesh = new MapMesh(mapBuilder, random, renderDevice, mapUnitY: 0.1f);
                    loadingLevel = false;
                });

                mapMesh?.Dispose();
                mapMesh = newMapMesh;
            });
        }

        public void Dispose()
        {
            //If you intend to reuse the BufferPool, disposing the simulation is a good idea- it returns all the buffers to the pool for reuse.
            //Here, we dispose it, but it's not really required; we immediately thereafter clear the BufferPool of all held memory.
            //Note that failing to dispose buffer pools can result in memory leaks.
            simulation.Dispose();
            threadDispatcher.Dispose();
            bufferPool.Clear();

            mapMesh?.Dispose();

            floorTexture.Dispose();
            wallTexture.Dispose();
            environmentMapSRV.Dispose();
        }

        unsafe void Initialize()
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(renderDevice, immediateContext, "papermill/Fixed-", "png");

            pbrRenderer.PrecomputeCubemaps(renderDevice, immediateContext, environmentMapSRV.Obj);

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

            //Taking off 1 thread could help stability https://github.com/bepu/bepuphysics2/blob/master/Documentation/PerformanceTips.md#general
            var numThreads = Math.Max(Environment.ProcessorCount - 1, 1);
            threadDispatcher = new SimpleThreadDispatcher(numThreads); 
        }

        private unsafe void LoadCCoTexture()
        {
            using var floorTextures = cc0TextureLoader.LoadTextureSet(FloorTexturePath);
            floorTexture = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: floorTextures.BaseColorMap,
                normalMap: floorTextures.NormalMap,
                physicalDescriptorMap: floorTextures.PhysicalDescriptorMap,
                aoMap: floorTextures.AmbientOcclusionMap
            );

            using var wallTextures = cc0TextureLoader.LoadTextureSet(WallTexturePath);
            wallTexture = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: wallTextures.BaseColorMap,
                normalMap: wallTextures.NormalMap,
                physicalDescriptorMap: wallTextures.PhysicalDescriptorMap,
                aoMap: wallTextures.AmbientOcclusionMap
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
            cameraControls.UpdateInput(clock);
            UpdatePhysics(clock);
            UpdateGui(clock);
            Render();
        }

        private unsafe void UpdatePhysics(Clock clock)
        {
            //Multithreading is pretty pointless for a simulation of one ball, but passing a IThreadDispatcher instance is all you have to do to enable multithreading.
            //If you don't want to use multithreading, don't pass a IThreadDispatcher.
            simulation.Timestep(clock.DeltaSeconds, threadDispatcher); //Careful of variable timestep here, not so good
        }

        private void UpdateGui(Clock clock)
        {
            sharpGui.Begin(clock);

            if (!loadingLevel)
            {
                var layout =
                    new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                    new MaxWidthLayout(scaleHelper.Scaled(300),
                    new ColumnLayout(nextScene) { Margin = new IntPad(10) }
                    ));
                var desiredSize = layout.GetDesiredSize(sharpGui);
                layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

                //Buttons
                if (sharpGui.Button(nextScene))
                {
                    LoadNextScene();
                }
            }

            sharpGui.End();
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
            pbrCameraAndLight.SetCameraPosition(cameraControls.Position, cameraControls.Orientation, preTransform, cameraProj);

            // Set Light
            pbrCameraAndLight.SetLight(lightDirection, lightColor, lightIntensity);

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
            pbrRenderer.Render(immediateContext, floorTexture.Obj, shape.VertexBuffer, shape.SkinVertexBuffer, shape.IndexBuffer, shape.NumIndices, ref cubePosition, ref cubeOrientation, pbrRenderAttribs);

            if (mapMesh != null)
            {
                var mesh = mapMesh.FloorMesh;
                pbrRenderer.Render(immediateContext, floorTexture.Obj, mesh.VertexBuffer, mesh.SkinVertexBuffer, mesh.IndexBuffer, mesh.NumIndices, ref Vector3.Zero, ref Quaternion.Identity, pbrRenderAttribs);

                mesh = mapMesh.WallMesh;
                pbrRenderer.Render(immediateContext, wallTexture.Obj, mesh.VertexBuffer, mesh.SkinVertexBuffer, mesh.IndexBuffer, mesh.NumIndices, ref Vector3.Zero, ref Quaternion.Identity, pbrRenderAttribs);
            }

            RenderGui();

            this.swapChain.Present(1);
        }

        private void RenderGui()
        {
            //Draw the gui
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            sharpGui.Render(immediateContext);
        }
    }
}
