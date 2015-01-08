using Engine;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Editor
{
    class FlagsEnumEditor : Dialog
    {
        private StretchLayoutContainer flowLayout = new StretchLayoutContainer(StretchLayoutContainer.LayoutType.Vertical, ScaleHelper.Scaled(2), new IntVector2());
        private ScrollView scrollView;
        private List<Widget> childWidgets = new List<Widget>();
        private Type enumType;
        private long currentValue;

        public FlagsEnumEditor(Type enumType, long currentValue)
            : base("Anomalous.GuiFramework.Editor.GUI.FlagsEnumEditor.FlagsEnumEditor.layout")
        {
            this.enumType = enumType;
            this.currentValue = currentValue;

            scrollView = window.findWidget("Scroll") as ScrollView;

            foreach (var item in options())
            {
                Button button = scrollView.createWidgetT("Button", "CheckBox", 0, 0, scrollView.Width, ScaleHelper.Scaled(20), Align.Left | Align.Top, "") as Button;
                button.Caption = item.First;
                button.ForwardMouseWheelToParent = true;
                CheckButton checkButton = new CheckButton(button);
                checkButton.Checked = (currentValue & item.Second) != 0;
                checkButton.CheckedChanged += (sender, e) =>
                {
                    if (checkButton.Checked)
                    {
                        this.currentValue |= item.Second;
                    }
                    else
                    {
                        this.currentValue &= ~item.Second;
                    }
                };
                flowLayout.addChild(new MyGUILayoutContainer(button));
                childWidgets.Add(button);
            }

            var size = flowLayout.DesiredSize;
            size.Width = scrollView.Width;
            scrollView.CanvasSize = size;
            var viewCoord = scrollView.ViewCoord;
            size.Width = viewCoord.width - viewCoord.left;
            scrollView.CanvasSize = size;
            flowLayout.WorkingSize = size;
            flowLayout.layout();
        }

        public override void Dispose()
        {
            foreach (var widget in childWidgets)
            {
                Gui.Instance.destroyWidget(widget);
            }
            childWidgets.Clear();
            flowLayout.clearChildren();
            base.Dispose();
        }

        public long CurrentValue
        {
            get
            {
                return currentValue;
            }
        }

        private IEnumerable<Pair<String, long>> options()
        {
            return enumType.GetFields(BindingFlags.Public | BindingFlags.Static).Select(fieldInfo => new Pair<String, long>(fieldInfo.Name, Convert.ToInt64(fieldInfo.GetRawConstantValue())));
        }
    }
}
