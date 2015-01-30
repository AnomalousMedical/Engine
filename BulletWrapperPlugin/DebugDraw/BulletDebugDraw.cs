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
        private CallbackHandler callbackHandler;

        public BulletDebugDraw()
        {
            callbackHandler = new CallbackHandler();
            nativeDraw = callbackHandler.create(this);
        }

        public void Dispose()
        {
            if (nativeDraw != IntPtr.Zero)
            {
                BulletDebugDraw_Delete(nativeDraw);
                callbackHandler.Dispose();
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
            drawingSurface.Color = new Color(color.x, color.y, color.z);
            drawingSurface.drawLine(from, to);
        }

        private void reportErrorWarning(String warning)
        {
            Log.Warning(warning);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void DrawLineCallback(ref Vector3 color, ref Vector3 from, ref Vector3 to
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ReportErrorWarningCallback(String warning
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr BulletDebugDraw_Create(DrawLineCallback drawLine, ReportErrorWarningCallback reportWarning
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BulletDebugDraw_Delete(IntPtr debugDraw);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BulletDebugDraw_setGlobalDebugMode(int debugMode);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BulletDebugDraw_enableGlobalDebugMode(int debugMode);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BulletDebugDraw_disableGlobalDebugMode(int debugMode);

        [DllImport(BulletInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int BulletDebugDraw_getGlobalDebugMode();

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static DrawLineCallback drawLineStorage;
            private static ReportErrorWarningCallback reportErrorWarningStorage;

            static CallbackHandler()
            {
                drawLineStorage = new DrawLineCallback(drawLine);
                reportErrorWarningStorage = new ReportErrorWarningCallback(reportErrorWarning);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(DrawLineCallback))]
            private static void drawLine(ref Vector3 color, ref Vector3 from, ref Vector3 to, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as BulletDebugDraw).drawLine(ref color, ref from, ref to);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(ReportErrorWarningCallback))]
            private static void reportErrorWarning(string warning, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as BulletDebugDraw).reportErrorWarning(warning);
            }

            private GCHandle handle;

            public IntPtr create(BulletDebugDraw obj)
            {
                handle = GCHandle.Alloc(obj);
                return BulletDebugDraw_Create(drawLineStorage, reportErrorWarningStorage, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private DrawLineCallback drawLineStorage;
            private ReportErrorWarningCallback reportErrorWarningStorage;

            public IntPtr create(BulletDebugDraw obj)
            {
                drawLineStorage = new DrawLineCallback(obj.drawLine);
                reportErrorWarningStorage = new ReportErrorWarningCallback(obj.reportErrorWarning);
                return BulletDebugDraw_Create(drawLineStorage, reportErrorWarningStorage);
            }

            public void Dispose()
            {

            }
        }
#endif
    }
}
