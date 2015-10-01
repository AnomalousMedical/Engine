using OgrePlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class StagingIndirectionTexture : IDisposable
    {
        private Image image;
        private PixelBox[] pixelBox;
        private IndirectionTexture indirectionTexture;

        public StagingIndirectionTexture(int numMipLevels)
        {
            int numMips = numMipLevels - 1;
            image = new Image((uint)(1 << numMips), (uint)(1 << numMips), 1, IndirectionTexture.BufferFormat, 1, (uint)numMips);
            pixelBox = new PixelBox[numMipLevels];

            for (uint i = 0; i < numMipLevels; ++i)
            {
                pixelBox[i] = image.getPixelBox(0, i);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < pixelBox.Length; ++i)
            {
                pixelBox[i].Dispose();
            }
            image.Dispose();
        }

        public void setData(IndirectionTexture indirectionTexture)
        {
            this.indirectionTexture = indirectionTexture;
            indirectionTexture.copyToStaging(pixelBox);
        }

        public void uploadToGpu()
        {
            //indirectionTexture.uploadStagingToGpu(pixelBox);
            indirectionTexture.uploadStagingToGpu(pixelBox, image);
        }

        public void debug_dumpTextures(String outputFolder)
        {
            image.saveAllLevels(outputFolder, "StagingBuffer", "png");
        }
    }
}
