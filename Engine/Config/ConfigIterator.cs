using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class ConfigIterator
    {
        private String baseName;
        private int current = 0;
        private ConfigSection section;

        public ConfigIterator(ConfigSection section, String baseName)
        {
            this.section = section;
            this.baseName = baseName;
        }

        public void reset()
        {
            current = 0;
        }

        public String next()
        {
            return section.getValue(baseName + current++, "");
        }

        public bool hasNext()
        {
            return section.hasValue(baseName + current);
        }
    }
}
