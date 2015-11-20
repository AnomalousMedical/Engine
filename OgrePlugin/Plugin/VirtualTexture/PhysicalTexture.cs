//#define DRAW_MIP_MARKERS
//#define DRAW_SKIPS

using Engine;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    public interface PhysicalTexture : IDisposable
    {
        void prepareForUpdates();

        void addPage(PixelBox source, IntRect destRect);

        void commitUpdates();

        void createTextureUnit(Pass pass);

        void removeTextureUnit(TextureUnitState textureUnit);

        String TextureName
        {
            get;
        }

        PixelFormat TextureFormat
        {
            get;
        }

        /// <summary>
        /// A numerical id for this texture, can be used in arrays.
        /// </summary>
        int Index { get; }
    }
}
