using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using EngineMath;

namespace Test
{
    class TestBehavior : Behavior
    {
        private enum TestEvents
        {
            Forward,
            Back,
            Left,
            Right,
        }

        static TestBehavior()
        {
            MessageEvent testInput = new MessageEvent(TestEvents.Forward);
            testInput.addButton(KeyboardButtonCode.KC_W);
            DefaultEvents.registerDefaultEvent(testInput);
        }

        protected override void constructed()
        {
            System.Console.WriteLine("Constructed");
        }

        protected override void destroy()
        {
            System.Console.WriteLine("Destroyed");
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            if (eventManager[TestEvents.Forward].Down)
            {
                Vector3 newPos = SimObject.Translation + Vector3.Forward * (float)(5.0f * clock.Seconds);
                SimObject.updateTranslation(ref newPos, this);
            }

        }
    }
}
