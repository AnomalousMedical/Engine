using System;
using System.Collections.Generic;
using System.Text;

namespace DiligentEngineGenerator
{
    class StructCppWriterContext
    {
        public List<String> DeleteStatements { get; set; } = new List<string>();

        public bool HasDeletes => DeleteStatements.Count > 0;
    }
}
