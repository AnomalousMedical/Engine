/*
This file serves as a template for the MyGUIEventTranslator implementations.
Copy the entire thing, replace the variables and then split it into two files.

Replace the following variables with what you want the class to be:
 * EVENTTRANSCLASS - The name of the event translator class.
 * EVENTARGS - The list of arguments for the class.
 * NATIVEEVENT - The event on the widget to subscribe to.
 * EVENTARGS - The EventArgs or subclass of such that you wish to fire.
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
    class EVENTTRANSCLASS : MyGUIEventTranslator
    {
        delegate void NativeEventDelegate(IntPtr widget, EVENTARGS);

        private NativeEventDelegate nativeEventCallback;

        public EVENTTRANSCLASS()
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
            return EVENTTRANSCLASS_Create(widget.WidgetPtr, nativeEventCallback);
        }

        private void nativeEvent(IntPtr widget, EVENTARGS)
        {
            fireEvent(EVENTARGS);
        }

        #region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr EVENTTRANSCLASS_Create(IntPtr widget, NativeEventDelegate nativeEventCallback);

        #endregion
    }
}

-----------------------------------------------------------
c++ class - put into a source file, will not need a header.
-----------------------------------------------------------

#include "StdAfx.h"
#include "../Include/MyGUIEventTranslator.h"

class EVENTTRANSCLASS : public MyGUIEventTranslator
{
public:
	typedef void (*NativeEventDelegate)(MyGUI::Widget* sender, EVENTARGS);

private:
	MyGUI::Widget* widget;
	NativeEventDelegate nativeEvent;

public:
	EVENTTRANSCLASS(MyGUI::Widget* widget, EVENTTRANSCLASS::NativeEventDelegate nativeEventCallback)
		:widget(widget),
		nativeEvent(nativeEventCallback)
	{

	}

	virtual ~EVENTTRANSCLASS()
	{

	}

	virtual void bindEvent()
	{
		widget->NATIVEEVENT = MyGUI::newDelegate(nativeEvent);
	}

	virtual void unbindEvent()
	{
		widget->NATIVEEVENT = NULL;
	}
};

extern "C" _AnomalousExport EVENTTRANSCLASS* EVENTTRANSCLASS_Create(MyGUI::Widget* widget, EVENTTRANSCLASS::NativeEventDelegate nativeEventCallback)
{
	return new EVENTTRANSCLASS(widget, nativeEventCallback);
}

*/