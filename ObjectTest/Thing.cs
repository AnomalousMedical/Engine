using Engine;
using Microsoft.Extensions.Logging;
using System;

namespace ObjectTest
{
    class Thing : IDisposable
    {
        private static int CurrentIndex = 0;

        private readonly IDestructionRequest destructionRequest;
        private readonly ILogger<Thing> logger;
        private readonly InjectedThing injectedThing;
        private int index = CurrentIndex++;

        public Thing(IDestructionRequest destructionRequest, ILogger<Thing> logger, InjectedThing injectedThing)
        {
            logger.LogInformation($"Created Thing {index}");

            this.destructionRequest = destructionRequest;
            this.logger = logger;
            this.injectedThing = injectedThing;
        }

        public void Dispose()
        {
            //Don't have to dispose injectedThing since it was created as part of this object's scope.
            logger.LogInformation($"Disposed Thing {index}");
        }

        public void RequestDestruction()
        {
            this.destructionRequest.RequestDestruction();
        }
    }
}
