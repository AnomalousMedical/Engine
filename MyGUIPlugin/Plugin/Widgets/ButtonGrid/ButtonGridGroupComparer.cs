using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public delegate int CompareButtonGroupUserObjects(Object x, Object y);

    class ButtonGridGroupComparer : IComparer<ButtonGridGroup>
    {
        CompareButtonGroupUserObjects compareFunc;

        public ButtonGridGroupComparer(CompareButtonGroupUserObjects compareFunc)
        {
            this.compareFunc = compareFunc;
        }

        public int Compare(ButtonGridGroup x, ButtonGridGroup y)
        {
            return compareFunc.Invoke(x.UserObject, y.UserObject);
        }
    }
}
