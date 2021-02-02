using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;

namespace DilligentEngine
{
    public enum BLEND_FACTOR : Int8
    {
        /// Undefined blend factor
        BLEND_FACTOR_UNDEFINED = 0,

        /// The blend factor is zero.\n
        /// Direct3D counterpart: D3D11_BLEND_ZERO/D3D12_BLEND_ZERO. OpenGL counterpart: GL_ZERO.
        BLEND_FACTOR_ZERO,

        /// The blend factor is one.\n
        /// Direct3D counterpart: D3D11_BLEND_ONE/D3D12_BLEND_ONE. OpenGL counterpart: GL_ONE.
        BLEND_FACTOR_ONE,

        /// The blend factor is RGB data from a pixel shader.\n
        /// Direct3D counterpart: D3D11_BLEND_SRC_COLOR/D3D12_BLEND_SRC_COLOR. OpenGL counterpart: GL_SRC_COLOR.
        BLEND_FACTOR_SRC_COLOR,

        /// The blend factor is 1-RGB, where RGB is the data from a pixel shader.\n
        /// Direct3D counterpart: D3D11_BLEND_INV_SRC_COLOR/D3D12_BLEND_INV_SRC_COLOR. OpenGL counterpart: GL_ONE_MINUS_SRC_COLOR.
        BLEND_FACTOR_INV_SRC_COLOR,

        /// The blend factor is alpha (A) data from a pixel shader.\n
        /// Direct3D counterpart: D3D11_BLEND_SRC_ALPHA/D3D12_BLEND_SRC_ALPHA. OpenGL counterpart: GL_SRC_ALPHA.
        BLEND_FACTOR_SRC_ALPHA,

        /// The blend factor is 1-A, where A is alpha data from a pixel shader.\n
        /// Direct3D counterpart: D3D11_BLEND_INV_SRC_ALPHA/D3D12_BLEND_INV_SRC_ALPHA. OpenGL counterpart: GL_ONE_MINUS_SRC_ALPHA.
        BLEND_FACTOR_INV_SRC_ALPHA,

        /// The blend factor is alpha (A) data from a render target.\n
        /// Direct3D counterpart: D3D11_BLEND_DEST_ALPHA/D3D12_BLEND_DEST_ALPHA. OpenGL counterpart: GL_DST_ALPHA.
        BLEND_FACTOR_DEST_ALPHA,

        /// The blend factor is 1-A, where A is alpha data from a render target.\n
        /// Direct3D counterpart: D3D11_BLEND_INV_DEST_ALPHA/D3D12_BLEND_INV_DEST_ALPHA. OpenGL counterpart: GL_ONE_MINUS_DST_ALPHA.
        BLEND_FACTOR_INV_DEST_ALPHA,

        /// The blend factor is RGB data from a render target.\n
        /// Direct3D counterpart: D3D11_BLEND_DEST_COLOR/D3D12_BLEND_DEST_COLOR. OpenGL counterpart: GL_DST_COLOR.
        BLEND_FACTOR_DEST_COLOR,

        /// The blend factor is 1-RGB, where RGB is the data from a render target.\n
        /// Direct3D counterpart: D3D11_BLEND_INV_DEST_COLOR/D3D12_BLEND_INV_DEST_COLOR. OpenGL counterpart: GL_ONE_MINUS_DST_COLOR.
        BLEND_FACTOR_INV_DEST_COLOR,

        /// The blend factor is (f,f,f,1), where f = min(As, 1-Ad),
        /// As is alpha data from a pixel shader, and Ad is alpha data from a render target.\n
        /// Direct3D counterpart: D3D11_BLEND_SRC_ALPHA_SAT/D3D12_BLEND_SRC_ALPHA_SAT. OpenGL counterpart: GL_SRC_ALPHA_SATURATE.
        BLEND_FACTOR_SRC_ALPHA_SAT,

        /// The blend factor is the constant blend factor set with IDeviceContext::SetBlendFactors().\n
        /// Direct3D counterpart: D3D11_BLEND_BLEND_FACTOR/D3D12_BLEND_BLEND_FACTOR. OpenGL counterpart: GL_CONSTANT_COLOR.
        BLEND_FACTOR_BLEND_FACTOR,

        /// The blend factor is one minus constant blend factor set with IDeviceContext::SetBlendFactors().\n
        /// Direct3D counterpart: D3D11_BLEND_INV_BLEND_FACTOR/D3D12_BLEND_INV_BLEND_FACTOR. OpenGL counterpart: GL_ONE_MINUS_CONSTANT_COLOR.
        BLEND_FACTOR_INV_BLEND_FACTOR,

        /// The blend factor is the second RGB data output from a pixel shader.\n
        /// Direct3D counterpart: D3D11_BLEND_SRC1_COLOR/D3D12_BLEND_SRC1_COLOR. OpenGL counterpart: GL_SRC1_COLOR.
        BLEND_FACTOR_SRC1_COLOR,

        /// The blend factor is 1-RGB, where RGB is the second RGB data output from a pixel shader.\n
        /// Direct3D counterpart: D3D11_BLEND_INV_SRC1_COLOR/D3D12_BLEND_INV_SRC1_COLOR. OpenGL counterpart: GL_ONE_MINUS_SRC1_COLOR.
        BLEND_FACTOR_INV_SRC1_COLOR,

        /// The blend factor is the second alpha (A) data output from a pixel shader.\n
        /// Direct3D counterpart: D3D11_BLEND_SRC1_ALPHA/D3D12_BLEND_SRC1_ALPHA. OpenGL counterpart: GL_SRC1_ALPHA.
        BLEND_FACTOR_SRC1_ALPHA,

        /// The blend factor is 1-A, where A is the second alpha data output from a pixel shader.\n
        /// Direct3D counterpart: D3D11_BLEND_INV_SRC1_ALPHA/D3D12_BLEND_INV_SRC1_ALPHA. OpenGL counterpart: GL_ONE_MINUS_SRC1_ALPHA.
        BLEND_FACTOR_INV_SRC1_ALPHA,

        /// Helper value that stores the total number of blend factors in the enumeration.
        BLEND_FACTOR_NUM_FACTORS
    };

    public enum BLEND_OPERATION : Int8
    {
        /// Undefined blend operation
        BLEND_OPERATION_UNDEFINED = 0,

        /// Add source and destination color components.\n
        /// Direct3D counterpart: D3D11_BLEND_OP_ADD/D3D12_BLEND_OP_ADD. OpenGL counterpart: GL_FUNC_ADD.
        BLEND_OPERATION_ADD,

        /// Subtract destination color components from source color components.\n
        /// Direct3D counterpart: D3D11_BLEND_OP_SUBTRACT/D3D12_BLEND_OP_SUBTRACT. OpenGL counterpart: GL_FUNC_SUBTRACT.
        BLEND_OPERATION_SUBTRACT,

        /// Subtract source color components from destination color components.\n
        /// Direct3D counterpart: D3D11_BLEND_OP_REV_SUBTRACT/D3D12_BLEND_OP_REV_SUBTRACT. OpenGL counterpart: GL_FUNC_REVERSE_SUBTRACT.
        BLEND_OPERATION_REV_SUBTRACT,

        /// Compute the minimum of source and destination color components.\n
        /// Direct3D counterpart: D3D11_BLEND_OP_MIN/D3D12_BLEND_OP_MIN. OpenGL counterpart: GL_MIN.
        BLEND_OPERATION_MIN,

        /// Compute the maximum of source and destination color components.\n
        /// Direct3D counterpart: D3D11_BLEND_OP_MAX/D3D12_BLEND_OP_MAX. OpenGL counterpart: GL_MAX.
        BLEND_OPERATION_MAX,

        /// Helper value that stores the total number of blend operations in the enumeration.
        BLEND_OPERATION_NUM_OPERATIONS
    };

    public enum LOGIC_OPERATION : Int8
    {
        /// Clear the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_CLEAR.
        LOGIC_OP_CLEAR = 0,

        /// Set the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_SET.
        LOGIC_OP_SET,

        /// Copy the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_COPY.
        LOGIC_OP_COPY,

        /// Perform an inverted-copy of the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_COPY_INVERTED.
        LOGIC_OP_COPY_INVERTED,

        /// No operation is performed on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_NOOP.
        LOGIC_OP_NOOP,

        /// Invert the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_INVERT.
        LOGIC_OP_INVERT,

        /// Perform a logical AND operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_AND.
        LOGIC_OP_AND,

        /// Perform a logical NAND operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_NAND.
        LOGIC_OP_NAND,

        /// Perform a logical OR operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_OR.
        LOGIC_OP_OR,

        /// Perform a logical NOR operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_NOR.
        LOGIC_OP_NOR,

        /// Perform a logical XOR operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_XOR.
        LOGIC_OP_XOR,

        /// Perform a logical equal operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_EQUIV.
        LOGIC_OP_EQUIV,

        /// Perform a logical AND and reverse operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_AND_REVERSE.
        LOGIC_OP_AND_REVERSE,

        /// Perform a logical AND and invert operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_AND_INVERTED.
        LOGIC_OP_AND_INVERTED,

        /// Perform a logical OR and reverse operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_OR_REVERSE.
        LOGIC_OP_OR_REVERSE,

        /// Perform a logical OR and invert operation on the render target.\n
        /// Direct3D12 counterpart: D3D12_LOGIC_OP_OR_INVERTED.
        LOGIC_OP_OR_INVERTED,

        /// Helper value that stores the total number of logical operations in the enumeration.
        LOGIC_OP_NUM_OPERATIONS
    };

    public enum COLOR_MASK : Int8
    {
        /// Do not store any components.
        COLOR_MASK_NONE = 0,

        /// Allow data to be stored in the red component.
        COLOR_MASK_RED = 1,

        /// Allow data to be stored in the green component.
        COLOR_MASK_GREEN = 2,

        /// Allow data to be stored in the blue component.
        COLOR_MASK_BLUE = 4,

        /// Allow data to be stored in the alpha component.
        COLOR_MASK_ALPHA = 8,

        /// Allow data to be stored in all components.
        COLOR_MASK_ALL = (((COLOR_MASK_RED | COLOR_MASK_GREEN) | COLOR_MASK_BLUE) | COLOR_MASK_ALPHA)
    };

    public class RenderTargetBlendDesc
    {
        /// Enable or disable blending for this render target. Default value: False.
        public Bool BlendEnable;

        /// Enable or disable a logical operation for this render target. Default value: False.

        public Bool LogicOperationEnable;

        /// Specifies the blend factor to apply to the RGB value output from the pixel shader
        /// Default value: Diligent::BLEND_FACTOR_ONE.
        public BLEND_FACTOR SrcBlend;

        /// Specifies the blend factor to apply to the RGB value in the render target
        /// Default value: Diligent::BLEND_FACTOR_ZERO.
        public BLEND_FACTOR DestBlend;

        /// Defines how to combine the source and destination RGB values
        /// after applying the SrcBlend and DestBlend factors.
        /// Default value: Diligent::BLEND_OPERATION_ADD.
        public BLEND_OPERATION BlendOp;

        /// Specifies the blend factor to apply to the alpha value output from the pixel shader.
        /// Blend factors that end in _COLOR are not allowed. 
        /// Default value: Diligent::BLEND_FACTOR_ONE.
        public BLEND_FACTOR SrcBlendAlpha;

        /// Specifies the blend factor to apply to the alpha value in the render target.
        /// Blend factors that end in _COLOR are not allowed. 
        /// Default value: Diligent::BLEND_FACTOR_ZERO.
        public BLEND_FACTOR DestBlendAlpha;

        /// Defines how to combine the source and destination alpha values
        /// after applying the SrcBlendAlpha and DestBlendAlpha factors.
        /// Default value: Diligent::BLEND_OPERATION_ADD.
        public BLEND_OPERATION BlendOpAlpha;

        /// Defines logical operation for the render target.
        /// Default value: Diligent::LOGIC_OP_NOOP.
        public LOGIC_OPERATION LogicOp;

        /// Render target write mask.
        /// Default value: Diligent::COLOR_MASK_ALL.
        public Uint8 RenderTargetWriteMask;
    }

    public class BlendStateDesc
    {
        /// Specifies whether to use alpha-to-coverage as a multisampling technique
        /// when setting a pixel to a render target. Default value: False.
        public Bool AlphaToCoverageEnable;

        /// Specifies whether to enable independent blending in simultaneous render targets.
        /// If set to False, only RenderTargets[0] is used. Default value: False.
        public Bool IndependentBlendEnable;

        /// An array of RenderTargetBlendDesc structures that describe the blend
        /// states for render targets
        public RenderTargetBlendDesc[] RenderTargets;
    }
}
