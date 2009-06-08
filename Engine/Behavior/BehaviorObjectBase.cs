using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;

namespace Engine
{
    /// <summary>
    /// All objects that are used by behaviors as helper objects must extend
    /// this interface or the BehaviorObject class. This allows them to provide
    /// the additional services that are required to edit and save them. Any
    /// complex object that needs to be saved or edited will not be able to do
    /// so unless the object implements this interface (or the BehaviorObject
    /// class) unless it is an EngineMath member or a basic type (e.g. int,
    /// float etc.).
    /// </summary>
    public interface BehaviorObjectBase : EditInterfaceOverride, Saveable
    {
    }
}
