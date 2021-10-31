using SharpGui;

namespace SceneTest.Battle
{
    interface IBattleScreenLayout
    {
        ColumnLayout InfoColumn { get; }

        void LayoutBattleMenu(params ILayoutItem[] items);
        void LayoutCommonItems();
    }
}