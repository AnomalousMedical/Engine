using Engine;
using Engine.Resources;
using Engine.Utility;
using OgrePlugin;
using OgrePlugin.VirtualTexture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static readonly String Version = typeof(UnifiedMaterialBuilder).GetTypeInfo().Assembly.GetName().Version.ToString();
#endif

        private const String GroupName = "UnifiedMaterialBuilder__Reserved";

        delegate IndirectionTexture CreateMaterial(Technique technique, MaterialDescription description, bool alpha, bool depthCheck);

        private Dictionary<Material, MaterialInfo> createdMaterials = new Dictionary<Material, MaterialInfo>(); //This is only for detection
        private List<MaterialPtr> builtInMaterials = new List<MaterialPtr>();
        private VirtualTextureManager virtualTextureManager;
        private String textureFormatExtension;
        private String normalTextureFormatExtension;
        private Dictionary<String, CreateMaterial> specialMaterialFuncs = new Dictionary<string, CreateMaterial>();
        private Dictionary<int, int> indirectionTextureUsageCounts = new Dictionary<int, int>();
        private bool disableHardwareSkinning = false;

        private PhysicalTexture normalTexture;
        private PhysicalTexture diffuseTexture;
        private PhysicalTexture specularTexture;
        private PhysicalTexture opacityTexture;

        private UnifiedShaderFactory shaderFactory;

        private bool createOpacityTexture = true;

        public event Action<UnifiedMaterialBuilder> InitializationComplete;

        public static int GetNumCompressedTexturesNeeded(CompressedTextureSupport textureFormat)
        {
            switch(textureFormat)
            {
                case CompressedTextureSupport.None:
                    return 3;
                default:
                    return 4;
            }
        }

        public static bool AreTexturesPagedOnDisk(CompressedTextureSupport textureFormat)
        {
            return textureFormat == CompressedTextureSupport.None;
        }

        public UnifiedMaterialBuilder(VirtualTextureManager virtualTextureManager, CompressedTextureSupport textureFormat, ResourceManager liveResourceManager)
        {
            PixelFormat otherFormat;
            PixelFormat normalFormat;
            NormaMapReadMode normalMapReadMode = NormaMapReadMode.RG;
            switch (textureFormat)
            {
                case CompressedTextureSupport.DXT_BC4_BC5:
                    textureFormatExtension = ".dds";
                    normalTextureFormatExtension = "_bc5.dds";
                    normalMapReadMode = NormaMapReadMode.RG;
                    createOpacityTexture = true;
                    otherFormat = PixelFormat.PF_DXT5;
                    normalFormat = PixelFormat.PF_BC5_UNORM;
                    break;
                case CompressedTextureSupport.DXT:
                    textureFormatExtension = ".dds";
                    normalTextureFormatExtension = ".dds";
                    normalMapReadMode = NormaMapReadMode.AG;
                    createOpacityTexture = true;
                    otherFormat = PixelFormat.PF_DXT5;
                    normalFormat = PixelFormat.PF_DXT5;
                    break;
                default:
                case CompressedTextureSupport.None:
                    textureFormatExtension = PagedImage.FileExtension;
                    normalTextureFormatExtension = PagedImage.FileExtension;
                    normalMapReadMode = NormaMapReadMode.RG;
                    createOpacityTexture = false;
                    otherFormat = PixelFormat.PF_A8R8G8B8;
                    normalFormat = PixelFormat.PF_A8R8G8B8;
                    break;
            }

            this.virtualTextureManager = virtualTextureManager;

            if (OgreInterface.Instance.RenderSystem.isShaderProfileSupported("hlsl"))
            {
                shaderFactory = new HlslUnifiedShaderFactory(liveResourceManager, normalMapReadMode, createOpacityTexture);
            }
            else if (OgreInterface.Instance.RenderSystem.isShaderProfileSupported("glsl"))
            {
                shaderFactory = new GlslUnifiedShaderFactory(liveResourceManager, normalMapReadMode, createOpacityTexture);
            }
            else if (OgreInterface.Instance.RenderSystem.isShaderProfileSupported("glsles"))
            {
                shaderFactory = new GlslesUnifiedShaderFactory(liveResourceManager, normalMapReadMode, createOpacityTexture);
            }
            else
            {
                throw new OgreException("Cannot create Unified Material Builder, device must support shader profiles hlsl, glsl, or glsles.");
            }

            diffuseTexture = virtualTextureManager.createPhysicalTexture("Diffuse", otherFormat);
            normalTexture = virtualTextureManager.createPhysicalTexture("NormalMap", normalFormat);
            specularTexture = virtualTextureManager.createPhysicalTexture("Specular", otherFormat);

            if (createOpacityTexture)
            {
                opacityTexture = virtualTextureManager.createPhysicalTexture("Opacity", otherFormat);
            }

            specialMaterialFuncs.Add("EyeOuter", createEyeOuterMaterial);
            specialMaterialFuncs.Add("ColorVertex", createColorVertexMaterial);

            OgreResourceGroupManager.getInstance().createResourceGroup(GroupName);

            MaterialPtr hiddenMaterial = MaterialManager.getInstance().create("HiddenMaterial", GroupName, false, null);
            setupHiddenMaterialPass(hiddenMaterial.Value.getTechnique(0).getPass(0), false, false);
            createdMaterials.Add(hiddenMaterial.Value, new MaterialInfo(hiddenMaterial.Value, null));
            builtInMaterials.Add(hiddenMaterial);

            //Delete stock resources
            MaterialManager.getInstance().remove("BaseWhite");
            MaterialManager.getInstance().remove("BaseWhiteNoLighting");
            MaterialManager.getInstance().remove("Ogre/Debug/AxesMat");

            //Rebuild with our materials
            var baseWhite = createFromDescription(new MaterialDescription()
            {
                Name = "BaseWhite"
            }, false);
            builtInMaterials.Add(baseWhite);

            var baseWhiteNoLighting = createFromDescription(new MaterialDescription()
            {
                Name = "BaseWhiteNoLighting"
            }, false);
            builtInMaterials.Add(baseWhiteNoLighting);

            MaterialPtr axesMat = MaterialManager.getInstance().create("Ogre/Debug/AxesMat", GroupName, false, null);
            axesMat.Value.setLightingEnabled(false);
            axesMat.Value.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
            axesMat.Value.setCullingMode(CullingMode.CULL_NONE);
            axesMat.Value.setDepthWriteEnabled(false);
            axesMat.Value.setDepthCheckEnabled(false);
            builtInMaterials.Add(axesMat);
            createdMaterials.Add(axesMat.Value, new MaterialInfo(axesMat.Value, null));
        }

        public void Dispose()
        {
            foreach (var material in builtInMaterials)
            {
                destroyMaterial(material);
            }
            shaderFactory.Dispose();
        }

        public bool isCreator(Material material)
        {
            return createdMaterials.ContainsKey(material);
        }

        public override void buildMaterial(MaterialDescription description, MaterialRepository repo)
        {
            if (disableHardwareSkinning)
            {
                description.NumHardwareBones = 0;
                description.NumHardwarePoses = 0;
            }

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

                PhysicalTexture physicalTexture;
                foreach (var technique in materialPtr.Value.Techniques)
                {
                    foreach (var pass in technique.Passes)
                    {
                        foreach (var textureUnitState in pass.TextureUnitStates)
                        {
                            if (virtualTextureManager.tryGetPhysicalTexture(textureUnitState.Name, out physicalTexture))
                            {
                                physicalTexture.removeTextureUnit(textureUnitState);
                            }
                        }
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

        /// <summary>
        /// This will return true if the material passed in uses virtual texturing.
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public bool isVirtualTextureMaterial(Material material)
        {
            return material.getTechnique(FeedbackBuffer.Scheme) != null;
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

        /// <summary>
        /// Disable any hardware skinning requests for constructed materials, will not modify existing materials
        /// this needs to be set before anything is loaded.
        /// </summary>
        public bool DisableHardwareSkinning
        {
            get
            {
                return disableHardwareSkinning;
            }
            set
            {
                disableHardwareSkinning = value;
            }
        }

        private void constructMaterial(MaterialDescription description, MaterialRepository repo, bool alpha)
        {
            var material = createFromDescription(description, alpha);
            repo.addMaterial(material, description);
        }

        private MaterialPtr createFromDescription(MaterialDescription description, bool alpha)
        {
            String name = description.Name;
            if (description.CreateAlphaMaterial && alpha) //Is this an automatic alpha material?
            {
                name += "Alpha";
            }
            MaterialPtr material = MaterialManager.getInstance().create(name, GroupName, false, null);

            CreateMaterial createMaterial = createUnifiedMaterial;
            if (description.IsSpecialMaterial && !specialMaterialFuncs.TryGetValue(description.SpecialMaterial, out createMaterial))
            {
                Logging.Log.Error("Could not find special material creation function {0} for material {1} in {2}.", description.SpecialMaterial, description.Name, description.SourceFile);
                createMaterial = createUnifiedMaterial; //Attempt to create something, out above clears this variable
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

            //If we have an indirection texture we need to seup the virtual texturing, if not no additional techniques will be created and the
            //entity must disable itself for feedback buffer rendering somehow (likely visibility mask).
            if (indirectionTex != null)
            {
                String vertexShaderName = shaderFactory.createFeedbackVertexProgram(indirectionTex.FeedbackBufferVPName, description.NumHardwareBones, description.NumHardwarePoses);
                String fragmentShaderName = shaderFactory.createFeedbackBufferFP(indirectionTex.FeedbackBufferFPName);
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

            material.Value.compile();
            material.Value.load();

            createdMaterials.Add(material.Value, new MaterialInfo(material.Value, indirectionTex));

            return material;
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
            pass.setFragmentProgram(shaderFactory.createHiddenFP("HiddenFP"));
        }

        //--------------------------------------
        //Specific material creation funcs
        //--------------------------------------
        private IndirectionTexture createUnifiedMaterial(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            //Create depth check pass if needed
            var pass = createDepthPass(technique, description, alpha, depthCheck);

            if (description.NoDepthWriteAlpha)
            {
                pass.setDepthWriteEnabled(false);
                pass.setDepthCheckEnabled(true);
                pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
                pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);
            }
            else
            {
                pass.setDepthWriteEnabled(description.EnableDepthWrite);
                pass.setDepthCheckEnabled(description.EnableDepthCheck);
                pass.setSceneBlending(description.SceneBlending);
                pass.setDepthFunction(description.DepthFunction);
            }

            //Setup this pass
            setupCommonPassAttributes(description, alpha, pass);

            //Setup shaders
            TextureMaps textureMaps;
            pass.setFragmentProgram(shaderFactory.createFragmentProgram(description, alpha, out textureMaps));
            pass.setVertexProgram(shaderFactory.createUnifiedVertexProgram(description, textureMaps));

            if((textureMaps & TextureMaps.Opacity) == TextureMaps.Opacity || description.HasOpacityValue)
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
                case TextureMaps.Normal | TextureMaps.Opacity:
                    indirectionTex = setupNormalOpacityTextures(description, pass);
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
                    gpuParams.Value.setNamedConstant("glossyStart", description.GlossyStart);
                    gpuParams.Value.setNamedConstant("glossyRange", description.GlossyRange);
                }

                if ((textureMaps & TextureMaps.Opacity) == 0 && description.HasOpacityValue)
                {
                    gpuParams.Value.setNamedConstant("opacity", description.OpacityValue);
                }
            }

            return indirectionTex;
        }

        //--------------------------------------
        //Specific material creation funcs
        //--------------------------------------
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
            pass.setVertexProgram(shaderFactory.createEyeOuterVertexProgram("EyeOuterVP"));

            pass.setFragmentProgram(shaderFactory.createEyeOuterFP("EyeOuterFP", alpha));

            return null;
        }

        private IndirectionTexture createColorVertexMaterial(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
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
            pass.setVertexProgram("colorvertex\\vs");
            if (alpha)
            {
                pass.setFragmentProgram("colorvertex\\fsAlpha");
            }
            else
            {
                pass.setFragmentProgram("colorvertex\\fs");
            }

            return null;
        }

        //--------------------------------------
        //Shared helpers
        //--------------------------------------
        private IndirectionTexture setupNormalTextures(MaterialDescription description, Pass pass)
        {
            normalTexture.createTextureUnit(pass);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMapName + normalTextureFormatExtension);
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
            normalTexture.createTextureUnit(pass);
            diffuseTexture.createTextureUnit(pass);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMapName + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if(description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                fileName = description.localizePath(description.DiffuseMapName + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Diffuse", fileName, getTextureSize(fileName));
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private IndirectionTexture setupNormalDiffuseSpecularTextures(MaterialDescription description, Pass pass)
        {
            normalTexture.createTextureUnit(pass);
            diffuseTexture.createTextureUnit(pass);
            specularTexture.createTextureUnit(pass);
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMapName + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if (description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                fileName = description.localizePath(description.DiffuseMapName + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Diffuse", fileName, getTextureSize(fileName));

                fileName = description.localizePath(description.SpecularMapName + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Specular", fileName, getTextureSize(fileName));
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private IndirectionTexture setupNormalDiffuseSpecularOpacityTextures(MaterialDescription description, Pass pass)
        {
            normalTexture.createTextureUnit(pass);
            diffuseTexture.createTextureUnit(pass);
            specularTexture.createTextureUnit(pass);
            if (createOpacityTexture)
            {
                opacityTexture.createTextureUnit(pass);
            }
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMapName + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if (description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                fileName = description.localizePath(description.DiffuseMapName + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Diffuse", fileName, getTextureSize(fileName));

                fileName = description.localizePath(description.SpecularMapName + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Specular", fileName, getTextureSize(fileName));

                if (createOpacityTexture)
                {
                    fileName = description.localizePath(description.OpacityMapName + textureFormatExtension);
                    indirectionTexture.addOriginalTexture("Opacity", fileName, getTextureSize(fileName));
                }
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private IndirectionTexture setupNormalDiffuseOpacityTextures(MaterialDescription description, Pass pass)
        {
            normalTexture.createTextureUnit(pass);
            diffuseTexture.createTextureUnit(pass);
            if (createOpacityTexture)
            {
                opacityTexture.createTextureUnit(pass);
            }
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMapName + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if (description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                fileName = description.localizePath(description.DiffuseMapName + textureFormatExtension);
                indirectionTexture.addOriginalTexture("Diffuse", fileName, getTextureSize(fileName));

                if (createOpacityTexture)
                {
                    fileName = description.localizePath(description.OpacityMapName + textureFormatExtension);
                    indirectionTexture.addOriginalTexture("Opacity", fileName, getTextureSize(fileName));
                }
            }
            setupIndirectionTexture(pass, indirectionTexture);
            return indirectionTexture;
        }

        private IndirectionTexture setupNormalOpacityTextures(MaterialDescription description, Pass pass)
        {
            normalTexture.createTextureUnit(pass);
            if (createOpacityTexture)
            {
                opacityTexture.createTextureUnit(pass);
            }
            IndirectionTexture indirectionTexture;
            String fileName = description.localizePath(description.NormalMapName + normalTextureFormatExtension);
            IntSize2 textureSize = getTextureSize(fileName);
            if (virtualTextureManager.createOrRetrieveIndirectionTexture(description.TextureSet, textureSize, description.KeepHighestMipLoaded, out indirectionTexture)) //Slow key
            {
                if (description.KeepHighestMipLoaded)
                {
                    indirectionTexture.KeepHighestMip = true;
                }

                indirectionTexture.addOriginalTexture("NormalMap", fileName, textureSize);

                if (createOpacityTexture)
                {
                    fileName = description.localizePath(description.OpacityMapName + textureFormatExtension);
                    indirectionTexture.addOriginalTexture("Opacity", fileName, getTextureSize(fileName));
                }
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
                    if (texturePath.EndsWith(PagedImage.FileExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        using (PagedImage pagedImage = new PagedImage())
                        {
                            pagedImage.loadInfoOnly(stream);
                            width = pagedImage.Width;
                            height = pagedImage.Height;
                        }
                    }
                    else
                    {
                        ImageUtility.GetImageInfo(stream, out width, out height);
                    }
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

            if(description.DisableBackfaceCulling)
            {
                pass.setCullingMode(CullingMode.CULL_NONE);
                pass.setManualCullingMode(ManualCullingMode.MANUAL_CULL_NONE);
            }

            if (alpha)
            {
                pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
                pass.setDepthFunction(CompareFunction.CMPF_LESS_EQUAL);
            }
        }

        private Pass createDepthPass(Technique technique, MaterialDescription description, bool alpha, bool depthCheck)
        {
            var pass = technique.getPass(0); //Make sure technique has one pass already defined
            if ((alpha || description.HasOpacityValue) && depthCheck)
            {
                //Setup depth check pass
                pass.setColorWriteEnabled(false);
                pass.setDepthBias(-1.0f);
                pass.setSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);

                pass.setVertexProgram(shaderFactory.createDepthCheckVertexProgram("DepthCheckVP", description.NumHardwareBones, description.NumHardwarePoses));
                pass.setFragmentProgram(shaderFactory.createHiddenFP("HiddenFP"));

                pass = technique.createPass(); //Get another pass
            }
            return pass;
        }

        private static void setupIndirectionTexture(Pass pass, IndirectionTexture indirectionTexture)
        {
            var indirectionTextureUnit = pass.createTextureUnitState(indirectionTexture.TextureName);
            indirectionTextureUnit.setFilteringOptions(FilterOptions.Point, FilterOptions.Point, FilterOptions.Point);
        }
    }
}
