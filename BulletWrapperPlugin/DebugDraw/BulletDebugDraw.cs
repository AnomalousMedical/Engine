using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using System.Runtime.InteropServices;
using Logging;
using Engine;

namespace BulletPlugin
{
    class BulletDebugDraw : IDisposable
    {
        internal IntPtr nativeDraw;
        private DebugDrawingSurface drawingSurface;
        private DrawLineCallback drawLineStorage;
        private ReportErrorWarningCallback reportErrorWarningStorage;

        public BulletDebugDraw()
        {
            drawLineStorage = new DrawLineCallback(drawLine);
            reportErrorWarningStorage = new ReportErrorWarningCallback(reportErrorWarning);
            nativeDraw = BulletDebugDraw_Create(drawLineStorage, reportErrorWarningStorage);
        }

        public void Dispose()
        {
            if(nativeDraw != IntPtr.Zero)
            {
                BulletDebugDraw_Delete(nativeDraw);
                nativeDraw = IntPtr.Zero;
            }
        }

        public void setDrawingSurface(DebugDrawingSurface drawingSurface)
        {
            this.drawingSurface = drawingSurface;
        }

        public static void setGlobalDebugMode(DebugDrawModes debugMode)
        {
            BulletDebugDraw_setGlobalDebugMode((int)debugMode);
            Log.Default.debug("Enabled {0}", debugMode);
        }

        public static void enableGlobalDebugMode(DebugDrawModes debugMode)
        {
            BulletDebugDraw_enableGlobalDebugMode((int)debugMode);
            Log.Default.debug("Enabled {0}", debugMode);
        }

        public static void disableGlobalDebugMode(DebugDrawModes debugMode)
        {
            BulletDebugDraw_disableGlobalDebugMode((int)debugMode);
            Log.Default.debug("Disabled {0}", debugMode);
        }

        public static DebugDrawModes getGlobalDebugMode()
        {
            return (DebugDrawModes)BulletDebugDraw_getGlobalDebugMode();
        }

        private void drawLine(ref Vector3 color, ref Vector3 from, ref Vector3 to)
        {
            drawingSurface.setColor(new Color(color.x, color.y, color.z));
            drawingSurface.drawLine(from, to);
        }

        private void reportErrorWarning(String warning)
        {
            Log.Warning(warning);
        }


        delegate void DrawLineCallback(ref Vector3 color, ref Vector3 from, ref Vector3 to);
        delegate void ReportErrorWarningCallback(String warning);
        [DllImport("BulletWrapper")]
        private static extern IntPtr BulletDebugDraw_Create(DrawLineCallback drawLine, ReportErrorWarningCallback reportWarning);

        [DllImport("BulletWrapper")]
        private static extern void BulletDebugDraw_Delete(IntPtr debugDraw);

        [DllImport("BulletWrapper")]
        private static extern void BulletDebugDraw_setGlobalDebugMode(int debugMode);

        [DllImport("BulletWrapper")]
        private static extern void BulletDebugDraw_enableGlobalDebugMode(int debugMode);

        [DllImport("BulletWrapper")]
        private static extern void BulletDebugDraw_disableGlobalDebugMode(int debugMode);

        [DllImport("BulletWrapper")]
        private static extern int BulletDebugDraw_getGlobalDebugMode();
    }
}
