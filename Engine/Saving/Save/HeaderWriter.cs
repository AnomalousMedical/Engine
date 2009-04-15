using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    public interface HeaderWriter
    {
        void writeHeader(ObjectIdentifier objectId);
    }
}
