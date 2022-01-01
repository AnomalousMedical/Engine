using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.HLSL;
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
        private readonly ActiveTextures activeTextures;
        private readonly PrimaryHitShader.Factory primaryHitShaderFactory;
        private readonly RTInstances rtInstances;
        private PrimaryHitShader floorShader;
        private TaskCompletionSource loadingTask = new TaskCompletionSource();
        private CC0TextureResult floorTexture;
        private BlasInstanceData blasInstanceData;

        public BattleArena
        (
            Description description,
            IScopedCoroutine coroutineRunner,
            IDestructionRequest destructionRequest,
            MeshBLAS floorMesh,
            TextureManager textureManager,
            ActiveTextures activeTextures,
            PrimaryHitShader.Factory primaryHitShaderFactory,
            RTInstances<IBattleManager> rtInstances
        )
        {
            this.destructionRequest = destructionRequest;
            this.floorMesh = floorMesh;
            this.textureManager = textureManager;
            this.activeTextures = activeTextures;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
            this.rtInstances = rtInstances;
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

                    floorMesh.AddQuad(new Vector3(-size, 0, size), new Vector3(size, 0, size), new Vector3(size, 0, -size), new Vector3(-size, 0, -size),
                                      Vector3.Up, Vector3.Up, Vector3.Up, Vector3.Up,
                                      new Vector2(0, 0),
                                      new Vector2(size, size));

                    await floorMesh.End("BattleArenaFloor");

                    var floorShaderSetup = primaryHitShaderFactory.Checkout(new PrimaryHitShader.Desc
                    {
                        ShaderType = PrimaryHitShaderType.Mesh,
                        HasNormalMap = true,
                        HasPhysicalDescriptorMap = true
                    });

                    await Task.WhenAll
                    (
                        floorTextureTask,
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
                        blasInstanceData = activeTextures.AddActiveTexture(floorTexture);
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
            activeTextures.RemoveActiveTexture(floorTexture);
            textureManager.TryReturn(floorTexture);
            rtInstances.RemoveShaderTableBinder(Bind);
            primaryHitShaderFactory.TryReturn(floorShader);
            rtInstances.RemoveTlasBuild(floorInstanceData);
        }

        private unsafe void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            blasInstanceData.vertexOffset = floorMesh.Instance.VertexOffset;
            blasInstanceData.indexOffset = floorMesh.Instance.IndexOffset;
            fixed (BlasInstanceData* ptr = &blasInstanceData)
            {
                floorShader.BindSbt(floorInstanceData.InstanceName, sbt, tlas, new IntPtr(ptr), (uint)sizeof(BlasInstanceData));
            }
        }

        public Task WaitForLoad()
        {
            return loadingTask.Task;
        }
    }
}
