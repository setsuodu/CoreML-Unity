//
//  ViewController.h
//  CoreMLDemo
//
//  Created by chenyi on 08/06/2017.
//  Copyright © 2017 chenyi. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>

@interface ViewController : NSObject

//C++
//+ (int) returnInt;

//+ (NSString*) returnString;

- (int) returnInstanceInt;

//- (NSString*) returnInstanceString;

//OC
+ (NSString*) createInstance;

+ (ViewController*) getInstanceForKey:(NSString*) key;

@end

#if defined (__cplusplus)
extern "C"
{
#endif
    
    void ImagePathToIOS(char* str)
    {
        NSLog(@"Unity传递过来的参数: %s", str);
    }
    
#if defined (__cplusplus)
}
#endif
