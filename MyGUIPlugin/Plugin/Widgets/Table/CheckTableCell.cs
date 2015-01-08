using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class CheckTableCell : TableCell
    {
        private CheckButton checkButton; //Instead of swapping actual widgets we just change what events we listen to on this object. This way we check the button on a single click.
        private bool value = false;
        private bool firingLostFocus = false;

        public CheckTableCell()
        {

        }

        public override void Dispose()
        {
            if (checkButton != null)
            {
                Gui.Instance.destroyWidget(checkButton.Button);
                checkButton = null;
            }
        }

        public override void setStaticMode()
        {
            ensureWidgetExists(Table.TableWidget);
            checkButton.Button.KeyButtonReleased -= checkButton_editMode_KeyButtonReleased;
            checkButton.Button.MouseLostFocus -= checkButton_editMode_MouseLostFocus;
            checkButton.Button.MouseButtonClick += checkButton_staticMode_MouseButtonClick;
        }

        public override void setEditMode()
        {
            ensureWidgetExists(Table.TableWidget);
            checkButton.Button.KeyButtonReleased += checkButton_editMode_KeyButtonReleased;
            checkButton.Button.MouseLostFocus += checkButton_editMode_MouseLostFocus;
            checkButton.Button.MouseButtonClick -= checkButton_staticMode_MouseButtonClick;
        }

        public override TableCell clone()
        {
            return new CheckTableCell();
        }

        protected override void sizeChanged()
        {
            if (checkButton != null)
            {
                checkButton.Button.setSize(Size.Width, Size.Height);
            }
        }

        protected override void positionChanged()
        {
            if (checkButton != null)
            {
                checkButton.Button.setPosition(Position.x, Position.y);
            }
        }

        protected override Object EditValueImpl
        {
            get
            {
                if (checkButton != null)
                {
                    return checkButton.Checked;
                }
                else
                {
                    return value;
                }
            }
        }

        protected override void commitEditValueToValueImpl()
        {
            Value = checkButton.Checked;
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
                    if (checkButton != null)
                    {
                        checkButton.Checked = sentValue;
                    }
                    fireCellValueChanged();
                }
            }
        }

        private void ensureWidgetExists(Widget parentWidget)
        {
            if (checkButton == null)
            {
                checkButton = new CheckButton(parentWidget.createWidgetT("Button", "TableCellCheckBox", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "") as Button);
                checkButton.Checked = value;
            }
        }

        void checkButton_editMode_MouseLostFocus(Widget source, EventArgs e)
        {
            //This fires twice from mygui during this process so block it
            if (!firingLostFocus)
            {
                firingLostFocus = true;
                clearCellEdit();
                firingLostFocus = false;
            }
        }

        void checkButton_editMode_KeyButtonReleased(Widget source, EventArgs e)
        {
            KeyEventArgs ke = (KeyEventArgs)e;
            if (ke.Key == Engine.Platform.KeyboardButtonCode.KC_RETURN)
            {
                clearCellEdit();
            }
        }

        void checkButton_staticMode_MouseButtonClick(Widget source, EventArgs e)
        {
            if (!Table.Columns[ColumnIndex].ReadOnly)
            {
                requestCellEdit();
            }
        }
    }
}
