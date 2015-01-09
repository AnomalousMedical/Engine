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
        private Button staticWidget;
        private EnumType value = default(EnumType);
        private long editedValue;

        public EnumFlagsTableCell()
        {

        }

        public override void Dispose()
        {
            if (staticWidget != null)
            {
                Gui.Instance.destroyWidget(staticWidget);
                staticWidget = null;
            }
        }

        public override void setStaticMode()
        {
            ensureStaticWidgetExists(TableWidget);
        }

        public override void setEditMode()
        {
            FlagsEnumEditor flagsEditor = new FlagsEnumEditor(typeof(EnumType), Convert.ToInt64(value));
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

        protected override void setAppearSelected(bool selected)
        {
            if (staticWidget != null)
            {
                staticWidget.Selected = selected;
            }
        }

        public override TableCell clone()
        {
            return new EditTableCell();
        }

        protected override void sizeChanged()
        {
            if (staticWidget != null)
            {
                staticWidget.setSize(Size.Width, Size.Height);
            }
        }

        protected override void positionChanged()
        {
            if (staticWidget != null)
            {
                staticWidget.setPosition(Position.x, Position.y);
            }
        }

        protected override Object EditValueImpl
        {
            get
            {
                return editedValue;
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
                        sentValue = (EnumType)Enum.ToObject(typeof(EnumType), value);
                    }
                    if (value is String)
                    {
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
                        sentValue = (EnumType)value;
                    }
                }

                if (!this.value.Equals(sentValue))
                {
                    this.value = sentValue;
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
                staticWidget = (Button)parentWidget.createWidgetT("Button", "StaticTableCell", Position.x, Position.y, Size.Width, Size.Height, Align.Default, "");
                staticWidget.MouseButtonClick += new MyGUIEvent(staticWidget_MouseButtonClick);
                staticWidget.Caption = value.ToString();
                staticWidget.TextAlign = Align.Left | Align.VCenter;
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
