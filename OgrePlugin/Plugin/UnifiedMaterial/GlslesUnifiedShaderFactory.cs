﻿using Engine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class GlslesUnifiedShaderFactory : UnifiedShaderFactory
    {
        private const String UnifiedShaderBase = ShaderPathBase + ".Unified.glsles.";
        private const String EyeShaderBase = ShaderPathBase + ".Eye.glsles.";

        public GlslesUnifiedShaderFactory(ResourceManager liveResourceManager, NormaMapReadlMode normalMapReadMode)
            : base(liveResourceManager, normalMapReadMode)
        {

        }

        #region Vertex Programs

        protected override HighLevelGpuProgramSharedPtr setupDepthCheckVP(String name, int numHardwareBones, int numHardwarePoses)
        {
            numHardwareBones = 0;
            numHardwarePoses = 0;

            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsles", GpuProgramType.GPT_VERTEX_PROGRAM);

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
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(numHardwareBones, numHardwarePoses, false));
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
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsles", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "HiddenVP.glsl";
            program.Value.load();

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupFeedbackBufferVP(String name, int numHardwareBones, int numHardwarePoses)
        {
            numHardwareBones = 0;
            numHardwarePoses = 0;

            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsles", GpuProgramType.GPT_VERTEX_PROGRAM);

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
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(numHardwareBones, numHardwarePoses, false));
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
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsles", GpuProgramType.GPT_VERTEX_PROGRAM);

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

        protected override HighLevelGpuProgramSharedPtr createUnifiedFrag(String name, TextureMaps maps, bool alpha, bool glossMap, bool highlight)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsles", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "UnifiedFS.glsl";
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines2(maps, alpha, glossMap, highlight));
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

                if (glossMap)
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

                if (highlight)
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

        protected override HighLevelGpuProgramSharedPtr createFeedbackBufferFP(String name, bool alpha)
        {
            alpha = false; //Never does alpha

            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsles", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "FeedbackBufferFP.glsl";
            //program.Value.setParam("attach", "VirtualTextureFuncs_GLSL");

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createHiddenFP(String name, bool alpha)
        {
            alpha = false; //Never does alpha

            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsles", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "HiddenFP.glsl";

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createEyeOuterFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsles", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = EyeShaderBase + "EyeOuterFP.glsl";
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

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