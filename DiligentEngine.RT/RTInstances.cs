using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public interface IShaderResourceBinder
    {
        void Bind(IShaderResourceBinding rayTracingSRB);
    }

    public interface IShaderTableBinder
    {
        /// <summary>
        /// Bind a shader to a shader binding table for the given TopLevelAS. Do not store copies of the inputs.
        /// </summary>
        /// <param name="sbt">The IShaderBindingTable, do not store a copy of this.</param>
        /// <param name="tlas">The ITopLevelAS, do not store a copy of this.</param>
        void Bind(IShaderBindingTable sbt, ITopLevelAS tlas);
    }

    public class CallbackShaderTableBinder : IShaderTableBinder
    {
        Action<IShaderBindingTable, ITopLevelAS> action;

        public CallbackShaderTableBinder(Action<IShaderBindingTable, ITopLevelAS> action)
        {
            this.action = action;
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            this.action(sbt, tlas);
        }
    }

    public class RTInstances
    {
        List<TLASBuildInstanceData> instances = new List<TLASBuildInstanceData>();
        List<IShaderTableBinder> shaderTableBinders = new List<IShaderTableBinder>();
        List<IShaderResourceBinder> shaderResourceBinders = new List<IShaderResourceBinder>();

        public void AddTlasBuild(TLASBuildInstanceData instance)
        {
            instances.Add(instance);
        }

        public void RemoveTlasBuild(TLASBuildInstanceData instance)
        {
            instances.Remove(instance);
        }

        public void AddShaderTableBinder(IShaderTableBinder shaderTableBinder)
        {
            shaderTableBinders.Add(shaderTableBinder);
        }

        public void RemoveShaderTableBinder(IShaderTableBinder shaderTableBinder)
        {
            shaderTableBinders.Remove(shaderTableBinder);
        }

        public void AddShaderResourceBinder(IShaderResourceBinder shaderTableBinder)
        {
            shaderResourceBinders.Add(shaderTableBinder);
        }

        public void RemoveShaderResourceBinder(IShaderResourceBinder shaderTableBinder)
        {
            shaderResourceBinders.Remove(shaderTableBinder);
        }

        public List<TLASBuildInstanceData> Instances => instances;

        public void BindShaders(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            foreach(var i in shaderTableBinders)
            {
                i.Bind(sbt, tlas);
            }
        }

        public void BindShaderResources(IShaderResourceBinding rayTracingSRB)
        {
            foreach (var i in shaderResourceBinders)
            {
                i.Bind(rayTracingSRB);
            }
        }
    }
}
