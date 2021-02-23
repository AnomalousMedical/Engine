using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Player : IDisposable
    {
        private AutoPtr<IShaderResourceBinding> pboMatBindingSprite;
        private SceneObjectManager sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private SceneObject sceneObject;
        private Sprite sprite = new Sprite(new Dictionary<string, SpriteAnimation>()
        {
        { "default", new SpriteAnimation((int)(0.3f * Clock.SecondsToMicro), SpriteFrame.MakeFramesFromHorizontal(24f / 192f, 32f / 128f, 192f, 8, 0).ToArray()) }
        });

        public Player(
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

                using var image = new UsingProxy<FreeImageBitmap>();
                yield return coroutine.Await(Task.Run(() =>
                {
                    using var stream = 
                        virtualFileSystem.openStream("spritewalk/rpg_sprite_walk_Color.png", Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read, Engine.Resources.FileShare.Read);
                    image.Value = FreeImageBitmap.FromStream(stream);
                }));
                var materials = new Dictionary<uint, (String, String)>()
                {
                    //{ 0xff6a0e91, ( "cc0Textures/Fabric012_1K", "jpg" ) }, //Shirt (purple)
                    //{ 0xffbf1b00, ( "cc0Textures/Fabric045_1K", "jpg" ) }, //Pants (red)
                    ////{ 0xfff0b878, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Skin
                    //{ 0xff492515, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Hair (brown)
                    //{ 0xff0002bf, ( "cc0Textures/Leather026_1K", "jpg" ) }, //Shoes (blue)
                };
                var scale = Math.Min(1024 / image.Value.Width, 1024 / image.Value.Height);

                using var ccoTextures = new UsingProxy<CC0MaterialTextureBuffers>();
                yield return coroutine.Await(Task.Run(() =>
                    ccoTextures.Value = cc0MaterialTextureBuilder.CreateMaterialSet(image.Value, scale, materials)
                ));

                using var colorTexture = textureLoader.CreateTextureFromImage(image.Value, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

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
                    position = new Vector3(0, 0.291666666666667f, 0),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1, 1.291666666666667f, 1),
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
