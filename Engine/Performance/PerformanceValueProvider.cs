using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class PerformanceValueProvider
    {
        private Func<String> getValueFunc;

        public PerformanceValueProvider(String name, Func<String> getValueFunc)
        {
            this.Name = name;
            this.getValueFunc = getValueFunc;
        }

        public String Name { get; private set; }

        public String Value
        {
            get
            {
                return getValueFunc();
            }
        }
    }
}
