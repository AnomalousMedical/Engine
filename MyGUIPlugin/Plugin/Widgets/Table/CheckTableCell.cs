using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class CheckTableCell : TableCell
    {
        private CheckButton editWidget;
        private StaticText staticWidget;
        private bool value = false;

        public CheckTableCell()
        {

        }

        public override void Dispose()
        {
            if (editWidget != null)
            {
                Gui.Instance.destroyWidget(editWidget.Button);
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
                    editWidget.Button.Visible = false;
                }
            }
        }

        public override void setEditMode()
        {
            ensureEditWidgetExists(Table.TableWidget);
            if (!editWidget.Button.Visible)
            {
                editWidget.Button.Visible = true;
                if (staticWidget != null)
                {
                    staticWidget.Visible = false;
                }
            }
        }

        public override TableCell clone()
        {
            return new CheckTableCell();
        }

        protected override void sizeChanged()
        {
            if (editWidget != null)
            {
                editWidget.Button.setSize(Size.Width, Size.Height);
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
                editWidget.Button.setPosition(Position.x, Position.y);
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
                    return editWidget.Checked;
                }
                else
                {
                    return value;
                }
            }
        }

        protected override void commitEditValueToValueImpl()
        {
            Value = editWidget.Checked;
        }

        public override object Value
        {
            get
            {
                return value;
            }
            set
            {
                bool sentValue = false;
                if (value != null)
                {
                    if (value is bool)
                    {
                        sentValue = (bool)value;
                    }
                    else if (value is String)
                    {
                        bool.TryParse(value.ToString(), out sentValue);
                    }
                }
                if (this.value != sentValue)
                {
                    this.value = sentValue;
                    if (editWidget != null)
                    {
                        editWidget.Checked = sentValue;
                    }
                    if (staticWidget != null)
                    {
                        staticWidget.Caption = value != null ? value.ToString() : null;
                    }
                    fireCellValueChanged();
                }
            }
        }

        private void ensureStaticWidgetExists(Widget parentWidget)
        {
            if (staticWidget == null)
            {
                staticWidget = (StaticText)parentWidget.createWidgetT("Button", "Button", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "");
                staticWidget.MouseButtonClick += new MyGUIEvent(staticWidget_MouseButtonClick);
                staticWidget.Caption = value.ToString();
                staticWidget.Visible = false;
            }
        }

        private void ensureEditWidgetExists(Widget parentWidget)
        {
            if (editWidget == null)
            {
                editWidget = new CheckButton(parentWidget.createWidgetT("Button", "CheckBox", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "") as Button);
                editWidget.Button.KeyButtonReleased += new MyGUIEvent(editWidget_KeyButtonReleased);
                editWidget.Button.MouseLostFocus += new MyGUIEvent(Button_MouseLostFocus);
                editWidget.Checked = value;
                editWidget.Button.Visible = false;
            }
        }

        void Button_MouseLostFocus(Widget source, EventArgs e)
        {
            clearCellEdit();
        }

        void editWidget_KeyButtonReleased(Widget source, EventArgs e)
        {
            KeyEventArgs ke = (KeyEventArgs)e;
            if (ke.Key == Engine.Platform.KeyboardButtonCode.KC_RETURN)
            {
                clearCellEdit();
            }
        }

        void staticWidget_MouseButtonClick(Widget source, EventArgs e)
        {
            if (!Table.Columns[ColumnIndex].ReadOnly)
            {
                requestCellEdit();
            }
        }
    }
}
