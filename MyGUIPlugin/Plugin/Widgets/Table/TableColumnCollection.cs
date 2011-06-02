using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class TableColumnCollection : TableElementCollection<TableColumn>
    {
        public override void add(TableColumn item)
        {
            base.add(item);
            item.createWidget();
        }

        public override void remove(TableColumn item)
        {
            item.destroyWidget();
            base.remove(item);
        }
    }
}
