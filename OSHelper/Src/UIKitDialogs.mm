#include "StdAfx.h"
#include "NativeOSWindow.h"
#include "NativeDialog.h"

#include <string>
#import <Foundation/Foundation.h>

//Process an entry for wildcards, returns true if the wildcard matches everything
bool processEntry(NSString* entry, NSMutableArray* fileTypeVector)
{
    NSUInteger dotPos = [entry rangeOfString:@"."].location;
    if(dotPos != NSNotFound)
    {
        //Consider the whole string the filter, from the . to the end
        NSString* card = [entry substringFromIndex:dotPos + 1];
        if([card isEqualToString: @"*"])
        {
            return true;
        }
        [fileTypeVector addObject:card];
    }
    return false;
}

bool processEntries(NSString* entry, NSMutableArray* fileTypeVector)
{
    NSArray* fileTypes = [entry componentsSeparatedByString:@";"];
    for(NSUInteger i = 0; i < [fileTypes count]; ++i)
    {
        if(processEntry([fileTypes objectAtIndex:i], fileTypeVector))
        {
            return true;
        }
    }
    return false;
}

//Wildcard, these are in the format description|extension|description|extension
//Will return true if the filters should be used and false if not.
bool convertWildcards(String utf16Wildcard, NSMutableArray* fileTypeVector)
{
    NSString* wildcard = [NSString stringWithFormat:@"%S", utf16Wildcard];
    if([wildcard length] == 0)
    {
        return false;
    }
    NSArray* fileTypes = [wildcard componentsSeparatedByString:@"|"];
    if([fileTypes count] == 1)
    {
        return !processEntries(wildcard, fileTypeVector);
    }
    else
    {
        for(NSUInteger i = 1; i < [fileTypes count]; i += 2)
        {
            if(processEntries([fileTypes objectAtIndex:i], fileTypeVector))
            {
                return false;
            }
        }
    }
    
    return fileTypeVector.count > 0;
}

extern "C" _AnomalousExport void FileOpenDialog_showModal(NativeOSWindow* parent, String message, String defaultDir, String defaultFile, String wildcard, bool selectMultiple, FileOpenDialogSetPathString setPathString, FileOpenDialogResultCallback resultCallback)
{

}

extern "C" _AnomalousExport void FileSaveDialog_showModal(NativeOSWindow* parent, String message, String defaultDir, String defaultFile, String wildcard, FileSaveDialogResultCallback resultCallback)
{
    
}

extern "C" _AnomalousExport void DirDialog_showModal(NativeOSWindow* parent, String message, String startPath, DirDialogResultCallback resultCallback)
{

}

extern "C" _AnomalousExport void ColorDialog_showModal(NativeOSWindow* parent, Color color, ColorDialogResultCallback resultCallback)
{

}