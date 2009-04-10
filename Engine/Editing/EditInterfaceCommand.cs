using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public delegate void EditInterfaceFunction(EditUICallback callback, EditInterfaceCommand caller);

    public class EditInterfaceCommand
    {
        private EditInterfaceFunction function;
        private String name;

        public EditInterfaceCommand(String name, EditInterfaceFunction function)
        {
            this.name = name;
            this.function = function;
        }

        public void execute(EditUICallback callback)
        {
            function.Invoke(callback, this);
        }

        public String Name
        {
            get
            {
                return name;
            }
        }
    }
}
