using Anomalous.OSPlatform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMinimalApplication
{
    class TimerApp : App
    {
        private Stopwatch sw = new Stopwatch();
        private long count = 0;

        public override bool OnInit()
        {
            sw.Start();
            return true;
        }

        public override int OnExit()
        {
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            return 0;
        }

        public override void OnIdle()
        {
            if(count++ > 100000000)
            {
                this.exit();
            }
        }
    }
}
