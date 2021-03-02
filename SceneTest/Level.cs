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

namespace SceneTest
{
    class Level : IDisposable
    {
        public class Description : SceneObjectDesc
        {
            public String FloorTexture { get; set; } = "cc0Textures/Ground037_1K";

            public String WallTexture { get; set; } = "cc0Textures/Bricks045_1K";

            public int RandomSeed { get; set; } = 1;

            public int Width { get; set; } = 150;

            public int Height { get; set; } = 150;
        }

        private readonly SceneObjectManager sceneObjectManager;
        private readonly IBepuScene bepuScene;
        private readonly ICC0TextureManager textureManager;
        private IShaderResourceBinding wallMatBinding;
        private IShaderResourceBinding floorMatBinding;
        private SceneObject wallSceneObject;
        private SceneObject floorSceneObject;
        private List<StaticHandle> staticHandles = new List<StaticHandle>();
        private TypedIndex shapeIndex;
        private bool disposed;
        private MapMesh mapMesh;

        public Vector3 StartPoint { get; set; }

        public Level(
            SceneObjectManager sceneObjectManager,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            IBepuScene bepuScene,
            ICC0TextureManager textureManager,
            Description description,
            GraphicsEngine graphicsEngine)
        {
            var random = new Random(description.RandomSeed);
            var mapBuilder = new csMapbuilder(random, description.Width, description.Height);
            mapBuilder.Build_ConnectedStartRooms();
            mapMesh = new MapMesh(mapBuilder, graphicsEngine.RenderDevice, mapUnitX: 3, mapUnitY: 3.5f, mapUnitZ:3);
            var startRoom = mapBuilder.StartRoom;
            var startX = startRoom.Left + startRoom.Width / 2;
            var startY = startRoom.Top + startRoom.Height / 2;
            StartPoint = mapMesh.PointToVector(startX, startY);

            this.sceneObjectManager = sceneObjectManager;
            this.bepuScene = bepuScene;
            this.textureManager = textureManager;
            wallSceneObject = new SceneObject()
            {
                vertexBuffer = mapMesh.WallMesh.VertexBuffer,
                skinVertexBuffer = mapMesh.WallMesh.SkinVertexBuffer,
                indexBuffer = mapMesh.WallMesh.IndexBuffer,
                numIndices = mapMesh.WallMesh.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = description.Translation,
                orientation = description.Orientation,
                scale = description.Scale,
                RenderShadow = true,
                GetShadows = false
            };
            floorSceneObject = new SceneObject()
            {
                vertexBuffer = mapMesh.FloorMesh.VertexBuffer,
                skinVertexBuffer = mapMesh.FloorMesh.SkinVertexBuffer,
                indexBuffer = mapMesh.FloorMesh.IndexBuffer,
                numIndices = mapMesh.FloorMesh.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = description.Translation,
                orientation = description.Orientation,
                scale = description.Scale,
                RenderShadow = false,
                GetShadows = true
            };

            var shape = new Box(mapMesh.MapUnitX, mapMesh.MapUnitY, mapMesh.MapUnitZ); //Each one creates its own, try to load from resources
            shapeIndex = bepuScene.Simulation.Shapes.Add(shape);

            var orientation = System.Numerics.Quaternion.Identity;
            foreach (var boundary in mapMesh.BoundaryCubeCenterPoints)
            {
                var staticHandle = bepuScene.Simulation.Statics.Add(
                    new StaticDescription(
                        boundary.ToSystemNumerics(),
                        orientation,
                        new CollidableDescription(shapeIndex, 0.1f)));

                staticHandles.Add(staticHandle);
            }

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                var wallTextureTask = textureManager.Checkout(new CCOTextureBindingDescription(description.WallTexture, getShadow: false));
                var floorTextureTask = textureManager.Checkout(new CCOTextureBindingDescription(description.FloorTexture, getShadow: true));
                wallMatBinding = await wallTextureTask;
                floorMatBinding = await floorTextureTask;
                if (disposed)
                {
                    textureManager.Return(wallMatBinding);
                    textureManager.Return(floorMatBinding);
                    return; //Stop loading
                }

                if (!destructionRequest.DestructionRequested)
                {
                    wallSceneObject.shaderResourceBinding = wallMatBinding;
                    this.sceneObjectManager.Add(wallSceneObject);

                    floorSceneObject.shaderResourceBinding = floorMatBinding;
                    this.sceneObjectManager.Add(floorSceneObject);
                }
            });
        }

        public void Dispose()
        {
            disposed = true;
            bepuScene.Simulation.Shapes.Remove(shapeIndex);
            var statics = bepuScene.Simulation.Statics;
            foreach (var staticHandle in staticHandles)
            {
                statics.Remove(staticHandle);
            }
            sceneObjectManager.Remove(wallSceneObject);
            sceneObjectManager.Remove(floorSceneObject);
            textureManager.TryReturn(wallMatBinding);
            textureManager.TryReturn(floorMatBinding);
            mapMesh.Dispose();
        }
    }
}
