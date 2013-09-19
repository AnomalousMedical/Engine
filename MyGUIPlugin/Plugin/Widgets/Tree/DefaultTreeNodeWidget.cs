using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    class DefaultTreeNodeWidget : TreeNodeWidget
    {
        private static int PrimaryWidgetWidth = ScaleHelper.Scaled(26);
        private static int PrimaryWidgetHeight = ScaleHelper.Scaled(14);
        private static int PlusMinusButtonSize = ScaleHelper.Scaled(14);
        private static int MainButtonX = ScaleHelper.Scaled(17);
        private static int MainButtonWidth = ScaleHelper.Scaled(10);
        private static int MainButtonHeight = ScaleHelper.Scaled(16);

        private Widget primaryWidget;
        private Button plusMinusButton;
        private Button mainButton;

        public DefaultTreeNodeWidget()
        {

        }

        public override void Dispose()
        {
            destroyWidget();
        }

        public override void createWidget(Widget parent, String caption, String imageResource)
        {
            primaryWidget = parent.createWidgetT("Widget", "PanelEmpty", 0, 0, PrimaryWidgetWidth, PrimaryWidgetHeight, Align.Default, "") as Widget;
            primaryWidget.ForwardMouseWheelToParent = true;

            plusMinusButton = primaryWidget.createWidgetT("Button", "ButtonExpandSkin", 0, 0, PlusMinusButtonSize, PlusMinusButtonSize, Align.Left | Align.HCenter, "") as Button;
            plusMinusButton.MouseButtonClick += new MyGUIEvent(plusMinusButton_MouseButtonClick);
            plusMinusButton.Visible = treeNode.HasChildren;
            plusMinusButton.ForwardMouseWheelToParent = true;

            mainButton = primaryWidget.createWidgetT("Button", "TreeIconButton", MainButtonX, 0, MainButtonWidth, MainButtonHeight, Align.Stretch, "") as Button;
            mainButton.Caption = caption;
            mainButton.MouseButtonClick += new MyGUIEvent(mainButton_MouseButtonClick);
            mainButton.MouseButtonDoubleClick += new MyGUIEvent(mainButton_MouseButtonDoubleClick);
            mainButton.MouseButtonReleased += new MyGUIEvent(mainButton_MouseButtonReleased);
            mainButton.MouseButtonPressed += new MyGUIEvent(mainButton_MouseButtonPressed);
            mainButton.Selected = treeNode.Selected;
            mainButton.ForwardMouseWheelToParent = true;
            ImageBox image = mainButton.ImageBox;
            if (image != null)
            {
                image.setItemResource(imageResource);
            }
        }

        public override void destroyWidget()
        {
            if (primaryWidget != null)
            {
                Gui.Instance.destroyWidget(primaryWidget);
                primaryWidget = null;
                plusMinusButton = null;
                mainButton = null;
            }
        }

        public override void setCoord(int left, int top, int width, int height)
        {
            primaryWidget.setCoord(left, top, width, height);
        }

        public override void updateExpandedStatus(bool expanded)
        {
            if (plusMinusButton != null)
            {
                plusMinusButton.Selected = !expanded;
                plusMinusButton.Visible = treeNode.HasChildren;
            }
        }

        public override void updateSelectionStatus(bool selected)
        {
            if (mainButton != null)
            {
                mainButton.Selected = selected;
            }
        }

        internal override void updateText()
        {
            if (mainButton != null)
            {
                mainButton.Caption = treeNode.Text;
            }
        }

        internal override void updateImageResource()
        {
            if (mainButton != null)
            {
                ImageBox image = mainButton.ImageBox;
                if (image != null)
                {
                    image.setItemResource(treeNode.ImageResource);
                }
            }
        }

        void plusMinusButton_MouseButtonClick(Widget source, EventArgs e)
        {
            fireExpandToggled();
        }

        void mainButton_MouseButtonClick(Widget source, EventArgs e)
        {
            fireNodeSelected();
        }

        void mainButton_MouseButtonPressed(Widget source, EventArgs e)
        {
            fireNodeMousePressed((MouseEventArgs)e);
        }

        void mainButton_MouseButtonReleased(Widget source, EventArgs e)
        {
            fireNodeMouseReleased((MouseEventArgs)e);
        }

        void mainButton_MouseButtonDoubleClick(Widget source, EventArgs e)
        {
            fireNodeMouseDoubleClicked();
        }
    }
}
