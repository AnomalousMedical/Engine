#pragma once

typedef bool(*OnInitDelegate)(HANDLE_FIRST_ARG);
typedef int(*OnExitDelegate)(HANDLE_FIRST_ARG);

class App
{
public:
	App(void);
    
	virtual ~App(void);
    
	void registerDelegates(OnInitDelegate onInitCB, OnExitDelegate onExitCB, NativeAction onIdleCB HANDLE_ARG);
    
    virtual void run() = 0;
    
    virtual void exit() = 0;
    
	void fireIdle()
    {
        onIdleCB(PASS_HANDLE);
    }
    
    bool fireInit()
    {
		return onInitCB(PASS_HANDLE);
    }
    
    int fireExit()
    {
		return onExitCB(PASS_HANDLE);
    }
    
private:
	OnInitDelegate onInitCB;
	OnExitDelegate onExitCB;
	NativeAction onIdleCB;
	HANDLE_INSTANCE
};