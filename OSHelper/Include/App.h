#pragma once

class App
{
public:
	App(void);
    
	virtual ~App(void);
    
	void registerDelegates(NativeFunc_Bool onInitCB, NativeFunc_Int onExitCB, NativeAction onIdleCB HANDLE_ARG);
    
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
	NativeFunc_Bool onInitCB;
	NativeFunc_Int onExitCB;
	NativeAction onIdleCB;
	HANDLE_INSTANCE
};