using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreNextPlugin
{
    public enum GPUVendor
    {
        GPU_UNKNOWN = 0,
        GPU_NVIDIA = 1,
        GPU_AMD = 2,
        GPU_INTEL = 3,
        GPU_S3 = 4,
        GPU_MATROX = 5,
        GPU_3DLABS = 6,
        GPU_SIS = 7,
        GPU_IMAGINATION_TECHNOLOGIES = 8,
        GPU_APPLE = 9,  // Apple Software Renderer
        GPU_NOKIA = 10,
        GPU_MS_SOFTWARE = 11, // Microsoft software device
        GPU_MS_WARP = 12, // Microsoft WARP (Windows Advanced Rasterization Platform) software device - http://msdn.microsoft.com/en-us/library/dd285359.aspx
        GPU_ARM = 13, // For the Mali chipsets
        GPU_QUALCOMM = 14,
        GPU_MOZILLA = 15, // WebGL on Mozilla/Firefox based browser
        GPU_WEBKIT = 16, // WebGL on WebKit/Chrome base browser
        /// placeholder
        GPU_VENDOR_COUNT = 17
    };
}
