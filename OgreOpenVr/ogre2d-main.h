// Ogre 2d: a small wrapper for 2d Graphics Programming in Ogre.
/*
   Wrapper for 2d Graphics in the Ogre 3d engine.

   Coded by H. Hernán Moraldo from Moraldo Games
   www.hernan.moraldo.com.ar/pmenglish/field.php

   Thanks for the Cegui team as their rendering code in Ogre gave me
   fundamental insight on the management of hardware buffers in Ogre.

   --------------------

   Copyright (c) 2006 Horacio Hernan Moraldo

   This software is provided 'as-is', without any express or
   implied warranty. In no event will the authors be held liable
   for any damages arising from the use of this software.

   Permission is granted to anyone to use this software for any
   purpose, including commercial applications, and to alter it and
   redistribute it freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you
   must not claim that you wrote the original software. If you use
   this software in a product, an acknowledgment in the product
   documentation would be appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and
   must not be misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
   distribution.

*/
#include "stdafx.h"

#ifndef __OGRE2D_MAIN_FILE
#define __OGRE2D_MAIN_FILE

#define WIN32_LEAN_AND_MEAN

#include <Ogre.h>
//#include <OgreMesh.h>
//#include <OgreHardwareBuffer.h>

//#include <Ogre.h>
//#include <OgreRoot.h>
//#include <OgreRenderQueueListener.h>

//#include <OgreViewport.h>//bzn
//#include <OgreCamera.h>
//#include <OgreEntity.h>
//#include <OgreMesh.h>

/*
#include <OgreCamera.h>
#include <OgreEntity.h>
#include <OgreLogManager.h>
#include <OgreOverlay.h>
#include <OgreOverlayElement.h>
#include <OgreOverlayManager.h>
#include <OgreRoot.h>
#include <OgreViewport.h>
#include <OgreSceneManager.h>
#include <OgreRenderWindow.h>
#include <OgreConfigFile.h>
#include <OgreRectangle2D.h>
#include <OgrePrerequisites.h>

#include <OgreMaterialManager.h>
#include <OgreTextureUnitState.h>
*/

#include <string>
#include <list>

//#include <OISKeyboard.h>

////////////////////////////////////////////////////////////////////////////////


#define MAXSPRITENAMESIZE	1024	// length of the name of texture

#define MISCSTRINGSIZE 1024 


#define MAXTEXTCHAR 94 // characters in texture font
 // virtual screen
#define VSCREEN_W 1280                  
#define VSCREEN_H 720
#define FONTSPACE 24

#define TEXW	512.0f
#define TEXH	1024.0f

#define BUTTONTEXTSCALE		0.333
#define BUTTONFRAMEX			206
#define BUTTONFRAMEY			38
#define BUTTONSIZEX				192
#define BUTTONSIZEY				24

#define SLIDERBUTTONSIZE	10

// optional default values
#define	SIMPLEBUTTONFRAMESIZE	4



#define BUTTONEDITTEXTSCALE			0.666
#define BUTTONEDITFRAMEX_LRG				412
#define BUTTONEDITSIZEX_LRG					384
#define BUTTONEDITFRAMEX_SML				206
#define BUTTONEDITSIZEX_SML					192

#define MAX_BUTTON_TEXT	64 // most text on a typical button, includes terminating NULL.  It is possible to have more, since button name memory is dynamically allocated.  For example, The MENU_SELECTOR has names that are MAX_PATH long
#define MAX_BUTTON_SWITCH	50 // Button can have this many toggled states
#define ABSOLUTE_MAX_BUTTON_TEXT 1024 // limit absolute maximum size to 1k

#define MAX_INFOTEXT_LINES	64
#define MAX_INFOTEXT_CHAR		256
#define INFOTEXT_CLEARTIME	10000.0

#define GUI_MAXTRI				32768 // each triangle comes in a pair that use a total of 4 vertices, since 2 verts are shared between triangles
#define GUI_MAXVERT				65536 // max allowed for 16 bit indices.

// struct used for passing vectors of filenames to the file selector
typedef struct
{
	char Filename[MAX_PATH] ;
}
FILENAME ;

#define MAXBUTTONINFO	4 // a few extra shorts of optional, multipurpose info
typedef struct
{
	char Label[MAX_BUTTON_TEXT] ; // names that appear next to buttons
	char *Name[MAX_BUTTON_SWITCH] ; // pointers to name memory.  Usually MAX_BUTTON_TEXT size, but longer for some, such as for folder operations. NameMem tells how much mem each button uses
	char Switch ; // what switch the button is currently using
	char MaxSwitch ; // 0 for non-switching buttons
	char Action ; // what action will be triggered by pressing this button.  MENU_NONE if none, postive to go to another menu,
	char State ;
	char Style ;
	short PositionX ;
	short PositionY ;
	short SizeX ;
	short SizeY ;
	short FrameSizeX ;
	short FrameSizeY ;
	short SpecialControl ; // what number special control this button is part of, or -1.
	short Info[MAXBUTTONINFO] ; // general purpose info
	unsigned short NameMem[MAX_BUTTON_SWITCH] ; // how much memory is put aside for each Name
	float MinVal ; // min value if storing a float or integer
	float MaxVal ; // max value if storing a float or integer
	bool Visible ;
	
	
	
	// button text is almost always a default white colour, but occasionally it needs to be a custom colour, such as text in a databox. 
	// stored as unsigned chars to save space.
	unsigned char R ;
	unsigned char G ;
	unsigned char B ;

}
BZNBUTTON ;

enum{
	BUTTONACTION_NONE,					// normal, default buttons and switches
	BUTTONACTION_CHECKBOX,			// checkbox, set maxswitch to 2
	BUTTONACTION_EDIT_FLOAT,		// floating point number edit box
	BUTTONACTION_EDIT_INTEGER,		// integer number edit box
	BUTTONACTION_EDIT_TEXT,			// text edit box
	BUTTONACTION_COLUMNSLIDER,		// slider for STATICTEXTCOLUMN
	BUTTONACTION_STATICTEXT,		// unchanging text (though can be modified if required)
	BUTTONACTION_STATICTEXTLJUST, // static text, left justified rather than centered.
	BUTTONACTION_STATICTEXTCOLUMN, // special type of text used in columns, ~ indicates next column instead of new switch.
	BUTTONACTION_STATICTEXTCOLUMN_NOCLICK, // special type of text used in columns, ~ indicates next column instead of new switch.
	BUTTONACTION_STATICTEXTCOLUMN_FEEDBACK, // special type of text used in columns, same as no-click above, gives feedback.
	BUTTONACTION_STATICTEXTBACK,	// unchanging text on a background
	BUTTONACTION_LIST,					// a list of options, same as a switch button but offers all options at once.

	BUTTONACTION_MAX
};

/////////////////////
// slider sub-actions
#define SLIDERDRAWSTATE_UP			1
#define SLIDERDRAWSTATE_DOWN		2
#define SLIDERDRAWSTATE_MOVE		3

#define SLIDERCONNECTIONTYPE_COLUMNBOX	0

enum{
	SLIDERINFO_DRAWSTATE,
	SLIDERINFO_CONNECTIONTYPE,
	SLIDERINFO_CONNECTION,
	SLIDERINFO_TIME,
	SLIDERINFO_MAX
};

/////////////////////

enum{
	BUTTONSTYLE_FANCY,
	BUTTONSTYLE_SIMPLE,
	BUTTONSTYLE_MAX
};

// special controls are made out of multiple sub-buttons to achieve some effect.
#define COLUMNMAX	8
#define COLUMNNAMESIZE	64

// used to fill up a columnbox
typedef struct
{
	short StartRow ;
	short MaxRow ;
	short DataTextSize[COLUMNMAX] ; // max characters per array element in the text. e.g. if it's based on NAMES[100][24] for 100 names each a max of 24 long, then this setting would be 24
	char *DataText[COLUMNMAX] ;
	double NextClickableTime ; // we keep track of the last time it was updated, so user doesn't accidentally click a selection that suddenly changed.
	int UpdateCount ; // how many times the columnbox has been changed.  This allows us to check whether the list has changed if a user tries to use old data from the list for something else.
}
COLUMNBOXFILLINFO ;

typedef struct
{
	short MaxColumn ;
	short ColumnSizeX[COLUMNMAX] ;
	char Name[COLUMNMAX*COLUMNNAMESIZE] ; // one long array for names, so access via Name[n*COLUMNNAMESIZE] and just treat like normal char strings.
	COLUMNBOXFILLINFO FillInfo ; 
	bool Feedback ;
}
COLUMNBOXINFO ;



typedef struct
{
		short Type ;
		short ButtonStart ;
		short ButtonEnd ;
		short MaxLine ;
		
		COLUMNBOXINFO ColumnBoxInfo ;
}
SPECIALCONTROL ;

typedef struct
{	
	short L ;
	short R ;
	short U ;
	short D ;
	bool Back ;
	bool Visible ;
	char Type ;

	std::vector<BZNBUTTON> Button ;
	std::vector<SPECIALCONTROL> SpecialControl ;

}
BZNMENU ;



enum{
	SPECIALCONTROL_DATABOX,
	SPECIALCONTROL_COLUMNBOX,
	SPECIALCONTROL_MAX
};


// menu types.
enum{
	MENUTYPE_DEFAULT,
	MENUTYPE_MAX
};

enum{
	BUTTONSTATE_INACTIVE,
	BUTTONSTATE_HOVER,		// mouse is over button but not pressing it
	BUTTONSTATE_PRESSED, 
	BUTTONSTATE_MAX
};

enum{
	EDITBOXACTION_NONE,						// nothing to report
	EDITBOXACTION_FINISHEDINPUT,	// user finished editing an editbox
	EDITBOXACTION_SPECIALINPUT		// user changed an editbox integer via the cursors or mousewheel, treat that as finished input and send MENURESULT but don't end the editbox control
};


#define MENURESULTINFOMAX	8 // additional info in results, if needed.
typedef struct
{
	int Menu ;
	int Button ;
	int Switch ;
	int SpecialControl ;
	int Info[MENURESULTINFOMAX] ;
	int Result ;
}
MENURESULT ;

#define MENURESULTINFO_COLUMNBOXINDEX		0
#define MENURESULTINFO_COLUMNBOXUPDATE	1


enum{
	MENURESULT_NOACTION,
	MENURESULT_BUTTONACTIVATED,
	//MENURESULT_EDITBOXENDED,
	//MENURESULT_EDITBOXALTERED
};
////////////////////////////////////////////////////////////////////////////////////
enum{
	MENU_NONE=-1, // reserved value
	MENU_SELECTOR, // reserved value
	MENU_INFOBOX, // reserved value
	MENU_LISTBOX // reserved value, this is assumed to be the last reserved menu before the user's menus
};
#define MAX_SELECTOR_OPTION	150
enum{
	MENU_SELECTOR_NAME,
	MENU_SELECTOR_EDIT,
	MENU_SELECTOR_CANCEL,
	MENU_SELECTOR_OK,
	MENU_SELECTOR_SCROLLUP,
	MENU_SELECTOR_SCROLLDOWN,
	MENU_SELECTOR_FOLDER,
	MENU_SELECTOR_PARENT,
	MENU_SELECTOR_DRIVES,
	MENU_SELECTOR_FIRSTOPTION,
};

// first two buttons on message box are for "OK" or "YES and NO", remaining switches can be used for message strings
enum{
	MENU_INFOBOX_YES,
	MENU_INFOBOX_NO,
	MENU_INFOBOX_MESSAGE, // first line of message text
	MENU_INFOBOX_MAX
};

enum{
	MESSAGEBOX_TYPE_OK,
	MESSAGEBOX_TYPE_YESNO,
	MESSAGEBOX_TYPE_MAX
};



struct Ogre2dSprite
{
   double x1, y1, x2, y2;// sprite coordinates
   double tx1, ty1, tx2, ty2;// texture coordinates
	float r, g, b ; // bzn colour
};


class Ogre2dManager:public Ogre::RenderQueueListener
{
private:
   Ogre::SceneManager* sceneMan;

   Ogre::uint8 targetQueue;
   bool afterQueue;
public:
   Ogre2dManager();
   virtual ~Ogre2dManager();

   /// Initializes this 2d Manager
   /** and registers it as the render queue listener.*/
	void init(Ogre::SceneManager* sceneManager, char *pName, char *pResourceLoc, bool bFilter) ;
   /// Finishes Ogre 2d Manager
	void end();




   /// Buffers a sprite to be sent to the screen at render time.
   /**
      Sprite coordinates are in screen space: top left pixel is (-1, 1), and bottom right
      is (1, -1). The texture space, instead, ranges from (0, 0) to (1, 1).

      /param textureName Name of the texture to use in this sprite (remember: texture
      name, not material name!). The texture has to be already loaded by Ogre for this
      to work.
      /param x1 x coordinate for the top left point in the sprite.
      /param y1 y coordinate for the top left point in the sprite.
      /param x2 x coordinate for the bottom right point in the sprite.
      /param y2 y coordinate for the bottom right point in the sprite.
      /param tx1 u coordinate for the texture, in the top left point of the sprite.
      /param ty1 v coordinate for the texture, in the top left point of the sprite.
      /param tx2 u coordinate for the texture, in the bottom right point of the sprite.
      /param ty2 u coordinate for the texture, in the bottom right point of the sprite.
   */
	 // bzn modified to add colour
   void spriteBltFull(double x1, double y1, double x2, double y2, double tx1, double ty1, double tx2, double ty2, float Rd, float Gr, float Bl);



	//////////////////////////////////////////////////////////////////////////////////////////////
 // mkultra333 stuff for text/gui

	BZNBUTTON m_NullButton ;

	void ClearSpriteBuffer() ;
	void AddSprite(float StartX, float StartY, float EndX, float EndY, float StartU, float StartV, float EndU, float EndV, float Rd, float Gr, float Bl) ;
	void AddSpriteToVirtualScreen(float PositionX, float PositionY, float Width, float Height, float StartU, float StartV, float EndU, float EndV, float Rd, float Gr, float Bl) ;

	float flTextCoordsLRG[MAXTEXTCHAR][4] ;
	float flTextCoordsMED[MAXTEXTCHAR][4] ;
	int PrintTextLRG(char text[], int nXPos, int nYPos, float flScaleX, float flScaleY, bool bJustWidth, float Rd, float Gr, float Bl) ;
	int PrintTextMED(char text[], int nXPos, int nYPos, float flScaleX, float flScaleY, bool bJustWidth, float Rd, float Gr, float Bl) ;
	void PrintTextLRG_Centred(char text[], int nYPos, float flScaleX, float flScaleY, float Rd, float Gr, float Bl) ;
	void PrintTextMED_Centred(char text[], int nYPos, float flScaleX, float flScaleY, float Rd, float Gr, float Bl) ;
	
	void SetupGui() ;
	void CleanupGui() ;

	void SetupCharCoords() ; 
	// some other coords
	RECT UVCOORD_CURSOR;
	RECT UVCOORD_BLANK;
	RECT UVCOORD_FRAMELRG;
	RECT UVCOORD_FRAMESML;
	RECT UVCOORD_BACK;
	



	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// my primitive gui 
	
	double m_GuiTime ;
	double m_GuiTimeSinceLastFrame ;

	unsigned int m_nFrameCount ;

	int m_nMenuMode ;
	int m_nOldMenuMode ;
	
	float m_flMouseX ;
	float m_flMouseY ;
	float m_flMouseZ ; // mouse wheel
	int m_nMouseWheelChange ; // 0=no change, 1=mouse wheel up, -1=mouse wheel down
	int m_nMouseLeft ;
	int m_nMouseRight ;

	bool m_bCursorVisible ;
	void ShowCursor() ;
	void HideCursor() ;

	void UpdateMouseInput(int nLeft, int nRight, float flPosX, float flPosY, float flWheel) ;

	float m_flViewportSizeX ;
	float m_flViewportSizeY ;
	void SetGuiViewportDimensions(float flXSize, float flYSize) ;

	int SetMenuVisibility(int nMenu, bool bVisible) ; // will fail if there is an active edit box
	bool GetMenuVisibility(int nMenu) ;
	bool AnyMenuIsVisible() ;
	int FindActiveMenu() ;
	void PrepareVisibleMenusForRender() ;
	MENURESULT UpdateMenu() ;
	void DrawMenu(int nMenu) ;
	void InitMenu(int nMenu) ;
	//void DrawButton(char pMessage[], float flPosX, float flPosY, int nActive, int nButton) ;
	void DrawButtonFancy(int nMenu, int nButton) ;
	void DrawButtonSimple(int nMenu, int nButton) ;
	
	int m_nEditBoxMenu ; // what is the menu that the active edit box is on
	int m_nEditBoxButton ; // what is the button that the active edit box is on
	int m_nEditBoxAction ; // 1 means we've gotten input
	char m_chEditBoxMessage[ABSOLUTE_MAX_BUTTON_TEXT] ; // provisional edit box message
	int m_nEditBoxPos ;
	
	int m_nSliderMenu ;
	int m_nSliderButton ;

	int m_nLastDeactivatedEditBoxMenu ;
	int m_nLastDeactivatedEditBoxButton ;

	int m_nQueueListBoxMenu ;
	int m_nQueueListBoxButton ;
	int m_nListBoxMenu ;
	int m_nListBoxButton ;
	void SetupListBoxMenu() ;

	void SetupSelectorMenu(char *pMenuName, char *pFolder, char *pFilename, std::vector<FILENAME> &InFileName, int nFirstFile) ;

	void HideInfoBoxMenu() ;
	bool ShowInfoBoxMenu(int nType, int nStyle, char *pMessage) ; // will fail if there's already an infobox visible

	void FinishEditBox() ;
	//void UpdateKeyboardInput(  OIS::Keyboard* m_pKB , int nShift, int nCtrl) ;
	//	// key toggles and delays
	//int m_nKT[OIS::KC_MEDIASELECT+1] ; // for preventing multiple key triggers when needed.
	//double m_dKD[OIS::KC_MEDIASELECT+1] ; // for repeating keys, used on edit boxes


	std::vector<BZNMENU> Menu ;

	float m_flTextScaleMedX ;
	float m_flTextScaleMedY ;

	void CreateMenu(int nMenu, int nLeft, int nRight, int nUp, int nDown, bool bBack, bool bVisible) ;
	void CreateMenuB(int nMenu, int nLeft, int nUp, int nSizeX, int nSizeY, bool bBack, bool bVisible) ;
	

	int AddSpecialControl_DataBox(int nMenu, int nPositionX, int nPositionY, int nSizeX, int nSizeY, bool bInput, bool bBack, bool bVisible) ; // special type of menu, displays rolling lines of text, may have input as well.
	int AddTextToDataBox(int nMenu, int nDataBox, char* pDataText, float flR, float flG, float flB) ;
	int CheckDataBoxInput(int nMenu, int nDataBox, char* pInput) ;	
	
	int AddSpecialControl_ColumnBox(int nMenu, int nPositionX, int nPositionY, int nSizeX, int nSizeY, COLUMNBOXINFO* pColumnBoxInfo, bool bFeedback, bool bBack, bool bVisible) ;
	void UpdateColumnBoxInfo(int nMenu, int nColumnBox, COLUMNBOXFILLINFO* pColumnBoxFillInfo) ;
	float SliderPercent(int nMaxRow, int nMaxLine, int nStartRow) ;
	void UpdateColumnBoxStartRow(int nMenu, int nColumnBox, int nStartRow) ;
	void FillColumnBox(int nMenu, int nColumnBox) ;
	
	void CreateButton(int nMenu, int nButton, int nPositionX, int nPositionY, int nSizeX, int nSizeY, int nFrameSizeX, int nFrameSizeY, int nAction, int nStyle, bool bVisible, float flMin, float flMax, char *pLabel, char *pName, int nNameMem) ;
	void CreateButtonSimple(int nMenu, int nButton, int nPositionX, int nPositionY, int nSizeX, int nSizeY, int nAction, char *pLabel, char *pName) ;
	void CreateButtonSimpleLongName(int nMenu, int nButton, int nPositionX, int nPositionY, int nSizeX, int nSizeY, int nAction, char *pLabel, char *pName) ;
	
	
	// Same as CreateButton, except must specify how much memory will be assigned to Names
	void CreateButtonSpecial(int nMenu, int nButton, int nPositionX, int nPositionY, int nSizeX, int nSizeY, int nFrameSizeX, int nFrameSizeY, int nAction, int nStyle, bool bVisible, float flMin, float flMax, char *pLabel, char *pName, int nMemorySize) ;
	
	bool ResetButtonStrings(int nMenu, int nButton) ; // clear all the name strings and set max to zero.
	bool AddButtonString(int nMenu, int nButton, char *pName) ;// set a specific switch name, and sets this as the last switch. return false if failed

	bool SetButtonLabel(int nMenu, int nButton, char *pLabel) ;
	bool SetButtonStrings(int nMenu, int nButton, char *pName) ; // set the button label and all strings.  Will set the correct MaxSwitch value automatically. return false if failed
	bool SetButtonString(int nMenu, int nButton, int nSwitch, char *pName) ; // set a particular switch name.  Doesn't check or update maxswitch
	bool SetButtonString_ColumnBox(int nMenu, int nButton, int nMaxName, int nMaxNameSize, char *pName) ;
	
	bool SetButtonSwitch(int nMenu, int nButton, int nSwitch) ; // set switch by number. returns true on success  
	bool SetButtonSwitch(int nMenu, int nButton, char *pSwitch) ; // set switch by matching string.  returns true on success
	int GetButtonSwitch(int nMenu, int nButton) ;

	bool SetEditBoxInteger(int nMenu, int nButton, int nNum) ;
	int GetEditBoxInteger(int nMenu, int nButton) ;
	bool SetEditBoxFloat(int nMenu, int nButton, float flNum) ;
	float GetEditBoxFloat(int nMenu, int nButton) ;

	bool SetButtonMin(int nMenu, int nButton, float flMin) ;
	bool SetButtonMax(int nMenu, int nButton, float flMax) ;
	bool SetButtonMinMax(int nMenu, int nButton, float flMin, float flMax) ;

	/////////////////////////////////////////////////////////////////////////////////////
	// file/folder functions, windows specific

	std::vector<char> m_FileNames ;
	char m_chCurrentFolder ;
	void GetFileNames(char *pFolder, std::vector<char> &FileNames) ;
	
	/////////////////////////////////////////////////////////////////////////////////////
	// infotext
	
	char m_chInfoText[MAX_INFOTEXT_LINES][MAX_INFOTEXT_CHAR] ;
	float m_flInfoTextColour[MAX_INFOTEXT_LINES][3] ;
	int m_nInfoTextLine ;
	int m_nInfoTextLongDisplay ;
	int m_dInfoTextTime ;
	void UpdateInfoText() ;
	void AddInfoText(char chMessage[], float flRd, float flGr, float flBl) ;
	void AddInfoTextBrief(char chMessage[], float flRd, float flGr, float flBl, int nDelay) ;
	void AddInfoTextData(char* pMessage, int flNum0, float flRd, float flGr, float flBl) ;
	void AddInfoTextData(char* pMessage, float flNum0, float flRd, float flGr, float flBl) ;

	float m_flInfoTextLineHeight ;
	float m_flInfoTextFontScaleX ;
	float m_flInfoTextFontScaleY ;


	// sprite buffer
	std::list<Ogre2dSprite> sprites;

	// new stuff for handling gui as a mesh
	bool CreateGuiMesh();
	void UpdateMesh() ;
	void DrawGui(Ogre::RenderWindow*	pRenderWnd, int* pBatchCount, int* pTriCount);
	void DrawGui(Ogre::RenderTexture*	pRenderTex, bool bSwap, int* pBatchCount, int* pTriCount);
	bool m_bGuiMeshReady ;
	Ogre::MeshPtr			m_GuiMesh;
	Ogre::Entity*			m_GuiMeshEntity;
	Ogre::SceneNode*		m_GuiMeshNode;
	Ogre::Camera*			m_pGuiCamera;
	char chName[MISCSTRINGSIZE] ;
	char chMeshName[MISCSTRINGSIZE] ;
	char chEntityName[MISCSTRINGSIZE] ;
	char chNodeName[MISCSTRINGSIZE] ;
	char chCameraName[MISCSTRINGSIZE] ;
	char chResourceName[MISCSTRINGSIZE] ;
	char chLocationName[MISCSTRINGSIZE] ;

};


#endif // __OGRE2D_MAIN_FILE 
