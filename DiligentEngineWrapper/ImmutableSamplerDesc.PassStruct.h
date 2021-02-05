#pragma once
#include "Primitives/interface/BasicTypes.h"
#include "Graphics/GraphicsEngine/interface/GraphicsTypes.h"

namespace Diligent 
{
struct ImmutableSamplerDescPassStruct
{
        SHADER_TYPE ShaderStages;
        Char* SamplerOrTextureName;
        /// Texture minification filter, see Diligent::FILTER_TYPE for details.
    /// Default value: Diligent::FILTER_TYPE_LINEAR.
        FILTER_TYPE MinFilter           DEFAULT_INITIALIZER(FILTER_TYPE_LINEAR);

        /// Texture magnification filter, see Diligent::FILTER_TYPE for details.
        /// Default value: Diligent::FILTER_TYPE_LINEAR.
        FILTER_TYPE MagFilter           DEFAULT_INITIALIZER(FILTER_TYPE_LINEAR);

        /// Mip filter, see Diligent::FILTER_TYPE for details. 
        /// Only FILTER_TYPE_POINT, FILTER_TYPE_LINEAR, FILTER_TYPE_ANISOTROPIC, and 
        /// FILTER_TYPE_COMPARISON_ANISOTROPIC are allowed.
        /// Default value: Diligent::FILTER_TYPE_LINEAR.
        FILTER_TYPE MipFilter           DEFAULT_INITIALIZER(FILTER_TYPE_LINEAR);

        /// Texture address mode for U coordinate, see Diligent::TEXTURE_ADDRESS_MODE for details
        /// Default value: Diligent::TEXTURE_ADDRESS_CLAMP.
        TEXTURE_ADDRESS_MODE AddressU   DEFAULT_INITIALIZER(TEXTURE_ADDRESS_CLAMP);

        /// Texture address mode for V coordinate, see Diligent::TEXTURE_ADDRESS_MODE for details
        /// Default value: Diligent::TEXTURE_ADDRESS_CLAMP.
        TEXTURE_ADDRESS_MODE AddressV   DEFAULT_INITIALIZER(TEXTURE_ADDRESS_CLAMP);

        /// Texture address mode for W coordinate, see Diligent::TEXTURE_ADDRESS_MODE for details
        /// Default value: Diligent::TEXTURE_ADDRESS_CLAMP.
        TEXTURE_ADDRESS_MODE AddressW   DEFAULT_INITIALIZER(TEXTURE_ADDRESS_CLAMP);

        /// Offset from the calculated mipmap level. For example, if a sampler calculates that a texture 
        /// should be sampled at mipmap level 1.2 and MipLODBias is 2.3, then the texture will be sampled at 
        /// mipmap level 3.5. Default value: 0.
        Float32 MipLODBias                  DEFAULT_INITIALIZER(0);

        /// Maximum anisotropy level for the anisotropic filter. Default value: 0.
        Uint32 MaxAnisotropy                DEFAULT_INITIALIZER(0);

        /// A function that compares sampled data against existing sampled data when comparsion
        /// filter is used. Default value: Diligent::COMPARISON_FUNC_NEVER.
        COMPARISON_FUNCTION ComparisonFunc  DEFAULT_INITIALIZER(COMPARISON_FUNC_NEVER);

        /// Border color to use if TEXTURE_ADDRESS_BORDER is specified for AddressU, AddressV, or AddressW. 
        /// Default value: {0,0,0,0}
        Float32 BorderColor[4]              DEFAULT_INITIALIZER({});

        /// Specifies the minimum value that LOD is clamped to before accessing the texture MIP levels.
        /// Must be less than or equal to MaxLOD.
        /// Default value: 0.
        float MinLOD                        DEFAULT_INITIALIZER(0);

        /// Specifies the maximum value that LOD is clamped to before accessing the texture MIP levels.
        /// Must be greater than or equal to MinLOD.
        /// Default value: +FLT_MAX.
        float MaxLOD                        DEFAULT_INITIALIZER(+3.402823466e+38F);
};
}