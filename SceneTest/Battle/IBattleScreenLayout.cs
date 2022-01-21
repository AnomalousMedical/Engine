using SharpGui;
using System.Collections.Generic;

namespace SceneTest.Battle
{
    interface IBattleScreenLayout
    {
        ColumnLayout InfoColumn { get; }

        void LayoutBattleMenu(params ILayoutItem[] items);
        public void LayoutBattleMenu(IEnumerable<ILayoutItem> items);
        void LayoutCommonItems();
    }
}