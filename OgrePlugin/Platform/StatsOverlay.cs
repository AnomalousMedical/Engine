using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;
using Engine;

namespace OgrePlugin
{
    class StatsOverlay
    {
        private PanelOverlayElement statsPanel;
        private TextAreaOverlayElement fpsTextArea;
        private TextAreaOverlayElement triangleCountTextArea;
        private TextAreaOverlayElement batchCountTextArea;
        private Overlay overlay;
        private String name;
        private float lastFPS;
        private uint lastTriangleCount = uint.MaxValue;
        private uint lastBatchCount = uint.MaxValue;
        //private bool visible = false;
        private Vector2 position = new Vector2();

        public StatsOverlay(String name)
        {
            this.name = name;
        }

        public void createOverlays()
        {
            overlay = OverlayManager.getInstance().create(name + "Overlay__");
            statsPanel = OverlayManager.getInstance().createOverlayElement(PanelOverlayElement.TypeName, name + "StatsOverlayPanel__") as PanelOverlayElement;
            statsPanel.setPosition(position.x, position.y);
            
            fpsTextArea = OverlayManager.getInstance().createOverlayElement(TextAreaOverlayElement.TypeName, name + "StatsFpsText__") as TextAreaOverlayElement;
            statsPanel.addChild(fpsTextArea);
            fpsTextArea.setFontName("StatsFont");
            fpsTextArea.setMetricsMode(GuiMetricsMode.GMM_PIXELS);
            fpsTextArea.setCharHeight(15.0f);

            triangleCountTextArea = OverlayManager.getInstance().createOverlayElement(TextAreaOverlayElement.TypeName, name + "StatsTriangleCountText__") as TextAreaOverlayElement;
            statsPanel.addChild(triangleCountTextArea);
            triangleCountTextArea.setFontName("StatsFont");
            triangleCountTextArea.setMetricsMode(GuiMetricsMode.GMM_PIXELS);
            triangleCountTextArea.setCharHeight(15.0f);
            triangleCountTextArea.setPosition(0.0f, 17.0f);

            batchCountTextArea = OverlayManager.getInstance().createOverlayElement(TextAreaOverlayElement.TypeName, name + "StatsBatchCountText__") as TextAreaOverlayElement;
            statsPanel.addChild(batchCountTextArea);
            batchCountTextArea.setFontName("StatsFont");
            batchCountTextArea.setMetricsMode(GuiMetricsMode.GMM_PIXELS);
            batchCountTextArea.setCharHeight(15.0f);
            batchCountTextArea.setPosition(0.0f, 34.0f);

            overlay.add2d(statsPanel);
        }

        public void setStats(RenderTarget renderTarget)
        {
            if (lastFPS != renderTarget.getLastFPS())
            {
                lastFPS = renderTarget.getLastFPS();
                fpsTextArea.setCaption("FPS: " + lastFPS);
            }
            if (lastTriangleCount != renderTarget.getTriangleCount())
            {
                lastTriangleCount = renderTarget.getTriangleCount();
                triangleCountTextArea.setCaption("Triangles: " + lastTriangleCount);
            }
            if (lastBatchCount != renderTarget.getBatchCount())
            {
                lastBatchCount = renderTarget.getBatchCount();
                batchCountTextArea.setCaption("Batches: " + lastBatchCount);
            }
        }

        public void setVisible(bool visible)
        {
            if (visible && !overlay.isVisible())
            {
                overlay.show();
                
            }
            else if(!visible && overlay.isVisible())
            {
                overlay.hide();
            }
        }

        public void destroyOverlays()
        {
            if (statsPanel != null)
            {
                overlay.remove2d(statsPanel);
                OverlayManager.getInstance().destroyOverlayElement(statsPanel);
                OverlayManager.getInstance().destroyOverlayElement(batchCountTextArea);
                OverlayManager.getInstance().destroyOverlayElement(triangleCountTextArea);
                OverlayManager.getInstance().destroyOverlayElement(fpsTextArea);
                OverlayManager.getInstance().destroy(overlay);
                statsPanel = null;
                fpsTextArea = null;
                overlay = null;
            }
        }

        public Vector2 StatsPosition
        {
            get
            {
                return new Vector2(fpsTextArea.getLeft(), fpsTextArea.getTop());
            }
            set
            {
                statsPanel.setPosition(value.x, value.y);
                position = value;
            }
        }
    }
}
