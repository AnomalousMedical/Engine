using Engine;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle
{
    class BattleScreenLayout : IBattleScreenLayout
    {
        private readonly IScreenPositioner screenPositioner;
        private readonly IScaleHelper scaleHelper;
        private readonly ISharpGui sharpGui;

        private ILayoutItem battleMenuLayout;
        private ColumnLayout battleMenuColumn;

        private ILayoutItem infoColumnLayout;
        private ColumnLayout infoColumn;

        public BattleScreenLayout(
            IScreenPositioner screenPositioner,
            IScaleHelper scaleHelper,
            ISharpGui sharpGui
        )
        {
            this.screenPositioner = screenPositioner;
            this.scaleHelper = scaleHelper;
            this.sharpGui = sharpGui;
            battleMenuColumn = new ColumnLayout() { Margin = new IntPad(10) };
            battleMenuLayout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                battleMenuColumn
                ));

            infoColumn = new ColumnLayout() { Margin = new IntPad(10) };
            infoColumnLayout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                infoColumn
                );
        }

        public void LayoutBattleMenu(params ILayoutItem[] items)
        {
            LayoutBattleMenu((IEnumerable<ILayoutItem>)items);
        }

        public void LayoutBattleMenu(IEnumerable<ILayoutItem> items)
        {
            battleMenuColumn.Add(items);
            var desiredSize = battleMenuLayout.GetDesiredSize(sharpGui);
            var rect = screenPositioner.GetBottomRightRect(desiredSize);
            rect.Top -= infoColumnRect.Height;
            battleMenuLayout.SetRect(rect);
            battleMenuColumn.Clear();
        }

        private IntRect infoColumnRect;
        public void LayoutCommonItems()
        {
            var desiredSize = infoColumnLayout.GetDesiredSize(sharpGui);
            infoColumnRect = screenPositioner.GetBottomRightRect(desiredSize);
            infoColumnLayout.SetRect(infoColumnRect);
        }

        public ColumnLayout InfoColumn => infoColumn;
    }
}
