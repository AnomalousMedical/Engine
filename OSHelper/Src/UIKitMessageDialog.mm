#include "StdAfx.h"
#include "NativeDialog.h"
#include "NativeOSWindow.h"

#include <CoreFoundation/CoreFoundation.h>

extern "C" _AnomalousExport void MessageDialog_showErrorDialog(NativeOSWindow* parent, String msg, String cap)
{
    
}

extern "C" _AnomalousExport NativeDialogResult MessageDialog_showQuestionDialog(NativeOSWindow* parent, String msg, String cap)
{
    return CANCEL;
}