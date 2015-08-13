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

        public GlslUnifiedShaderFactory(ResourceManager liveResourceManager, NormaMapReadlMode normalMapReadMode)
            : base(liveResourceManager, normalMapReadMode)
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

        protected override HighLevelGpuProgramSharedPtr setupUnifiedVP(String name, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            if (numHardwareBones > 0 || numHardwarePoses > 0)
            {
                program.Value.SourceFile = UnifiedShaderBase + "MainVPHardwareSkin.glsl";
            }
            else
            {
                program.Value.SourceFile = UnifiedShaderBase + "MainVP.glsl";
            }

            program.Value.setParam("attach", "Pack_VP_GLSL");
            program.Value.SkeletalAnimationIncluded = numHardwareBones > 0;
            program.Value.NumberOfPoses = (ushort)numHardwarePoses;
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(numHardwareBones, numHardwarePoses, parity));
            program.Value.load();

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                if (numHardwareBones > 0 || numHardwarePoses > 0)
                {
                    defaultParams.Value.setNamedAutoConstant("worldEyePosition", AutoConstantType.ACT_CAMERA_POSITION);
                    defaultParams.Value.setNamedAutoConstant("lightAttenuation", AutoConstantType.ACT_LIGHT_ATTENUATION, 0);
                    defaultParams.Value.setNamedAutoConstant("worldLightPosition", AutoConstantType.ACT_LIGHT_POSITION, 0);

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
                    defaultParams.Value.setNamedAutoConstant("eyePosition", AutoConstantType.ACT_CAMERA_POSITION_OBJECT_SPACE);
                    defaultParams.Value.setNamedAutoConstant("lightAttenuation", AutoConstantType.ACT_LIGHT_ATTENUATION, 0);
                    defaultParams.Value.setNamedAutoConstant("lightPosition", AutoConstantType.ACT_LIGHT_POSITION_OBJECT_SPACE, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupNoTexturesVP(String name, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            if (numHardwareBones > 0 || numHardwarePoses > 0)
            {
                program.Value.SourceFile = UnifiedShaderBase + "NoTexturesVPHardwareSkin.glsl";
            }
            else
            {
                program.Value.SourceFile = UnifiedShaderBase + "NoTexturesVP.glsl";
            }

            program.Value.SkeletalAnimationIncluded = numHardwareBones > 0;
            program.Value.NumberOfPoses = (ushort)numHardwarePoses;
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(numHardwareBones, numHardwarePoses, parity));
            program.Value.load();

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                if (numHardwareBones > 0 || numHardwarePoses > 0)
                {
                    defaultParams.Value.setNamedAutoConstant("worldEyePosition", AutoConstantType.ACT_CAMERA_POSITION);
                    defaultParams.Value.setNamedAutoConstant("lightAttenuation", AutoConstantType.ACT_LIGHT_ATTENUATION, 0);
                    defaultParams.Value.setNamedAutoConstant("worldLightPosition", AutoConstantType.ACT_LIGHT_POSITION, 0);

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
                    defaultParams.Value.setNamedAutoConstant("eyePosition", AutoConstantType.ACT_CAMERA_POSITION_OBJECT_SPACE);
                    defaultParams.Value.setNamedAutoConstant("lightAttenuation", AutoConstantType.ACT_LIGHT_ATTENUATION, 0);
                    defaultParams.Value.setNamedAutoConstant("lightPosition", AutoConstantType.ACT_LIGHT_POSITION_OBJECT_SPACE, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupDepthCheckVP(String name, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            parity = false; //Does not do parity

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
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(numHardwareBones, numHardwarePoses, parity));
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

        protected override HighLevelGpuProgramSharedPtr setupHiddenVP(String name, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            parity = false; //Does not do parity

            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_VERTEX_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "HiddenVP.glsl";
            program.Value.SkeletalAnimationIncluded = numHardwareBones > 0;
            program.Value.NumberOfPoses = (ushort)numHardwarePoses;
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(numHardwareBones, numHardwarePoses, parity));
            program.Value.load();

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr setupFeedbackBufferVP(String name, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            parity = false; //Does not do parity

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
            program.Value.setParam("preprocessor_defines", DetermineVertexPreprocessorDefines(numHardwareBones, numHardwarePoses, parity));
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

        protected override HighLevelGpuProgramSharedPtr setupEyeOuterVP(String name, int numHardwareBones, int numHardwarePoses, bool parity)
        {
            numHardwareBones = 0;
            numHardwarePoses = 0;
            parity = false;

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

        protected override HighLevelGpuProgramSharedPtr createNormalMapSpecularFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "NormalMapSpecularFP.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL VirtualTextureFuncs_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("specularColor", AutoConstantType.ACT_SURFACE_SPECULAR_COLOUR);
                defaultParams.Value.setNamedAutoConstant("glossyness", AutoConstantType.ACT_SURFACE_SHININESS);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);
                defaultParams.Value.setNamedConstant("normalTexture", 0);
                defaultParams.Value.setNamedConstant("colorTexture", 1);
                defaultParams.Value.setNamedConstant("indirectionTex", 2);
                if (alpha)
                {
                    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createNormalMapSpecularHighlightFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "NormalMapSpecularHighlightFP.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL VirtualTextureFuncs_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("specularColor", AutoConstantType.ACT_SURFACE_SPECULAR_COLOUR);
                defaultParams.Value.setNamedAutoConstant("glossyness", AutoConstantType.ACT_SURFACE_SHININESS);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);
                defaultParams.Value.setNamedAutoConstant("highlightColor", AutoConstantType.ACT_CUSTOM, 1);
                defaultParams.Value.setNamedConstant("normalTexture", 0);
                defaultParams.Value.setNamedConstant("colorTexture", 1);
                defaultParams.Value.setNamedConstant("indirectionTex", 2);
                if (alpha)
                {
                    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createNormalMapSpecularMapFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "NormalMapSpecularMapFP.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL VirtualTextureFuncs_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("glossyness", AutoConstantType.ACT_SURFACE_SHININESS);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);
                defaultParams.Value.setNamedConstant("normalTexture", 0);
                defaultParams.Value.setNamedConstant("colorTexture", 1);
                defaultParams.Value.setNamedConstant("specularTexture", 2);
                defaultParams.Value.setNamedConstant("indirectionTex", 3);
                if (alpha)
                {
                    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createNormalMapSpecularOpacityMapFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "NormalMapSpecularOpacityMapFP.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL VirtualTextureFuncs_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("specularColor", AutoConstantType.ACT_SURFACE_SPECULAR_COLOUR);
                defaultParams.Value.setNamedAutoConstant("glossyness", AutoConstantType.ACT_SURFACE_SHININESS);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);
                defaultParams.Value.setNamedConstant("normalTexture", 0);
                defaultParams.Value.setNamedConstant("colorTexture", 1);
                defaultParams.Value.setNamedConstant("opacityTexture", 2);
                defaultParams.Value.setNamedConstant("indirectionTex", 3);
                if (alpha)
                {
                    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createNormalMapSpecularMapGlossMapFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "NormalMapSpecularMapGlossMapFP.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL VirtualTextureFuncs_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);
                defaultParams.Value.setNamedConstant("glossyStart", 40.0f);
                defaultParams.Value.setNamedConstant("glossyRange", 0.0f);
                defaultParams.Value.setNamedConstant("normalTexture", 0);
                defaultParams.Value.setNamedConstant("colorTexture", 1);
                defaultParams.Value.setNamedConstant("specularTexture", 2);
                defaultParams.Value.setNamedConstant("indirectionTex", 3);
                if (alpha)
                {
                    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createNormalMapSpecularMapOpacityMapFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "NormalMapSpecularMapOpacityMapFP.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL VirtualTextureFuncs_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("glossyness", AutoConstantType.ACT_SURFACE_SHININESS);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);
                defaultParams.Value.setNamedConstant("normalTexture", 0);
                defaultParams.Value.setNamedConstant("colorTexture", 1);
                defaultParams.Value.setNamedConstant("specularTexture", 2);
                defaultParams.Value.setNamedConstant("opacityTexture", 3);
                defaultParams.Value.setNamedConstant("indirectionTex", 4);
                if (alpha)
                {
                    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createNormalMapSpecularMapOpacityMapGlossMapFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "NormalMapSpecularMapOpacityMapGlossMapFP.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL VirtualTextureFuncs_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);
                defaultParams.Value.setNamedConstant("glossyStart", 40.0f);
                defaultParams.Value.setNamedConstant("glossyRange", 0.0f);
                defaultParams.Value.setNamedConstant("normalTexture", 0);
                defaultParams.Value.setNamedConstant("colorTexture", 1);
                defaultParams.Value.setNamedConstant("specularTexture", 2);
                defaultParams.Value.setNamedConstant("opacityGlossTexture", 3);
                defaultParams.Value.setNamedConstant("indirectionTex", 4);
                if (alpha)
                {
                    defaultParams.Value.setNamedAutoConstant("alpha", AutoConstantType.ACT_CUSTOM, 0);
                }
            }

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createNoTexturesColoredFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "NoTexturesColoredFP.glsl";
            program.Value.setParam("attach", "Lighting_FP_GLSL Unpack_FP_GLSL");
            program.Value.setParam("preprocessor_defines", DetermineFragmentPreprocessorDefines(alpha));

            using (var defaultParams = program.Value.getDefaultParameters())
            {
                defaultParams.Value.setNamedAutoConstant("diffuseColor", AutoConstantType.ACT_SURFACE_DIFFUSE_COLOUR);
                defaultParams.Value.setNamedAutoConstant("lightDiffuseColor", AutoConstantType.ACT_LIGHT_DIFFUSE_COLOUR, 0);
                defaultParams.Value.setNamedAutoConstant("specularColor", AutoConstantType.ACT_SURFACE_SPECULAR_COLOUR);
                defaultParams.Value.setNamedAutoConstant("glossyness", AutoConstantType.ACT_SURFACE_SHININESS);
                defaultParams.Value.setNamedAutoConstant("emissiveColor", AutoConstantType.ACT_SURFACE_EMISSIVE_COLOUR);
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

            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "FeedbackBufferFP.glsl";
            program.Value.setParam("attach", "VirtualTextureFuncs_GLSL");

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createHiddenFP(String name, bool alpha)
        {
            alpha = false; //Never does alpha

            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

            program.Value.SourceFile = UnifiedShaderBase + "HiddenFP.glsl";

            return program;
        }

        protected override HighLevelGpuProgramSharedPtr createEyeOuterFP(String name, bool alpha)
        {
            var program = HighLevelGpuProgramManager.Instance.createProgram(name, ResourceGroupName, "glsl", GpuProgramType.GPT_FRAGMENT_PROGRAM);

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
