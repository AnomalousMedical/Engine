using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.Resources;
using DiligentEngine.RT.ShaderSets;
using DungeonGenerator;
using Engine;
using RogueLikeMapBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rectangle = Engine.IntRect;
using Point = Engine.IntVector2;
using Size = Engine.IntSize2;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SceneTest
{
    class Level : IDisposable
    {
        public class Description
        {
            public Vector3 Translation { get; set; } = Vector3.Zero;

            public int RandomSeed { get; set; } = 0;

            public int Width { get; set; } = 50;

            public int Height { get; set; } = 50;

            public float MapUnitX { get; set; } = 3.0f;

            public float MapUnitY { get; set; } = 0.1f;

            public float MapUnitZ { get; set; } = 3.0f;

            /// <summary>
            /// Room minimum size
            /// </summary>
            public Size RoomMin { get; set; } = new Size(3, 3);

            /// <summary>
            /// Room max size
            /// </summary>
            public Size RoomMax { get; set; } = new Size(10, 10);

            /// <summary>
            /// Number of rooms to build
            /// </summary>
            public int MaxRooms { get; set; } = 15;

            /// <summary>
            /// Minimum distance between rooms
            /// </summary>
            public int RoomDistance { get; set; } = 5;

            /// <summary>
            /// Minimum distance of room from existing corridors
            /// </summary>
            public int CorridorDistance { get; set; } = 2;

            /// <summary>
            /// Minimum corridor length
            /// </summary>
            public int CorridorMinLength { get; set; } = 3;
            /// <summary>
            /// Maximum corridor length
            /// </summary>
            public int CorridorMaxLength { get; set; } = 15;
            /// <summary>
            /// Maximum turns
            /// </summary>
            public int CorridorMaxTurns { get; set; } = 5;
            /// <summary>
            /// The distance a corridor has to be away from a closed cell for it to be built
            /// </summary>
            public int CorridorSpace { get; set; } = 10;


            /// <summary>
            /// Probability of building a corridor from a room or corridor. Greater than value = room
            /// </summary>
            public int BuildProb { get; set; } = 50;

            /// <summary>
            /// Break out
            /// </summary>
            public int BreakOut { get; set; } = 250;

            /// <summary>
            /// True if this level has a go previous level connector. Default: true
            /// </summary>
            public bool GoPrevious { get; set; } = true;
        }

        private readonly RTInstances<ILevelManager> rtInstances;
        private readonly RayTracingRenderer renderer;
        private readonly IDestructionRequest destructionRequest;
        private readonly IBepuScene bepuScene;
        private readonly TextureManager textureManager;
        private readonly ILogger<Level> logger;
        private readonly IBiomeManager biomeManager;
        private PrimaryHitShader floorShader;
        private PrimaryHitShader wallShader;
        private CC0TextureResult floorTexture;
        private CC0TextureResult wallTexture;
        private readonly TLASBuildInstanceData wallInstanceData;
        private readonly TLASBuildInstanceData floorInstanceData;
        private List<StaticHandle> staticHandles = new List<StaticHandle>();
        private TypedIndex boundaryCubeShapeIndex;
        private TypedIndex floorCubeShapeIndex;
        private bool disposed;
        private MapMesh mapMesh;
        private bool physicsActive = false;
        private IObjectResolver objectResolver;
        private LevelConnector nextLevelConnector;
        private LevelConnector previousLevelConnector;
        private IBiome biome;
        private bool goPrevious;

        private Task levelGenerationTask;
        private Vector3 mapUnits;

        private Vector3 endPointLocal;
        private Vector3 startPointLocal;
        private Vector3 currentPosition;

        public Vector3 StartPoint => startPointLocal + currentPosition;
        public Vector3 EndPoint => endPointLocal + currentPosition;

        public Vector3 LocalStartPoint => startPointLocal;
        public Vector3 LocalEndPoint => endPointLocal;

        public Level
        (
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            IBepuScene bepuScene,
            Description description,
            ILogger<Level> logger,
            IObjectResolverFactory objectResolverFactory,
            IBiomeManager biomeManager,
            MeshBLAS floorMesh,
            MeshBLAS wallMesh,
            TextureManager textureManager,
            PrimaryHitShader.Factory primaryHitShaderFactory,
            RTInstances<ILevelManager> rtInstances,
            RayTracingRenderer renderer
        )
        {
            this.mapUnits = new Vector3(description.MapUnitX, description.MapUnitY, description.MapUnitZ);
            this.objectResolver = objectResolverFactory.Create();
            this.destructionRequest = destructionRequest;
            this.bepuScene = bepuScene;
            this.logger = logger;
            this.biomeManager = biomeManager;
            this.textureManager = textureManager;
            this.rtInstances = rtInstances;
            this.renderer = renderer;
            this.goPrevious = description.GoPrevious;

            this.currentPosition = description.Translation;

            this.floorInstanceData = new TLASBuildInstanceData()
            {
                InstanceName = RTId.CreateId("LevelFloor"),
                Mask = RtStructures.OPAQUE_GEOM_MASK,
                Transform = new InstanceMatrix(currentPosition, Quaternion.Identity)
            };
            this.wallInstanceData = new TLASBuildInstanceData()
            {
                InstanceName = RTId.CreateId("LevelWall"),
                Mask = RtStructures.OPAQUE_GEOM_MASK,
                Transform = new InstanceMatrix(currentPosition, Quaternion.Identity)
            };

            var random = new Random(description.RandomSeed);
            biome = biomeManager.GetBiome(random.Next(0, biomeManager.Count));

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                var floorTextureDesc = new CCOTextureBindingDescription(biome.FloorTexture);
                var wallTextureDesc = new CCOTextureBindingDescription(biome.WallTexture);

                var floorTextureTask = textureManager.Checkout(floorTextureDesc);
                var wallTextureTask = textureManager.Checkout(wallTextureDesc);

                var floorShaderSetup = primaryHitShaderFactory.Create(floorMesh.Name, floorTextureDesc.NumTextures, PrimaryHitShaderType.Cube);
                var wallShaderSetup = primaryHitShaderFactory.Create(wallMesh.Name, wallTextureDesc.NumTextures, PrimaryHitShaderType.Cube);

                this.levelGenerationTask = Task.Run(() =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var random = new Random(description.RandomSeed);
                    var mapBuilder = new csMapbuilder(random, description.Width, description.Height)
                    {
                        BreakOut = description.BreakOut,
                        BuildProb = description.BuildProb,
                        CorridorDistance = description.CorridorDistance,
                        CorridorSpace = description.CorridorSpace,
                        Corridor_Max = description.CorridorMaxLength,
                        Corridor_MaxTurns = description.CorridorMaxTurns,
                        Corridor_Min = description.CorridorMinLength,
                        MaxRooms = description.MaxRooms,
                        RoomDistance = description.RoomDistance,
                        Room_Max = description.RoomMax,
                        Room_Min = description.RoomMin
                    };
                    mapBuilder.Build_ConnectedStartRooms();
                    mapBuilder.AddEastConnector();
                    int startX, startY;
                    if (description.GoPrevious)
                    {
                        mapBuilder.AddWestConnector();
                        var startConnector = mapBuilder.WestConnector.Value;
                        startX = startConnector.x;
                        startY = startConnector.y;
                    }
                    else
                    {
                        Rectangle startRoom = new Rectangle(int.MaxValue, 0, 0, 0);
                        foreach (var room in mapBuilder.Rooms)
                        {
                            if (room.Left < startRoom.Left)
                            {
                                startRoom = room;
                            }
                        }

                        startX = startRoom.Left + startRoom.Width / 2;
                        startY = startRoom.Top + startRoom.Height / 2;
                    }

                    mapMesh = new MapMesh(mapBuilder, random, floorMesh, wallMesh, mapUnitX: description.MapUnitX, mapUnitY: description.MapUnitY, mapUnitZ: description.MapUnitZ);

                    startPointLocal = mapMesh.PointToVector(startX, startY);
                    var endConnector = mapBuilder.EastConnector.Value;
                    endPointLocal = mapMesh.PointToVector(endConnector.x, endConnector.y);

                    sw.Stop();
                    logger.LogInformation($"Generated level {description.RandomSeed} in {sw.ElapsedMilliseconds} ms.");
                });

                await levelGenerationTask; //Need the level before kicking off the calls to End() below.

                await Task.WhenAll
                (
                    floorTextureTask,
                    wallTextureTask,
                    floorMesh.End("LevelFloor"),
                    wallMesh.End("LevelWall"),
                    floorShaderSetup,
                    wallShaderSetup
                );

                this.floorShader = floorShaderSetup.Result;
                this.wallShader = wallShaderSetup.Result;
                this.floorTexture = floorTextureTask.Result;
                this.wallTexture = wallTextureTask.Result;

                if (!destructionRequest.DestructionRequested)
                {
                    this.floorInstanceData.pBLAS = mapMesh.FloorMesh.Instance.BLAS.Obj;
                    this.wallInstanceData.pBLAS = mapMesh.WallMesh.Instance.BLAS.Obj;

                    rtInstances.AddTlasBuild(floorInstanceData);
                    rtInstances.AddTlasBuild(wallInstanceData);
                    rtInstances.AddShaderTableBinder(Bind);
                    renderer.AddShaderResourceBinder(Bind);
                }
            });
        }

        internal void RequestDestruction()
        {
            this.destructionRequest.RequestDestruction();
        }

        public void Dispose()
        {
            disposed = true;
            objectResolver.Dispose();
            DestroyPhysics();
            textureManager.TryReturn(wallTexture);
            textureManager.TryReturn(floorTexture);
            renderer.RemoveShaderResourceBinder(Bind);
            rtInstances.RemoveShaderTableBinder(Bind);
            this.wallShader?.Dispose();
            this.floorShader?.Dispose();
            rtInstances.RemoveTlasBuild(floorInstanceData);
            rtInstances.RemoveTlasBuild(wallInstanceData);
        }

        /// <summary>
        /// Levels are created in the background. Await this function to wait until it has finished
        /// being created. This only means the level is defined, not that its mesh is created or textures loaded.
        /// </summary>
        /// <returns></returns>
        public async Task WaitForLevelGeneration()
        {
            if(levelGenerationTask != null)
            {
                await levelGenerationTask;
            }
        }

        public void SetPosition(in Vector3 position)
        {
            this.currentPosition = position;
            this.wallInstanceData.Transform = new InstanceMatrix(position, Quaternion.Identity);
            this.floorInstanceData.Transform = new InstanceMatrix(position, Quaternion.Identity);
            this.previousLevelConnector?.SetPosition(StartPoint + new Vector3(-(mapUnits.x / 2f + 0.5f), 0f, 0f));
            this.nextLevelConnector?.SetPosition(EndPoint + new Vector3((mapUnits.x / 2f + 0.5f), 0f, 0f));
        }

        public IBiome Biome => biome;

        /// <summary>
        /// Add physics shapes to scene. Should wait until the level generation is complete first.
        /// </summary>
        public void SetupPhysics()
        {
            if (physicsActive)
            {
                //Don't do anything if physics are active
                return;
            }

            physicsActive = true;

            float yBoundaryScale = 50f;

            //Add stuff to physics scene
            var boundaryCubeShape = new Box(mapMesh.MapUnitX, mapMesh.MapUnitY * yBoundaryScale, mapMesh.MapUnitZ); //Each one creates its own, try to load from resources
            boundaryCubeShapeIndex = bepuScene.Simulation.Shapes.Add(boundaryCubeShape);

            var floorCubeShape = new Box(mapMesh.MapUnitX, mapMesh.MapUnitY, mapMesh.MapUnitZ); //Each one creates its own, try to load from resources
            floorCubeShapeIndex = bepuScene.Simulation.Shapes.Add(floorCubeShape);

            var boundaryOrientation = System.Numerics.Quaternion.Identity;

            foreach (var boundary in mapMesh.FloorCubeCenterPoints)
            {
                //TODO: Figure out where nans are coming from
                var orientation = boundary.Orientation.isNumber() ? boundary.Orientation : Quaternion.Identity;
                var staticHandle = bepuScene.Simulation.Statics.Add(
                    new StaticDescription(
                        (boundary.Position + currentPosition).ToSystemNumerics(),
                        orientation.ToSystemNumerics(),
                        new CollidableDescription(floorCubeShapeIndex, 0.1f)));

                staticHandles.Add(staticHandle);
            }

            foreach (var boundary in mapMesh.BoundaryCubeCenterPoints)
            {
                var staticHandle = bepuScene.Simulation.Statics.Add(
                    new StaticDescription(
                        (boundary + currentPosition).ToSystemNumerics(),
                        boundaryOrientation,
                        new CollidableDescription(boundaryCubeShapeIndex, 0.1f)));

                staticHandles.Add(staticHandle);
            }

            if (goPrevious)
            {
                this.previousLevelConnector = objectResolver.Resolve<LevelConnector, LevelConnector.Description>(o =>
                {
                    o.Scale = new Vector3(mapUnits.x, 50f, mapUnits.z);
                    o.Translation = StartPoint + new Vector3(-mapUnits.x * 2f, 0f, 0f);
                    o.GoPrevious = true;
                });
            }

            this.nextLevelConnector = objectResolver.Resolve<LevelConnector, LevelConnector.Description>(o =>
            {
                o.Scale = new Vector3(mapUnits.x, 50f, mapUnits.z);
                o.Translation = EndPoint + new Vector3(mapUnits.x * 2f, 0f, 0f);
                o.GoPrevious = false;
            });
        }

        /// <summary>
        /// Remove physics shapes from scene. Should wait until the level generation is complete first.
        /// </summary>
        public void DestroyPhysics()
        {
            if (!physicsActive)
            {
                //Do nothing if physics aren't active.
                return;
            }
            physicsActive = false;

            this.previousLevelConnector?.RequestDestruction();
            this.nextLevelConnector?.RequestDestruction();

            this.previousLevelConnector = null;
            this.nextLevelConnector = null;

            var statics = bepuScene.Simulation.Statics;
            foreach (var staticHandle in staticHandles)
            {
                statics.Remove(staticHandle);
            }
            bepuScene.Simulation.Shapes.Remove(boundaryCubeShapeIndex);
            bepuScene.Simulation.Shapes.Remove(floorCubeShapeIndex);
            staticHandles.Clear();
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            sbt.BindHitGroupForInstance(tlas, floorInstanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, floorShader.ShaderGroupName, IntPtr.Zero, 0);
            sbt.BindHitGroupForInstance(tlas, wallInstanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, wallShader.ShaderGroupName, IntPtr.Zero, 0);
        }

        public void Bind(IShaderResourceBinding rayTracingSRB)
        {
            floorShader.BindBlas(mapMesh.FloorMesh.Instance, rayTracingSRB);
            floorShader.BindTextures(rayTracingSRB, floorTexture);

            wallShader.BindBlas(mapMesh.WallMesh.Instance, rayTracingSRB);
            wallShader.BindTextures(rayTracingSRB, wallTexture);
        }
    }
}
