using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public class RTInstances
    {
        public delegate void ShaderTableBinder(IShaderBindingTable sbt, ITopLevelAS tlas);

        List<TLASBuildInstanceData> instances = new List<TLASBuildInstanceData>();
        List<ShaderTableBinder> shaderTableBinders = new List<ShaderTableBinder>();

        internal List<TLASBuildInstanceData> Instances => instances;

        public void AddTlasBuild(TLASBuildInstanceData instance)
        {
            instances.Add(instance);
        }

        public void RemoveTlasBuild(TLASBuildInstanceData instance)
        {
            instances.Remove(instance);
        }

        public void AddShaderTableBinder(ShaderTableBinder binder)
        {
            shaderTableBinders.Add(binder);
        }

        public void RemoveShaderTableBinder(ShaderTableBinder binder)
        {
            shaderTableBinders.Remove(binder);
        }

        internal void BindShaders(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            foreach (var i in shaderTableBinders)
            {
                i(sbt, tlas);
            }
        }
    }

    public class RTInstances<T> : RTInstances { }
}
