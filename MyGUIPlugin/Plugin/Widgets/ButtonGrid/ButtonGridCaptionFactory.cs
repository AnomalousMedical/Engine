using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    public abstract class ButtonGridCaptionFactory
    {
        protected internal abstract ButtonGridCaption createCaption(String name);

        protected internal abstract void destroyCaption(ButtonGridCaption caption);

        internal void setButtonGrid(ButtonGrid grid)
        {
            this.Grid = grid;
        }

        protected ButtonGrid Grid { get; private set; }
    }
}
