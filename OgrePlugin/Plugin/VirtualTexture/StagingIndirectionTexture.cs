using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class StagingIndirectionTexture : IDisposable
    {
        private Image[] images;
        private PixelBox[] pixelBox;
        private IndirectionTexture indirectionTexture;

        public StagingIndirectionTexture(int numMipLevels)
        {
            images = new Image[numMipLevels];
            pixelBox = new PixelBox[numMipLevels];
            int currentMipLevel = 1;

            for (int i = numMipLevels - 1; i >= 0; --i)
            {
                images[i] = new Image((uint)currentMipLevel, (uint)currentMipLevel, 1, PixelFormat.PF_A8R8G8B8, 1, 0);
                pixelBox[i] = images[i].getPixelBox();
                currentMipLevel <<= 1;
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < images.Length; ++i)
            {
                pixelBox[i].Dispose();
                images[i].Dispose();
            }
        }

        public void setData(IndirectionTexture indirectionTexture)
        {
            this.indirectionTexture = indirectionTexture;
            indirectionTexture.copyToStaging(pixelBox);
        }

        public void uploadToGpu()
        {
            indirectionTexture.uploadStagingToGpu(pixelBox);
        }
    }
}
