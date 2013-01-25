using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public interface ButtonGridSelectionStrategy
    {
        void itemChosen(ButtonGridItem item);

        void itemRemoved(ButtonGridItem item);

        void itemsCleared();
    }
}
