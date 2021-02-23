using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncContextTest
{
    class SyncThing
    {
        private readonly IScopedCoroutine coroutine;

        public SyncThing(IScopedCoroutine coroutine)
        {
            this.coroutine = coroutine;

            IEnumerator<YieldAction> run()
            {
                Console.WriteLine("Outside SyncThing async begin");
                yield return coroutine.Await(async () =>
                {
                    Console.WriteLine("Inside SyncThing async");
                    await Task.Delay(1000);
                    Console.WriteLine("Inside SyncThing async");
                    await Task.Delay(1000);
                    Console.WriteLine("Inside SyncThing async");
                    await Task.Delay(1000);
                    Console.WriteLine("Inside SyncThing async");
                    await Task.Delay(1000);
                });
                Console.WriteLine("Outside SyncThing async end");
            }
            this.coroutine.Run(run());
        }
    }
}
