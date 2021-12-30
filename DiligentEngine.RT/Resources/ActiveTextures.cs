using Engine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Resources
{
    public class ActiveTextures : IDisposable
    {
        class TextureBinding
        {
            public int count;
            public HLSL.BlasInstanceData data;
        }

        public int MaxTextures => 100; //This can be much higher
        internal List<IDeviceObject> Textures => textures;

        private Stack<int> availableSlots;

        private Dictionary<CC0TextureResult, TextureBinding> textureLookup;

        private List<IDeviceObject> textures;
        private AutoPtr<ITexture> placeholderTexture;
        private IDeviceObject placeholderTextureDeviceObject;

        public ActiveTextures(TextureLoader textureLoader, IResourceProvider<ShaderLoader<RTShaders>> resourceProvider)
        {
            using var placeholderStream = resourceProvider.openFile("assets/Placeholder.png");
            placeholderTexture = textureLoader.LoadTexture(placeholderStream, "Placeholder", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, false);
            placeholderTextureDeviceObject = placeholderTexture.Obj.GetDefaultView(TEXTURE_VIEW_TYPE.TEXTURE_VIEW_SHADER_RESOURCE);

            textureLookup = new Dictionary<CC0TextureResult, TextureBinding>(MaxTextures);
            textures = new List<IDeviceObject>(MaxTextures);
            availableSlots = new Stack<int>(MaxTextures);
            for (var i = 0; i < MaxTextures; ++i)
            {
                availableSlots.Push(i);
                textures.Add(placeholderTextureDeviceObject);
            }
        }

        public void Dispose()
        {
            placeholderTexture.Dispose();
        }

        /// <summary>
        /// Add an active texture. Must be done from main thread.
        /// </summary>
        /// <param name="texture"></param>
        public HLSL.BlasInstanceData AddActiveTexture(CC0TextureResult texture)
        {
            TextureBinding binding;
            if(!textureLookup.TryGetValue(texture, out binding))
            {
                binding = new TextureBinding();
                textureLookup.Add(texture, binding);
                if(texture.BaseColorSRV != null)
                {
                    binding.data.baseTexture = GetTextureSlot();
                    textures[binding.data.baseTexture] = texture.BaseColorSRV;
                }
                if (texture.NormalMapSRV != null)
                {
                    binding.data.normalTexture = GetTextureSlot();
                    textures[binding.data.normalTexture] = texture.NormalMapSRV;
                }
                if (texture.PhysicalDescriptorMapSRV != null)
                {
                    binding.data.physicalTexture = GetTextureSlot();
                    textures[binding.data.physicalTexture] = texture.PhysicalDescriptorMapSRV;
                }
                if (texture.EmissiveSRV != null)
                {
                    binding.data.emissiveTexture = GetTextureSlot();
                    textures[binding.data.emissiveTexture] = texture.EmissiveSRV;
                }
            }
            binding.count++;
            return binding.data;
        }

        /// <summary>
        /// Remove an active texture. Must be done from main thread.
        /// </summary>
        /// <param name="texture"></param>
        public void RemoveActiveTexture(CC0TextureResult texture)
        {
            if(textureLookup.TryGetValue(texture, out var binding))
            {
                binding.count--;
                if(binding.count == 0)
                {
                    if (texture.BaseColorSRV != null)
                    {
                        ReturnTextureSlot(binding.data.baseTexture);
                        textures[binding.data.baseTexture] = placeholderTextureDeviceObject;
                    }
                    if (texture.NormalMapSRV != null)
                    {
                        ReturnTextureSlot(binding.data.normalTexture);
                        textures[binding.data.normalTexture] = placeholderTextureDeviceObject;
                    }
                    if (texture.PhysicalDescriptorMapSRV != null)
                    {
                        ReturnTextureSlot(binding.data.physicalTexture);
                        textures[binding.data.physicalTexture] = placeholderTextureDeviceObject;
                    }
                    if (texture.EmissiveSRV != null)
                    {
                        ReturnTextureSlot(binding.data.emissiveTexture);
                        textures[binding.data.emissiveTexture] = placeholderTextureDeviceObject;
                    }
                    textureLookup.Remove(texture);
                }
            }
        }

        private int GetTextureSlot()
        {
            if(availableSlots.Count == 0)
            {
                throw new InvalidOperationException($"Ran out of texture slots. The current max is {this.MaxTextures}.");
            }
            return availableSlots.Pop();
        }

        private void ReturnTextureSlot(int slot)
        {
            availableSlots.Push(slot);
        }
    }
}
