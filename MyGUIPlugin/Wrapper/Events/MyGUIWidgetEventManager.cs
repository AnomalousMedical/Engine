using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class manages events on a widget.
    /// </summary>
    class MyGUIWidgetEventManager : IDisposable
    {
        private Dictionary<Type, MyGUIWidgetEventTranslator> events = new Dictionary<Type, MyGUIWidgetEventTranslator>();
        private Widget widget;

        public MyGUIWidgetEventManager(Widget widget)
        {
            this.widget = widget;
        }

        public void Dispose()
        {
            foreach (MyGUIWidgetEventTranslator trans in events.Values)
            {
                trans.Dispose();
            }
        }

        public void addDelegate<T>(MyGUIEvent evt)
            where T : MyGUIWidgetEventTranslator, new()
        {
            MyGUIWidgetEventTranslator trans;
            events.TryGetValue(typeof(T), out trans);
            if(trans == null)
            {
                trans = new T();
                trans.initialize(widget);
                events.Add(typeof(T), trans);
            }
            trans.BoundEvent += evt;
        }

        public void removeDelegate<T>(MyGUIEvent evt)
        {
            MyGUIWidgetEventTranslator trans;
            events.TryGetValue(typeof(T), out trans);
            if(trans != null)
            {
                trans.BoundEvent -= evt;
            }
        }
    }
}
