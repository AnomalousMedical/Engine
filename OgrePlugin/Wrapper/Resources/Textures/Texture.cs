using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
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
	    TU_AUTOMIPMAP = 16,
	    /// this texture will be a render target, i.e. used as a target for render to texture
	    /// setting this flag will ignore all other texture usages except TU_AUTOMIPMAP
	    TU_RENDERTARGET = 32,
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

        public HardwarePixelBufferSharedPtr getBuffer(uint face = 0, uint mipmap = 0)
        {
            HardwareBufferManager bufferManager = HardwareBufferManager.getInstance();
            return bufferManager.getPixelBufferObject(Texture_getBuffer(texture, new UIntPtr(face), new UIntPtr(mipmap), bufferManager.ProcessPixelBufferCallback));
        }

        public TextureType TextureType
        {
            get
            {
                return Texture_getTextureType(texture);
            }
        }

        public UInt32 Width
        {
            get
            {
                return Texture_getWidth(texture);
            }
        }

        public UInt32 Height
        {
            get
            {
                return Texture_getHeight(texture);
            }
        }

        public UInt32 Depth
        {
            get
            {
                return Texture_getDepth(texture);
            }
        }

        public PixelFormat Format
        {
            get
            {
                return Texture_getFormat(texture);
            }
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Texture_getBuffer(IntPtr texture, UIntPtr face, UIntPtr mipmap, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern TextureType Texture_getTextureType(IntPtr texture);
						    
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern UInt32 Texture_getHeight(IntPtr texture);
						    
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern UInt32 Texture_getWidth(IntPtr texture);
						    
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern UInt32 Texture_getDepth(IntPtr texture);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern PixelFormat Texture_getFormat(IntPtr texture);

#endregion
    }
}
