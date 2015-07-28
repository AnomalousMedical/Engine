﻿using Engine;
using Engine.Resources;
using Engine.Utility;
using OgrePlugin;
using OgrePlugin.VirtualTexture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public class UnifiedMaterialBuilder : MaterialBuilder, IDisposable
    {
        /// <summary>
        /// A version string to use to determine if the cache should be rebuilt, 
        /// if your saved version does not match this, rebuild the microcode cache.
        /// </summary>
        public const String Version = "1.0";

        private const String GroupName = "UnifiedMaterialBuilder__Reserved";

        delegate IndirectionTexture CreateMaterial(Technique technique, MaterialDescription description, bool alpha, bool depthCheck);

        private List<Material> createdMaterials = new List<Material>(); //This is only for detection
        private VirtualTextureManager virtualTextureManager;
        private String textureFormatExtension;
        private Dictionary<String, CreateMaterial> materialCreationFuncs = new Dictionary<string, CreateMaterial>();

        private PhysicalTexture normalTexture;
        private PhysicalTexture diffuseTexture;
        private PhysicalTexture specularTexture;
        private PhysicalTexture opacityTexture;

        private UnifiedShaderFactory shaderFactory;

        public event Action<UnifiedMaterialBuilder> InitializationComplete;

        public UnifiedMaterialBuilder(VirtualTextureManager virtualTextureManager, CompressedTextureSupport textureFormat, ResourceManager liveResourceManager)
        {
            switch(textureFormat)
            {
                case CompressedTextureSupport.DXT:
                    textureFormatExtension = ".dds";
                    break;
                case CompressedTextureSupport.None:
                    textureFormatExtension = ".png";
                    break;
            }

            this.virtualTextureManager = virtualTextureManager;
            shaderFactory = new UnifiedShaderFactory(liveResourceManager);

            diffuseTexture = virtualTextureManager.createPhysicalTexture("Diffuse", PixelFormatUsageHint.NotSpecial);
            normalTexture = virtualTextureManager.createPhysicalTexture("NormalMap", PixelFormatUsageHint.NormalMap);
            specularTexture = virtualTextureManager.createPhysicalTexture("Specular", PixelFormatUsageHint.NotSpecial);
            opacityTexture = virtualTextureManager.createPhysicalTexture("Opacity", PixelFormatUsageHint.OpacityMap);

            //Debug texture colors
            //normalTexture.color(Color.Blue);
            //diffuseTexture.color(Color.Red);
            //specularTexture.color(Color.Green);
            //opacityTexture.color(Color.HotPink);

            materialCreationFuncs.Add("NormalMapSpecular", createNormalMapSpecular);
            materialCreationFuncs.Add("NormalMapSpecularHighlight", createNormalMapSpecularHighlight);
            materialCreationFuncs.Add("NormalMapSpecularMap", createNormalMapSpecularMap);
            materialCreationFuncs.Add("NormalMapSpecularMapGlossMap", createNormalMapSpecularMapGlossMap);
            materialCreationFuncs.Add("NormalMapSpecularOpacityMap", createNormalMapSpecularOpacityMap);
            materialCreationFuncs.Add("NormalMapSpecularMapOpacityMap", createNormalMapSpecularMapOpacityMap);
            materialCreationFuncs.Add("NormalMapSpecularMapOpacityMapNoDepth", createNormalMapSpecularMapOpacityMapNoDepth);
            materialCreationFuncs.Add("EyeOuter", createEyeOuterMaterial);
            materialCreationFuncs.Add("ColoredNoTexture", createColoredNoTexture);

            OgreResourceGroupManager.getInstance().createResourceGroup(GroupName);
        }

        public void Dispose()
        {
            shaderFactory.Dispose();
        }

        public bool isCreator(Material material)
        {
            return createdMaterials.Contains(material);
        }

        public override void buildMaterial(MaterialDescription description, MaterialRepository repo)
        {
            constructMaterial(description, repo, description.IsAlpha);
            if (description.CreateAlphaMaterial && !description.IsAlpha)
            {
                constructMaterial(description, repo, true);
            }
        }

        public override void destroyMaterial(MaterialPtr materialPtr)
        {
            createdMaterials.Remove(materialPtr.Value);
            MaterialManager.getInstance().remove(materialPtr.Value.Name);
            materialPtr.Dispose();
        }

        public override void initializationComplete()
        {
            if(InitializationComplete != null)
            {
                InitializationComplete.Invoke(this);
            }
        }

        public override string Name
        {
            get
            {
                return "VirtualTexture";
            }
        }

        public int MaterialCount
        {
            get
            {
                return createdMaterials.Count;
            }
        }

        private void constructMaterial(MaterialDescription description, MaterialRepository repo, bool alpha)
        {
            String name = description.Name;
            if (description.CreateAlphaMaterial && alpha) //Is this an automatic alpha material?
            {
                name += "Alpha";
            }
            MaterialPtr material = MaterialManager.getInstance().create(name, GroupName, false, null);
            IndirectionTexture indirectionTex = null;
            CreateMaterial createMaterial;
            if(materialCreationFuncs.TryGetValue(description.ShaderName, out createMaterial))
            {
                indirectionTex = createMaterial(material.Value.getTechnique(0), description, alpha, true);
                if (alpha)
                {
                    //Create no depth check technique
                    Technique technique = material.Value.createTechnique();
                    technique.setLodIndex(1);
                    technique.createPass();
                    createMaterial(technique, description, alpha, false);
                }

                if (indirectionTex != null)
                {
                    String vertexShaderName = shaderFactory.createVertexProgram(indirectionTex.FeedbackBufferVPName, description.NumHardwareBones, description.NumHardwarePoses, false);
                    String fragmentShaderName = shaderFactory.createFragmentProgram(indirectionTex.FeedbackBufferFPName, false);
                    indirectionTex.setupFeedbackBufferTechnique(material.Value, vertexShaderName);
                }
                else
                {
                    //This is probably not the best way to hide things (should use visibility masks) but this allows us control per material instead of per entity
                    //and has mostly the same cost as rendering these in regular colors anyway without creating garbage data in the feedback buffer.
                    //In short its a good band-aid for now.
                    setupHiddenMaterialTechnique(material.Value, description);
                }

                material.Value.compile();
                material.Value.load();

                createdMaterials.Add(material.Value);
                repo.addMaterial(material, description);
            }
            else
            {
                Logging.Log.Error("Tried to create a material '{0}' with a shader '{1}' that does not exist. Please check your shader name.", description.Name, description.ShaderName);
            }
        }

        private void setupHiddenMaterialTechnique(Material material, MaterialDescription description)
        {
            var technique = material.createTechnique();
            technique.setName(FeedbackBuffer.Scheme);
            technique.setSchemeName(FeedbackBuffer.Scheme);

            var pass = technique.createPass();
            pass.setVertexProgram(shaderFactory.createVertexProgram("HiddenVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));
            pass.setFragmentProgram(shaderFactory.createFragmentProgram("HiddenFP", false));
        }

        //--------------------------------------
        //Specific material creation funcs
        //--------------------------------------
        private IndirectionTexture createNormalMapSpecularMapGlossMap(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("UnifiedVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("NormalMapSpecularMapGlossMapFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
                setGlossyness(description, gpuParams);
            }

            //Setup textures
            return setupNormalDiffuseSpecularTextures(description, pass);
        }

        private IndirectionTexture createNormalMapSpecularMap(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("UnifiedVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("NormalMapSpecularMapFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
            }

            //Setup textures
            return setupNormalDiffuseSpecularTextures(description, pass);
        }

        private IndirectionTexture createNormalMapSpecular(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("UnifiedVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("NormalMapSpecularFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
            }

            //Setup textures
            return setupNormalDiffuseTextures(description, pass);
        }

        private IndirectionTexture createNormalMapSpecularHighlight(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("UnifiedVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("NormalMapSpecularHighlightFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
            }

            //Setup textures
            return setupNormalDiffuseTextures(description, pass);
        }

        private IndirectionTexture createNormalMapSpecularOpacityMap(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Setup material specific depth values
            pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
            pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("UnifiedVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("NormalMapSpecularOpacityMapFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
            }

            //Setup textures
            return setupNormalDiffuseOpacityTextures(description, pass);
        }

        private IndirectionTexture createNormalMapSpecularMapOpacityMap(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Setup material specific depth values
            pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
            pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("UnifiedVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("NormalMapSpecularMapOpacityMapFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
            }

            //Setup textures
            return setupNormalDiffuseSpecularOpacityTextures(description, pass);
        }

        private IndirectionTexture createNormalMapSpecularMapOpacityMapNoDepth(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Setup material specific depth values
            pass.setDepthWriteEnabled(false);
            pass.setDepthCheckEnabled(true);
            pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
            pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("UnifiedVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("NormalMapSpecularMapOpacityMapFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
            }

            //Setup textures
            return setupNormalDiffuseSpecularOpacityTextures(description, pass);
        }

        private IndirectionTexture createEyeOuterMaterial(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Setup material specific depth values
            pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
            pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);
            pass.setDepthWriteEnabled(false);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("EyeOuterVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("EyeOuterFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
            }

            return null;
        }

        private IndirectionTexture createColoredNoTexture(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Material specific, setup shaders
            pass.setVertexProgram(shaderFactory.createVertexProgram("NoTexturesVP", description.NumHardwareBones, description.NumHardwarePoses, description.Parity));

            pass.setFragmentProgram(shaderFactory.createFragmentProgram("NoTexturesColoredFP", alpha));
            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams);
            }

            return null;
        }

        //--------------------------------------
        //Shared helpers
        //--------------------------------------
        private IndirectionTexture setupNormalDiffuseTextures(MaterialDescription description, Pass pass)
        {
            var texUnit = pass.createTextureUnitState(normalTexture.TextureName);
            pass.createTextureUnitState(diffuseTexture.TextureName);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMap + textureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, out indirectionTexture)) //Slow key
            {
                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                fileName = description.localizePath(description.DiffuseMap + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Diffuse", fileName, getTextureSize(fileName));
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private IndirectionTexture setupNormalDiffuseSpecularTextures(MaterialDescription description, Pass pass)
        {
            pass.createTextureUnitState(normalTexture.TextureName);
            pass.createTextureUnitState(diffuseTexture.TextureName);
            pass.createTextureUnitState(specularTexture.TextureName);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMap + textureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, out indirectionTexture)) //Slow key
            {
                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                fileName = description.localizePath(description.DiffuseMap + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Diffuse", fileName, getTextureSize(fileName));

                fileName = description.localizePath(description.SpecularMap + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Specular", fileName, getTextureSize(fileName));
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private IndirectionTexture setupNormalDiffuseSpecularOpacityTextures(MaterialDescription description, Pass pass)
        {
            pass.createTextureUnitState(normalTexture.TextureName);
            pass.createTextureUnitState(diffuseTexture.TextureName);
            pass.createTextureUnitState(specularTexture.TextureName);
            pass.createTextureUnitState(opacityTexture.TextureName);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMap + textureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, out indirectionTexture)) //Slow key
            {
                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                fileName = description.localizePath(description.DiffuseMap + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Diffuse", fileName, getTextureSize(fileName));

                fileName = description.localizePath(description.SpecularMap + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Specular", fileName, getTextureSize(fileName));

                fileName = description.localizePath(description.OpacityMap + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Opacity", fileName, getTextureSize(fileName));
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private IndirectionTexture setupNormalDiffuseOpacityTextures(MaterialDescription description, Pass pass)
        {
            pass.createTextureUnitState(normalTexture.TextureName);
            pass.createTextureUnitState(diffuseTexture.TextureName);
            pass.createTextureUnitState(opacityTexture.TextureName);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMap + textureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, out indirectionTexture)) //Slow key
            {
                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                fileName = description.localizePath(description.DiffuseMap + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Diffuse", fileName, getTextureSize(fileName));

                fileName = description.localizePath(description.OpacityMap + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Opacity", fileName, getTextureSize(fileName));
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private static IntSize2 getTextureSize(String texturePath)
        {
            try
            {
                using (Stream stream = VirtualFileSystem.Instance.openStream(texturePath, Engine.Resources.FileMode.Open)) //Probably not the best idea to directly access vfs here
                {
                    int width, height;
                    ImageUtility.GetImageInfo(stream, out width, out height);
                    return new IntSize2(width, height);
                }
            }
            catch(Exception ex)
            {

            }

            return new IntSize2(2048, 2048); //Hardcoded size for now.
        }

        private static void setupCommonPassAttributes(MaterialDescription description, bool alpha, Pass pass)
        {
            pass.setSpecular(description.SpecularColor);
            pass.setDiffuse(description.DiffuseColor);
            pass.setEmissive(description.EmissiveColor);
            pass.setShininess(description.Shinyness);

            if (alpha)
            {
                pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
                pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);
            }
        }

        private Pass createDepthPass(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            var pass = technique.getPass(0); //Make sure technique has one pass already defined
            if (alpha && depthCheck)
            {
                //Setup depth check pass
                pass.setColorWriteEnabled(false);
                pass.setDepthBias(-1.0f);
                pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);

                pass.setVertexProgram(shaderFactory.createVertexProgram("DepthCheckVP", description.NumHardwareBones, description.NumHardwarePoses, false));
                pass.setFragmentProgram(shaderFactory.createFragmentProgram("HiddenFP", false));

                pass = technique.createPass(); //Get another pass
            }
            return pass;
        }

        private static void setupIndirectionTexture(Pass pass, IndirectionTexture indirectionTexture)
        {
            var indirectionTextureUnit = pass.createTextureUnitState(indirectionTexture.TextureName);
            indirectionTextureUnit.setFilteringOptions(FilterOptions.Point, FilterOptions.Point, FilterOptions.None);
        }

        private static void setGlossyness(MaterialDescription description, GpuProgramParametersSharedPtr gpuParams)
        {
            gpuParams.Value.setNamedConstant("glossyStart", description.GlossyStart);
            gpuParams.Value.setNamedConstant("glossyRange", description.GlossyRange);
        }
    }
}