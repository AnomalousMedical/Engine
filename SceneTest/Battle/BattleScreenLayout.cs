using Engine;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class ScreenColumn
    {
        private ILayoutItem layout;
        private ColumnLayout column;

        public ScreenColumn(ColumnLayout column, ILayoutItem layout)
        {
            this.layout = layout;
            this.column = column;
        }

        public void Add(ILayoutItem item)
        {
            column.Add(item);
        }

        public void Remove(ILayoutItem item)
        {
            column.Remove(item);
        }

        public IntSize2 GetDesiredSize(ISharpGui sharpGui)
        {
            return layout.GetDesiredSize(sharpGui);
        }

        public void SetRect(IntRect rect)
        {
            layout.SetRect(rect);
        }
    }

    class BattleScreenLayout : IBattleScreenLayout
    {
        private readonly IScreenPositioner screenPositioner;
        private readonly IScaleHelper scaleHelper;
        private readonly ISharpGui sharpGui;

        private ILayoutItem battleMenuLayout;
        private ColumnLayout battleMenuColumn;

        private RowLayout infoRow;

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

            var progressColumn = new ColumnLayout() { Margin = new IntPad(10) };
            var progressColumnLayout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                progressColumn
                ));
            ProgressColumn = new ScreenColumn(progressColumn, progressColumnLayout);

            var mpColumn = new ColumnLayout() { Margin = new IntPad(10) };
            var mpColumnLayout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                mpColumn
                ));
            MpColumn = new ScreenColumn(mpColumn, mpColumnLayout);

            var hpColumn = new ColumnLayout() { Margin = new IntPad(10) };
            var hpColumnLayout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                hpColumn
                ));
            HpColumn = new ScreenColumn(hpColumn, hpColumnLayout);

            var nameColumn = new ColumnLayout() { Margin = new IntPad(10) };
            var nameColumnLayout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                nameColumn
                ));
            NameColumn = new ScreenColumn(nameColumn, nameColumnLayout);

            infoRow = new RowLayout(nameColumn, hpColumn, mpColumn, progressColumn);
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
            //var desiredSize = infoRow.GetDesiredSize(sharpGui);
            //infoRow.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            var desiredSize = ProgressColumn.GetDesiredSize(sharpGui);
            ProgressColumn.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            //desiredSize = HpColumn.GetDesiredSize(sharpGui);
            //HpColumn.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            //desiredSize = MpColumn.GetDesiredSize(sharpGui);
            //MpColumn.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

            //desiredSize = NameColumn.GetDesiredSize(sharpGui);
            //NameColumn.SetRect(screenPositioner.GetBottomRightRect(desiredSize));
        }

        public ScreenColumn ProgressColumn { get; }

        public ScreenColumn HpColumn { get; }

        public ScreenColumn MpColumn { get; }

        public ScreenColumn NameColumn { get; }
    }
}
