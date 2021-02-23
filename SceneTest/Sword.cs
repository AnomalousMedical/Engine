using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Sword : IDisposable
    {
        private AutoPtr<IShaderResourceBinding> pboMatBindingSprite;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private SceneObject sceneObject;
        private Sprite sprite = new Sprite();

        public Sword(
            SceneObjectManager sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            ICC0MaterialTextureBuilder cc0MaterialTextureBuilder,
            VirtualFileSystem virtualFileSystem,
            TextureLoader textureLoader,
            IPbrCameraAndLight pbrCameraAndLight,
            PbrRenderer pbrRenderer,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine)
        {
            IEnumerator<YieldAction> co()
            {
                this.sceneObjectManager = sceneObjectManager;
                this.sprites = sprites;
                this.destructionRequest = destructionRequest;

                using var stream = virtualFileSystem.openStream("original/Sword.png", Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read, Engine.Resources.FileShare.Read);
                using var image = FreeImageBitmap.FromStream(stream);
                var materials = new Dictionary<uint, (String, String)>()
                {
                { 0xff6c351c, ( "cc0Textures/Wood049_1K", "jpg" ) }, //Hilt (brown)
                { 0xffadadad, ( "cc0Textures/Metal032_1K", "jpg" ) }, //Blade (grey)
                };
                var scale = Math.Min(1024 / image.Width, 1024 / image.Height);

                using var ccoTextures = new UsingProxy<CC0MaterialTextureBuffers>();
                yield return coroutine.Await(Task.Run(() =>
                    ccoTextures.Value = cc0MaterialTextureBuilder.CreateMaterialSet(image, scale, materials)
                ));

                using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

                using var normalTexture = ccoTextures.Value.NormalMap != null ?
                    textureLoader.CreateTextureFromImage(ccoTextures.Value.NormalMap, 1, "normalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                using var physicalTexture = ccoTextures.Value.PhysicalDescriptorMap != null ?
                    textureLoader.CreateTextureFromImage(ccoTextures.Value.PhysicalDescriptorMap, 1, "physicalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                using var aoTexture = ccoTextures.Value.AmbientOcclusionMap != null ?
                    textureLoader.CreateTextureFromImage(ccoTextures.Value.AmbientOcclusionMap, 1, "aoTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                pboMatBindingSprite = pbrRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    baseColorMap: colorTexture?.Obj,
                    normalMap: normalTexture?.Obj,
                    physicalDescriptorMap: physicalTexture?.Obj,
                    aoMap: aoTexture?.Obj,
                    alphaMode: PbrAlphaMode.ALPHA_MODE_MASK,
                    isSprite: true
                );

                sceneObject = new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = new Vector3(-1, 0, 0),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1, 1.714285714285714f, 1) * 0.5f,
                    shaderResourceBinding = pboMatBindingSprite.Obj,
                    RenderShadowPlaceholder = true,
                    Sprite = sprite,
                };
                sprites.Add(sprite);
                sceneObjectManager.Add(sceneObject);
            }
            coroutine.Run(co());
        }

        public void Dispose()
        {
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            pboMatBindingSprite.Dispose();
        }
    }
}
