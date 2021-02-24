using DiligentEngine;
using DiligentEngine.GltfPbr;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class CC0TextureManager : ICC0TextureManager
    {
        class Entry
        {
            public AutoPtr<IShaderResourceBinding> srb;
            public int count;
            public string name;
            public Task task;
        }

        private Dictionary<IShaderResourceBinding, Entry> bindingToEntries = new Dictionary<IShaderResourceBinding, Entry>();
        private Dictionary<String, Entry> namesToEntries = new Dictionary<String, Entry>();

        private readonly CC0TextureLoader textureLoader;
        private readonly PbrRenderer pbrRenderer;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly ILogger<CC0TextureManager> logger;

        public CC0TextureManager(
            CC0TextureLoader textureLoader,
            PbrRenderer pbrRenderer,
            IPbrCameraAndLight pbrCameraAndLight,
            ILogger<CC0TextureManager> logger)
        {
            this.textureLoader = textureLoader;
            this.pbrRenderer = pbrRenderer;
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.logger = logger;
        }

        public async Task<IShaderResourceBinding> Checkout(String baseName)
        {
            Entry entry;
            if (!namesToEntries.TryGetValue(baseName, out entry))
            {
                entry = new Entry()
                {
                    name = baseName,
                    task = Task.Run(() =>
                    {
                        using var ccoTextures = textureLoader.LoadTextureSet(baseName);
                        entry.srb = pbrRenderer.CreateMaterialSRB(
                            pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                            pLightAttribs: pbrCameraAndLight.LightAttribs,
                            baseColorMap: ccoTextures.BaseColorMap,
                            normalMap: ccoTextures.NormalMap,
                            physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                            aoMap: ccoTextures.AmbientOcclusionMap
                        );
                    })
                };
                namesToEntries[baseName] = entry;
                await entry.task;
                entry.task = null; //Clear the task on the entry after finished
                bindingToEntries[entry.srb.Obj] = entry;
            }
            else if(entry.task != null)
            {
                await entry.task;
            }
            ++entry.count;
            return entry.srb.Obj;
        }

        public void Return(IShaderResourceBinding binding)
        {
            if (bindingToEntries.TryGetValue(binding, out var entry))
            {
                --entry.count;
                if (entry.count == 0)
                {
                    bindingToEntries.Remove(binding);
                    namesToEntries.Remove(entry.name);
                    entry.srb.Dispose();
                }
            }
            else
            {
                logger.LogInformation($"A {nameof(IShaderResourceBinding)} was returned that did not have an entry. Do not return items you did not check out and do not return items more than once.");
            }
        }
    }
}
