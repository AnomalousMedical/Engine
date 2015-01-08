using Engine;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Anomalous.GuiFramework.Editor
{
    public class EnumFlagsTableCell<EnumType> : TableCell
        where EnumType : struct
    {
        private ComboBox editWidget;
        private TextBox staticWidget;
        private EnumType value = default(EnumType);
        private long editedValue;

        public EnumFlagsTableCell()
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
            ensureStaticWidgetExists(TableWidget);
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
            //ensureEditWidgetExists(TableWidget);
            //if (!editWidget.Visible)
            //{
            //    editWidget.Visible = true;
            //    if (staticWidget != null)
            //    {
            //        staticWidget.Visible = false;
            //    }
            //    InputManager.Instance.setKeyFocusWidget(editWidget);
            //}
            long lval = Convert.ToInt64(Enum.Parse(typeof(EnumType), value.ToString()));
            FlagsEnumEditor flagsEditor = new FlagsEnumEditor(typeof(EnumType), lval);
            flagsEditor.Modal = true;
            flagsEditor.center();
            flagsEditor.Visible = true;
            flagsEditor.Closed += (sender, args) =>
                {
                    editedValue = flagsEditor.CurrentValue;
                    flagsEditor.Dispose();
                    clearCellEdit();
                };
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
                    return editWidget.SelectedItemData;
                }
                else
                {
                    return value;
                }
            }
        }

        protected override void commitEditValueToValueImpl()
        {
            Value = editedValue;
        }

        public override object Value
        {
            get
            {
                return value;
            }
            set
            {
                EnumType sentValue = default(EnumType);
                if (value != null)
                {
                    if(value is long)
                    {
                        //Going through a string is the safest way to make sure we don't unbox to the wrong enum underlying type and we can't cast directly.
                        //This isn't performance sensitive anyway so it doesn't really matter even though this is a bit crazy.
                        long parsed;
                        if (long.TryParse(value.ToString(), out parsed))
                        {
                            sentValue = (EnumType)Enum.Parse(typeof(EnumType), value.ToString());
                        }
                    }
                    if (value is String)
                    {
                        //If we are a String try to parse
                        try
                        {
                            sentValue = (EnumType)Enum.Parse(typeof(EnumType), value.ToString());
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (value is EnumType)
                    {
                        //If we are the direct type just use it.
                        sentValue = (EnumType)value;
                    }
                }

                if (!this.value.Equals(sentValue))
                {
                    this.value = sentValue;
                    if (editWidget != null)
                    {
                        editWidget.SelectedIndex = editWidget.findItemIndexWithData(sentValue);
                    }
                    if (staticWidget != null)
                    {
                        staticWidget.Caption = sentValue.ToString();
                    }
                    fireCellValueChanged();
                }
            }
        }

        private void ensureStaticWidgetExists(Widget parentWidget)
        {
            if (staticWidget == null)
            {
                staticWidget = (TextBox)parentWidget.createWidgetT("Button", "StaticTableCell", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "");
                staticWidget.MouseButtonClick += new MyGUIEvent(staticWidget_MouseButtonClick);
                staticWidget.Caption = value.ToString();
                staticWidget.TextAlign = Align.Left | Align.VCenter;
                staticWidget.Visible = false;
            }
        }

        private void ensureEditWidgetExists(Widget parentWidget)
        {
            if (editWidget == null)
            {
                editWidget = parentWidget.createWidgetT("ComboBox", "ComboBox", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "") as ComboBox;
                editWidget.EditStatic = true;
                editWidget.EventComboChangePosition += editWidget_EventComboChangePosition;
                editWidget.KeyButtonReleased += new MyGUIEvent(editWidget_KeyButtonReleased);
                foreach (var option in comboOptions())
                {
                    editWidget.addItem(option.First, option.Second);
                }
                editWidget.SelectedIndex = editWidget.findItemIndexWithData(value);
                editWidget.Visible = false;
            }
        }

        void editWidget_KeyButtonReleased(Widget source, EventArgs e)
        {
            KeyEventArgs ke = (KeyEventArgs)e;
            if (ke.Key == Engine.Platform.KeyboardButtonCode.KC_RETURN)
            {
                clearCellEdit();
            }
        }

        void editWidget_EventComboChangePosition(Widget source, EventArgs e)
        {
            clearCellEdit();
        }

        void staticWidget_MouseButtonClick(Widget source, EventArgs e)
        {
            if (!Table.Columns[ColumnIndex].ReadOnly)
            {
                requestCellEdit();
            }
        }

        private IEnumerable<Pair<String, Object>> comboOptions()
        {
            return typeof(EnumType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(fieldInfo => new Pair<String, Object>(fieldInfo.Name, Enum.ToObject(typeof(EnumType), fieldInfo.GetRawConstantValue())));
        }
    }
}
