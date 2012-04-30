using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace libRocketPlugin
{
    public interface RocketFileSystemExtension
    {
        bool canOpenFile(String file);

        Stream openFile(String file);
    }
}
