using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class Widget : IDisposable
    {
        private static WrapperCollection<Widget> widgets = new WrapperCollection<Widget>(createWrapper);

        internal static Widget getWidget(IntPtr widget)
        {
            return widgets.getObject(widget);
        }

        internal static void destroyAllWrappers()
        {
            widgets.clearObjects();
        }

        private static Widget createWrapper(IntPtr widget, object[] args)
        {
            return new Widget(widget);
        }

        IntPtr widget;

        protected Widget(IntPtr widget)
        {
            this.widget = widget;
        }

        public void Dispose()
        {
            widget = IntPtr.Zero;
        }

        internal IntPtr WidgetPtr
        {
            get
            {
                return widget;
            }
        }
    }
}
