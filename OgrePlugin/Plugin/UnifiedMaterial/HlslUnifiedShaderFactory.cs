using Engine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class HlslUnifiedShaderFactory : UnifiedShaderFactory
    {
        private const String UnifiedShaderBase = ShaderPathBase + ".Unified.D3D11.";
        private const String EyeShaderBase = ShaderPathBase + ".Eye.D3D11.";

        public HlslUnifiedShaderFactory(ResourceManager liveResourceManager, NormaMapReadMode normalMapReadMode, bool separateOpacityMap)
            : base(liveResourceManager, normalMapReadMode, separateOpacityMap)
        {

        }

        #region Vertex Programs

        protected override HighLevelGpuProgramSharedPtr setupUnifiedVertex(String name, TextureMaps maps, MaterialDescription description)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "UnifiedVS.hlsl";
            program.Value.setParam("entry_point", "mainVP");

            program.Value.setParam("target", "vs_4_0");
            program.Value.setParam("column_major_matrices", "false");
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
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "DepthCheck.hlsl";
            if (numHardwareBones > 0 || numHardwarePoses > 0)
            {
                program.Value.setParam("entry_point", "depthCheckSkinPose");
            }
            else
            {
                program.Value.setParam("entry_point", "depthCheckVP");
            }

            program.Value.setParam("target", "vs_4_0");
            program.Value.setParam("column_major_matrices", "false");
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
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "Hidden.hlsl";
            program.Value.setParam("entry_point", "hiddenVP");
            program.Value.setParam("target", "vs_4_0");
            program.Value.setParam("column_major_matrices", "false");
            program.Value.load();

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupFeedbackBufferVP(String name, int numHardwareBones, int numHardwarePoses)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "FeedbackBuffer.hlsl";
            if (numHardwareBones > 0 || numHardwarePoses > 0)
            {
                program.Value.setParam("entry_point", "FeedbackBufferVPHardwareSkinPose");
            }
            else
            {
                program.Value.setParam("entry_point", "FeedbackBufferVP");
            }

            program.Value.setParam("target", "vs_4_0");
            program.Value.setParam("column_major_matrices", "false");
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
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = EyeShaderBase + "Eye.hlsl";
            program.Value.setParam("entry_point", "mainVP");

            program.Value.setParam("target", "vs_4_0");
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
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "UnifiedFS.hlsl";
            program.Value.setParam("entry_point", "UnifiedFragmentShader");
            program.Value.setParam("target", "ps_4_0");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(maps, alpha, description.HasGlossMap, description.IsHighlight, description.HasOpacityValue));
            program.Value.load();

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                try
                {
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
                catch (Exception)
                {
                    Logging.Log.Warning($"Could not set params for frag program '{name}'.");
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupFeedbackBufferFP(String name)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "FeedbackBuffer.hlsl";
            program.Value.setParam("entry_point", "FeedbackBufferFP");
            program.Value.setParam("target", "ps_4_0");

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupHiddenFP(String name)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "Hidden.hlsl";
            program.Value.setParam("entry_point", "hiddenFP");
            program.Value.setParam("target", "ps_4_0");

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupEyeOuterFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "hlsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = EyeShaderBase + "Eye.hlsl";
            program.Value.setParam("entry_point", "eyeOuterFP");
            program.Value.setParam("target", "ps_4_0");
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
