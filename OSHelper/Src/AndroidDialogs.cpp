#include "Stdafx.h"
#include "NativeOSWindow.h"
#include "NativeDialog.h"

extern "C" _AnomalousExport void FileOpenDialog_showModal(NativeOSWindow* parent, String message, String defaultDir, String defaultFile, String wildcard, bool selectMultiple, FileOpenDialogSetPathString setPathString, FileOpenDialogResultCallback resultCallback HANDLE_ARG)
{
	
}

extern "C" _AnomalousExport void FileSaveDialog_showModal(NativeOSWindow* parent, String message, String defaultDir, String defaultFile, String wildcard, FileSaveDialogResultCallback resultCallback HANDLE_ARG)
{
	
}

extern "C" _AnomalousExport void DirDialog_showModal(NativeOSWindow* parent, String message, String startPath, DirDialogResultCallback resultCallback HANDLE_ARG)
{
	
}

extern "C" _AnomalousExport void ColorDialog_showModal(NativeOSWindow* parent, Color color, ColorDialogResultCallback resultCallback HANDLE_ARG)
{
	
}