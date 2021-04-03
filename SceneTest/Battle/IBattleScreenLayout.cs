using SharpGui;

namespace SceneTest
{
    interface IBattleScreenLayout
    {
        ScreenColumn HpColumn { get; }
        ScreenColumn MpColumn { get; }
        ScreenColumn NameColumn { get; }
        ScreenColumn ProgressColumn { get; }

        void LayoutBattleMenu(params ILayoutItem[] items);
        void LayoutCommonItems();
    }
}