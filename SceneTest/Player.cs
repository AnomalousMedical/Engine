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
    class Player : IDisposable
    {
        private readonly AutoPtr<IShaderResourceBinding> pboMatBindingSprite;
        private readonly SceneObjectManager sceneObjectManager;
        private readonly SpriteManager sprites;
        private readonly IDestructionRequest destructionRequest;
        private readonly SceneObject sceneObject;
        private readonly Sprite sprite = new Sprite(new Dictionary<string, SpriteAnimation>()
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
            IDestructionRequest destructionRequest)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            sprites.Add(sprite);

            using var stream = virtualFileSystem.openStream("spritewalk/rpg_sprite_walk_Color.png", Engine.Resources.FileMode.Open);
            using var image = FreeImageBitmap.FromStream(stream);
            var materials = new Dictionary<uint, (String, String)>()
            {
                //{ 0xff6a0e91, ( "cc0Textures/Fabric012_1K", "jpg" ) }, //Shirt (purple)
                //{ 0xffbf1b00, ( "cc0Textures/Fabric045_1K", "jpg" ) }, //Pants (red)
                ////{ 0xfff0b878, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Skin
                //{ 0xff492515, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Hair (brown)
                //{ 0xff0002bf, ( "cc0Textures/Leather026_1K", "jpg" ) }, //Shoes (blue)
            };
            var scale = Math.Min(1024 / image.Width, 1024 / image.Height);

            using var ccoTextures = cc0MaterialTextureBuilder.CreateMaterialSet(image, scale, materials);

            using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

            using var normalTexture = ccoTextures.NormalMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.NormalMap, 1, "normalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var physicalTexture = ccoTextures.PhysicalDescriptorMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.PhysicalDescriptorMap, 1, "physicalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var aoTexture = ccoTextures.AmbientOcclusionMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.AmbientOcclusionMap, 1, "aoTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

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
            sceneObjectManager.Add(sceneObject);
        }

        public void Dispose()
        {
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            pboMatBindingSprite.Dispose();
        }
    }
}
