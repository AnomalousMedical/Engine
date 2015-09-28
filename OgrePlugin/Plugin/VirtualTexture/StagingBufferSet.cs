using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class StagingBufferSet : IDisposable
    {
        private StagingPhysicalPage[] stagingPhysicalPages;
        private StagingIndirectionTexture oldIndirectionTextureStaging;
        private StagingIndirectionTexture newIndirectionTextureStaging;
        private bool updateOldIndirectionTexture = false;
        private bool updateNewIndirectionTexture = false;
        private ManualResetEventSlim gpuUploadWaitEvent = new ManualResetEventSlim(true);
        private byte oldIndirectionId;
        private byte newIndirectionId;

        public StagingBufferSet(int stagingImageCapacity, int maxMipCount)
        {
            stagingPhysicalPages = new StagingPhysicalPage[stagingImageCapacity];
            oldIndirectionTextureStaging = new StagingIndirectionTexture(maxMipCount);
            newIndirectionTextureStaging = new StagingIndirectionTexture(maxMipCount);
        }

        public void Dispose()
        {
            gpuUploadWaitEvent.Dispose();
            foreach (var stagingImage in stagingPhysicalPages)
            {
                if (stagingImage != null)
                {
                    stagingImage.Dispose();
                }
            }
            oldIndirectionTextureStaging.Dispose();
            newIndirectionTextureStaging.Dispose();
        }

        public void addedPhysicalTexture(PhysicalTexture physicalTexture, int textelsPerPhysicalPage)
        {
            stagingPhysicalPages[physicalTexture.Index] = new StagingPhysicalPage(textelsPerPhysicalPage, physicalTexture.TextureFormat);
        }

        public void reset()
        {
            updateOldIndirectionTexture = false;
            updateNewIndirectionTexture = false;
            gpuUploadWaitEvent.Reset();
        }

        public void returnedToPool()
        {
            gpuUploadWaitEvent.Set();
        }

        public void setIndirectionTextures(IndirectionTexture oldIndirectionTexture, IndirectionTexture newIndirectionTexture)
        {
            updateOldIndirectionTexture = oldIndirectionTexture != null;
            updateNewIndirectionTexture = oldIndirectionTexture != newIndirectionTexture;
            if (updateOldIndirectionTexture)
            {
                oldIndirectionTextureStaging.setData(oldIndirectionTexture);
                oldIndirectionId = oldIndirectionTexture.Id;
            }
            if (updateNewIndirectionTexture)
            {
                newIndirectionTextureStaging.setData(newIndirectionTexture);
                newIndirectionId = newIndirectionTexture.Id;
            }
        }

        internal void waitForGpuUpload()
        {
            gpuUploadWaitEvent.Wait();
        }

        public void uploadTexturesToGpu(Func<byte, bool> shouldUploadIndirection)
        {
            for (int u = 0; u < stagingPhysicalPages.Length; ++u)
            {
                stagingPhysicalPages[u].copyToGpu(Dest);
            }

            if (updateOldIndirectionTexture && shouldUploadIndirection(oldIndirectionId))
            {
                oldIndirectionTextureStaging.uploadToGpu();
            }

            if (updateNewIndirectionTexture && shouldUploadIndirection(newIndirectionId))
            {
                newIndirectionTextureStaging.uploadToGpu();
            }
        }

        internal void setPhysicalPage(PixelBox sourceBox, PhysicalTexture physicalTexture, int padding)
        {
            stagingPhysicalPages[physicalTexture.Index].setData(sourceBox, physicalTexture, padding);
        }

        public IntRect Dest { get; set; }
    }
}
