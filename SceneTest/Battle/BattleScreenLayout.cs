using Engine;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
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
            battleMenuColumn.Add(items);
            var desiredSize = battleMenuLayout.GetDesiredSize(sharpGui);
            battleMenuLayout.SetRect(screenPositioner.GetBottomLeftRect(desiredSize));
            battleMenuColumn.Clear();
        }

        public void LayoutCommonItems()
        {
            var desiredSize = infoColumnLayout.GetDesiredSize(sharpGui);
            infoColumnLayout.SetRect(screenPositioner.GetBottomRightRect(desiredSize));
        }

        public ColumnLayout InfoColumn => infoColumn;
    }
}
