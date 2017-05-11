using Engine.Shim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSPlatform.Win32
{
    class TimerShim : Engine.Shim.Timer
    {
        private System.Timers.Timer timer;

        public TimerShim(System.Timers.Timer timer)
        {
            this.timer = timer;
        }

        public bool AutoReset
        {
            get
            {
                return timer.AutoReset;
            }
            set
            {
                timer.AutoReset = value;
            }
        }

        private Dictionary<Action<object, ElapsedEventArgs>, System.Timers.ElapsedEventHandler> eventHandlers = new Dictionary<Action<object, ElapsedEventArgs>, System.Timers.ElapsedEventHandler>();
        public event Action<object, ElapsedEventArgs> Elapsed
        {
            add
            {
                if (!eventHandlers.ContainsKey(value))
                {
                    var handler = new System.Timers.ElapsedEventHandler((sender, e) =>
                     {
                         value.Invoke(sender, new ElapsedEventArgs(e.SignalTime));
                     });
                    eventHandlers.Add(value, handler);
                    timer.Elapsed += handler;
                }
            }
            remove
            {
                System.Timers.ElapsedEventHandler handler;
                if(eventHandlers.TryGetValue(value, out handler))
                {
                    eventHandlers.Remove(value);
                    timer.Elapsed -= handler;
                }
            }
        }

        public void Dispose()
        {
            timer.Dispose();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
