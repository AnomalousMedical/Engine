﻿using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using FreeImageAPI;
using Engine.CameraMovement;
using System.Linq;

namespace SceneTest
{
    class SceneTestUpdateListener : UpdateListener, IDisposable
    {
        //Camera Settings
        float YFov = (float)Math.PI / 4.0f;
        float ZNear = 0.1f;
        float ZFar = 100f;

        //Clear Color
        Engine.Color ClearColor = Engine.Color.FromARGB(0xff2a63cc);

        //Light
        Vector3 lightDirection = Vector3.Up;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3;

        private readonly NativeOSWindow window;
        private readonly DoubleSizeCube shape;
        private readonly Plane plane;
        private readonly TextureLoader textureLoader;
        private readonly CC0TextureLoader cc0TextureLoader;
        private readonly EnvironmentMapBuilder envMapBuilder;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly ICC0MaterialTextureBuilder cC0MaterialTextureBuilder;
        private readonly VirtualFileSystem virtualFileSystem;
        private readonly FirstPersonFlyCamera cameraControls;
        private readonly SimpleShadowMapRenderer shadowMapRenderer;
        private readonly TimeClock timeClock;
        private ISwapChain swapChain;
        private IRenderDevice renderDevice;
        private IDeviceContext immediateContext;
        private PbrRenderAttribs pbrRenderAttribs = PbrRenderAttribs.CreateDefault();

        private PbrRenderer pbrRenderer;
        private AutoPtr<ITextureView> environmentMapSRV;
        private AutoPtr<IShaderResourceBinding> pboMatBindingSprite;
        private AutoPtr<IShaderResourceBinding> pboMatBindingTinyDinoSprite;
        private AutoPtr<IShaderResourceBinding> pboMatBindingSwordSprite;
        private AutoPtr<IShaderResourceBinding> pboMatBindingSceneObject;
        private AutoPtr<IShaderResourceBinding> pboMatBindingFloor;

        private List<SceneObject> sceneObjects = new List<SceneObject>();

        public unsafe SceneTestUpdateListener(
            GraphicsEngine graphicsEngine,
            NativeOSWindow window,
            PbrRenderer m_GLTFRenderer,
            DoubleSizeCube shape,
            Plane plane,
            TextureLoader textureLoader,
            CC0TextureLoader cc0TextureLoader,
            EnvironmentMapBuilder envMapBuilder,
            IPbrCameraAndLight pbrCameraAndLight,
            ICC0MaterialTextureBuilder cC0MaterialTextureBuilder,
            VirtualFileSystem virtualFileSystem,
            FirstPersonFlyCamera cameraControls,
            SimpleShadowMapRenderer shadowMapRenderer,
            TimeClock timeClock)
        {
            cameraControls.Position = new Vector3(0, 0, -12);

            this.pbrRenderer = m_GLTFRenderer;
            this.swapChain = graphicsEngine.SwapChain;
            this.renderDevice = graphicsEngine.RenderDevice;
            this.immediateContext = graphicsEngine.ImmediateContext;
            this.window = window;
            this.shape = shape;
            this.plane = plane;
            this.textureLoader = textureLoader;
            this.cc0TextureLoader = cc0TextureLoader;
            this.envMapBuilder = envMapBuilder;
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.cC0MaterialTextureBuilder = cC0MaterialTextureBuilder;
            this.virtualFileSystem = virtualFileSystem;
            this.cameraControls = cameraControls;
            this.shadowMapRenderer = shadowMapRenderer;
            this.timeClock = timeClock;
            Initialize();
        }

        public void Dispose()
        {
            pboMatBindingSwordSprite.Dispose();
            pboMatBindingTinyDinoSprite.Dispose();
            pboMatBindingFloor.Dispose();
            pboMatBindingSceneObject.Dispose();
            pboMatBindingSprite.Dispose();
            environmentMapSRV.Dispose();
        }

        unsafe void Initialize()
        {
            environmentMapSRV = envMapBuilder.BuildEnvMapView(renderDevice, immediateContext, "papermill/Fixed-", "png");

            pbrRenderer.PrecomputeCubemaps(renderDevice, immediateContext, environmentMapSRV.Obj);
            shadowMapRenderer.CreateShadowPSO(swapChain, renderDevice, pbrCameraAndLight.CameraAttribs, pbrCameraAndLight.LightAttribs);

            LoadFloorTexture();
            LoadSceneObjectTexture();
            LoadTinyDinoSprite();
            LoadPlayerSprite();
            LoadSwordSprite();

            //Create a manual shiny texture to see env map
            //CreateShinyTexture();

            //Make scene
            {
                //Background cubes
                pbrRenderAttribs.AlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE;

                AddBrick(new Vector3(-3, 0, 1), Quaternion.Identity);
                AddBrick(new Vector3(-3, 0, 2), Quaternion.Identity);
                AddBrick(new Vector3(-3, 0, 3), Quaternion.Identity);
                AddBrick(new Vector3(0, 0, 3), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, 3), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, 2), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, 1), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, 0), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, -1), Quaternion.Identity);
                AddBrick(new Vector3(1, 0, -2), Quaternion.Identity);
            }

            {
                //Floor
                sceneObjects.Add(new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                    position = new Vector3(0, -1, 0),
                    orientation = Quaternion.shortestArcQuat(Vector3.Forward, Vector3.Down),
                    scale = new Vector3(10, 10, 10),
                    shaderResourceBinding = pboMatBindingFloor.Obj,
                    GetShadows = true,
                });
            }

            {
                //Tiny Dino
                sceneObjects.Add(new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = new Vector3(-4, 0, -3),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1.466666666666667f, 1, 1),
                    shaderResourceBinding = pboMatBindingTinyDinoSprite.Obj
                });
            }

            {
                //Player
                sceneObjects.Add(new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = new Vector3(0, 0.291666666666667f, 0),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1, 1.291666666666667f, 1),
                    shaderResourceBinding = pboMatBindingSprite.Obj
                });
            }

            {
                //Sword
                sceneObjects.Add(new SceneObject()
                {
                    vertexBuffer = plane.VertexBuffer,
                    skinVertexBuffer = plane.SkinVertexBuffer,
                    indexBuffer = plane.IndexBuffer,
                    numIndices = plane.NumIndices,
                    pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                    position = new Vector3(-1, 0, 0),
                    orientation = Quaternion.Identity,
                    scale = new Vector3(1, 1.714285714285714f, 1) * 0.5f,
                    shaderResourceBinding = pboMatBindingSwordSprite.Obj
                });
            }
        }

        private unsafe void CreateShinyTexture()
        {
            const uint texDim = 10;
            var physDesc = new UInt32[texDim * texDim];
            var physDescSpan = new Span<UInt32>(physDesc);
            physDescSpan.Fill(0xff0000ff);

            var TexDesc = new TextureDesc();
            TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            TexDesc.Usage = USAGE.USAGE_IMMUTABLE;
            TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE;
            TexDesc.Depth = 1;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;
            TexDesc.MipLevels = 1;
            TexDesc.Format = TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;
            TexDesc.Width = 10;
            TexDesc.Height = 10;

            fixed (UInt32* pPhysDesc = physDesc)
            {
                var Level0Data = new TextureSubResData { pData = new IntPtr(pPhysDesc), Stride = texDim * 4 };
                var InitData = new TextureData { pSubResources = new List<TextureSubResData> { Level0Data } };

                using var physicalDescriptorMap = renderDevice.CreateTexture(TexDesc, InitData);

                pboMatBindingSprite = pbrRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    physicalDescriptorMap: physicalDescriptorMap.Obj
                );
            }
        }

        private unsafe void LoadFloorTexture()
        {
            using var ccoTextures = cc0TextureLoader.LoadTextureSet("cc0Textures/Ground037_1K");
            pboMatBindingFloor = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: ccoTextures.BaseColorMap,
                normalMap: ccoTextures.NormalMap,
                physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                aoMap: ccoTextures.AmbientOcclusionMap,
                shadowMapSRV: shadowMapRenderer.ShadowMapSRV
            );
        }

        private unsafe void LoadSceneObjectTexture()
        {
            using var ccoTextures = cc0TextureLoader.LoadTextureSet("cc0Textures/Bricks045_1K");
            pboMatBindingSceneObject = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: ccoTextures.BaseColorMap,
                normalMap: ccoTextures.NormalMap,
                physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                aoMap: ccoTextures.AmbientOcclusionMap
            );
        }

        private unsafe void LoadPlayerSprite()
        {
            using var stream = virtualFileSystem.openStream("spritewalk/Simple_Color.png", Engine.Resources.FileMode.Open);
            using var image = FreeImageBitmap.FromStream(stream);
            var materials = new Dictionary<uint, (String, String)>()
            {
                { 0xff6a0e91, ( "cc0Textures/Fabric048_1K", "jpg" ) }, //Shirt (purple)
                { 0xffbf1b00, ( "cc0Textures/Fabric045_1K", "jpg" ) }, //Pants (red)
                //{ 0xfff0b878, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Skin
                { 0xff492515, ( "cc0Textures/Carpet008_1K", "jpg" ) }, //Hair (brown)
                { 0xff0002bf, ( "cc0Textures/Leather026_1K", "jpg" ) }, //Shoes (blue)
            };
            var scale = Math.Min(1024 / image.Width, 1024 / image.Height);

            using var ccoTextures = cC0MaterialTextureBuilder.CreateMaterialSet(image, scale, materials);
            
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
                alphaMode: PbrAlphaMode.ALPHA_MODE_MASK
            );
        }

        private unsafe void LoadTinyDinoSprite()
        {
            using var stream = virtualFileSystem.openStream("original/TinyDino_Color.png", Engine.Resources.FileMode.Open);
            using var image = FreeImageBitmap.FromStream(stream);
            var materials = new Dictionary<uint, (String, String)>()
            {
                { 0xff168516, ( "cc0Textures/Leather008_1K", "jpg" ) }, //Skin (green)
                { 0xffff0000, ( "cc0Textures/SheetMetal004_1K", "jpg" ) }, //Spines (red)
            };
            var scale = Math.Min(1024 / image.Width, 1024 / image.Height);

            using var ccoTextures = cC0MaterialTextureBuilder.CreateMaterialSet(image, scale, materials);

            using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

            using var normalTexture = ccoTextures.NormalMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.NormalMap, 1, "normalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var physicalTexture = ccoTextures.PhysicalDescriptorMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.PhysicalDescriptorMap, 1, "physicalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var aoTexture = ccoTextures.AmbientOcclusionMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.AmbientOcclusionMap, 1, "aoTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            pboMatBindingTinyDinoSprite = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: colorTexture?.Obj,
                normalMap: normalTexture?.Obj,
                physicalDescriptorMap: physicalTexture?.Obj,
                aoMap: aoTexture?.Obj,
                alphaMode: PbrAlphaMode.ALPHA_MODE_MASK
            );
        }

        private unsafe void LoadSwordSprite()
        {
            using var stream = virtualFileSystem.openStream("original/Sword.png", Engine.Resources.FileMode.Open);
            using var image = FreeImageBitmap.FromStream(stream);
            var materials = new Dictionary<uint, (String, String)>()
            {
                { 0xff6c351c, ( "cc0Textures/Wood049_1K", "jpg" ) }, //Hilt (brown)
                { 0xffadadad, ( "cc0Textures/Metal038_1K", "jpg" ) }, //Blade (grey)
            };
            var scale = Math.Min(1024 / image.Width, 1024 / image.Height);

            using var ccoTextures = cC0MaterialTextureBuilder.CreateMaterialSet(image, scale, materials);

            using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

            using var normalTexture = ccoTextures.NormalMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.NormalMap, 1, "normalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var physicalTexture = ccoTextures.PhysicalDescriptorMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.PhysicalDescriptorMap, 1, "physicalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            using var aoTexture = ccoTextures.AmbientOcclusionMap != null ?
                textureLoader.CreateTextureFromImage(ccoTextures.AmbientOcclusionMap, 1, "aoTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

            pboMatBindingSwordSprite = pbrRenderer.CreateMaterialSRB(
                pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                pLightAttribs: pbrCameraAndLight.LightAttribs,
                baseColorMap: colorTexture?.Obj,
                normalMap: normalTexture?.Obj,
                physicalDescriptorMap: physicalTexture?.Obj,
                aoMap: aoTexture?.Obj,
                alphaMode: PbrAlphaMode.ALPHA_MODE_MASK
            );
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public unsafe void sendUpdate(Clock clock)
        {
            //Update
            timeClock.Update(clock);
            cameraControls.UpdateInput(clock);
            UpdateLight(clock);

            //Render
            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            var PreTransform = swapChain.GetDesc_PreTransform;
            var preTransformMatrix = CameraHelpers.GetSurfacePretransformMatrix(new Vector3(0, 0, 1), PreTransform);
            var cameraProjMatrix = CameraHelpers.GetAdjustedProjectionMatrix(YFov, ZNear, ZFar, window.WindowWidth, window.WindowHeight, PreTransform);

            //Draw Scene
            // Render shadow map
            shadowMapRenderer.BeginShadowMap(renderDevice, immediateContext, lightDirection);
            foreach (var sceneObj in sceneObjects.Where(i => i.shaderResourceBinding == pboMatBindingSceneObject.Obj)) //Render all bricks for shadow map
            {
                pbrRenderAttribs.AlphaMode = sceneObj.pbrAlphaMode;
                shadowMapRenderer.RenderShadowMap(immediateContext, sceneObj.vertexBuffer, sceneObj.skinVertexBuffer, sceneObj.indexBuffer, sceneObj.numIndices, ref sceneObj.position, ref sceneObj.orientation, ref sceneObj.scale, pbrRenderAttribs);
            }

            //Render scene colors
            pbrRenderer.Begin(immediateContext);
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            // Clear the back buffer
            immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            // Set Light
            var WorldToShadowMapUVDepthMatrix = shadowMapRenderer.WorldToShadowMapUVDepthMatr;
            pbrCameraAndLight.SetLightAndShadow(ref lightDirection, ref lightColor, lightIntensity, ref WorldToShadowMapUVDepthMatrix);

            // Set Camera
            //pbrCameraAndLight.SetCameraMatrices(ref cameraProjMatrix, ref WorldToShadowMapUVDepthMatr, ref Vector3.Forward);
            pbrCameraAndLight.SetCameraPosition(cameraControls.Position, cameraControls.Orientation, ref preTransformMatrix, ref cameraProjMatrix);

            foreach (var sceneObj in sceneObjects)
            {
                pbrRenderAttribs.AlphaMode = sceneObj.pbrAlphaMode;
                pbrRenderAttribs.GetShadows = sceneObj.GetShadows;
                pbrRenderer.Render(immediateContext, sceneObj.shaderResourceBinding, sceneObj.vertexBuffer, sceneObj.skinVertexBuffer, sceneObj.indexBuffer, sceneObj.numIndices, ref sceneObj.position, ref sceneObj.orientation, ref sceneObj.scale, pbrRenderAttribs);
            }

            this.shadowMapRenderer.RenderShadowMapVis(immediateContext);

            this.swapChain.Present(1);
        }

        private unsafe void UpdateLight(Clock clock)
        {
            //var rotAmount = clock.CurrentTimeMicro * Clock.MicroToSeconds % (2 * (float)Math.PI);
            //var rot = new Quaternion(0f, rotAmount, 0f);
            //lightDirection = rot.toRotationMatrix4x4() * Vector3.Down;
            //if (lightDirection.y < 0)
            //{
            //    lightIntensity = -3 * lightDirection.y;
            //    pbrRenderAttribs.AverageLogLum = 0.3f;
            //}
            //else
            //{
            //    pbrRenderAttribs.AverageLogLum = Math.Max(lightDirection.y * .7f, 0.3f);
            //    lightIntensity = 0;
            //}

            //lightDirection = Vector3.Down;

            if (timeClock.IsDay)
            {
                var dayFactor = (timeClock.DayFactor - 0.5f) * 2.0f;
                var noonFactor = 1.0f - Math.Abs(dayFactor);
                lightDirection = (Vector3.Down * noonFactor + new Vector3(dayFactor, 0, 0)).normalized();
                lightIntensity = 3 * noonFactor;

                //pbrRenderAttribs.AverageLogLum = 0.3f;
            }
            else
            {
                var nightFactor = (timeClock.NightFactor - 0.5f) * 2.0f;
                var midnightFactor = 1.0f - Math.Abs(nightFactor);
                lightDirection = (Vector3.Down * midnightFactor + new Vector3(nightFactor, 0, 0)).normalized();

                lightIntensity = 0.3f * midnightFactor;

                //pbrRenderAttribs.AverageLogLum = 0.7f;
            }
        }

        private void AddBrick(Vector3 trans, Quaternion rot)
        {
            sceneObjects.Add(new SceneObject()
            {
                vertexBuffer = shape.VertexBuffer,
                skinVertexBuffer = shape.SkinVertexBuffer,
                indexBuffer = shape.IndexBuffer,
                numIndices = shape.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE,
                position = trans * 2,
                orientation = rot,
                scale = Vector3.ScaleIdentity,
                shaderResourceBinding = pboMatBindingSceneObject.Obj
            });
        }
    }
}
