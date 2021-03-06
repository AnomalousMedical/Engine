﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using MyGUIPlugin;

namespace Anomalous.GuiFramework.Editor
{
    public class ExpandingEditInterfaceViewer : IDisposable
    {
        protected ExpandingNode rootNode;
        private EditInterface editInterface;
        private Widget parentWidget;
        private GuiFrameworkUICallback uiCallback;

        public ExpandingEditInterfaceViewer(Widget parentWidget, GuiFrameworkUICallback uiCallback)
        {
            this.parentWidget = parentWidget;
            this.uiCallback = uiCallback;
        }

        public void Dispose()
        {
            if (editInterface != null)
            {
                rootNode.Dispose();
            }
        }

        public virtual EditInterface EditInterface
        {
            get
            {
                return editInterface;
            }
            set
            {
                if (editInterface != value)
                {
                    if (editInterface != null)
                    {
                        rootNode.Dispose();
                    }
                    editInterface = value;
                    if (editInterface != null)
                    {
                        rootNode = new ExpandingNode(editInterface, parentWidget, uiCallback);
                        rootNode.Expanded = true;
                        rootNode.ExpandButtonVisible = false;
                    }
                }
            }
        }

        public ExpandingNode RootNode
        {
            get
            {
                return rootNode;
            }
        }

        public virtual void layout()
        {
            rootNode.layout();
        }
    }
}
