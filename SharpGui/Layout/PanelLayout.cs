using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGui
{
    public struct PanelLayout
    {
        public SharpPanel Panel;
        public ILayoutItem Child;

        public PanelLayout(SharpPanel panel, ILayoutItem child)
        {
            this.Panel = panel;
            this.Child = child;
        }

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            var size = Child.GetDesiredSize(sharpGui);
            this.Panel.CalcDesiredSize = size;
            size = sharpGui.MeasurePanel(Panel);
            return size;
        }

        public void SetRect(IntRect rect)
        {
            Panel.Rect = rect;

            var panelPadding = Panel.CalcIntPad;

            Child.SetRect(new IntRect(
                rect.Left + panelPadding.Left,
                rect.Top + panelPadding.Top,
                rect.Width - panelPadding.Left - panelPadding.Right,
                rect.Height - panelPadding.Top - panelPadding.Bottom
                ));
        }
    }
}
