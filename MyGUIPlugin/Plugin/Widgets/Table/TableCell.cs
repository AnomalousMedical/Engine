using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public abstract class TableCell : TableElement
    {
        private IntSize2 size;
        private IntVector2 position;

        public abstract void setStaticMode(Widget table);

        public abstract void setDynamicMode(Widget table);

        public abstract TableCell clone();

        protected abstract void sizeChanged();

        protected abstract void positionChanged();

        public abstract Object Value { get; set; }

        public IntSize2 Size
        {
            get
            {
                return size;
            }
            set
            {
                if (size != value)
                {
                    size = value;
                    sizeChanged();
                }
            }
        }

        public IntVector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                if (position != value)
                {
                    position = value;
                    positionChanged();
                }
            }
        }
    }
}
