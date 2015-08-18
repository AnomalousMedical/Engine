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

    [Flags]
    internal enum TextureMaps
    {
        None = 0,
        Normal = 1,
        Diffuse = 1 << 1,
        Specular = 1 << 2,
        Opacity = 1 << 3,
    }

    abstract class UnifiedShaderFactory : IDisposable
    {
        protected const String ShaderPathBase = "OgrePlugin.Resources";

        protected delegate HighLevelGpuProgramSharedPtr CreateFragmentProgramDelegate(String name, bool alpha);

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

            fragmentBuilderFuncs.Add("FeedbackBufferFP", createFeedbackBufferFP);
            fragmentBuilderFuncs.Add("HiddenFP", createHiddenFP);
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

        public String createUnifiedVertexProgram(MaterialDescription description, TextureMaps maps)
        {
            String baseName = "Textured";
            if(maps == TextureMaps.None)
            {
                baseName = "Untextured";
            }

            String shaderName = DetermineVertexShaderName(baseName.ToString(), description.NumHardwareBones, description.NumHardwarePoses, description.Parity);
            if (!createdPrograms.ContainsKey(shaderName))
            {
                var program = createUnifiedVertex(shaderName, maps, description);
                createdPrograms.Add(shaderName, program);
            }
            return shaderName;
        }

        public String createHiddenVertexProgram(String name)
        {
            if (!createdPrograms.ContainsKey(name))
            {
                var program = setupHiddenVP(name, 0, 0, false);
                createdPrograms.Add(name, program);
            }
            return name;
        }

        public String createDepthCheckVertexProgram(String baseName, int numHardwareBones, int numHardwarePoses)
        {
            String shaderName = DetermineVertexShaderName(baseName, numHardwareBones, numHardwarePoses, false);
            if (!createdPrograms.ContainsKey(shaderName))
            {
                var program = setupDepthCheckVP(shaderName, numHardwareBones, numHardwarePoses, false);
                createdPrograms.Add(shaderName, program);
            }
            return shaderName;
        }

        public String createFeedbackVertexProgram(String baseName, int numHardwareBones, int numHardwarePoses)
        {
            String shaderName = DetermineVertexShaderName(baseName, numHardwareBones, numHardwarePoses, false);
            if (!createdPrograms.ContainsKey(shaderName))
            {
                var program = setupFeedbackBufferVP(shaderName, numHardwareBones, numHardwarePoses, false);
                createdPrograms.Add(shaderName, program);
            }
            return shaderName;
        }

        public String createEyeOuterVertexProgram(String name)
        {
            if (!createdPrograms.ContainsKey(name))
            {
                var program = setupEyeOuterVP(name, 0, 0, false);
                createdPrograms.Add(name, program);
            }
            return name;
        }

        protected virtual HighLevelGpuProgramSharedPtr createUnifiedVertex(String name, TextureMaps maps, MaterialDescription description)
        {
            throw new NotImplementedException();
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

        public String createFragmentProgram(MaterialDescription description, bool alpha, out TextureMaps textureMaps)
        {
            StringBuilder name = new StringBuilder();
            textureMaps = TextureMaps.None;

            if (String.IsNullOrWhiteSpace(description.NormalMap))
            {
                name.Append("Normal");
            }
            else
            {
                textureMaps |= TextureMaps.Normal;
                name.Append("NormalMap");
            }

            if (String.IsNullOrWhiteSpace(description.DiffuseMap))
            {
                name.Append("Diffuse");
            }
            else
            {
                textureMaps |= TextureMaps.Diffuse;
                name.Append("DiffuseMap");
            }

            if (String.IsNullOrWhiteSpace(description.SpecularMap))
            {
                name.Append("Specular");
            }
            else
            {
                textureMaps |= TextureMaps.Specular;
                name.Append("SpecularMap");
            }

            if(description.HasGlossMap)
            {
                name.Append("GlossMap");
            }

            if (!String.IsNullOrWhiteSpace(description.OpacityMap))
            {
                textureMaps |= TextureMaps.Opacity;
                name.Append("OpacityMap");
            }

            if(description.IsHighlight)
            {
                name.Append("Highlight");
            }

            String shaderName = DetermineFragmentShaderName(name.ToString(), alpha);
            if (!createdPrograms.ContainsKey(shaderName))
            {
                var program = createUnifiedFrag(shaderName, textureMaps, alpha, description.HasGlossMap, description.IsHighlight);
                createdPrograms.Add(shaderName, program);
            }
            return shaderName;
        }

        protected abstract HighLevelGpuProgramSharedPtr createUnifiedFrag(String name, TextureMaps maps, bool alpha, bool glossMap, bool highlight);

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

        protected String DetermineVertexPreprocessorDefines2(TextureMaps maps, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            StringBuilder definesBuilder = new StringBuilder();
            if (parity)
            {
                definesBuilder.Append("PARITY=1,");
            }
            if (numHardwareBones > 0)
            {
                definesBuilder.AppendFormat("BONES_PER_VERTEX={0},", numHardwareBones);
            }
            if (numHardwarePoses > 0)
            {
                definesBuilder.AppendFormat("POSE_COUNT={0},", numHardwarePoses);
            }
            if(maps == TextureMaps.None)
            {
                definesBuilder.Append("NO_MAPS=1,");
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

        protected String DetermineFragmentPreprocessorDefines2(TextureMaps maps, bool alpha, bool glossMap, bool highlight)
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
            if(maps == TextureMaps.None)
            {
                definesBuilder.Append("NO_MAPS=1,");
            }
            else
            {
                StringBuilder mapDefineBuilder = new StringBuilder();
                if((maps & TextureMaps.Normal) == TextureMaps.Normal)
                {
                    mapDefineBuilder.Append("NORMAL_");
                    definesBuilder.Append("NORMAL_MAP=1,");
                }
                if ((maps & TextureMaps.Diffuse) == TextureMaps.Diffuse)
                {
                    mapDefineBuilder.Append("DIFFUSE_");
                    definesBuilder.Append("DIFFUSE_MAP=1,");

                    if (glossMap)
                    {
                        definesBuilder.Append("GLOSS_MAP=1,");
                    }
                }
                if ((maps & TextureMaps.Specular) == TextureMaps.Specular)
                {
                    mapDefineBuilder.Append("SPECULAR_");
                    definesBuilder.Append("SPECULAR_MAP=1,");
                }
                if ((maps & TextureMaps.Opacity) == TextureMaps.Opacity)
                {
                    mapDefineBuilder.Append("OPACITY_");
                    definesBuilder.Append("OPACITY_MAP=1,");

                    if (glossMap)
                    {
                        definesBuilder.Append("GLOSS_MAP=1,GLOSS_CHANNEL_OPACITY_GREEN=1,");
                    }
                }
                mapDefineBuilder.Append("MAPS=1");
                definesBuilder.Append(mapDefineBuilder.ToString());
            }
            return definesBuilder.ToString();
        }
    }
}
