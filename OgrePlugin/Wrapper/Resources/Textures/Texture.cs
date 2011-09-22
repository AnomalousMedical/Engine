using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    /** Enum identifying the texture usage
    */
    public enum TextureUsage : uint
    {
	    TU_STATIC = HardwareBuffer.Usage.HBU_STATIC,
	    TU_DYNAMIC = HardwareBuffer.Usage.HBU_DYNAMIC,
	    TU_WRITE_ONLY = HardwareBuffer.Usage.HBU_WRITE_ONLY,
	    TU_STATIC_WRITE_ONLY = HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY, 
	    TU_DYNAMIC_WRITE_ONLY = HardwareBuffer.Usage.HBU_DYNAMIC_WRITE_ONLY,
        TU_DYNAMIC_WRITE_ONLY_DISCARDABLE = HardwareBuffer.Usage.HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE,
	    /// mipmaps will be automatically generated for this texture
	    TU_AUTOMIPMAP = 0x100,
	    /// this texture will be a render target, i.e. used as a target for render to texture
	    /// setting this flag will ignore all other texture usages except TU_AUTOMIPMAP
	    TU_RENDERTARGET = 0x200,
	    /// default to automatic mipmap generation static textures
	    TU_DEFAULT = TU_AUTOMIPMAP | TU_STATIC_WRITE_ONLY
        
    };

    /** Enum identifying the texture type
    */
    public enum TextureType
    {
        /// 1D texture, used in combination with 1D texture coordinates
        TEX_TYPE_1D = 1,
        /// 2D texture, used in combination with 2D texture coordinates (default)
        TEX_TYPE_2D = 2,
        /// 3D volume texture, used in combination with 3D texture coordinates
        TEX_TYPE_3D = 3,
        /// 3D cube map, used in combination with 3D texture coordinates
        TEX_TYPE_CUBE_MAP = 4
    };

    public class Texture : Resource
    {
        internal static Texture createWrapper(IntPtr texture)
        {
            return new Texture(texture);
        }

        private IntPtr texture;

        private Texture(IntPtr texture)
            :base(texture)
        {
            this.texture = texture;
        }

        public override void Dispose()
        {
            texture = IntPtr.Zero;
            base.Dispose();
        }

        public HardwarePixelBufferSharedPtr getBuffer()
        {
            HardwareBufferManager bufferManager = HardwareBufferManager.getInstance();
            return bufferManager.getPixelBufferObject(Texture_getBuffer(texture, bufferManager.ProcessPixelBufferCallback));
        }

#region PInvoke

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Texture_getBuffer(IntPtr texture, ProcessWrapperObjectDelegate processWrapper);

#endregion
    }
}
