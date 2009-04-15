using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    public interface ValueWriter
    {
        void writeValue(SaveEntry entry);

        Type getWriteType();
    }
}
