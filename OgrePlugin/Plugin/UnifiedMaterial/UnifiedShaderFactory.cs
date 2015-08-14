using Engine.Resources;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    internal enum NormaMapReadlMode
    {
        /// <summary>
        /// Normal map x, y in RG channels, (bc5 format)
        /// </summary>
        RG,
        /// <summary>
        /// Normal map x, y in AG channels, (dxt5_nm)
        /// </summary>
        AG
    }

    abstract class UnifiedShaderFactory : IDisposable
    {
        protected const String ShaderPathBase = "OgrePlugin.Resources";

        protected delegate HighLevelGpuProgramSharedPtr CreateVertexProgramDelegate(String name, int numHardwareBones, int numHardwarePoses, bool parity);
        protected delegate HighLevelGpuProgramSharedPtr CreateFragmentProgramDelegate(String name, bool alpha);

        private Dictionary<String, CreateVertexProgramDelegate> vertexBuilderFuncs = new Dictionary<string, CreateVertexProgramDelegate>();
        private Dictionary<String, CreateFragmentProgramDelegate> fragmentBuilderFuncs = new Dictionary<string, CreateFragmentProgramDelegate>();
        private Dictionary<String, HighLevelGpuProgramSharedPtr> createdPrograms = new Dictionary<string, HighLevelGpuProgramSharedPtr>();
        private ResourceGroup shaderResourceGroup;
        private NormaMapReadlMode normalMapReadMode;

        public UnifiedShaderFactory(ResourceManager liveResourceManager, NormaMapReadlMode normalMapReadMode)
        {
            this.normalMapReadMode = normalMapReadMode;

            var rendererResources = liveResourceManager.getSubsystemResource("Ogre");
            shaderResourceGroup = rendererResources.addResourceGroup("UnifiedShaderFactory");
            shaderResourceGroup.addResource(GetType().AssemblyQualifiedName, "EmbeddedResource", true);
            
            liveResourceManager.initializeResources();

            vertexBuilderFuncs.Add("UnifiedVP", setupUnifiedVP);
            vertexBuilderFuncs.Add("DepthCheckVP", setupDepthCheckVP);
            vertexBuilderFuncs.Add("NoTexturesVP", setupNoTexturesVP);
            vertexBuilderFuncs.Add("FeedbackBufferVP", setupFeedbackBufferVP);
            vertexBuilderFuncs.Add("HiddenVP", setupHiddenVP);
            vertexBuilderFuncs.Add("EyeOuterVP", setupEyeOuterVP);

            fragmentBuilderFuncs.Add("FeedbackBufferFP", createFeedbackBufferFP);
            fragmentBuilderFuncs.Add("NormalMapSpecularFP", createNormalMapSpecularFP);
            fragmentBuilderFuncs.Add("NormalMapSpecularHighlightFP", createNormalMapSpecularHighlightFP);
            fragmentBuilderFuncs.Add("NormalMapSpecularMapFP", createNormalMapSpecularMapFP);
            fragmentBuilderFuncs.Add("NormalMapSpecularOpacityMapFP", createNormalMapSpecularOpacityMapFP);
            fragmentBuilderFuncs.Add("NormalMapSpecularMapGlossMapFP", createNormalMapSpecularMapGlossMapFP);
            fragmentBuilderFuncs.Add("NormalMapSpecularMapOpacityMapFP", createNormalMapSpecularMapOpacityMapFP);
            fragmentBuilderFuncs.Add("NormalMapSpecularMapOpacityMapGlossMapFP", createNormalMapSpecularMapOpacityMapGlossMapFP);
            fragmentBuilderFuncs.Add("HiddenFP", createHiddenFP);
            fragmentBuilderFuncs.Add("NoTexturesColoredFP", createNoTexturesColoredFP);
            fragmentBuilderFuncs.Add("EyeOuterFP", createEyeOuterFP);
        }

        public virtual void Dispose()
        {
            foreach(var program in createdPrograms.Values)
            {
                program.Dispose();
            }
        }

        #region Vertex Programs

        public String createVertexProgram(String baseName, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            String shaderName = DetermineVertexShaderName(baseName, numHardwareBones, numHardwarePoses, parity);
            if(!createdPrograms.ContainsKey(shaderName))
            {
                CreateVertexProgramDelegate buildFunc;
                if(vertexBuilderFuncs.TryGetValue(baseName, out buildFunc))
                {
                    var program = buildFunc(shaderName, numHardwareBones, numHardwarePoses, parity);
                    createdPrograms.Add(shaderName, program);
                }
                else
                {
                    Logging.Log.Error("Cannot build vertex shader '{0}' no setup function defined.", shaderName);
                }
            }
            return shaderName;
        }

        protected abstract HighLevelGpuProgramSharedPtr setupUnifiedVP(String name, int numHardwareBones, int numHardwarePoses, bool parity);

        protected abstract HighLevelGpuProgramSharedPtr setupNoTexturesVP(String name, int numHardwareBones, int numHardwarePoses, bool parity);

        protected abstract HighLevelGpuProgramSharedPtr setupDepthCheckVP(String name, int numHardwareBones, int numHardwarePoses, bool parity);

        protected abstract HighLevelGpuProgramSharedPtr setupHiddenVP(String name, int numHardwareBones, int numHardwarePoses, bool parity);

        protected abstract HighLevelGpuProgramSharedPtr setupFeedbackBufferVP(String name, int numHardwareBones, int numHardwarePoses, bool parity);

        protected abstract HighLevelGpuProgramSharedPtr setupEyeOuterVP(String name, int numHardwareBones, int numHardwarePoses, bool parity);

        #endregion Vertex Programs

        #region Fragment Programs

        public String createFragmentProgram(String baseName, bool alpha)
        {
            String shaderName = DetermineFragmentShaderName(baseName, alpha);
            if (!createdPrograms.ContainsKey(shaderName))
            {
                CreateFragmentProgramDelegate buildFunc;
                if (fragmentBuilderFuncs.TryGetValue(baseName, out buildFunc))
                {
                    var program = buildFunc(shaderName, alpha);
                    createdPrograms.Add(shaderName, program);
                }
                else
                {
                    Logging.Log.Error("Cannot build fragment shader '{0}' no setup function defined.", shaderName);
                }
            }
            return shaderName;
        }

        protected abstract HighLevelGpuProgramSharedPtr createNormalMapSpecularFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createNormalMapSpecularHighlightFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createNormalMapSpecularMapFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createNormalMapSpecularOpacityMapFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createNormalMapSpecularMapGlossMapFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createNormalMapSpecularMapOpacityMapFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createNormalMapSpecularMapOpacityMapGlossMapFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createNoTexturesColoredFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createFeedbackBufferFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createHiddenFP(String name, bool alpha);

        protected abstract HighLevelGpuProgramSharedPtr createEyeOuterFP(String name, bool alpha);

        #endregion Fragment Programs

        public String ResourceGroupName
        {
            get
            {
                return shaderResourceGroup.FullName;
            }
        }

        private static String DetermineVertexShaderName(String baseName, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            StringBuilder programName = new StringBuilder(baseName);
            if (numHardwareBones > 0)
            {
                programName.AppendFormat("HardwareSkin{0}BonePerVertex", numHardwareBones);
            }
            if (numHardwarePoses > 0)
            {
                programName.AppendFormat("{0}Pose", numHardwarePoses);
            }
            if (parity)
            {
                programName.AppendFormat("Parity");
            }
            return programName.ToString();
        }

        private static String DetermineFragmentShaderName(String baseName, bool alpha)
        {
            if (alpha)
            {
                baseName += "Alpha";
            }
            return baseName;
        }

        protected String DetermineVertexPreprocessorDefines(int numHardwareBones, int numHardwarePoses, bool parity)
        {
            StringBuilder definesBuilder = new StringBuilder();
            if (parity)
            {
                definesBuilder.AppendFormat("PARITY=1,");
            }
            if(numHardwareBones > 0)
            {
                definesBuilder.AppendFormat("BONES_PER_VERTEX={0},", numHardwareBones);
            }
            if(numHardwarePoses > 0)
            {
                definesBuilder.AppendFormat("POSE_COUNT={0},", numHardwarePoses);
            }
            return definesBuilder.ToString();
        }

        protected String DetermineFragmentPreprocessorDefines(bool alpha)
        {
            StringBuilder definesBuilder = new StringBuilder("VIRTUAL_TEXTURE=1,");
            if(alpha)
            {
                definesBuilder.Append("ALPHA=1,");
            }
            switch(normalMapReadMode)
            {
                case NormaMapReadlMode.RG:
                    definesBuilder.Append("RG_NORMALS=1,");
                    break;
            }
            return definesBuilder.ToString();
        }

        protected String DetermineFragmentPreprocessorDefines2(bool alpha, bool normalMap, bool diffuseMap, bool specularMap, bool glossMap, bool highlight)
        {
            StringBuilder definesBuilder = new StringBuilder("VIRTUAL_TEXTURE=1,");
            if (alpha)
            {
                definesBuilder.Append("ALPHA=1,");
            }
            if(highlight)
            {
                definesBuilder.Append("HIGHLIGHT=1,");
            }
            switch (normalMapReadMode)
            {
                case NormaMapReadlMode.RG:
                    definesBuilder.Append("RG_NORMALS=1,");
                    break;
            }
            if(normalMap || diffuseMap || specularMap || glossMap)
            {
                definesBuilder.Append("HAS_TEXTURES=1,");
                if(normalMap)
                {
                    definesBuilder.Append("NORMAL_MAP=1,");
                }
                if(diffuseMap)
                {
                    definesBuilder.Append("DIFFUSE_MAP=1,");
                }
                if (specularMap)
                {
                    definesBuilder.Append("SPECULAR_MAP=1,");
                }
                if (glossMap)
                {
                    definesBuilder.Append("GLOSS_MAP=1,");
                }
                if(normalMap && diffuseMap && !specularMap && !glossMap)
                {
                    definesBuilder.Append("NORMAL_DIFFUSE_MAPS=1,"); 
                }
                if(normalMap && diffuseMap && specularMap && !glossMap)
                {
                    definesBuilder.Append("NORMAL_DIFFUSE_SPECULAR_MAPS=1,"); 
                }
            }
            return definesBuilder.ToString();
        }
    }
}
