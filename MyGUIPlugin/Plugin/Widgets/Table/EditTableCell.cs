using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    class EditTableCell : TableCell
    {
        private Edit editWidget;
        private Widget staticWidget;
        private String value = null;

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
            if (staticWidget != null)
            {
                Gui.Instance.destroyWidget(staticWidget);
                staticWidget = null;
            }
        }

        public override void setStaticMode()
        {
            ensureStaticWidgetExists(Table.TableWidget);
            if (!staticWidget.Visible)
            {
                staticWidget.Visible = true;
                if (editWidget != null)
                {
                    editWidget.Visible = false;
                }
            }
        }

        public override void setEditMode()
        {
            ensureEditWidgetExists(Table.TableWidget);
            if (!editWidget.Visible)
            {
                editWidget.Visible = true;
                if (staticWidget != null)
                {
                    staticWidget.Visible = false;
                }
                InputManager.Instance.setKeyFocusWidget(editWidget);
            }
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
            if (staticWidget != null)
            {
                staticWidget.setSize(Size.Width, Size.Height);
            }
        }

        protected override void positionChanged()
        {
            if (editWidget != null)
            {
                editWidget.setPosition(Position.x, Position.y);
            }
            if (staticWidget != null)
            {
                staticWidget.setPosition(Position.x, Position.y);
            }
        }

        protected override Object EditValueImpl
        {
            get
            {
                if (editWidget != null)
                {
                    return editWidget.Caption;
                }
                else
                {
                    return value;
                }
            }
        }

        protected override void commitEditValueToValueImpl()
        {
            Value = editWidget.Caption;
        }

        public override object Value
        {
            get
            {
                return value;
            }
            set
            {
                String sentValueString = value.ToString();
                if (this.value != sentValueString)
                {
                    this.value = sentValueString;
                    if (editWidget != null)
                    {
                        editWidget.Caption = sentValueString;
                    }
                    if (staticWidget != null)
                    {
                        staticWidget.Caption = sentValueString;
                    }
                    fireCellValueChanged();
                }
            }
        }

        private void ensureStaticWidgetExists(Widget parentWidget)
        {
            if (staticWidget == null)
            {
                staticWidget = parentWidget.createWidgetT("Button", "Button", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "");
                staticWidget.MouseButtonClick += new MyGUIEvent(staticWidget_MouseButtonClick);
                staticWidget.Caption = value;
                staticWidget.Visible = false;
            }
        }

        private void ensureEditWidgetExists(Widget parentWidget)
        {
            if (editWidget == null)
            {
                editWidget = parentWidget.createWidgetT("Edit", "Edit", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "") as Edit;
                editWidget.KeyLostFocus += new MyGUIEvent(editWidget_KeyLostFocus);
                editWidget.Caption = value;
                editWidget.Visible = false;
            }
        }

        void editWidget_KeyLostFocus(Widget source, EventArgs e)
        {
            clearCellEdit();
        }

        void staticWidget_MouseButtonClick(Widget source, EventArgs e)
        {
            requestCellEdit();
        }
    }
}
