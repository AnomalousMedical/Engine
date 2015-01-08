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
