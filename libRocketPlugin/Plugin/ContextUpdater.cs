using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace libRocketPlugin
{
    class ContextUpdater : UpdateListener, IDisposable
    {
        private Context context;
        private EventManager eventManager;
        private float lastMouseWheel = 0;

        public ContextUpdater(Context context, EventManager eventManager)
        {
            this.context = context;
            this.eventManager = eventManager;
            eventManager.Mouse.ButtonDown += Mouse_ButtonDown;
            eventManager.Mouse.ButtonUp += Mouse_ButtonUp;
            eventManager.Mouse.Moved += Mouse_Moved;
        }

        public void Dispose()
        {
            eventManager.Mouse.ButtonDown -= Mouse_ButtonDown;
            eventManager.Mouse.ButtonUp -= Mouse_ButtonUp;
            eventManager.Mouse.Moved -= Mouse_Moved;
        }

        public void sendUpdate(Clock clock)
        {
            //context.Update();
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {

        }

        void Mouse_Moved(Mouse mouse, MouseButtonCode buttonCode)
        {
            Vector3 absMouse = mouse.getAbsMouse();
            context.ProcessMouseMove((int)absMouse.x, (int)absMouse.y, 0);
            int wheel = (int)(lastMouseWheel - absMouse.z);
            if (wheel != 0)
            {
                context.ProcessMouseWheel(wheel / 120, 0);
            }
            lastMouseWheel = absMouse.z;
        }

        void Mouse_ButtonUp(Mouse mouse, MouseButtonCode buttonCode)
        {
            context.ProcessMouseButtonUp((int)buttonCode, 0);
        }

        void Mouse_ButtonDown(Mouse mouse, MouseButtonCode buttonCode)
        {
            context.ProcessMouseButtonDown((int)buttonCode, 0);
        }
    }
}
