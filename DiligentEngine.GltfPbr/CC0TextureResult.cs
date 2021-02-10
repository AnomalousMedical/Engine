using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{

    public class CC0TextureResult : IDisposable
    {
        private AutoPtr<ITexture> baseColorMap;
        private AutoPtr<ITexture> normalMap;
        private AutoPtr<ITexture> physicalDescriptorMap;
        private AutoPtr<ITexture> ambientOcclusionMap;

        internal CC0TextureResult()
        {
        }

        public void Dispose()
        {
            baseColorMap?.Dispose();
            normalMap?.Dispose();
            physicalDescriptorMap?.Dispose();
            ambientOcclusionMap?.Dispose();
        }

        public ITexture BaseColorMap => baseColorMap?.Obj;
        internal void SetBaseColorMap(AutoPtr<ITexture> value)
        {
            this.baseColorMap = value;
        }

        public ITexture NormalMap => normalMap?.Obj;
        internal void SetNormalMap(AutoPtr<ITexture> value)
        {
            this.normalMap = value;
        }
        public ITexture PhysicalDescriptorMap => physicalDescriptorMap?.Obj;
        internal void SetPhysicalDescriptorMap(AutoPtr<ITexture> value)
        {
            this.physicalDescriptorMap = value;
        }
        public ITexture AmbientOcclusionMap => ambientOcclusionMap?.Obj;
        internal void SetAmbientOcclusionMap(AutoPtr<ITexture> value)
        {
            this.ambientOcclusionMap = value;
        }

    }
}
