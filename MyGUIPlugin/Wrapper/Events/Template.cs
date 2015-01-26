/*
This file serves as a template for the MyGUIEventTranslator implementations.
Copy the entire thing, replace the variables and then split it into two files.

Replace the following variables with what you want the class to be:
 * EVENT_TRANS_CLASS - The name of the event translator class.
 * CALLBACK_ARGS - The list of arguments for the class.
 * NATIVE_EVENT - The event on the widget to subscribe to.
 * EVENT_ARGS_TYPE - The EventArgs or subclass of such that you wish to fire.
 * MyGUI::Widget* - If MyGUI::Widget* is not the class with the event (optional).

-----------------------------------------------------------
c# class
-----------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;

namespace MyGUIPlugin
{
    class EVENT_TRANS_CLASS : MyGUIWidgetEventTranslator
    {
        static EVENT_ARGS_TYPE eventArgs = new EVENT_ARGS_TYPE();

        private CallbackHandler callbackHandler;

        public EVENT_TRANS_CLASS()
        {
            callbackHandler = new CallbackHandler();
        }

        public override void Dispose()
        {
            base.Dispose();
            callbackHandler.Dispose();
        }

        protected override IntPtr doInitialize(Widget widget)
        {
            return callbackHandler.create(this, widget);
        }

        private void nativeEvent(IntPtr widget, CALLBACK_ARGS)
        {
            //Fill out the EVENT_ARGS_TYPE
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr EVENT_TRANS_CLASS_Create(IntPtr widget, NativeEventDelegate nativeEventCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeEventDelegate(IntPtr widget, CALLBACK_ARGS
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static NativeEventDelegate nativeEventCallback;

            static CallbackHandler()
            {
                nativeEventCallback = new NativeEventDelegate(nativeEvent);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeEventDelegate))]
            private static void nativeEvent(IntPtr widget, CALLBACK_ARGS, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as EVENT_TRANS_CLASS).nativeEvent(widget, CALLBACK_ARGS);
            }

            private GCHandle handle;

            public IntPtr create(EVENT_TRANS_CLASS obj, Widget widget)
            {
                handle = GCHandle.Alloc(obj);
                return EVENT_TRANS_CLASS_Create(widget.WidgetPtr, nativeEventCallback, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
                nativeEventCallback = null;
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private NativeEventDelegate nativeEventCallback;

            public IntPtr create(EVENT_TRANS_CLASS obj, Widget widget)
            {
                nativeEventCallback = new NativeEventDelegate(obj.nativeEvent);
                return EVENT_TRANS_CLASS_Create(widget.WidgetPtr, nativeEventCallback);
            }

            public void Dispose()
            {
                nativeEventCallback = null;
            }
        }
#endif

        #endregion
    }
}

-----------------------------------------------------------
c++ class - put into a source file, will not need a header.
-----------------------------------------------------------

#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EVENT_TRANS_CLASS : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, CALLBACK_ARGS HANDLE_ARG);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;
    HANDLE_INSTANCE


public:
	EVENT_TRANS_CLASS(MyGUI::Widget* widget, EVENT_TRANS_CLASS::NativeEventDelegate nativeEventCallback HANDLE_ARG)
		:widget(widget),
		nativeEvent(nativeEventCallback)
        ASSIGN_HANDLE_INITIALIZER
	{

	}

	virtual ~EVENT_TRANS_CLASS()
	{

	}

	virtual void bindEvent()
	{
#ifdef FULL_AOT_COMPILE
		widget->NATIVE_EVENT = MyGUI::newDelegate(this, &EVENT_TRANS_CLASS::fireEvent);
#else
		widget->NATIVE_EVENT = MyGUI::newDelegate(nativeEvent);
#endif
	}

	virtual void unbindEvent()
	{
		widget->NATIVE_EVENT = NULL;
	}
};

extern "C" _AnomalousExport EVENT_TRANS_CLASS* EVENT_TRANS_CLASS_Create(MyGUI::Widget* widget, EVENT_TRANS_CLASS::NativeEventDelegate nativeEventCallback HANDLE_ARG)
{
	return new EVENT_TRANS_CLASS(widget, nativeEventCallback PASS_HANDLE_ARG);
}

*/