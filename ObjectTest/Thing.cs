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
        private readonly CreatedThing createdThing;

        public Thing(IDestructionRequest destructionRequest, ILogger<Thing> logger, InjectedThing injectedThing)
        {
            logger.LogInformation($"Created Thing {index}");

            this.destructionRequest = destructionRequest;
            this.logger = logger;
            this.injectedThing = injectedThing; //Will not need to be dispose since part of the resolved scope

            this.createdThing = new CreatedThing(logger); //Will need to be disposed since it was created here.
                                                          //If this was created with a factory method same deal.
                                                          //Created here destroyed here.
        }

        public void Dispose()
        {
            //Don't have to dispose injectedThing since it was created as part of this object's scope.

            this.createdThing.Dispose(); //Have to dispose created thing, since it was created here

            logger.LogInformation($"Disposed Thing {index}");
        }

        public void RequestDestruction()
        {
            this.destructionRequest.RequestDestruction();
        }
    }
}
