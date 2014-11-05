using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class LateLinkEntry
    {
        private Behavior behavior;
        private Action action;

        public LateLinkEntry(Behavior behavior, Action action)
        {
            this.behavior = behavior;
            this.action = action;
        }

        public void execute()
        {
            try
            {
                action.Invoke();
            }
            catch (BehaviorBlacklistException bbe)
            {
                SimObjectErrorManager.AddError(BehaviorFactory.createError(bbe));
            }
            catch (Exception e)
            {
                behavior._unanticipatedBlacklistError(e);
                SimObjectErrorManager.AddError(BehaviorFactory.createError(behavior, e));
            }
        }
    }
}
