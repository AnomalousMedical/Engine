using SharpGui;

namespace SceneTest
{
    interface IBattleScreenLayout
    {
        void AddProgressColumnItem(ILayoutItem item);
        void LayoutBattleMenu(params ILayoutItem[] items);
        void LayoutCommonItems();
        void RemoveProgressColumnItem(ILayoutItem item);
    }
}