//列出HookBridge.mm中的方法
extern "C" {
    
    //C++
    void CallMethod();
    
    //OC
    const char* CreateInstance();
    
    const int GetInstanceInt(const char* instanceKey);
}
