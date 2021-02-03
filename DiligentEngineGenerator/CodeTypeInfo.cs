using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class CodeTypeInfo
    {
        public Dictionary<String, CodeEnum> Enums { get; set; } = new Dictionary<string, CodeEnum>();

        public Dictionary<String, CodeStruct> Structs { get; set; } = new Dictionary<string, CodeStruct>();

        public Dictionary<String, CodeInterface> Interfaces { get; set; } = new Dictionary<string, CodeInterface>();
    }
}
