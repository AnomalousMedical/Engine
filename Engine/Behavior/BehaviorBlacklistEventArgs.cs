using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class BehaviorBlacklistEventArgs
    {
        public BehaviorBlacklistEventArgs(String message, Behavior behavior, BehaviorManager behaviorManager)
        {
            this.Message = message;
            this.Behavior = behavior;
            this.BehaviorManager = behaviorManager;
        }

        public String Message { get; private set; }

        public Behavior Behavior { get; private set; }

        public BehaviorManager BehaviorManager { get; private set; }
    }
}
