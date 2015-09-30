﻿using OgrePlugin;
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
                images[i] = new Image((uint)currentMipLevel, (uint)currentMipLevel, 1, IndirectionTexture.BufferFormat, 1, 0);
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
            //indirectionTexture.debug_CheckTexture(images);
        }

        public void uploadToGpu()
        {
            indirectionTexture.uploadStagingToGpu(pixelBox);
        }

        public void debug_dumpTextures(String outputFolder)
        {
            for(int i = 0; i < images.Length; ++i)
            {
                images[i].save(Path.Combine(outputFolder, String.Format("StagingBuffer_{0}.png", i)));
            }
        }
    }
}
