using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Anomalous.GuiFramework.Editor
{
    public interface AddItemTemplate
    {
        String TypeName { get; }

        String ImageName { get; }

        String Group { get; }

        bool isValid(out String errorMessage);

        EditInterface EditInterface { get; }

        void reset();
    }
}
