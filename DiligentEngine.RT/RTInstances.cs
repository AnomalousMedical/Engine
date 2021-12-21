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
}
