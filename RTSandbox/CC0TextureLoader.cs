using DiligentEngine;
using Engine.Resources;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RTHack
{
    /// <summary>
    /// This loader can load the textures from https://cc0textures.com. It will reformat for the gltf renderer.
    /// </summary>
    public class CC0TextureLoader
    {
        const uint FixIndex = 0x0000ff00; //Fixes y
        const byte FixShift = 8;

        /// <summary>
        /// CC0 textures use an inverted y axis compared to our lights, so invert it here.
        /// </summary>
        /// <param name="map"></param>
        public unsafe static void FixCC0Normal(FreeImageBitmap map)
        {
            //Even though this alters x, this might not really be the correct math, but it does look right
            var firstPixel = (uint*)((byte*)map.Scan0.ToPointer() + (map.Height - 1) * map.Stride);
            var lastPixel = map.Width * map.Height;
            for (var i = 0; i < lastPixel; ++i)
            {
                uint pixelValue = firstPixel[i];
                uint fixItem = FixIndex & pixelValue;
                fixItem >>= FixShift;
                fixItem = 255 - fixItem;
                fixItem <<= FixShift;
                firstPixel[i] = (pixelValue & ~FixIndex) + fixItem;
            }
        }
    }
}
