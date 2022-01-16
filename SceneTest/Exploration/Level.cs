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
using DiligentEngine.RT.HLSL;

namespace SceneTest
{
    class Level : IDisposable
    {
        public class Description
        {
            public int Index { get; set; }

            public Vector3 Translation { get; set; } = Vector3.Zero;

            public int RandomSeed { get; set; } = 0;

            public int Width { get; set; } = 50;

            public int Height { get; set; } = 50;

            public float MapUnitX { get; set; } = 3.0f;

            public float MapUnitY { get; set; } = 0.1f;

            public float MapUnitZ { get; set; } = 1.5f;

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

            public int MinFights { get; set; } = 5;

            public int MaxFigths { get; set; } = 15;
        }

        private readonly RTInstances<ILevelManager> rtInstances;
        private readonly RayTracingRenderer renderer;
        private readonly IDestructionRequest destructionRequest;
        private readonly IBepuScene bepuScene;
        private readonly TextureManager textureManager;
        private readonly ActiveTextures activeTextures;
        private readonly PrimaryHitShader.Factory primaryHitShaderFactory;
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
        private MapMesh mapMesh;
        private bool physicsActive = false;
        private IObjectResolver objectResolver;
        private LevelConnector nextLevelConnector;
        private LevelConnector previousLevelConnector;
        private List<BattleTrigger> battleTriggers = new List<BattleTrigger>();
        private List<TreasureTrigger> treasureTriggers = new List<TreasureTrigger>();
        private IBiome biome;
        private bool goPrevious;
        private BlasInstanceData floorBlasInstanceData;
        private BlasInstanceData wallBlasInstanceData;
        private int seed;
        private int minFights;
        private int maxFights;

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
            ActiveTextures activeTextures,
            PrimaryHitShader.Factory primaryHitShaderFactory,
            RTInstances<ILevelManager> rtInstances,
            RayTracingRenderer renderer
        )
        {
            this.seed = description.RandomSeed;
            this.minFights = description.MinFights;
            this.maxFights = description.MaxFigths;
            this.mapUnits = new Vector3(description.MapUnitX, description.MapUnitY, description.MapUnitZ);
            this.objectResolver = objectResolverFactory.Create();
            this.destructionRequest = destructionRequest;
            this.bepuScene = bepuScene;
            this.logger = logger;
            this.biomeManager = biomeManager;
            this.textureManager = textureManager;
            this.activeTextures = activeTextures;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
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

                    //Ensure we can't have more fights than corridors
                    if (maxFights > mapMesh.MapBuilder.Corridors.Count)
                    {
                        maxFights = mapMesh.MapBuilder.Corridors.Count;
                        minFights = maxFights - 5;
                        if (minFights < 0)
                        {
                            minFights = 0;
                        }
                    }

                    sw.Stop();
                    logger.LogInformation($"Generated level {description.Index} seed {description.RandomSeed} in {sw.ElapsedMilliseconds} ms.");
                });

                await levelGenerationTask; //Need the level before kicking off the calls to End() below.

                await Task.WhenAll
                (
                    floorMesh.End("LevelFloor"),
                    wallMesh.End("LevelWall")
                );

                //TODO: The level BLASes must be loaded before the shaders, see todo in PrimaryHitShader
                var floorShaderSetup = primaryHitShaderFactory.Checkout(new PrimaryHitShader.Desc
                {
                    ShaderType = PrimaryHitShaderType.Mesh,
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    Reflective = biome.ReflectFloor
                });
                var wallShaderSetup = primaryHitShaderFactory.Checkout(new PrimaryHitShader.Desc
                {
                    ShaderType = PrimaryHitShaderType.Mesh,
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    Reflective = biome.ReflectWall
                });

                await Task.WhenAll
                (
                    floorTextureTask,
                    wallTextureTask,
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

                    rtInstances.AddShaderTableBinder(Bind);
                    floorBlasInstanceData = activeTextures.AddActiveTexture(floorTexture);
                    wallBlasInstanceData = activeTextures.AddActiveTexture(wallTexture);
                    rtInstances.AddTlasBuild(floorInstanceData);
                    rtInstances.AddTlasBuild(wallInstanceData);
                }
            });
        }

        internal void RequestDestruction()
        {
            this.destructionRequest.RequestDestruction();
        }

        public void Dispose()
        {
            objectResolver.Dispose();
            DestroyPhysics();
            activeTextures.RemoveActiveTexture(wallTexture);
            activeTextures.RemoveActiveTexture(floorTexture);
            textureManager.TryReturn(wallTexture);
            textureManager.TryReturn(floorTexture);
            rtInstances.RemoveShaderTableBinder(Bind);
            primaryHitShaderFactory.TryReturn(floorShader);
            primaryHitShaderFactory.TryReturn(wallShader);
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
            foreach(var trigger in battleTriggers)
            {
                trigger.SetLevelPosition(position);
            }
            foreach (var trigger in treasureTriggers)
            {
                trigger.SetLevelPosition(position);
            }
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

            //Setup enemies
            var enemyRandom = new Random(seed);
            var numFights = enemyRandom.Next(minFights, maxFights);
            var corridorMax = mapMesh.MapBuilder.Corridors.Count;
            var usedCorridors = new HashSet<int>();
            for (var i = 0; i < numFights; ++i)
            {
                var corridorTry = 0;
                var corridorIndex = enemyRandom.Next(corridorMax);
                while (usedCorridors.Contains(corridorIndex))
                {
                    if(++corridorTry > 50)
                    {
                        //If we generate too many bad random numbers, just get the first index we can from the list
                        for (corridorIndex = 0; corridorIndex < corridorMax && usedCorridors.Contains(corridorIndex); ++corridorIndex) { }
                        if (corridorIndex >= corridorMax)
                        {
                            throw new InvalidOperationException("This should not happen, but ran out of corridors trying to place enemies. This is guarded in the constructor.");
                        }
                    }
                    else
                    {
                        corridorIndex = enemyRandom.Next(corridorMax);
                    }
                }
                usedCorridors.Add(corridorIndex);
                var point = mapMesh.MapBuilder.Corridors[corridorIndex];

                var battleTrigger = objectResolver.Resolve<BattleTrigger, BattleTrigger.Description>(o =>
                {
                    o.MapOffset = mapMesh.PointToVector(point.x, point.y);
                    o.Translation = currentPosition + o.MapOffset;
                    var enemy = biome.GetEnemy(RpgMath.EnemyType.Normal);
                    o.Sprite = enemy.Asset.CreateSprite();
                    o.SpriteMaterial = enemy.Asset.CreateMaterial();
                });
                battleTriggers.Add(battleTrigger);
            }

            //Setup treasure, just put one in each room for now
            foreach(var room in mapMesh.MapBuilder.Rooms)
            {
                var point = new Point(room.Left + room.Width / 2, room.Top + room.Height / 2);

                var treasureTrigger = objectResolver.Resolve<TreasureTrigger, TreasureTrigger.Description>(o =>
                {
                    o.MapOffset = mapMesh.PointToVector(point.x, point.y);
                    o.Translation = currentPosition + o.MapOffset;
                    var treasure = biome.Treasure;
                    o.Sprite = treasure.Asset.CreateSprite();
                    o.SpriteMaterial = treasure.Asset.CreateMaterial();
                });
                this.treasureTriggers.Add(treasureTrigger);
            }
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

            foreach(var battleTrigger in battleTriggers)
            {
                battleTrigger.RequestDestruction();
            }
            battleTriggers.Clear();

            foreach (var treasureTrigger in treasureTriggers)
            {
                treasureTrigger.RequestDestruction();
            }
            treasureTriggers.Clear();

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

        private unsafe void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            floorBlasInstanceData.vertexOffset = mapMesh.FloorMesh.Instance.VertexOffset;
            floorBlasInstanceData.indexOffset = mapMesh.FloorMesh.Instance.IndexOffset;
            fixed (BlasInstanceData* ptr = &floorBlasInstanceData)
            {
                floorShader.BindSbt(floorInstanceData.InstanceName, sbt, tlas, new IntPtr(ptr), (uint)sizeof(BlasInstanceData));
            }

            wallBlasInstanceData.vertexOffset = mapMesh.WallMesh.Instance.VertexOffset;
            wallBlasInstanceData.indexOffset = mapMesh.WallMesh.Instance.IndexOffset;
            fixed (BlasInstanceData* ptr = &wallBlasInstanceData)
            {
                wallShader.BindSbt(wallInstanceData.InstanceName, sbt, tlas, new IntPtr(ptr), (uint)sizeof(BlasInstanceData));
            }
        }
    }
}
