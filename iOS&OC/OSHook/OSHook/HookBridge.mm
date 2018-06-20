#import "HookBridge.h"

#import "ViewController.h"
#import "GoogLeNetPlaces.h"
#import "UIImage+Utils.h"

void CallMethod() {
    // To do some things.
    NSLog(@"haha...");
}

//使用实例调用.m中的方法
const char* CreateInstance() {
    return [[ViewController createInstance] cStringUsingEncoding:NSUTF8StringEncoding];
}

const int GetInstanceInt(const char* instanceKey) {
    NSString *key = [NSString stringWithCString:instanceKey encoding:NSUTF8StringEncoding];
    ViewController *hook = [ViewController getInstanceForKey:key];
    return [hook returnInstanceInt];
}
