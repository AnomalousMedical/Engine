using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    public class StringListlikeEditInterface : ListlikeEditInterface<String>
    {
        public StringListlikeEditInterface(IList<String> list, String name)
            :base(list, name)
        {

        }

        public StringListlikeEditInterface(LinkedList<String> list, String name)
            : base(list, name)
        {

        }

        public override string createNew()
        {
            return "";
        }

        public override void removed(string value)
        {
            
        }

        public override string parseString(string value)
        {
            return value;
        }

        public override bool canParseString(string value, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }
    }
}
