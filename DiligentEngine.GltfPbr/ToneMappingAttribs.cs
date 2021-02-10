using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    public enum ToneMappingMode
    {
        TONE_MAPPING_MODE_EXP           = 0,
        TONE_MAPPING_MODE_REINHARD      = 1,
        TONE_MAPPING_MODE_REINHARD_MOD  = 2,
        TONE_MAPPING_MODE_UNCHARTED2    = 3,
        TONE_MAPPING_FILMIC_ALU         = 4,
        TONE_MAPPING_LOGARITHMIC        = 5,
        TONE_MAPPING_ADAPTIVE_LOG       = 6,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ToneMappingAttribs
    {
        public static readonly ToneMappingAttribs Default = new ToneMappingAttribs()
        {
            iToneMappingMode      = (int)ToneMappingMode.TONE_MAPPING_MODE_UNCHARTED2,
            bAutoExposure         = true,
            fMiddleGray           = 0.18f,
            bLightAdaptation      = true,
            fWhitePoint           = 3f,
            fLuminanceSaturation  = 1f,
        };

        // Tone mapping mode.
        public int iToneMappingMode;
        // Automatically compute exposure to use in tone mapping.
        public bool bAutoExposure;
        // Middle gray value used by tone mapping operators.
        public float fMiddleGray;
        // Simulate eye adaptation to light changes.
        public bool bLightAdaptation;

        // White point to use in tone mapping.
        public float fWhitePoint;
        // Luminance point to use in tone mapping.
        public float fLuminanceSaturation;
        public uint Padding0;
        public uint Padding1;
    };
}
