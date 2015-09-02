using Engine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class GlslUnifiedShaderFactory : UnifiedShaderFactory
    {
        private const String UnifiedShaderBase = ShaderPathBase + ".Unified.glsl.";
        private const String EyeShaderBase = ShaderPathBase + ".Eye.glsl.";

        private List<HighLevelGpuProgramSharedPtr> localPrograms = new List<HighLevelGpuProgramSharedPtr>();

        public GlslUnifiedShaderFactory(ResourceManager liveResourceManager, NormaMapReadMode normalMapReadMode, bool separateOpacityMap)
            : base(liveResourceManager, normalMapReadMode, separateOpacityMap)
        {
            var packProgram = HighLevelGpuProgramManager.Instance.createProgram("Pack_VP_GLSL", ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);
            packProgram.Value.SourceFile = UnifiedShaderBase + "Pack.glsl";
            packProgram.Value.load();
            localPrograms.Add(packProgram);

            var lightingProgram = HighLevelGpuProgramManager.Instance.createProgram("Lighting_FP_GLSL", ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);
            lightingProgram.Value.SourceFile = UnifiedShaderBase + "Lighting.glsl";
            lightingProgram.Value.load();
            localPrograms.Add(lightingProgram);

            var unpackProgram = HighLevelGpuProgramManager.Instance.createProgram("Unpack_FP_GLSL", ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);
            unpackProgram.Value.SourceFile = UnifiedShaderBase + "Unpack.glsl";
            unpackProgram.Value.load();
            localPrograms.Add(unpackProgram);

            var texMipLevelProgram = HighLevelGpuProgramManager.Instance.createProgram("VirtualTextureFuncs_GLSL", ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);
            texMipLevelProgram.Value.SourceFile = UnifiedShaderBase + "VirtualTextureFuncs.glsl";
            texMipLevelProgram.Value.load();
            localPrograms.Add(texMipLevelProgram);
        }

        public override void Dispose()
        {
            foreach (var program in localPrograms)
            {
                program.Dispose();
            }
            base.Dispose();
        }

        #region Vertex Programs

        protected override HighLevelGpuProgramSharedPtr setupUnifiedVertex(String name, TextureMaps maps, MaterialDescription description)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "MainVP.glsl";

            program.Value.setParam("attach", "Pack_VP_GLSL");
            program.Value.SkeletalAnimationIncluded = description.NumHardwareBones > 0;
            program.Value.NumberOfPoses = (ushort)description.NumHardwarePoses;
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(maps, description.NumHardwareBones, description.NumHardwarePoses, description.Parity));
            program.Value.load();

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                if (description.NumHardwareBones > 0 || description.NumHardwarePoses > 0)
                {
                    defaultParams.Value.setNamedAutoConstant("eyePosition", AutoConstantType.ACT_CAMERA_POSITION);
                    defaultParams.Value.setNamedAutoConstant("lightAttenuation", AutoConstantType.ACT_LIGHT_ATTENUATION, 0);
                    defaultParams.Value.setNamedAutoConstant("lightPosition", AutoConstantType.ACT_LIGHT_POSITION, 0);

                    defaultParams.Value.setNamedAutoConstant("worldMatrix3x4Array", AutoConstantType.ACT_WORLD_MATRIX_ARRAY_3x4);
                    defaultParams.Value.setNamedAutoConstant("cameraMatrix", AutoConstantType.ACT_VIEWPROJ_MATRIX);

                    if (description.NumHardwarePoses > 0)
                    {
                        defaultParams.Value.setNamedAutoConstant("poseAnimAmount", AutoConstantType.ACT_ANIMATION_PARAMETRIC);
                    }
                }
                else
                {
                    defaultParams.Value.setNamedAutoConstant("cameraMatrix", AutoConstantType.ACT_WORLDVIEWPROJ_MATRIX);
                    defaultParams.Value.setNamedAutoConstant("eyePosition", AutoConstantType.ACT_CAMERA_POSITION_OBJECT_SPACE);
                    defaultParams.Value.setNamedAutoConstant("lightAttenuation", AutoConstantType.ACT_LIGHT_ATTENUATION, 0);
                    defaultParams.Value.setNamedAutoConstant("lightPosition", AutoConstantType.ACT_LIGHT_POSITION_OBJECT_SPACE, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupDepthCheckVP(String name, int numHardwareBones, int numHardwarePoses)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            if (numHardwareBones > 0 || numHardwarePoses > 0)
            {
                program.Value.SourceFile = UnifiedShaderBase + "DepthCheckVPHardwareSkin.glsl";
            }
            else
            {
                program.Value.SourceFile = UnifiedShaderBase + "DepthCheckVP.glsl";
            }

            program.Value.SkeletalAnimationIncluded = numHardwareBones > 0;
            program.Value.NumberOfPoses = (ushort)numHardwarePoses;
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(TextureMaps.None, numHardwareBones, numHardwarePoses, false));
            program.Value.load();

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                if (numHardwareBones > 0 || numHardwarePoses > 0)
                {
                    defaultParams.Value.setNamedAutoConstant("worldMatrix3x4Array", AutoConstantType.ACT_WORLD_MATRIX_ARRAY_3x4);
                    defaultParams.Value.setNamedAutoConstant("viewProjectionMatrix", AutoConstantType.ACT_VIEWPROJ_MATRIX);

                    if (numHardwarePoses > 0)
                    {
                        defaultParams.Value.setNamedAutoConstant("poseAnimAmount", AutoConstantType.ACT_ANIMATION_PARAMETRIC);
                    }
                }
                else
                {
                    defaultParams.Value.setNamedAutoConstant("worldViewProj", AutoConstantType.ACT_WORLDVIEWPROJ_MATRIX);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupHiddenVP(String name)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "HiddenVP.glsl";
            program.Value.load();

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupFeedbackBufferVP(String name, int numHardwareBones, int numHardwarePoses)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            if (numHardwareBones > 0 || numHardwarePoses > 0)
            {
                program.Value.SourceFile = UnifiedShaderBase + "FeedbackBufferVPHardwareSkin.glsl";
            }
            else
            {
                program.Value.SourceFile = UnifiedShaderBase + "FeedbackBufferVP.glsl";
            }

            program.Value.SkeletalAnimationIncluded = numHardwareBones > 0;
            program.Value.NumberOfPoses = (ushort)numHardwarePoses;
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(TextureMaps.None, numHardwareBones, numHardwarePoses, false));
            program.Value.load();

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                if (numHardwareBones > 0 || numHardwarePoses > 0)
                {
                    defaultParams.Value.setNamedAutoConstant("worldMatrix3x4Array", AutoConstantType.ACT_WORLD_MATRIX_ARRAY_3x4);
                    defaultParams.Value.setNamedAutoConstant("viewProjectionMatrix", AutoConstantType.ACT_VIEWPROJ_MATRIX);

                    if (numHardwarePoses > 0)
                    {
                        defaultParams.Value.setNamedAutoConstant("poseAnimAmount", AutoConstantType.ACT_ANIMATION_PARAMETRIC);
                    }
                }
                else
                {
                    defaultParams.Value.setNamedAutoConstant("worldViewProj", AutoConstantType.ACT_WORLDVIEWPROJ_MATRIX);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupEyeOuterVP(String name)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = EyeShaderBase + "EyeOuterVP.glsl";
            program.Value.load();

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("worldViewProj", AutoConstantType.ACT_WORLDVIEWPROJ_MATRIX);
                defaultParams.Value.setNamedAutoConstant("eyePosition", AutoConstantType.ACT_CAMERA_POSITION_OBJECT_SPACE);
                defaultParams.Value.setNamedAutoConstant("lightAttenuation", AutoConstantType.ACT_LIGHT_ATTENUATION, 0);
                defaultParams.Value.setNamedAutoConstant("lightPosition", AutoConstantType.ACT_LIGHT_POSITION_OBJECT_SPACE, 0);
            }

            return program;
        }

        #endregion Vertex Programs

        #region Fragment Programs

        protected override HighLevelGpuProgramSharedPtr setupUnifiedFrag(String name, MaterialDescription description, TextureMaps maps, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "UnifiedFS.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL VirtualTextureFuncs_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(maps, alpha, description.HasGlossMap, description.IsHighlight, description.HasOpacityValue));
            program.Value.load();

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                int textureIndex = 0;
                if ((maps & TextureMaps.Normal) == TextureMaps.Normal)
                {
                    defaultParams.Value.setNamedConstant("normalTexture", textureIndex++);
                }
                if ((maps & TextureMaps.Diffuse) == TextureMaps.Diffuse)
                {
                    defaultParams.Value.setNamedConstant("colorTexture", textureIndex++);
                }
                if ((maps & TextureMaps.Specular) == TextureMaps.Specular)
                {
                    defaultParams.Value.setNamedConstant("specularTexture", textureIndex++);
                }
                if ((maps & TextureMaps.Opacity) == TextureMaps.Opacity)
                {
                    defaultParams.Value.setNamedConstant("opacityTexture", textureIndex++);
                }
                if (maps != TextureMaps.None)
                {
                    defaultParams.Value.setNamedConstant("indirectionTex", textureIndex++);
                }

                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);

                if (description.HasGlossMap)
                {
                    defaultParams.Value.setNamedConstant("glossyStart", 40.0f);
                    defaultParams.Value.setNamedConstant("glossyRange", 0.0f);
                }
                else
                {
                    defaultParams.Value.setNamedAutoConstant("glossyness", AutoConstantType.ACT_SURFACE_SHININESS);
                }

                if ((maps & TextureMaps.Diffuse) == 0)
                {
                    defaultParams.Value.setNamedAutoConstant("diffuseColor", AutoConstantType.ACT_SURFACE_DIFFUSE_COLOUR);
                }

                //Check for need to pass specular color
                if ((maps & TextureMaps.Specular) == 0)
                {
                    defaultParams.Value.setNamedAutoConstant("specularColor", AutoConstantType.ACT_SURFACE_SPECULAR_COLOUR);
                }

                if (description.IsHighlight)
                {
                    defaultParams.Value.setNamedAutoConstant("highlightColor", AutoConstantType.ACT_CUSTOM, 1);
                }

                if (alpha)
                {
                    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupFeedbackBufferFP(String name)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "FeedbackBufferFP.glsl";
            program.Value.setParam("attach", "VirtualTextureFuncs_GLSL");

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupHiddenFP(String name)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "HiddenFP.glsl";

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupEyeOuterFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = EyeShaderBase + "EyeOuterFP.glsl";
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(TextureMaps.None, alpha, false, false, false));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("specularColor", AutoConstantType.ACT_SURFACE_SPECULAR_COLOUR);
                //if (alpha)
                //{
                //    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                //}
            }

            return program;
        }

        #endregion Fragment Programs
    }
}
