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
        private int index = CurrentIndex++;

        public Thing(IDestructionRequest destructionRequest, ILogger<Thing> logger)
        {
            logger.LogInformation($"Created Thing {index}");

            this.destructionRequest = destructionRequest;
            this.logger = logger;
        }

        public void Dispose()
        {
            logger.LogInformation($"Disposed Thing {index}");
        }

        public void RequestDestruction()
        {
            this.destructionRequest.RequestDestruction();
        }
    }
}
