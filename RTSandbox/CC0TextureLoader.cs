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

        /// <summary>
        /// CC0 textures use an inverted y axis compared to our lights, so invert it here.
        /// </summary>
        /// <param name="map"></param>
        public static void FixCC0Normal(FreeImageBitmap map)
        {
            unsafe
            {
                //This is assuming axgx layout like everything else, dont really care about r and b since g is
                //always the same and that is what we want to flip.
                var firstPixel = (uint*)((byte*)map.Scan0.ToPointer() + (map.Height - 1) * map.Stride);
                var lastPixel = map.Width * map.Height;
                for (var i = 0; i < lastPixel; ++i)
                {
                    uint pixelValue = firstPixel[i];
                    uint normalY = 0x0000ff00 & pixelValue;
                    normalY >>= 8;
                    normalY = 255 - normalY;
                    normalY <<= 8;
                    firstPixel[i] = (pixelValue & 0xffff00ff) + normalY;
                }
            }
        }
    }
}
