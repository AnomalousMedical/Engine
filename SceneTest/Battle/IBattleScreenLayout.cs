using SharpGui;

namespace SceneTest
{
    interface IBattleScreenLayout
    {
        ColumnLayout InfoColumn { get; }

        void LayoutBattleMenu(params ILayoutItem[] items);
        void LayoutCommonItems();
    }
}