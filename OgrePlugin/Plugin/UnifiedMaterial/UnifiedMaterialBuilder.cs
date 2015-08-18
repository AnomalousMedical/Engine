using Engine;
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
        class MaterialInfo
        {
            public MaterialInfo(Material material, IndirectionTexture indirectionTexture)
            {
                this.Material = material;
                this.IndirectionTexture = indirectionTexture;
            }

            public Material Material { get; private set; }

            public IndirectionTexture IndirectionTexture { get; private set; }
        }

        /// <summary>
        /// A version string to use to determine if the cache should be rebuilt, 
        /// if your saved version does not match this, rebuild the microcode cache.
        /// </summary>
#if NEVER_CACHE_SHADERS
        public static readonly String Version = Guid.NewGuid().ToString();
#else
        public static readonly String Version = "6.0";
#endif

        private const String GroupName = "UnifiedMaterialBuilder__Reserved";

        delegate IndirectionTexture CreateMaterial(Technique technique, MaterialDescription description, bool alpha, bool depthCheck);

        private Dictionary<Material, MaterialInfo> createdMaterials = new Dictionary<Material, MaterialInfo>(); //This is only for detection
        private VirtualTextureManager virtualTextureManager;
        private String textureFormatExtension;
        private String normalTextureFormatExtension;
        private Dictionary<String, CreateMaterial> materialCreationFuncs = new Dictionary<string, CreateMaterial>();
        private Dictionary<int, int> indirectionTextureUsageCounts = new Dictionary<int, int>();

        private PhysicalTexture normalTexture;
        private PhysicalTexture diffuseTexture;
        private PhysicalTexture specularTexture;
        private PhysicalTexture opacityTexture;

        private UnifiedShaderFactory shaderFactory;

        public event Action<UnifiedMaterialBuilder> InitializationComplete;

        class BuiltInMaterialRepo : MaterialRepository
        {
            private List<MaterialPtr> builtInMaterials = new List<MaterialPtr>();

            public void addMaterial(MaterialPtr material, MaterialDescription description)
            {
                builtInMaterials.Add(material);
            }

            public void addMaterial(MaterialPtr material)
            {
                builtInMaterials.Add(material);
            }

            public void Dispose(UnifiedMaterialBuilder materialBuilder)
            {
                foreach(var material in builtInMaterials)
                {
                    materialBuilder.destroyMaterial(material);
                }
            }
        }
        private BuiltInMaterialRepo builtInMaterialRepo = new BuiltInMaterialRepo();

        public UnifiedMaterialBuilder(VirtualTextureManager virtualTextureManager, CompressedTextureSupport textureFormat, ResourceManager liveResourceManager)
        {
            NormaMapReadlMode normalMapReadMode = NormaMapReadlMode.RG;
            switch (textureFormat)
            {
                case CompressedTextureSupport.DXT_BC4_BC5:
                    textureFormatExtension = ".dds";
                    normalTextureFormatExtension = "_bc5.dds";
                    normalMapReadMode = NormaMapReadlMode.RG;
                    break;
                case CompressedTextureSupport.DXT:
                    textureFormatExtension = ".dds";
                    normalTextureFormatExtension = ".dds";
                    normalMapReadMode = NormaMapReadlMode.AG;
                    break;
                case CompressedTextureSupport.None:
                    textureFormatExtension = ".png";
                    normalTextureFormatExtension = ".png";
                    normalMapReadMode = NormaMapReadlMode.AG;
                    break;
            }

            this.virtualTextureManager = virtualTextureManager;

            if (OgreInterface.Instance.RenderSystem.isShaderProfileSupported("hlsl"))
            {
                shaderFactory = new HlslUnifiedShaderFactory(liveResourceManager, normalMapReadMode);
            }
            else if (OgreInterface.Instance.RenderSystem.isShaderProfileSupported("glsl"))
            {
                shaderFactory = new GlslUnifiedShaderFactory(liveResourceManager, normalMapReadMode);
            }
            else if (OgreInterface.Instance.RenderSystem.isShaderProfileSupported("glsles"))
            {
                shaderFactory = new GlslesUnifiedShaderFactory(liveResourceManager, normalMapReadMode);
            }
            else
            {
                throw new OgreException("Cannot create Unified Material Builder, device must support shader profiles hlsl, glsl, or glsles.");
            }

            diffuseTexture = virtualTextureManager.createPhysicalTexture("Diffuse", PixelFormatUsageHint.NotSpecial);
            normalTexture = virtualTextureManager.createPhysicalTexture("NormalMap", PixelFormatUsageHint.NormalMap);
            specularTexture = virtualTextureManager.createPhysicalTexture("Specular", PixelFormatUsageHint.NotSpecial);
            opacityTexture = virtualTextureManager.createPhysicalTexture("Opacity", PixelFormatUsageHint.OpacityMap);

            materialCreationFuncs.Add("NormalMapSpecularMapGlossMap", createNormalMapSpecularMapGlossMap);
            materialCreationFuncs.Add("EyeOuter", createEyeOuterMaterial);

            OgreResourceGroupManager.getInstance().createResourceGroup(GroupName);

            MaterialPtr hiddenMaterial = MaterialManager.getInstance().create("HiddenMaterial", GroupName, false, null);
            setupHiddenMaterialPass(hiddenMaterial.Value.getTechnique(0).getPass(0), false, false);
            builtInMaterialRepo.addMaterial(hiddenMaterial);
        }

        public void Dispose()
        {
            builtInMaterialRepo.Dispose(this);
            shaderFactory.Dispose();
        }

        public bool isCreator(Material material)
        {
            return createdMaterials.ContainsKey(material);
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
            MaterialInfo info;
            if (createdMaterials.TryGetValue(materialPtr.Value, out info))
            {
                int count = 0;
                if (info.IndirectionTexture != null && indirectionTextureUsageCounts.TryGetValue(info.IndirectionTexture.Id, out count))
                {
                    --count;
                    if (count == 0)
                    {
                        virtualTextureManager.destroyIndirectionTexture(info.IndirectionTexture);
                        indirectionTextureUsageCounts.Remove(info.IndirectionTexture.Id);
                    }
                    else
                    {
                        indirectionTextureUsageCounts[info.IndirectionTexture.Id] = count;
                    }
                }

                createdMaterials.Remove(materialPtr.Value);
                MaterialManager.getInstance().remove(materialPtr.Value.Name);
                materialPtr.Dispose();
            }
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
            
            CreateMaterial createMaterial;
            if (String.IsNullOrEmpty(description.ShaderName) || !materialCreationFuncs.TryGetValue(description.ShaderName, out createMaterial))
            {
                createMaterial = createUnifiedMaterial;
            }

            IndirectionTexture indirectionTex = createMaterial(material.Value.getTechnique(0), description, alpha, true);

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

                int count = 0;
                if (!indirectionTextureUsageCounts.TryGetValue(indirectionTex.Id, out count))
                {
                    indirectionTextureUsageCounts.Add(indirectionTex.Id, 0);
                }
                else
                {
                    indirectionTextureUsageCounts[indirectionTex.Id] = count + 1;
                }
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

            createdMaterials.Add(material.Value, new MaterialInfo(material.Value, indirectionTex));
            repo.addMaterial(material, description);
        }

        private void setupHiddenMaterialTechnique(Material material, MaterialDescription description)
        {
            var technique = material.createTechnique();
            technique.setName(FeedbackBuffer.Scheme);
            technique.setSchemeName(FeedbackBuffer.Scheme);

            setupHiddenMaterialPass(technique.createPass(), description.NumHardwareBones > 0, description.NumHardwarePoses > 0);
        }

        private void setupHiddenMaterialPass(Pass pass, bool hasHardwareBones, bool hasHardwarePose)
        {
            pass.setDepthWriteEnabled(false);
            pass.setColorWriteEnabled(false);
            pass.setDepthCheckEnabled(true);
            pass.setDepthFunction(CompareFunction.CMPF_ALWAYS_FAIL);
            pass.setVertexProgram(shaderFactory.createHiddenVertexProgram("HiddenVP"));
            pass.setFragmentProgram(shaderFactory.createFragmentProgram("HiddenFP", false));
        }

        //--------------------------------------
        //Specific material creation funcs
        //--------------------------------------
        private IndirectionTexture createUnifiedMaterial(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Setup shaders
            TextureMaps textureMaps;
            pass.setFragmentProgram(shaderFactory.createFragmentProgram(description, alpha, out textureMaps));
            pass.setVertexProgram(shaderFactory.createVertexProgram(description, textureMaps));

            if (description.NoDepthWriteAlpha)
            {
                pass.setDepthWriteEnabled(false);
                pass.setDepthCheckEnabled(true);
                pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
                pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);
            }

            if((textureMaps & TextureMaps.Opacity) == TextureMaps.Opacity)
            {
                pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
                pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);
            }

            //Setup Textures
            IndirectionTexture indirectionTex = null;
            switch(textureMaps)
            {
                case TextureMaps.Normal:
                    indirectionTex = setupNormalTextures(description, pass);
                    break;
                case TextureMaps.Normal | TextureMaps.Diffuse:
                    indirectionTex = setupNormalDiffuseTextures(description, pass);
                    break;
                case TextureMaps.Normal | TextureMaps.Diffuse | TextureMaps.Specular:
                    indirectionTex = setupNormalDiffuseSpecularTextures(description, pass);
                    break;
                case TextureMaps.Normal | TextureMaps.Diffuse | TextureMaps.Opacity:
                    indirectionTex = setupNormalDiffuseOpacityTextures(description, pass);
                    break;
                case TextureMaps.Normal | TextureMaps.Diffuse | TextureMaps.Opacity | TextureMaps.Specular:
                    indirectionTex = setupNormalDiffuseSpecularOpacityTextures(description, pass);
                    break;
            }

            using (var gpuParams = pass.getFragmentProgramParameters())
            {
                if (indirectionTex != null)
                {
                    virtualTextureManager.setupVirtualTextureFragmentParams(gpuParams, indirectionTex);
                }

                if (description.HasGlossMap)
                {
                    setGlossyness(description, gpuParams);
                }
            }

            return indirectionTex;
        }

        //--------------------------------------
        //Specific material creation funcs
        //--------------------------------------

        private IndirectionTexture createNormalMapSpecularMapGlossMap(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Hack, Ensure things are true
            description.HasGlossMap = true;

            return createUnifiedMaterial(technique, description, alpha, depthCheck);
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

            return null;
        }

        //--------------------------------------
        //Shared helpers
        //--------------------------------------
        private IndirectionTexture setupNormalTextures(MaterialDescription description, Pass pass)
        {
            var texUnit = pass.createTextureUnitState(normalTexture.TextureName);
            pass.createTextureUnitState(diffuseTexture.TextureName);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMap + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if (description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private IndirectionTexture setupNormalDiffuseTextures(MaterialDescription description, Pass pass)
        {
            var texUnit = pass.createTextureUnitState(normalTexture.TextureName);
            pass.createTextureUnitState(diffuseTexture.TextureName);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMap + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if(description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

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
            String fileName = description.localizePath(description.NormalMap + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if (description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

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
            String fileName = description.localizePath(description.NormalMap + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if (description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

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
            String fileName = description.localizePath(description.NormalMap + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if (description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

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
