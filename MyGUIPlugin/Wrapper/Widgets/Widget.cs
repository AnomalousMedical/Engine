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

        private IntPtr widget;
        private MyGUIEventManager eventManager;

        protected Widget(IntPtr widget)
        {
            this.widget = widget;
            eventManager = new MyGUIEventManager(this);
        }

        public void Dispose()
        {
            eventManager.Dispose();
            widget = IntPtr.Zero;
        }

        internal IntPtr WidgetPtr
        {
            get
            {
                return widget;
            }
        }

#region Events

        public event MyGUIEvent MouseButtonClick
        {
            add
            {
                eventManager.addDelegate<ClickEventTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<ClickEventTranslator>(value);
            }
        }

#endregion
    }
}
