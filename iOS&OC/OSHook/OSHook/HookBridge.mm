//
//  HookBridge.cpp
//  OSHook
//
//  Created by 薛宇涛 on 2017/1/15.
//  Copyright © 2017年 薛宇涛. All rights reserved.
//

#import "HookBridge.h"
#import "ViewController.h"
#import "GoogLeNetPlaces.h"
#import "UIImage+Utils.h"

void CallMethod(){
    // To do some things.
}

const char* ReturnString(){
    return "hello there";
}

int ReturnInt(){
    return 5;
}

//使用实例调用.m中的方法
const char* CreateInstance(){
    return [[ViewController createInstance] cStringUsingEncoding:NSUTF8StringEncoding];
}

const int GetInstanceInt(const char* instanceKey){
    NSString *key = [NSString stringWithCString:instanceKey encoding:NSUTF8StringEncoding];
    ViewController *hook = [ViewController getInstanceForKey:key];
    return [hook returnInstanceInt];
}

//extern
extern "C" void CallOC()
{
    NSLog(@"调用到了OC");
}
