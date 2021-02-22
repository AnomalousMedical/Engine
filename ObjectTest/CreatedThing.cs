using Engine;
using Microsoft.Extensions.Logging;
using System;

namespace ObjectTest
{
    class CreatedThing : IDisposable
    {
        private static int CurrentIndex = 0;

        private readonly ILogger<Thing> logger;
        private int index = CurrentIndex++;

        public CreatedThing(ILogger<Thing> logger)
        {
            logger.LogInformation($"Created CreatedThing {index}");

            this.logger = logger;
        }

        public void Dispose()
        {
            logger.LogInformation($"Disposed CreatedThing {index}");
        }
    }
}
