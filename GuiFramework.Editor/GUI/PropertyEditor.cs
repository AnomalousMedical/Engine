using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Anomalous.GuiFramework.Editor
{
    public interface PropertyEditor
    {
        EditInterface EditInterface { get; set; }
    }
}
