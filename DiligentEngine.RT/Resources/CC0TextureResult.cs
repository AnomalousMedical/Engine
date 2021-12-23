using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Resources
{

    public class CC0TextureResult : IDisposable
    {
        private List<AutoPtr<ITexture>> texturePointers = new List<AutoPtr<ITexture>>();

        private List<IDeviceObject> baseColorSRVs = new List<IDeviceObject>();
        private List<IDeviceObject> normalSRVs = new List<IDeviceObject>();
        private List<IDeviceObject> physicalDescriptorSRVs = new List<IDeviceObject>();
        private List<IDeviceObject> ambientOcclusionSRVs = new List<IDeviceObject>();

        internal CC0TextureResult()
        {
        }

        public void Dispose()
        {
            foreach (var i in texturePointers) { i.Dispose(); }
        }

        public int NumTextures => baseColorSRVs.Count;

        public List<IDeviceObject> BaseColorSRVs => baseColorSRVs;
        internal void SetBaseColorMap(AutoPtr<ITexture> value)
        {
            this.baseColorSRVs.Add(value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));
            this.texturePointers.Add(value);
        }

        public List<IDeviceObject> NormalMapSRVs => normalSRVs;
        internal void SetNormalMap(AutoPtr<ITexture> value)
        {
            this.normalSRVs.Add(value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));
            this.texturePointers.Add(value);
        }

        public List<IDeviceObject> PhysicalDescriptorMapSRVs => physicalDescriptorSRVs;
        internal void SetPhysicalDescriptorMap(AutoPtr<ITexture> value)
        {
            this.physicalDescriptorSRVs.Add(value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));
            this.texturePointers.Add(value);
        }

        public List<IDeviceObject> AmbientOcclusionMapSRVs => ambientOcclusionSRVs;

        internal void SetAmbientOcclusionMap(AutoPtr<ITexture> value)
        {
            this.ambientOcclusionSRVs.Add(value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE));
            this.texturePointers.Add(value);
        }
    }
}
