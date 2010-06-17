using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    class MyGUIEventManager : IDisposable
    {
        private Dictionary<Type, MyGUIEventTranslator> events = new Dictionary<Type, MyGUIEventTranslator>();
        private Widget widget;

        public MyGUIEventManager(Widget widget)
        {
            this.widget = widget;
        }

        public void Dispose()
        {
            foreach (MyGUIEventTranslator trans in events.Values)
            {
                trans.Dispose();
            }
        }

        public void addDelegate<T>(MyGUIEvent evt)
            where T : MyGUIEventTranslator, new()
        {
            MyGUIEventTranslator trans;
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
            MyGUIEventTranslator trans;
            events.TryGetValue(typeof(T), out trans);
            if(trans != null)
            {
                trans.BoundEvent -= evt;
            }
        }
    }
}
