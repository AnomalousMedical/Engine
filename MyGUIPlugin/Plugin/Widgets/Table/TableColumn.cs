using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class TableColumn : TableElement
    {
        private TableCell cellPrototype;
        protected Widget columnWidget; 
        private IntSize2 size;
        private IntVector2 position;

        public TableColumn(String name)
            :this(name, new EditTableCell())
        {

        }

        public TableColumn(String name, TableCell cellPrototype)
        {
            this.cellPrototype = cellPrototype;
            this.Name = name;
            this.Width = 100;
            this.ReadOnly = false;
        }

        public override void Dispose()
        {
            cellPrototype.Dispose();
            destroyWidget();
        }

        public TableCell createCell()
        {
            return cellPrototype.clone();
        }

        public virtual void createWidget()
        {
            if (columnWidget == null)
            {
                columnWidget = Table.TableWidget.createWidgetT("StaticText", "StaticText", position.x, position.y, size.Width, size.Height, Align.Default, "");
                columnWidget.Caption = Name;
            }
        }

        public void destroyWidget()
        {
            if (columnWidget != null)
            {
                Gui.Instance.destroyWidget(columnWidget);
                columnWidget = null;
            }
        }

        public String Name { get; set; }

        public int Width { get; set; }

        public bool ReadOnly { get; set; }

        public int ColumnIndex
        {
            get
            {
                return Table.Columns.getItemIndex(this);
            }
        }

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

        protected virtual void sizeChanged()
        {
            if (columnWidget != null)
            {
                columnWidget.setSize(Size.Width, Size.Height);
            }
        }

        protected virtual void positionChanged()
        {
            if (columnWidget != null)
            {
                columnWidget.setPosition(Position.x, Position.y);
            }
        }
    }
}
