using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.Resources;
using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle
{
    class BattleArena : IDisposable
    {
        public class Description : SceneObjectDesc
        {
            public String Texture { get; set; } = "cc0Textures/Bricks045_1K";
        }

        private TLASBuildInstanceData floorInstanceData;
        private readonly IDestructionRequest destructionRequest;
        private readonly MeshBLAS floorMesh;
        private readonly TextureManager textureManager;
        private readonly RTInstances rtInstances;
        private readonly RayTracingRenderer renderer;
        private PrimaryHitShader floorShader;
        TaskCompletionSource loadingTask = new TaskCompletionSource();

        CC0TextureResult floorTexture;

        public BattleArena
        (
            Description description,
            IScopedCoroutine coroutineRunner,
            IDestructionRequest destructionRequest,
            MeshBLAS floorMesh,
            TextureManager textureManager,
            PrimaryHitShader.Factory primaryHitShaderFactory,
            RTInstances rtInstances,
            RayTracingRenderer renderer
        )
        {
            this.destructionRequest = destructionRequest;
            this.floorMesh = floorMesh;
            this.textureManager = textureManager;
            this.rtInstances = rtInstances;
            this.renderer = renderer;
            coroutineRunner.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction();
                try
                {
                    var floorTextureDesc = new CCOTextureBindingDescription(description.Texture);

                    var floorTextureTask = textureManager.Checkout(floorTextureDesc);

                    //Right now this just makes a plane, which does make one per arena request, but whatever for now
                    //Will replace this with more dynamic geometry later that will make this worth it
                    //Note this all happens on the main thread too, but can be backgrounded if it becomes more complex
                    floorMesh.Begin(1);

                    var size = 10f;

                    floorMesh.AddQuad(new Vector3(-size, size, 0), new Vector3(size, size, 0), new Vector3(size, -size, 0), new Vector3(-size, -size, 0),
                                      Vector3.Up, Vector3.Up, Vector3.Up, Vector3.Up,
                                      new Vector2(0, 0),
                                      new Vector2(1, 1));

                    var floorShaderSetup = primaryHitShaderFactory.Create(floorMesh.Name, floorTextureDesc.NumTextures, PrimaryHitShaderType.Cube);

                    await Task.WhenAll
                    (
                        floorTextureTask,
                        floorMesh.End("BattleArenaFloor"),
                        floorShaderSetup
                    );

                    this.floorShader = floorShaderSetup.Result;
                    this.floorTexture = floorTextureTask.Result;

                    if (!destructionRequest.DestructionRequested)
                    {
                        this.floorInstanceData = new TLASBuildInstanceData()
                        {
                            InstanceName = RTId.CreateId("BattleArenaFloor"),
                            CustomId = 3, //Texture index
                            pBLAS = floorMesh.Instance.BLAS.Obj,
                            Mask = RtStructures.OPAQUE_GEOM_MASK,
                            Transform = new InstanceMatrix(Vector3.Zero, Quaternion.Identity)
                        };

                        rtInstances.AddTlasBuild(floorInstanceData);
                        rtInstances.AddShaderTableBinder(Bind);
                        renderer.AddShaderResourceBinder(Bind);
                    }

                    loadingTask.SetResult();
                }
                catch (Exception ex)
                {
                    loadingTask.SetException(ex);
                }
            });
        }

        public void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }

        public void Dispose()
        {
            textureManager.TryReturn(floorTexture);
            renderer.RemoveShaderResourceBinder(Bind);
            rtInstances.RemoveShaderTableBinder(Bind);
            this.floorShader?.Dispose();
            rtInstances.RemoveTlasBuild(floorInstanceData);
        }

        public void SetTransform(InstanceMatrix matrix)
        {
            this.floorInstanceData.Transform = matrix;
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            sbt.BindHitGroupForInstance(tlas, floorInstanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, floorShader.ShaderGroupName, IntPtr.Zero, 0);
        }

        public void Bind(IShaderResourceBinding rayTracingSRB)
        {
            floorShader.BindBlas(floorMesh.Instance, rayTracingSRB);
            floorShader.BindTextures(rayTracingSRB, floorTexture);
        }

        public Task WaitForLoad()
        {
            return loadingTask.Task;
        }
    }
}
