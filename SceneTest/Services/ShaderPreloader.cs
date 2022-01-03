using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Services
{
    internal class ShaderPreloader : IDisposable
    {
        private readonly PrimaryHitShader.Factory shaderFactory;
        private List<PrimaryHitShader> shaders = new List<PrimaryHitShader>();
        private bool disposed = false;

        public ShaderPreloader(PrimaryHitShader.Factory shaderFactory, ICoroutineRunner coroutine)
        {
            this.shaderFactory = shaderFactory;

            coroutine.RunTask(async () =>
            {
                var descriptions = new List<PrimaryHitShader.Desc>();
                descriptions.Add(new PrimaryHitShader.Desc
                {
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    ShaderType = PrimaryHitShaderType.Mesh
                });

                descriptions.Add(new PrimaryHitShader.Desc
                {
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    Reflective = true,
                    ShaderType = PrimaryHitShaderType.Mesh
                });

                descriptions.Add(new PrimaryHitShader.Desc
                {
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    ShaderType = PrimaryHitShaderType.Sprite
                });

                descriptions.Add(new PrimaryHitShader.Desc
                {
                    HasNormalMap = true,
                    HasPhysicalDescriptorMap = true,
                    Reflective = true,
                    ShaderType = PrimaryHitShaderType.Sprite
                });

                var loadTasks = new List<Task<PrimaryHitShader>>();
                foreach(var desc in descriptions)
                {
                    loadTasks.Add(shaderFactory.Checkout(desc));
                }

                await Task.WhenAll(loadTasks);

                if (disposed)
                {
                    //Return shaders, we were disposed
                    foreach (var loadTask in loadTasks)
                    {
                        shaderFactory.TryReturn(loadTask.Result);
                    }
                }
                else
                {
                    foreach (var loadTask in loadTasks)
                    {
                        shaders.Add(loadTask.Result);
                    }
                }
            });
        }

        public void Dispose()
        {
            disposed = true;
            foreach (var shader in shaders)
            {
                shaderFactory.TryReturn(shader);
            }
        }
    }
}
