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

        internal CC0TextureResult()
        {
        }

        public void Dispose()
        {
            foreach (var i in texturePointers) { i.Dispose(); }
        }

        public IDeviceObject BaseColorSRV { get; private set; }

        public bool HasOpacity { get; private set; }

        public bool Reflective { get; private set; }

        internal void SetBaseColorMap(AutoPtr<ITexture> value, bool hasOpacity, bool reflective)
        {
            this.HasOpacity = hasOpacity;
            this.Reflective = reflective;
            this.BaseColorSRV = value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
            this.texturePointers.Add(value);
        }

        public IDeviceObject NormalMapSRV { get; private set; }
        internal void SetNormalMap(AutoPtr<ITexture> value)
        {
            this.NormalMapSRV = value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
            this.texturePointers.Add(value);
        }

        public IDeviceObject PhysicalDescriptorMapSRV { get; private set; }
        internal void SetPhysicalDescriptorMap(AutoPtr<ITexture> value)
        {
            this.PhysicalDescriptorMapSRV = value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
            this.texturePointers.Add(value);
        }

        public IDeviceObject AmbientOcclusionMapSRV { get; private set; }

        internal void SetAmbientOcclusionMap(AutoPtr<ITexture> value)
        {
            this.AmbientOcclusionMapSRV = value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
            this.texturePointers.Add(value);
        }

        public IDeviceObject EmissiveSRV { get; private set; }

        internal void SetEmissiveMap(AutoPtr<ITexture> value)
        {
            this.EmissiveSRV = value.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);
            this.texturePointers.Add(value);
        }
    }
}
