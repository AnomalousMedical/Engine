using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPlugin.Characters;
using BepuPlugin.Demo;
using BepuUtilities.Memory;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BepuPlugin
{
    class BepuScene : IDisposable, IBepuScene
    {
        Simulation simulation;
        SimpleThreadDispatcher threadDispatcher;
        BufferPool bufferPool;
        CharacterControllers characters;
        private readonly EventManager eventManager;
        CharacterInput character;

        public BepuScene(EventManager eventManager)
        {
            //The buffer pool is a source of raw memory blobs for the engine to use.
            bufferPool = new BufferPool();

            characters = new CharacterControllers(bufferPool);

            //The PositionFirstTimestepper is the simplest timestepping mode, but since it integrates velocity into position at the start of the frame, directly modified velocities outside of the timestep
            //will be integrated before collision detection or the solver has a chance to intervene. That's fine in this demo. Other built-in options include the PositionLastTimestepper and the SubsteppingTimestepper.
            //Note that the timestepper also has callbacks that you can use for executing logic between processing stages, like BeforeCollisionDetection.
            simulation = Simulation.Create(bufferPool, new CharacterNarrowphaseCallbacks(characters), new DemoPoseIntegratorCallbacks(new Vector3(0, -10, 0)), new PositionFirstTimestepper());

            //Taking off 1 thread could help stability https://github.com/bepu/bepuphysics2/blob/master/Documentation/PerformanceTips.md#general
            var numThreads = Math.Max(Environment.ProcessorCount - 1, 1);
            threadDispatcher = new SimpleThreadDispatcher(numThreads);
            this.eventManager = eventManager;
        }

        public void Dispose()
        {
            //If you intend to reuse the BufferPool, disposing the simulation is a good idea- it returns all the buffers to the pool for reuse.
            //Here, we dispose it, but it's not really required; we immediately thereafter clear the BufferPool of all held memory.
            //Note that failing to dispose buffer pools can result in memory leaks.
            simulation.Dispose();
            threadDispatcher.Dispose();
            characters.Dispose();
            bufferPool.Clear();
        }

        public void Update(Clock clock, Vector3 cameraForward)
        {
            var timestep = clock.DeltaSeconds; //NEED to make this fixed.

            character?.UpdateCharacterGoals(cameraForward, timestep);
            simulation.Timestep(timestep, threadDispatcher);
        }

        public CharacterInput CreateCharacter(Vector3 position)
        {
            character = new CharacterInput(eventManager, characters, position, new Capsule(0.5f, 1), 0.1f, 1, 20, 100, 6, 4, MathF.PI * 0.4f);
            return character;
        }

        public void DestroyCharacter(CharacterInput character)
        {
            character.Dispose();
        }

        public Simulation Simulation => simulation;
    }
}
