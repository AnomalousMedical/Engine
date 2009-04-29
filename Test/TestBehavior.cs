using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Engine.Attributes;
using Engine.Editing;

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

            MessageEvent testInput2 = new MessageEvent(TestEvents.Back);
            testInput2.addButton(KeyboardButtonCode.KC_S);
            DefaultEvents.registerDefaultEvent(testInput2);
        }

        [Editable]
        private String testVar = "foobar";

        [Editable]
        [DoNotSave]
        private Vector3 testDoNotSave = Vector3.UnitX;

        protected override void constructed()
        {
            System.Console.WriteLine("Constructed " + testVar);
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
                updateTranslation(ref newPos);
            }
            if (eventManager[TestEvents.Back].Down)
            {
                throw new Exception("Test Exception", new Exception("Inner"));
            }

        }
    }
}
