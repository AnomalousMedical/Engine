using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncContextTest
{
    class AnnoyingThing
    {
        private readonly IScopedCoroutine coroutine;
        private readonly IDestructionRequest destructionRequest;
        private Guid guid = Guid.NewGuid();

        public AnnoyingThing(IScopedCoroutine coroutine, IDestructionRequest destructionRequest)
        {
            this.coroutine = coroutine;
            this.destructionRequest = destructionRequest;
            IEnumerator<YieldAction> run()
            {
                while (true)
                {
                    Console.WriteLine($"Annoying Thing {guid} {Thread.CurrentThread.ManagedThreadId}");
                    yield return coroutine.WaitSeconds(1);
                }
            }
            this.coroutine.Run(run());
        }

        public void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }
    }
}
