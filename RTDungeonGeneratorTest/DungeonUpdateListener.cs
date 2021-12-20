using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
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
using DiligentEngine.RT;
using BepuPlugin;

namespace RTDungeonGeneratorTest
{
    class DungeonUpdateListener : UpdateListener, IDisposable
    {
        private readonly GraphicsEngine graphicsEngine;
        private readonly NativeOSWindow window;
        //private readonly Cube shape;
        private readonly ILogger<DungeonUpdateListener> logger;
        private readonly EventManager eventManager;
        private readonly FirstPersonFlyCamera cameraControls;
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly BLASBuilder blasBuilder;
        private readonly IBepuScene bepuScene;
        private readonly RayTracingRenderer renderer;
        private readonly RTInstances instances;
        private readonly RTGui gui;
        private MapMesh mapMesh;

        private SharpButton nextScene = new SharpButton() { Text = "Next Scene" };
        private bool loadingLevel = false;
        private int currentSeed = 23;

        public DungeonUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            //Cube shape,
            ILogger<DungeonUpdateListener> logger,
            EventManager eventManager,
            FirstPersonFlyCamera cameraControls,
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            ICoroutineRunner coroutineRunner,
            BLASBuilder blasBuilder,
            IBepuScene bepuScene,
            RayTracingRenderer renderer,
            RTInstances instances,
            RTGui gui)
        {
            this.graphicsEngine = graphicsEngine;
            this.window = window;
            this.logger = logger;
            this.eventManager = eventManager;
            this.cameraControls = cameraControls;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.coroutineRunner = coroutineRunner;
            this.blasBuilder = blasBuilder;
            this.bepuScene = bepuScene;
            this.renderer = renderer;
            this.instances = instances;
            this.gui = gui;
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

                    newMapMesh = new MapMesh(mapBuilder, random, blasBuilder, mapUnitY: 0.1f);
                    loadingLevel = false;
                });

                mapMesh?.Dispose();
                mapMesh = newMapMesh;

                renderer.BindBlas(newMapMesh.WallMesh.Instance);

                var instanceData = new TLASBuildInstanceData()
                {
                    InstanceName = Guid.NewGuid().ToString(),
                    CustomId = 0, //Texture index
                    pBLAS = newMapMesh.WallMesh.Instance.BLAS.Obj,
                    Mask = RtStructures.OPAQUE_GEOM_MASK,
                    Transform = new InstanceMatrix(Vector3.Zero, Quaternion.Identity)
                };
                instances.AddTlasBuild(instanceData);

                instances.AddShaderTableBinder(new CallbackShaderTableBinder((sbt, tlas) =>
                {
                    sbt.BindHitGroupForInstance(tlas, instanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, "CubePrimaryHit", IntPtr.Zero);
                }));
            });
        }

        public void Dispose()
        {
            mapMesh?.Dispose();
        }

        unsafe void Initialize()
        {
            SetupBepu();
        }

        private void SetupBepu()
        {
            
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
            bepuScene.Update(clock, new System.Numerics.Vector3(0, 0, 1));
            UpdateGui(clock);
            Render();
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

            gui.Update(clock);

            sharpGui.End();
        }

        private unsafe void Render()
        {
            var swapChain = graphicsEngine.SwapChain;
            var immediateContext = graphicsEngine.ImmediateContext;

            renderer.Render(cameraControls.Position, cameraControls.Orientation, gui.LightPos, gui.LightPos);

            //This is the old clear loop, leaving in place in case we want or need the screen clear, but I think with pure rt there is no need
            //since we blit a texture to the full screen over and over.
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            //var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            //immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            sharpGui.Render(immediateContext);

            swapChain.Present(1);
        }
    }
}
