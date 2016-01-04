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
    /// <summary>
    /// Interface for physical textures since we may need different strategies to handle them
    /// (buffered vs direct, see subclasses).
    /// </summary>
    public interface PhysicalTexture : IDisposable
    {
        void prepareForUpdates();

        void addPage(PixelBox source, IntRect destRect);

        void commitUpdates();

        void createTextureUnit(Pass pass);

        void removeTextureUnit(TextureUnitState textureUnit);

        void suspendTexture(String placeholderName);

        void restoreTexture();

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
