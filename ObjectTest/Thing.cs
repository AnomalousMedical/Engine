using Engine;
using Microsoft.Extensions.Logging;
using System;

namespace ObjectTest
{
    class Thing : IDisposable
    {
        private readonly IDestructionRequest destructionRequest;
        private readonly ILogger<Thing> logger;

        public Thing(IDestructionRequest destructionRequest, ILogger<Thing> logger)
        {
            logger.LogInformation("Created Thing");

            this.destructionRequest = destructionRequest;
            this.logger = logger;
        }

        public void Dispose()
        {
            logger.LogInformation("Disposed Thing");
        }

        public void RequestDestruction()
        {
            this.destructionRequest.RequestDestruction();
        }
    }
}
