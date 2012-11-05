using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public static class PixelFactorManager
    {
        public static float PixelFactor
        {
            get
            {
                return PixelFactorManager_GetPixelFactor();
            }
            set
            {
                PixelFactorManager_SetPixelFactor(value);
            }
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float PixelFactorManager_GetPixelFactor();

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void PixelFactorManager_SetPixelFactor(float value);

        #endregion
    }
}
