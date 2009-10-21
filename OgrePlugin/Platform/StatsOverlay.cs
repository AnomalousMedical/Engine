using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;

namespace OgrePlugin
{
    class StatsOverlay
    {
        private PanelOverlayElement statsPanel;
        private TextAreaOverlayElement fpsTextArea;
        private Overlay overlay;
        private String name;
        private float lastFPS;
        private bool visible = false;

        public StatsOverlay(String name)
        {
            this.name = name;
        }

        public void createOverlays()
        {
            overlay = OverlayManager.getInstance().create(name + "Overlay__");
            statsPanel = OverlayManager.getInstance().createOverlayElement(PanelOverlayElement.TypeName, name + "StatsOverlayPanel__") as PanelOverlayElement;
            fpsTextArea = OverlayManager.getInstance().createOverlayElement(TextAreaOverlayElement.TypeName, name + "StatsFpsText__") as TextAreaOverlayElement;
            statsPanel.addChild(fpsTextArea);
            fpsTextArea.setFontName("StatsFont");
            fpsTextArea.setMetricsMode(GuiMetricsMode.GMM_PIXELS);
            fpsTextArea.setCharHeight(15.0f);

            overlay.add2d(statsPanel);
        }

        public void setStats(RenderTarget renderTarget)
        {
            if (lastFPS != renderTarget.getLastFPS())
            {
                lastFPS = renderTarget.getLastFPS();
                fpsTextArea.setCaption("FPS: " + lastFPS);
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
                OverlayManager.getInstance().destroyOverlayElement(fpsTextArea);
                OverlayManager.getInstance().destroy(overlay);
                statsPanel = null;
                fpsTextArea = null;
                overlay = null;
            }
        }
    }
}
