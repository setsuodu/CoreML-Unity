//
//  HookBridge.h
//  OSHook
//
//  Created by 薛宇涛 on 2017/1/15.
//  Copyright © 2017年 薛宇涛. All rights reserved.
//

//列出HookBridge.mm中的方法

extern "C"{
    
    //C++
    
    void CallMethod();
    
    const char* ReturnString();
    
    int ReturnInt();
    
    //OC
    
    const char* CreateInstance();
    
    const int GetInstanceInt(const char* instanceKey);
    
}
