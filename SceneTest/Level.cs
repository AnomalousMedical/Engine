using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
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
        public class Description : SceneObjectDesc
        {
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

        private readonly SceneObjectManager<LevelManager> sceneObjectManager;
        private readonly IDestructionRequest destructionRequest;
        private readonly IBepuScene bepuScene;
        private readonly ICC0TextureManager textureManager;
        private readonly ILogger<Level> logger;
        private readonly IBiomeManager biomeManager;
        private IShaderResourceBinding wallMatBinding;
        private IShaderResourceBinding floorMatBinding;
        private SceneObject wallSceneObject;
        private SceneObject floorSceneObject;
        private List<StaticHandle> staticHandles = new List<StaticHandle>();
        private TypedIndex boundaryCubeShapeIndex;
        private TypedIndex floorCubeShapeIndex;
        private bool disposed;
        private MapMesh mapMesh;
        private bool physicsActive = false;
        private IObjectResolver objectResolver;
        private LevelConnector nextLevelConnector;
        private LevelConnector previousLevelConnector;
        private Biome biome;

        private Task levelGenerationTask;
        private Vector3 mapUnits;

        private Vector3 endPointLocal;
        private Vector3 startPointLocal;
        public Vector3 StartPoint => startPointLocal + wallSceneObject.position;
        public Vector3 EndPoint => endPointLocal + wallSceneObject.position;

        public Level(
            SceneObjectManager<LevelManager> sceneObjectManager,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            IBepuScene bepuScene,
            ICC0TextureManager textureManager,
            Description description,
            GraphicsEngine graphicsEngine,
            ILogger<Level> logger,
            IObjectResolverFactory objectResolverFactory,
            IBiomeManager biomeManager)
        {
            this.mapUnits = new Vector3(description.MapUnitX, description.MapUnitY, description.MapUnitZ);
            this.objectResolver = objectResolverFactory.Create();
            this.sceneObjectManager = sceneObjectManager;
            this.destructionRequest = destructionRequest;
            this.bepuScene = bepuScene;
            this.textureManager = textureManager;
            this.logger = logger;
            this.biomeManager = biomeManager;
            wallSceneObject = new SceneObject()
            {
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = description.Translation,
                orientation = description.Orientation,
                scale = description.Scale,
                RenderShadow = false,
                GetShadows = true
            };
            floorSceneObject = new SceneObject()
            {
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = description.Translation,
                orientation = description.Orientation,
                scale = description.Scale,
                RenderShadow = false,
                GetShadows = true
            };

            var random = new Random(description.RandomSeed);
            biome = biomeManager.GetBiome(random.Next(0, biomeManager.Count));

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

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

                    Rectangle eastmostRoom = new Rectangle(int.MaxValue, 0, 0, 0);
                    Rectangle westmostRoom = new Rectangle(0, 0, 0, 0);
                    foreach(var room in mapBuilder.Rooms)
                    {
                        if(room.Left < eastmostRoom.Left)
                        {
                            eastmostRoom = room;
                        }
                        if(room.Right > westmostRoom.Right)
                        {
                            westmostRoom = room;
                        }
                    }

                    mapMesh = new MapMesh(mapBuilder, random, graphicsEngine.RenderDevice, mapUnitX: description.MapUnitX, mapUnitY: description.MapUnitY, mapUnitZ: description.MapUnitZ);
                    var startRoom = eastmostRoom;
                    var startX = startRoom.Left;
                    var startY = startRoom.Top + startRoom.Height / 2;
                    if(startX > 0)
                    {
                        var xOffset = startX - 1;
                        var cell = mapBuilder.map[xOffset, startY];
                        if(cell >= csMapbuilder.CorridorCell)
                        {
                            startY = startRoom.Top;
                            cell = mapBuilder.map[xOffset, startY];
                            var bottom = startRoom.Bottom;
                            while(cell >= csMapbuilder.CorridorCell && startY < startRoom.Bottom)
                            {
                                cell = mapBuilder.map[xOffset, ++startY];
                            }

                            if(startY == startRoom.Bottom)
                            {
                                //Pretty unlikely, but use center of room in this case, this means the whole east wall is corridors
                                startX = startRoom.Left + startRoom.Width / 2;
                                startY = startRoom.Top + startRoom.Height / 2;
                            }
                        }
                    }
                    startPointLocal = mapMesh.PointToVector(startX, startY);

                    var endRoom = westmostRoom;
                    var endX = endRoom.Right;
                    var endY = endRoom.Top + endRoom.Height / 2;
                    if (endX + 1 < mapBuilder.Map_Size.Width)
                    {
                        var xOffset = endX + 1;
                        var cell = mapBuilder.map[xOffset, endY];
                        if (cell >= csMapbuilder.CorridorCell)
                        {
                            endY = endRoom.Top;
                            cell = mapBuilder.map[xOffset, endY];
                            var bottom = endRoom.Bottom;
                            while (cell >= csMapbuilder.CorridorCell && endY < endRoom.Bottom)
                            {
                                cell = mapBuilder.map[xOffset, ++endY];
                            }

                            if (endY == endRoom.Bottom)
                            {
                                //Pretty unlikely, but use center of room in this case, this means the whole east wall is corridors
                                endX = endRoom.Left + endRoom.Width / 2;
                                endY = endRoom.Top + endRoom.Height / 2;
                            }
                        }
                    }
                    endPointLocal = mapMesh.PointToVector(endX, endY);
                    sw.Stop();
                    logger.LogInformation($"Generated level {description.RandomSeed} in {sw.ElapsedMilliseconds} ms.");
                });
                var wallTextureTask = textureManager.Checkout(new CCOTextureBindingDescription(biome.WallTexture, getShadow: true));
                var floorTextureTask = textureManager.Checkout(new CCOTextureBindingDescription(biome.FloorTexture, getShadow: true));

                await levelGenerationTask;

                if (description.GoPrevious)
                {
                    this.previousLevelConnector = objectResolver.Resolve<LevelConnector, LevelConnector.Description>(o =>
                    {
                        o.Scale = new Vector3(description.MapUnitX, 0.05f, description.MapUnitZ);
                        o.Texture = biome.FloorTexture;
                        o.Translation = StartPoint + new Vector3(-(mapUnits.x / 2f + 0.5f), 0f, 0f);
                        o.GoPrevious = true;
                    });
                }

                this.nextLevelConnector = objectResolver.Resolve<LevelConnector, LevelConnector.Description>(o =>
                {
                    o.Scale = new Vector3(3.0f, 0.05f, 3.0f);
                    o.Texture = biome.FloorTexture;
                    o.Translation = EndPoint + new Vector3(mapUnits.x / 2f + 0.5f, 0f, 0f);
                    o.GoPrevious = false;
                });

                wallMatBinding = await wallTextureTask;
                floorMatBinding = await floorTextureTask;
                if (disposed)
                {
                    mapMesh.Dispose();
                    textureManager.Return(wallMatBinding);
                    textureManager.Return(floorMatBinding);
                    return; //Stop loading
                }

                //Setup vertex buffers
                wallSceneObject.vertexBuffer = mapMesh.WallMesh.VertexBuffer;
                wallSceneObject.skinVertexBuffer = mapMesh.WallMesh.SkinVertexBuffer;
                wallSceneObject.indexBuffer = mapMesh.WallMesh.IndexBuffer;
                wallSceneObject.numIndices = mapMesh.WallMesh.NumIndices;

                floorSceneObject.vertexBuffer = mapMesh.FloorMesh.VertexBuffer;
                floorSceneObject.skinVertexBuffer = mapMesh.FloorMesh.SkinVertexBuffer;
                floorSceneObject.indexBuffer = mapMesh.FloorMesh.IndexBuffer;
                floorSceneObject.numIndices = mapMesh.FloorMesh.NumIndices;

                if (!destructionRequest.DestructionRequested)
                {
                    wallSceneObject.shaderResourceBinding = wallMatBinding;
                    this.sceneObjectManager.Add(wallSceneObject);

                    floorSceneObject.shaderResourceBinding = floorMatBinding;
                    this.sceneObjectManager.Add(floorSceneObject);
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
            sceneObjectManager.Remove(wallSceneObject);
            sceneObjectManager.Remove(floorSceneObject);
            textureManager.TryReturn(wallMatBinding);
            textureManager.TryReturn(floorMatBinding);
            mapMesh?.Dispose();
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
            this.floorSceneObject.position = position;
            this.wallSceneObject.position = position;
            this.previousLevelConnector?.SetPosition(StartPoint + new Vector3(-(mapUnits.x / 2f + 0.5f), 0f, 0f));
            this.nextLevelConnector?.SetPosition(EndPoint + new Vector3((mapUnits.x / 2f + 0.5f), 0f, 0f));
        }

        public Biome Biome => biome;

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
                        (boundary.Position + wallSceneObject.position).ToSystemNumerics(),
                        orientation.ToSystemNumerics(),
                        new CollidableDescription(floorCubeShapeIndex, 0.1f)));

                staticHandles.Add(staticHandle);
            }

            foreach (var boundary in mapMesh.BoundaryCubeCenterPoints)
            {
                var staticHandle = bepuScene.Simulation.Statics.Add(
                    new StaticDescription(
                        (boundary + wallSceneObject.position).ToSystemNumerics(),
                        boundaryOrientation,
                        new CollidableDescription(boundaryCubeShapeIndex, 0.1f)));

                staticHandles.Add(staticHandle);
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

            var statics = bepuScene.Simulation.Statics;
            foreach (var staticHandle in staticHandles)
            {
                statics.Remove(staticHandle);
            }
            bepuScene.Simulation.Shapes.Remove(boundaryCubeShapeIndex);
            bepuScene.Simulation.Shapes.Remove(floorCubeShapeIndex);
            staticHandles.Clear();
        }
    }
}
