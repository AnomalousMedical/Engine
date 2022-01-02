using DiligentEngine;
using Engine;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Resources
{
    public class CCOTextureBindingDescription
    {
        public CCOTextureBindingDescription()
        {
        }

        public CCOTextureBindingDescription(string baseName, bool reflective = false)
        {
            this.BaseName = baseName;
            Reflective = reflective;
        }

        public String BaseName { get; }

        public bool Reflective { get; }

        public override bool Equals(object obj)
        {
            return obj is CCOTextureBindingDescription description
                    && BaseName == description.BaseName
                    && Reflective == description.Reflective;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BaseName, Reflective);
        }
    }

    public class TextureManager
    {
        private PooledResourceManager<CCOTextureBindingDescription, CC0TextureResult> pooledResources = new PooledResourceManager<CCOTextureBindingDescription, CC0TextureResult>();
        private readonly CC0TextureLoader textureLoader;
        private readonly ILogger<TextureManager> logger;

        public TextureManager(
            CC0TextureLoader textureLoader,
            ILogger<TextureManager> logger
            )
        {
            this.textureLoader = textureLoader;
            this.logger = logger;
        }

        public Task<CC0TextureResult> Checkout(CCOTextureBindingDescription desc)
        {
            return pooledResources.Checkout(desc, async () =>
            {
                var sw = new Stopwatch();
                sw.Start();
                CC0TextureResult result = null;
                result = await textureLoader.LoadTextureSet(desc.BaseName, desc.Reflective);
                sw.Stop();
                logger.LogInformation($"Loaded cc0 texture '{desc.BaseName}' in {sw.ElapsedMilliseconds} ms.");

                return pooledResources.CreateResult(result);
            });
        }

        public void Return(CC0TextureResult binding)
        {
            pooledResources.Return(binding);
        }

        public void TryReturn(CC0TextureResult binding)
        {
            if(binding != null)
            {
                Return(binding);
            }
        }
    }
}
