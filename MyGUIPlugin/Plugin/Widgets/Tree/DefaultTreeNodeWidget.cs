using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    class DefaultTreeNodeWidget : TreeNodeWidget
    {
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
            primaryWidget = parent.createWidgetT("Widget", "PanelEmpty", 0, 0, 26, 16, Align.Default, "") as Widget;

            plusMinusButton = primaryWidget.createWidgetT("Button", "ButtonMinusPlus", 0, 0, 16, 16, Align.Left | Align.HCenter, "") as Button;
            plusMinusButton.MouseButtonClick += new MyGUIEvent(plusMinusButton_MouseButtonClick);
            plusMinusButton.Visible = treeNode.Children.Count > 0;

            mainButton = primaryWidget.createWidgetT("Button", "TreeIconButton", 17, 0, 10, 16, Align.Stretch, "") as Button;
            mainButton.Caption = caption;
            mainButton.MouseButtonClick += new MyGUIEvent(mainButton_MouseButtonClick);
            mainButton.MouseButtonDoubleClick += new MyGUIEvent(mainButton_MouseButtonDoubleClick);
            mainButton.MouseButtonReleased += new MyGUIEvent(mainButton_MouseButtonReleased);
            mainButton.MouseButtonPressed += new MyGUIEvent(mainButton_MouseButtonPressed);
            mainButton.Selected = treeNode.Selected;
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
                plusMinusButton.Visible = treeNode.Children.Count > 0;
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
