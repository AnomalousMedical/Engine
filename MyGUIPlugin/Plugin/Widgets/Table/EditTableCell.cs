using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    class EditTableCell : TableCell
    {
        private Edit editWidget;

        public EditTableCell()
        {

        }

        public override void Dispose()
        {
            if (editWidget != null)
            {
                Gui.Instance.destroyWidget(editWidget);
                editWidget = null;
            }
        }

        public override void setStaticMode(Widget table)
        {
            ensureWidgetExists(table);
            editWidget.EditStatic = true;
        }

        public override void setDynamicMode(Widget table)
        {
            ensureWidgetExists(table);
            editWidget.EditStatic = false;
        }

        public override TableCell clone()
        {
            return new EditTableCell();
        }

        protected override void sizeChanged()
        {
            if (editWidget != null)
            {
                editWidget.setSize(Size.Width, Size.Height);
            }
        }

        protected override void positionChanged()
        {
            if (editWidget != null)
            {
                editWidget.setPosition(Position.x, Position.y);
            }
        }

        public override object Value
        {
            get
            {
                return editWidget.Caption;
            }
            set
            {
                editWidget.Caption = value.ToString();
            }
        }

        private void ensureWidgetExists(Widget parentWidget)
        {
            if (editWidget == null)
            {
                editWidget = parentWidget.createWidgetT("Edit", "Edit", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "") as Edit;
            }
        }
    }
}
