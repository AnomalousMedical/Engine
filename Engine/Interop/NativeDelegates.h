#if FULL_AOT_COMPILE
#define HANDLE_INSTANCE void* instanceHandle;
#define HANDLE_FIRST_ARG void* instanceHandle 
#define HANDLE_ARG , void* instanceHandle 
#define ASSIGN_HANDLE this->instanceHandle = instanceHandle;
#define PASS_HANDLE instanceHandle
#define PASS_HANDLE_ARG , instanceHandle
#else
#define HANDLE_INSTANCE 
#define HANDLE_FIRST_ARG
#define HANDLE_ARG 
#define ASSIGN_HANDLE 
#define PASS_HANDLE 
#define PASS_HANDLE_ARG 
#endif

typedef bool(*NativeFunc_Bool)(HANDLE_FIRST_ARG);

typedef int(*NativeFunc_Int)(HANDLE_FIRST_ARG);

typedef void(*NativeAction)(HANDLE_FIRST_ARG);

typedef void(*NativeAction_Bool)(bool arg0 HANDLE_FIRST_ARG);

typedef void(*NativeAction_Float_Float)(float arg0, float arg1 HANDLE_FIRST_ARG);

typedef void(*NativeAction_Float_Float_NoHandle)(float arg0, float arg1);