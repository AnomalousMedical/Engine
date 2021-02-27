using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin;
using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Floor : IDisposable
    {
        public class Description : SceneObjectDesc
        {
            public Description()
            {
                this.Translation = new Vector3(0, -1, 0);
                this.Orientation = Quaternion.shortestArcQuat(Vector3.Forward, Vector3.Down);
                this.Scale = new Vector3(10, 10, 1);
            }

            public String Texture { get; set; } = "cc0Textures/Ground037_1K";
        }

        private readonly SceneObjectManager sceneObjectManager;
        private readonly ICC0TextureManager textureManager;
        private readonly IBepuScene bepuScene;
        private IShaderResourceBinding matBinding;
        private SceneObject sceneObject;
        private StaticHandle staticHandle;

        public Floor(
            SceneObjectManager sceneObjectManager,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ICC0TextureManager textureManager,
            IBepuScene bepuScene,
            Description description)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.textureManager = textureManager;
            this.bepuScene = bepuScene;
            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = description.Translation,
                orientation = description.Orientation,
                scale = description.Scale,
                GetShadows = true,
            };

            //Load this stuff better
            var simulation = bepuScene.Simulation;
            var box = new Box(description.Scale.x, description.Scale.y, description.Scale.z);
            staticHandle = simulation.Statics.Add(
                new StaticDescription(
                    new System.Numerics.Vector3(description.Translation.x, description.Translation.y, description.Translation.z),
                    new System.Numerics.Quaternion(description.Orientation.x, description.Orientation.y, description.Orientation.z, description.Orientation.w),
                    new CollidableDescription(simulation.Shapes.Add(box), 0.1f)));

            IEnumerator<YieldAction> co()
            {
                //This will load 1 texture per brick
                yield return coroutine.Await(async () =>
                {
                    matBinding = await textureManager.Checkout(new CCOTextureBindingDescription(description.Texture, getShadow: true));
                });

                sceneObject.shaderResourceBinding = matBinding;

                this.sceneObjectManager.Add(sceneObject);
            }
            coroutine.Run(co());
        }

        public void Dispose()
        {
            bepuScene.Simulation.Statics.Remove(staticHandle);
            sceneObjectManager.Remove(sceneObject);
            textureManager.Return(matBinding);
        }
    }
}
