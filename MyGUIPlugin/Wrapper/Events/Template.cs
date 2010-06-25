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
    class EVENT_TRANS_CLASS : MyGUIEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget, CALLBACK_ARGS);
        static EVENT_ARGS_TYPE eventArgs = new EVENT_ARGS_TYPE();

        private NativeEventDelegate nativeEventCallback;

        public EVENT_TRANS_CLASS()
        {
            nativeEventCallback = new NativeEventDelegate(nativeEvent);
        }

        public override void Dispose()
        {
            base.Dispose();
            nativeEventCallback = null;
        }

        protected override IntPtr doInitialize(Widget widget)
        {
            return EVENT_TRANS_CLASS_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, CALLBACK_ARGS)
        {
            //Fill out the EVENT_ARGS_TYPE
            fireEvent(eventArgs);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EVENT_TRANS_CLASS_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

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
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, CALLBACK_ARGS);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EVENT_TRANS_CLASS(MyGUI::Widget* widget, EVENT_TRANS_CLASS::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EVENT_TRANS_CLASS()
	{

	}

	virtual void bindEvent()
	{
		widget->NATIVE_EVENT = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->NATIVE_EVENT = NULL;
	}
};

extern "C" _AnomalousExport EVENT_TRANS_CLASS* EVENT_TRANS_CLASS_Create(MyGUI::Widget* widget, EVENT_TRANS_CLASS::NativeEventDelegate nativeEventCallback)
{
	return new EVENT_TRANS_CLASS(widget, nativeEventCallback);
}

*/