using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    public class PbrOptions
    {
        public Action<PbrRendererCreateInfo> CustomizePbrOptions { get; set; }
    }
}
